using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class WinningBar : MonoBehaviour
{
    public GameObject timerBar;
    public int time;

    // Start is called before the first frame update
    void Start()
    {
        AnimateBar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimateBar()
    {
        LeanTween.scaleX(timerBar, 1, time);
    }
}
