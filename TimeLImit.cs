using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLImit : MonoBehaviour
{

    float cuurrentTime = 0f;
    public float startingTime = 10f;
    public Text countDownText;
    public GameObject gameOverPanel;
   // public AudioSource timeAudio;
    void Start()
    {
        cuurrentTime = startingTime;
    }


    void Update()
    {
        cuurrentTime -= 1 * Time.deltaTime;
        countDownText.text = "Remaining Time:" + cuurrentTime.ToString("0");
        if (cuurrentTime <= 0)
        {
            cuurrentTime = 0;
     //       timeAudio.Play();
           
            Time.timeScale = 0f;
        }
    }
}
