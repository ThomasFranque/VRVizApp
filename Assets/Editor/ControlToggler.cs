using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CtrlToggle))]
public class ControlToggler : Editor
{
    CtrlToggle _ctrlToggler;

    private void OnEnable()
    {
        _ctrlToggler = target as CtrlToggle;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Space(20);

        if (_ctrlToggler._loader != default && GUILayout.Button("Toggle Loader "+ (!_ctrlToggler._loader.gameObject.activeSelf ? "ON" : "OFF"), GUILayout.Height(35)))
        {
            _ctrlToggler.ToggleLoader();
        }
        
        Space(12);

        if (GUILayout.Button("VR Controls", GUILayout.Height(_ctrlToggler.IsInVR ? 20 : 60)))
        {
            _ctrlToggler.ChangeToVRControls();
        }

        Space(3);

        if (GUILayout.Button("PC Controls", GUILayout.Height(!_ctrlToggler.IsInVR ? 20 : 60)))
        {
            _ctrlToggler.ChangeToPCControls();
        }

        serializedObject.ApplyModifiedProperties();
    }
    private void Space(int height)
    {
        EditorGUILayout.LabelField("", GUILayout.Height(height));
    }

}