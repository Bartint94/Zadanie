using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    public static float maxDistance;
    public Vector3 startPos;
    public Vector3 midPoint;

   
    private void OnEnable()
    {
        startPos = transform.position;
        midPoint = GameManager.midPoint;
    }
   public void ReturnBullet()
    {
        GameManager.bulletList.Remove(this);
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
   private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, midPoint) >= maxDistance)
        {
            GameManager.bulletList.Remove(this);
            ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Object"))
        {
            var manager = other.GetComponentInParent<ShootingObjectManager>();
            if (!manager.isDead)
            {
                manager.Dye();
            }
            GameManager.bulletList.Remove(this);
            ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        }
    }

}
