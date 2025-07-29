using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class EmissiveController : MonoBehaviour
{
    [SerializeField] private Material emissiveMaterial;


    private void Start()
    {

        if (emissiveMaterial == null)
        {
            emissiveMaterial = GetComponent<Renderer>().material;
        }
    }

    public void OnEmissiveEnable()
    {

        emissiveMaterial.EnableKeyword("_EMISSION");
    }

    public void OnEmissiveDisable()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
    }
}
