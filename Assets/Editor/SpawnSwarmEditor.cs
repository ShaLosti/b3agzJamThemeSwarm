using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwarmSpawnController))]
public class SpawnSwarmEditor : ExtendetEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SwarmSpawnController swarmSpawnController = (SwarmSpawnController)target;
        if (swarmSpawnController == null)
            return;

        if (GUILayout.Button("Spawn one"))
        {
            swarmSpawnController.SpawnOneSwarm();
        }
    }
}
