using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(World))]
public class WorldInspector : Editor
{

    World mTarget;

    void OnEnable()
    {
        mTarget = (World)target;
    }

    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //if (GUILayout.Button("Change Center"))
        //{
        //    mTarget.ChangeCenter(mTarget.CenterX, mTarget.CenterY);
        //}
    }
}
