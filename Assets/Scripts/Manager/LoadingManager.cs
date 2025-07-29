using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    public Slider slider;
    private float progressValue;
    public void Start()
    {
#if UNITY_IOS
        StartCoroutine(LoadScene());
#endif
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        int disableProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync("Start");
        op.allowSceneActivation = false;//允许场景打开
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100);
            while (disableProgress < toProgress)
            {
                ++disableProgress;
                slider.value = disableProgress / 100.0f;//0.01开始
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (disableProgress < toProgress)
        {
            ++disableProgress;
            slider.value = disableProgress / 100.0f;
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
   
}
