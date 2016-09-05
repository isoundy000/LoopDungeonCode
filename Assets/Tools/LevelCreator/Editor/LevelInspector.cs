using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelInspector : Editor
{

    Level mTarget;

    void OnEnable()
    {
        mTarget = (Level)target;
    }

    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Normalize Level"))
        {
            mTarget.NormalizeLevel();
        }

        if (GUILayout.Button("Generate Floor"))
        {
            mTarget.GenerateFloor();
        }
    }
}
