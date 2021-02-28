using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SwarmMovePointsKeeper : MonoBehaviour
{
    [SerializeField]
    MovePoint[] movePoints;

    Dictionary<int, MovePoint[]> movePointsDictionary = new Dictionary<int, MovePoint[]>();
    static SwarmMovePointsKeeper instance;
    bool init = false;

    public static SwarmMovePointsKeeper Instance { get => instance; }
    public bool Initialized { get => init; }
    public int LastLvl { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetupMovePoints();
        GC.Collect();
    }

    private void SetupMovePoints()
    {
        int lvl = movePoints[0].MovePointLvl;
        int lastIndex = 0;
        int k = 0;
        MovePoint[] tempMovePoint = new MovePoint[100];
        for (int i = 0; i < movePoints.Length; i++)
        {
            if (lvl != movePoints[i].MovePointLvl)
            {
                tempMovePoint = new MovePoint[i - lastIndex];
                k = 0;
                for (int j = lastIndex; j < i; j++)
                {
                    tempMovePoint[k] = movePoints[j];
                    k++;
                }

                movePointsDictionary.Add(lvl, tempMovePoint);
                lastIndex = i;
            }
            lvl = movePoints[i].MovePointLvl;
        }
        k = 0;
        tempMovePoint = new MovePoint[movePoints.Length - lastIndex];
        for (int j = lastIndex; j < movePoints.Length; j++)
        {
            tempMovePoint[k] = movePoints[j];
            k++;
        }

        movePoints = new MovePoint[0];
        movePointsDictionary.Add(lvl, tempMovePoint);
        LastLvl = lvl;
        init = true;
    }

    public void SetAndSortAllMovePoints()
    {
        var movePointsGroup = GetComponentsInChildren<MovePointsGroup>();
        movePoints = movePointsGroup.SelectMany(x => x.transform.GetComponentsInChildren<MovePoint>()).ToArray();
        Array.Sort(movePoints);
        //Array.ForEach(movePoints, x => Debug.Log(x.MovePointLvl));
    }
    public MovePoint GetMovePoint(int lvl)
    {
        return movePointsDictionary[lvl]
            [UnityEngine.Random.Range(0, movePointsDictionary[lvl].Length)];
    }
    public int GetMaxLvl()
    {
        return movePointsDictionary.Count - 1;
    }
}
