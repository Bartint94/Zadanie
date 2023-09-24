using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

public class ShootingObjectAttack : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    [SerializeField] Transform aimTransform;

    float elapsed;
    float shootCooldown = 1f;

    public bool useJobs;
    private void Start()
    {
    }
    public void SpawnBullet()
    {
        var spawnBullet = ObjectPoolManager.Spawn(bullet.gameObject, aimTransform.transform.position, transform.rotation);
        var bulletS = spawnBullet.GetComponent<Bullet>();
        GameManager.bulletList.Add(bulletS);
        
    }
    private void FixedUpdate()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= shootCooldown)
        {
            SpawnBullet();
            elapsed = 0f;
        }
    }
}