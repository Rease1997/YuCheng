using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public HighlightableObject arrowA;
    public HighlightableObject arrowB;
    // Start is called before the first frame update
    void Start()
    {
        arrowA.FlashingOn(Color.red, Color.yellow, 0.8f);
        arrowB.FlashingOn(Color.red, Color.yellow, 0.8f);
    }
    // Update is called once per frame
    void Update()
    {
        arrowA.FlashingOn(Color.red, Color.yellow, 0.8f);
        arrowB.FlashingOn(Color.red, Color.yellow, 0.8f);
    }
}
