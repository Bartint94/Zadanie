using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ShootingObjectManager shootingObject;
    [SerializeField] float space;

   

    int positionId;
    public int valueToSpawn;
    public static int deaths;

    public List<Spawns> spawns = new List<Spawns>();

    public float bulletMaxDistance;
    public static Vector3 midPoint;

    public bool useJobs;
    public static List<Bullet> bulletList = new List<Bullet>();
    
    public class Spawns
    {
        public Vector3 spawnPos;
        public bool isFree;
        public int id;
    }
    private void Start()
    {
        deaths = 0;
        SpawnObjects(valueToSpawn);
    }

    void SpawnObjects(int value)
    {
        float square = Mathf.Sqrt(value);
        square = Mathf.Ceil(square);
        bulletMaxDistance = Mathf.Sqrt(Mathf.Pow(square,2) + Mathf.Pow(square,2)) / 2 * space;
        float midColumnPoint = (square * space - space) / 2;
        midPoint = new Vector3(midColumnPoint, 0, midColumnPoint);

        Bullet.maxDistance = bulletMaxDistance;

        positionId = 0;

        for (int row = 0; row < square; row++)
        {
            for (int column = 0; column < square; column++)
            {
                Vector3 spawnPos = new Vector3(row * space, 0f, column * space);


                var spawn = new Spawns() { spawnPos = spawnPos };
                spawn.id = positionId;

                if (positionId < value)
                {
                    var objSpawned = ObjectPoolManager.Spawn(shootingObject.gameObject, spawnPos, Quaternion.identity);
                    var shootingManager = objSpawned.GetComponent<ShootingObjectManager>();
                    shootingManager.positionId = positionId;

                    spawn.isFree = false;
                }
                else
                {
                    spawn.isFree = true;
                }

                spawns.Add(spawn);
                positionId++;

            }
        }
    }

    Spawns FindFreePosition()
    {
        Spawns freeSpawn = spawns.Find(x => x.isFree == true);
        freeSpawn.isFree = false;
        return freeSpawn;

    }
    public void Respawn(GameObject gameObject)
    {
        StartCoroutine(RespawnAfterSononds(gameObject));    
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    IEnumerator RespawnAfterSononds(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        var freeSpawn = FindFreePosition();

        var objSpawned = ObjectPoolManager.Spawn(shootingObject.gameObject, freeSpawn.spawnPos, Quaternion.identity);
        var shootingManager = objSpawned.GetComponent<ShootingObjectManager>();
        shootingManager.positionId = freeSpawn.id;
    }
    NativeArray<float3> positionList;
    NativeArray<float3> forwardList;
    private void FixedUpdate()
    {
        if (useJobs)
        {
            positionList = new NativeArray<float3>(bulletList.Count, Allocator.TempJob);
            forwardList = new NativeArray<float3>(bulletList.Count, Allocator.TempJob);

            for (int i = 0; i < bulletList.Count; i++)
            {
                positionList[i] = bulletList[i].transform.position;
                forwardList[i] = bulletList[i].transform.forward;
            }
          
            MovementJob _movementJob = new MovementJob
            {
                deltaTime = Time.deltaTime,
                positionList = positionList,
                forwardList = forwardList,
            };

            JobHandle jobHandle = _movementJob.Schedule(bulletList.Count, 70);
            jobHandle.Complete();

            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].transform.position = positionList[i];
            }
            positionList.Dispose();
            forwardList.Dispose();
        }
        else
        {
            foreach (var t in bulletList)
            {
                t.transform.position += t.transform.forward * 5f * Time.deltaTime;
            }
        }

    }
    private void OnDestroy()
    {
        if(positionList.IsCreated)
            positionList.Dispose();
        
        if(forwardList.IsCreated)
            forwardList.Dispose();
    }


}
[BurstCompile]
public struct MovementJob : IJobParallelFor
{
    public NativeArray<float3> forwardList;
    public NativeArray<float3> positionList;
    public float deltaTime;

    public void Execute(int index)
    {
        positionList[index] = positionList[index] + forwardList[index] * 5 * deltaTime;
    }
  
}



