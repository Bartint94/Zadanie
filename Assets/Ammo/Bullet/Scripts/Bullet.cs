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
        // transform.Translate(0f,0f, speed * Time.deltaTime);  

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
            other.GetComponentInParent<ShootingObjectManager>().Dye();
            GameManager.bulletList.Remove(this);
            ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        }
    }

}
