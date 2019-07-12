#ifndef TEXELSNAP_INCLUDED
#define TEXELSNAP_INCLUDED

float2 TexelSnapComponents(float2 uv, float2 density) {
	float2 originalUV = uv;
    // Convert UV to integer texture space size, then back to UV space. Then add half a texel to center it.
	float2 centerUV = floor(originalUV * density) / density + (2.0 / density);
	float2 dUV = centerUV - originalUV;

	// Get the change in UVs across fragments
	float2 dUVdS = ddx(originalUV);
	float2 dUVdT = ddy(originalUV);

	// Invert the matrix created by the ddx/y of the UVs
	float2x2 dSTdUV = float2x2(dUVdT[1], -dUVdS[1], -dUVdT[0], dUVdS[0]) * (1 / (dUVdS[0] * dUVdT[1] - dUVdS[1] * dUVdT[0]));

	// Convert the UV delta to a fragment space delta
	return mul(dUV, dSTdUV);
}

float4 TexelSnap4(float4 value, float2 components) {
	return value + (ddx(value) * components.x + ddy(value) * components.y);
}

float3 TexelSnap3(float3 value, float2 components) {
	return value + (ddx(value) * components.x + ddy(value) * components.y);
}

float2 TexelSnap2(float2 value, float2 components) {
	return value + (ddx(value) * components.x + ddy(value) * components.y);
}

float TexelSnap1(float value, float2 components) {
	return value + (ddx(value) * components.x + ddy(value) * components.y);
}

#endif