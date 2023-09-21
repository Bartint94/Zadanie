using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    private void FixedUpdate()
    {
        transform.Translate(0f,0f, speed * Time.deltaTime);    
    }
}
