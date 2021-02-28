using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Swarm : MonoBehaviour, ILivingEntity
{
    private bool goBySteps;
    private int goByStepsIndex = 0;
    [SerializeField]
    int currentLvl;
    private MovePoint targetObjectMovePoint;
    Vector3 targetMovePoint = Vector3.zero;

    Vector3 currentPlrPos;
    [Header("Setup objects")]
    [SerializeField]
    Rigidbody2D swarmRigidbody2D;
    [SerializeField]
    Transform swarmTransform;

    float speed;
    private bool goingToEnd = false;
    [Range(0.0f, 50.0f), SerializeField]
    float normalSpeed;
    [Range(0.0f, 100.0f), SerializeField]
    float chargeSpeed;

    Vector3 disableSimualtionZonePos;
    Vector3 dir;

    WaitForSeconds waitForSeconds = new WaitForSeconds(.1f);

    [SerializeField]
    private int minMoneytake;
    [SerializeField]
    private int maxMoneyTake;

    public int GoldTake { get => Random.Range(minMoneytake, maxMoneyTake); }
    public MovePoint TargetObjectMovePoint { get => targetObjectMovePoint; }

    public virtual void Setup(Vector3 _disableSimualtionZonePos, bool attack = false)
    {
        disableSimualtionZonePos = _disableSimualtionZonePos;
        speed = normalSpeed;
        if (attack)
        {
            speed = chargeSpeed;
            currentLvl = Random.Range(SwarmMovePointsKeeper.Instance.LastLvl - 3, SwarmMovePointsKeeper.Instance.LastLvl);
            targetObjectMovePoint = SwarmMovePointsKeeper.Instance.GetMovePoint(currentLvl);
            targetMovePoint = targetObjectMovePoint.transform.position;
            return;
        }
        OnTargetPointReached();
    }
    protected virtual void Start()
    {
        StartCoroutine(RigidBodySimulationHolder());
    }
    protected virtual void Update()
    {
        currentPlrPos = swarmTransform.position;

        //Vector2 dir = (targetMovePoint - currentPlrPos).normalized;
        dir = Utils.FastNormalized(targetMovePoint - currentPlrPos);

        //swarmTransform.Translate(dir * speed * Time.deltaTime);
        currentPlrPos += dir * speed * Time.deltaTime;
        swarmTransform.position = currentPlrPos;

        currentPlrPos = swarmTransform.position;
        if ((currentPlrPos - targetMovePoint).sqrMagnitude < .1f * .1f)
            OnTargetPointReached();
    }

    protected IEnumerator RigidBodySimulationHolder()
    {
        if (goingToEnd)
            yield break;
        while (true)
        {
            if (Mathf.Abs((currentPlrPos - disableSimualtionZonePos).y) <= .5f)
                swarmRigidbody2D.simulated = false;                

            yield return waitForSeconds;
        }
    }

    protected virtual void OnTargetPointReached()
    {
        if (SwarmMovePointsKeeper.Instance.LastLvl != currentLvl)
            DealWithLvl();

        if (SwarmMovePointsKeeper.Instance.LastLvl == currentLvl)
        {
            speed = chargeSpeed;
            goingToEnd = true;
            swarmRigidbody2D.simulated = true;
        }
        else
        {
            speed = normalSpeed;
        }
        if (!goBySteps)
        {
            targetObjectMovePoint = SwarmMovePointsKeeper.Instance.GetMovePoint(currentLvl);
            targetMovePoint = targetObjectMovePoint.transform.position;
        }
        else
        {
            targetObjectMovePoint = targetObjectMovePoint.connectedMovePoints
                [Random.Range(0, targetObjectMovePoint.connectedMovePoints.Length)];

            targetMovePoint = targetObjectMovePoint.transform.position;
        }
    }

    private void DealWithLvl()
    {
        if (currentLvl > 0 && Random.Range(0, 3) == 0)
        {
            currentLvl--;
        }
        else
        {
            int maxLvl = Random.Range(1, 4);
            if (currentLvl + maxLvl > SwarmMovePointsKeeper.Instance.LastLvl)
                currentLvl = SwarmMovePointsKeeper.Instance.LastLvl;
            else
                currentLvl += maxLvl;

            if(currentLvl == SwarmMovePointsKeeper.Instance.LastLvl)
            {
                goBySteps = true;
            }
        }
    }
}
