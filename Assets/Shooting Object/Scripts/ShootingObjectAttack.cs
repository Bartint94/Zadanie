using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform aimTransform;

    float elapsed;
    float shootCooldown = 1f;
    void SpawnBullet()
    {
        Instantiate(bullet,aimTransform.position,transform.rotation);
    }
    private void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= shootCooldown)
        {
            SpawnBullet();
            elapsed = 0f;
        }
    }

}
