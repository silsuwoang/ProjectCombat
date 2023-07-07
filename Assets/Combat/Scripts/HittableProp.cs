using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableProp : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    
    [SerializeField] private float hp;
    [SerializeField] private GameObject normalObject;
    [SerializeField] private GameObject brokenObject;


    private void Start()
    {
        healthComponent.Init(hp, null,
            () =>
            {
                if (normalObject)
                {
                    normalObject.SetActive(false);
                }
                if (brokenObject)
                {
                    brokenObject.SetActive(true);
                }
            }, null);
    }
}
