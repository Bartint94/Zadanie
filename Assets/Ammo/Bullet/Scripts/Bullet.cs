using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    public static float maxDistance;
    public Vector3 startPos;
    public ShootingObjectAttack attack;
    private void OnEnable()
    {
        startPos = transform.position;
    }
    private void FixedUpdate()
    {
       // transform.Translate(0f,0f, speed * Time.deltaTime);  
        
       // if(Vector3.Distance(transform.position, startPos) >= maxDistance )
        {
         //   manager.bulletList.Remove(this);
         //   ObjectPoolManager.ReturnObjectToPool(gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            GameManager.bulletList.Remove(this);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

}
