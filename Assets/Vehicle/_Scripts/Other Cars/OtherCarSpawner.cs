using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class OtherCarSpawner : MonoBehaviour
{
    [SerializeField]
    private OtherCarController otherCar;
    [SerializeField]
    private BoxCollider spawnArea;
/*    [SerializeField]
    private float speed=5f;*/
    [SerializeField]
    private float carsPerSecond = 10;

    [SerializeField]
    private bool useObjectPool=false;

    private float lastSpawnTime;
    private ObjectPool<OtherCarController> carPool;
    private void Awake()
    {
        carPool = new ObjectPool<OtherCarController>(CreatePooledObject,OnTakeFromPool, OnReturnToPool, OnDestroyObject,false,200,100000);
    }



    private OtherCarController CreatePooledObject()
    {
       OtherCarController instance= Instantiate(otherCar, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObejctPool;
        instance.gameObject.SetActive(false);
            
        return instance;
    }
    private void ReturnObejctPool(OtherCarController instance)
    {
        carPool.Release(instance);
    }
    private void OnTakeFromPool(OtherCarController instance)
    {

        instance.gameObject.SetActive(true);
        spawnOtherCar(instance);
        instance.transform.SetParent(transform,true);
    }

    private void OnReturnToPool(OtherCarController instance)
    {

        instance.gameObject.SetActive(false);
    }
    private void OnDestroyObject(OtherCarController instance)
    {

        Destroy(instance.gameObject);   
    }


    private void OnGUI()
    {
        if (useObjectPool)
        {

            GUI.Label(new Rect(10,10 , 200, 3), $"Total pool size: { carPool.CountAll}");
            GUI.Label(new Rect(10,10 , 200, 3), $"Active objects: { carPool.CountActive}");
        }
    }
    private void Update()
    {

        float delay = 1 / carsPerSecond;

        if (lastSpawnTime + delay < Time.time)
        {
        int carsToSpawnInFrame = Mathf.CeilToInt(Time.deltaTime / delay);
            while (carsToSpawnInFrame > 0)
            {
                if (!useObjectPool)
                {
                    OtherCarController instance=Instantiate(otherCar, Vector3.zero,Quaternion.identity);
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

    private void spawnOtherCar(OtherCarController otherCar)
    {
        Vector3 spawnLocation = new Vector3(

        spawnArea.transform.position.x + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.x, spawnArea.bounds.extents.x),
        spawnArea.transform.position.y + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.y, spawnArea.bounds.extents.y),
        spawnArea.transform.position.z + spawnArea.center.x + Random.Range(-1 * spawnArea.bounds.extents.z, spawnArea.bounds.extents.z));

        otherCar.transform.position = spawnLocation;
  
        
    }


}
