using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ShootingObjectManager shootingObject;
    [SerializeField] float space;

    public int valueToSpawn;
    

    private void Start()
    {
        SpawnObjects(valueToSpawn);
    }
    void SpawnObjects(int value)
    {
        float square = Mathf.Sqrt(value);
        square = Mathf.Ceil(square);

        int positionId = 0;

        for (int row = 0; row < square; row++)
        {
            for(int column  = 0; column < square; column++)
            {
                Vector3 spawnPos = new Vector3(row * space, 0f, column * space);
                if(positionId < value)
                {
                    var objectManager = Instantiate(shootingObject, spawnPos, Quaternion.identity);
                    objectManager.positionId = positionId;
                }

                positionId++;
            }
        }
    }
}
