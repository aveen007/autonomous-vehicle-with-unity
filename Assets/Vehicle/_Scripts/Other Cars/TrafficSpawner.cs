using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TrafficSpawner : MonoBehaviour
{
    [SerializeField]
    private TrafficController otherCar;
    [SerializeField]
    private BoxCollider spawnArea;
    /*    [SerializeField]
        private float speed=5f;*/
    [SerializeField]
    private float carsPerSecond = 0.1f;
    [SerializeField]
    private int trafficPerScenen = 7;

    [SerializeField]
    private bool useObjectPool = false;

    private float lastSpawnTime;
    private ObjectPool<TrafficController> carPool;
    private void Awake()
    {
        carPool = new ObjectPool<TrafficController>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 200, 100000);
    }



    private TrafficController CreatePooledObject()
    {
        TrafficController instance = Instantiate(otherCar, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObejctPool;
        instance.gameObject.SetActive(false);

        return instance;
    }
    private void ReturnObejctPool(TrafficController instance)
    {
        carPool.Release(instance);
    }
    private void OnTakeFromPool(TrafficController instance)
    {

        instance.gameObject.SetActive(true);
        spawnOtherCar(instance);
        instance.transform.SetParent(transform, true);
    }

    private void OnReturnToPool(TrafficController instance)
    {

        instance.gameObject.SetActive(false);
    }
    private void OnDestroyObject(TrafficController instance)
    {

        Destroy(instance.gameObject);
    }


    private void OnGUI()
    {
        if (useObjectPool)
        {

            GUI.Label(new Rect(10, 10, 200, 3), $"Total pool size: {carPool.CountAll}");
            GUI.Label(new Rect(10, 10, 200, 3), $"Active objects: {carPool.CountActive}");
        }
    }
    private void Update()
    {

        float delay = 1 / carsPerSecond;

        if (lastSpawnTime + delay < Time.time&&transform.childCount<trafficPerScenen)
        {
            int carsToSpawnInFrame = Mathf.CeilToInt(Time.deltaTime / delay);
            while (carsToSpawnInFrame > 0)
            {
                if (!useObjectPool)
                {
                    TrafficController instance = Instantiate(otherCar, Vector3.zero, Quaternion.identity);
                    instance.transform.SetParent(transform, true);

                    spawnOtherCar(instance);
                }
                else
                {
                    carPool.Get();
                }
                carsToSpawnInFrame--;
            }
            lastSpawnTime = Time.time;
        }
    }

    private void spawnOtherCar(TrafficController otherCar)
    {
        Vector3 spawnLocation = new Vector3(

        spawnArea.transform.position.x + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.x, spawnArea.bounds.extents.x),
        spawnArea.transform.position.y + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.y, spawnArea.bounds.extents.y),
        spawnArea.transform.position.z + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.z, spawnArea.bounds.extents.z));

        otherCar.transform.position = spawnLocation;


    }


}