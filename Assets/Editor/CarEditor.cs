using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Car))]
public class CarEditor : ExtendetEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Car car = (Car)target;

        if (car == null)
            return;

        if(GUILayout.Button("Stop car"))
        {
            car.Stop();
        }

        if (GUILayout.Button("Move car"))
        {
            car.Go();
        }
    }
}
