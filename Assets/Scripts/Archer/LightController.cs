using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
  


    private void EnableLight()
    {
        gameObject.GetComponent<Light>().enabled = true;
    }

    private void DisableLight()
    {
        gameObject.GetComponent<Light>().enabled = false;
    }
}
