using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MovePoint : MonoBehaviour, IComparable
{
    MovePointsGroup group;
    [SerializeField]
    public MovePoint[] connectedMovePoints;
    public MovePointsGroup Group
    {
        get
        {
            if (group == null)
                group = transform.parent.GetComponent<MovePointsGroup>();
            return group;
        }
    }
    public int MovePointLvl { get => Group.MovePointsGroupIndex; }

    public int CompareTo(object obj)
    {
        MovePoint movePoint = (MovePoint)obj;
        return Group.MovePointsGroupIndex
            .CompareTo(movePoint.Group.MovePointsGroupIndex);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Group == null)
            return;
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.normal.textColor = Color.red;
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, .1f);
        Handles.Label(transform.position, Group.MovePointsGroupIndex.ToString(), gUIStyle);
    }
#endif

}
