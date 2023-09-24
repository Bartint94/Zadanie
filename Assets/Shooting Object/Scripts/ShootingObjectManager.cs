using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectManager : MonoBehaviour
{
    public int positionId;
    int life = 3;
    public GameManager gameManager;
    [SerializeField] Material[] material;
    Material[] materialSetUp = new Material[1];
    [SerializeField] MeshRenderer meshRenderer;
    public bool isDead;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        materialSetUp[0] = material[0];
        meshRenderer.materials = materialSetUp;
    }

    public void Dye()
    {
            life--;
        if(life == 2)
        {
            materialSetUp[0] = material[1];
            meshRenderer.materials = materialSetUp;
        }
        if(life == 1) 
        {
            materialSetUp[0] = material[2];
            meshRenderer.materials = materialSetUp;
        }
        if(life > 0)
        {
            gameManager.Respawn(gameObject);
            gameManager.spawns[positionId].isFree = true;
        }
        else
        {
            if(!isDead)
            {
                GameManager.deaths++;
                Debug.Log(GameManager.deaths);
            }
            isDead = true;
            Destroy(gameObject);
        }
    }
}
