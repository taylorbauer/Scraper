using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutomaticVerticalSize))]
// Learned how to do this from: https://www.youtube.com/watch?v=_LX8zbkbOGw
public class AutomaticVerticalSizeEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Recalculate size")) {
            AutomaticVerticalSize myscript = ((AutomaticVerticalSize)target);
            myscript.AdjustSize();
        }
    }
}
