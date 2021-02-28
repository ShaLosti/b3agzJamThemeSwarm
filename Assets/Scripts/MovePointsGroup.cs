using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MovePointsGroup : MonoBehaviour
{
    [SerializeField]
    private int movePointsGroupIndex;
    public int MovePointsGroupIndex { get => movePointsGroupIndex; }

    private void OnValidate()
    {
#if UNITY_EDITOR
        gameObject.name = movePointsGroupIndex.ToString();
#endif
    }
}
