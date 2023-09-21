using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectMovement : MonoBehaviour
{
    float elapsed;
    float randTime;
    private void Start()
    {
        elapsed = 0f;
        RandomRotationTime();
    }
    void RandomRotation()
    {
        float randomValue = Random.Range(0f, 360f);
        transform.Rotate(0f, transform.rotation.y + randomValue, 0f);
    }
    void RandomRotationTime()
    {
        randTime = Random.Range(0f, 1f);
    }
    private void Update()
    {
        elapsed += Time.deltaTime;  

        if(elapsed >= 3)//randTime)
        {
            RandomRotation();
            RandomRotationTime();
            elapsed = 0f;
        }
    }

}
