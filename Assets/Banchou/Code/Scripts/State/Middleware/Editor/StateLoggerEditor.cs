using System.Linq;

using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

using Banchou.Middleware;

[CustomEditor(typeof(StateLogger))]
public class StateLoggerEditor : Editor {
    private StateLogger _target;
    private Vector2 _scrollPosition;
    private int _openLogIndex;
    private int _openTab;
    private string[] _tabs = new [] {
        "Action",
        "State",
        "Diff"
    };

    private void OnEnable() {
        _target = (StateLogger)target;
    }

    public override void OnInspectorGUI() {
        EditorGUILayout.BeginHorizontal();
        _openTab = GUILayout.SelectionGrid(_openTab, _tabs, 3);
        
        string text = string.Empty;

        if (_target.History.Count > 0) {
            var openLog = _target.History[_openLogIndex];
            switch (_openTab) {
                case 0: text = JsonConvert.SerializeObject(openLog.Action, Formatting.Indented); break;
                case 1: text = JsonConvert.SerializeObject(openLog.State, Formatting.Indented); break;
            }
        }

        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.SelectableLabel(text, EditorStyles.textArea, GUILayout.Height(200f));
        
        _scrollPosition = GUILayout.BeginScrollView(
            _scrollPosition,
            alwaysShowHorizontal: false,
            alwaysShowVertical: true,
            GUILayout.MaxHeight(150f)
        );

        _openLogIndex = GUILayout.SelectionGrid(
            _openLogIndex,
            _target.History.Select(log => log.Action.GetType().Name).ToArray(),
            1
        );
        GUILayout.EndScrollView();
    }
}
