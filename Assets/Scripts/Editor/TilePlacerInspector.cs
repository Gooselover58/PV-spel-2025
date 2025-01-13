using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilePlacer))]
public class TilePlacerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TilePlacer script = (TilePlacer)target;

        if (GUILayout.Button("Generate"))
        {
            script.Generate();
        }

        if (GUILayout.Button("Clear"))
        {
            script.Clear();
        }

        if (GUILayout.Button("Make Multiplayer"))
        {
            script.Clear();
        }
    }
}
