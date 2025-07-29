using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnumeratorTool : UnitySingleton<IEnumeratorTool>
{
    public void StartCoroutineNew(IEnumerator coroutine)
    {
        
        StartCoroutine(coroutine);
    }
    public void StopCoroutineNew(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }

     

}
