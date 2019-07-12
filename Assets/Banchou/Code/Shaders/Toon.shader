Shader "Ergo Sum/Texel-Lit Toon"
{
	Properties
	{
		_BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
		_TexelDensity("Texel Density", Vector) = (0, 0, 0, 0)
		_ShadowTint ("Shadow Tint", Color) = (0, 0, 0, 1)

        _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Float) = 0.1
	}
	SubShader
	{
		Tags {
			"RenderType" = "Opaque"
			"RenderPipeline" = "LightweightPipeline"
			"IgnoreProjectors" = "True"
		}
		LOD 100

		//Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Back

        Pass {
            Name "Outline"
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"

            uniform float4 _OutlineColor;
            uniform float _OutlineThickness;

            struct VertexInput {
                float4 vertex: POSITION;
                float3 normal: NORMAL;
            };

            struct VertexOutput {
                float4 pos: SV_POSITION;
            };

            VertexOutput vert(VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal * _OutlineThickness, 1));
                return o;
            }

            float4 frag(VertexOutput i) : COLOR {
                return float4(_OutlineColor.rgb, 0);
            }
            ENDHLSL
        }

		Pass
		{
			Tags { "LightMode" = "LightweightForward" }

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

			#pragma vertex LitPassVertexSimple
			#pragma fragment frag

            
            #pragma multi_compile _ _VERTEX_LIGHTS
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile _ _SHADOWS_ENABLED
            #pragma multi_compile _ _SHADOWS_CASCADE
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            

			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			
			uniform float4 _BaseMap_TexelSize;
            uniform float2 _TexelDensity;
			uniform float4 _ShadowTint;

            uniform float4 _RimColor;
			uniform float _RimAmount;
			uniform float _RimThreshold;

			#define _MAIN_LIGHT_SHADOWS 1 // stupid error won't go away

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitForwardPass.hlsl"
			#include "Utility/TexelSnap.hlsl"

			half4 frag (Varyings i) : SV_Target {
                half4 finalFragment = half4(0,0,0,0);

				float2 snapComponents = TexelSnapComponents(i.uv, _TexelDensity);
				float3 texelNormal = TexelSnap3(i.normal, snapComponents);
				int pixelLightCount = GetAdditionalLightsCount();
				int l = 0;

                Light mainLight = GetMainLight();
				Light light = mainLight;
				do {
					float nDotL = dot(texelNormal, light.direction);

					float shadowAttenuation = 0.0;
					half4 shadowTint = 0.0;
					if (l == 0) {
					 	shadowAttenuation = MainLightRealtimeShadow(TexelSnap4(i.shadowCoord, snapComponents));
						shadowTint = _ShadowTint;// * (-nDotL + 1.0);
					} else {
						shadowAttenuation = AdditionalLightRealtimeShadow(l, i.posWS.xyz);
					}
					nDotL *= shadowAttenuation;

                    // Snaps the nDotL intensity to bands
					// TODO: Optimize this
					if (nDotL < 0.42) {
						nDotL = .42;//floor(.42 * _TexelDensity.x) / _TexelDensity.x;
					} else if (nDotL >= 0.42 && nDotL < 0.45) {
						nDotL = .7;//floor(.7 * _TexelDensity.x) / _TexelDensity.x;
					} else {
						nDotL = 1.0;
					}

					half3 attenuatedLightColor = light.color * light.distanceAttenuation;
					finalFragment += nDotL * SampleAlbedoAlpha(i.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)) * _BaseColor
						* half4(attenuatedLightColor, 1.0)
						+ shadowTint;
                        
					light = GetAdditionalLight(l, i.posWS.xyz);
				} while (l++ < pixelLightCount);

                float rimDot = 1 - dot(i.viewDir, texelNormal);
                float rimIntensity = rimDot * pow(dot(texelNormal, mainLight.direction) , _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor;

                finalFragment += rim;
                
				return finalFragment;
			}
			ENDHLSL
		}
		Pass
        {
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
		Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.lightweight/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
	}
}