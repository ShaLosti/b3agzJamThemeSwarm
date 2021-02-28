using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwarmMovePointsKeeper))]
public class SwarmPointsEditor : ExtendetEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SwarmMovePointsKeeper movePointsKeeper = (SwarmMovePointsKeeper)target;

        if (movePointsKeeper == null)
            return;

        if (GUILayout.Button("Pick move points"))
        {
            movePointsKeeper.SetAndSortAllMovePoints();
        }
    }
}
