using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmSpawnController : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    Swarm[] swarmPrefabs;

    float currentTime;
    [SerializeField]
    float normalSpawnTime;
    [SerializeField]
    float normalSpawnCount;
    float maxSpawnTime;
    float spawnCount;

    bool waveNow = false;

    Vector3 disableSimulationZoneY;

    private IEnumerator Start()
    {
        maxSpawnTime = normalSpawnTime;
        spawnCount = normalSpawnCount;
        disableSimulationZoneY = FindObjectOfType<DisableSimulationZone>().transform.position;
        yield return new WaitUntil(() => SwarmMovePointsKeeper.Instance.Initialized);
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < spawnPoints.Length; i++)
            spawnPoints[i] = transform.GetChild(i);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= maxSpawnTime)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnOneSwarm();
            }
            currentTime = 0;
        }
    }
    public void StartWave(WaveInfo waveInfo)
    {
        currentTime = 0;
        maxSpawnTime = waveInfo.spawnTime;
        spawnCount = waveInfo.spawnCount;
        waveNow = true;
    }
    public void StopWave()
    {
        currentTime = 0;
        maxSpawnTime = normalSpawnTime;
        spawnCount = normalSpawnCount;
        waveNow = false;
    }
    public void SpawnOneSwarm()
    {
        Swarm swarm = Instantiate(swarmPrefabs[Random.Range(0, swarmPrefabs.Length)],
            spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        swarm.Setup(disableSimulationZoneY, waveNow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
