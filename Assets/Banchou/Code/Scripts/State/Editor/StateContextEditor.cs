using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Banchou;

//[CustomEditor(typeof(StateContext))]
public class StateContextEditor : Editor {
    private StateContext _target;
    private SerializedProperty _initialState;
    private string _debugAction;

    public void OnEnable() {
        _target = (StateContext)target;
        _initialState = serializedObject.FindProperty("_initialState");
    }

    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_initialState);
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        
        if (Application.isPlaying) {
            _debugAction = EditorGUILayout.TextArea(_debugAction, GUILayout.MinHeight(100f));
            if (GUILayout.Button("Dispatch")) {
                var rawAction = JObject.Parse(_debugAction);
                var typeProperty = rawAction.Property("Type");
                if (typeProperty != null && typeProperty.Value.Type == JTokenType.String) {
                    var typeName = typeProperty.Value.ToString();
                    var actionType = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(assembly => assembly.GetType(typeName))
                        .Where(t => t != null)
                        .First();
                    var action = rawAction.ToObject(actionType);

                    _target.Dispatch(action);
                }
            }
        }
    }
}

