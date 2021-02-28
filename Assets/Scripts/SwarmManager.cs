using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    [SerializeField]
    WaveIncController waveIncController;
    [SerializeField]
    WaveInfo[] waveInfo;
    public static SwarmManager instance;

    float currentTime;
    bool waveNow = false;
    int waveIndex;
    int nextWaveIndex;

    SwarmSpawnController swarmSpawnController;
    private void Awake()
    {
        instance = this;
        swarmSpawnController = FindObjectOfType<SwarmSpawnController>();
    }
    private void Start()
    {
        currentTime = 0;
        waveIndex = -1;
        nextWaveIndex = 0;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (waveNow)
        {
            if(currentTime >= waveInfo[waveIndex].waveDuration)
            {
                StopWave();
            }
            return;
        }
        if (currentTime >= waveInfo[nextWaveIndex].timeSinceLastWave - 2f)
            waveIncController.Alert(true);
        if (currentTime >= waveInfo[nextWaveIndex].timeSinceLastWave)
        {
            NextWave();
        }
    }

    private void StopWave()
    {
        swarmSpawnController.StopWave();
        currentTime = 0;
        waveNow = false;
        if (nextWaveIndex >= waveInfo.Length)
        {
            waveIndex--;
            nextWaveIndex--;
        }
    }
    private void StartWave()
    {
        swarmSpawnController.StartWave(waveInfo[waveIndex]);
    }
    public void NextWave()
    {
        waveNow = true;
        currentTime = 0;
        waveIndex++;
        nextWaveIndex++;
        waveIncController.Alert(false);
        StartWave();
    }

}
[System.Serializable]
public struct WaveInfo
{
    public float timeSinceLastWave;
    public float waveDuration;
    public float spawnCount;
    public float spawnTime;
}