using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameManager gameManager;
    [SerializeField] Button startGameButton;
    [SerializeField] Animator[] animators;
    [SerializeField] Toggle t100;

    private void Awake()
    {
        animators = mainMenu.GetComponentsInChildren<Animator>();
    }
    private void Update()
    {
        Test();
    }

    void Test()
    {
        for(int i = 0; i < animators.Length; i++)
        {
            if (animators[i].GetBool("Selected") == true)
            {
                Invoke(nameof(ToggleOn),.3f); return;
            }
        }
        Invoke(nameof(ToggleOff), .3f);
    }
    void ToggleOff()
    {
        startGameButton.gameObject.SetActive(false);
    }
    void ToggleOn()
    {
        
        startGameButton.gameObject.SetActive(true); 
    }
   
}
