using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
public class CarsManager : MonoBehaviour
{
    [SerializeField]
    Transform carSpawnPoints;
    [SerializeField]
    Car carPrefab;
    Car lastCar;
    Queue<Car> carPool = new Queue<Car>();

    float waitSeconds;
    [SerializeField]
    float minWaitSeconds;
    [SerializeField]
    float maxWaitSeconds;
    float waitTime;
    bool wait = false;

    float leftDestroyPoint;
    private void Start()
    {
        leftDestroyPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        SpawnCar();
    }
    public void SpawnCar()
    {
        //Transform carSpawnPoint = carSpawnPoints[Random.Range(0, carSpawnPoints.Length)];
        Car car;
        if (carPool.Count == 0)
        {
            car = Instantiate(carPrefab, carSpawnPoints.position, Quaternion.identity);
            car.Init(carSpawnPoints.right, leftDestroyPoint);
            car.onCarRemove += CarRemove;
        }
        else
        {
            car = carPool.Dequeue();
            car.gameObject.SetActive(true);
            car.transform.SetPositionAndRotation(carSpawnPoints.position, Quaternion.identity);
        }
        car.Setup();
        lastCar = car;

        wait = false;
        waitSeconds = 0;
    }
    private void Update()
    {
        if ((lastCar.transform.position - carSpawnPoints.position).sqrMagnitude > 3f * 3f)
        {
            if (wait)
            {
                if (waitSeconds < waitTime)
                    waitSeconds += Time.deltaTime;
                else
                    SpawnCar();
                return;
            }

            if (Random.Range(0, 5) == 0)
                SpawnCar();
            else
            {
                waitTime = Random.Range(minWaitSeconds, maxWaitSeconds);
                wait = true;
            }
        }
    }
    public void CarRemove(Car car)
    {
        car.gameObject.SetActive(false);
        carPool.Enqueue(car);
    }
}
