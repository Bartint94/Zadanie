using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ShootingObjectManager shootingObject;
    [SerializeField] float space;

    public int valueToSpawn;
    public float boundsOffset;

    public float bulletMaxDistance;

    public bool useJobs;
    public static List<Bullet> bulletList = new List<Bullet>();
    private void Start()
    {
        SpawnObjects(valueToSpawn);
    }
    void SpawnObjects(int value)
    {
        float square = Mathf.Sqrt(value);
        square = Mathf.Ceil(square);
        bulletMaxDistance = Mathf.Sqrt(Mathf.Pow(square,2) + Mathf.Pow(square,2));
        Bullet.maxDistance = bulletMaxDistance;

        int positionId = 0;

        for (int row = 0; row < square; row++)
        {
            for(int column  = 0; column < square; column++)
            {
                Vector3 spawnPos = new Vector3(row * space, 0f, column * space);
                if(positionId < value)
                {
                    var objectManager = ObjectPoolManager.Spawn(shootingObject.gameObject, spawnPos, Quaternion.identity);
                    var script = objectManager.GetComponent<ShootingObjectManager>();
                    
                    script.positionId = positionId;
                   
                }

                positionId++;
            }
        }
    }
    private void Update()
    {
        if (useJobs)
        {
            NativeArray<float3> positionList = new NativeArray<float3>(bulletList.Count, Allocator.TempJob);
            NativeArray<float3> forwardList = new NativeArray<float3>(bulletList.Count, Allocator.TempJob);
            NativeArray<float3> newPosition = new NativeArray<float3>(bulletList.Count, Allocator.TempJob);

            for (int i = 0; i < bulletList.Count; i++)
            {
                positionList[i] = bulletList[i].transform.position;
                forwardList[i] = bulletList[i].transform.forward;
            }
            MovementJob _movementJob = new MovementJob
            {
                deltaTime = Time.deltaTime,
                position = positionList,
                forward = forwardList,
                newPosition = newPosition,

            };

            JobHandle jobHandle = _movementJob.Schedule(bulletList.Count, 30);
            jobHandle.Complete();

            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].transform.position = newPosition[i];
            }
            positionList.Dispose();
            forwardList.Dispose();
            newPosition.Dispose();
        }
        else
        {
            foreach (var t in bulletList)
            {
                t.transform.position += t.transform.forward * 5f * Time.deltaTime;
            }
        }

    }

}
[BurstCompile]
public struct MovementJob : IJobParallelFor
{
    public NativeArray<float3> newPosition;
    public NativeArray<float3> forward;
    public NativeArray<float3> position;
    public float deltaTime;
    public void Execute(int index)
    {
        newPosition[index] = position[index] + forward[index] * 5 * deltaTime;
    }
}



