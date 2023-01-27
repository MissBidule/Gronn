using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float currentTime;
    public float startingTime = 10f;

    [SerializeField] Text CountdownText; 

    void Start()
    {
        currentTime = 0;
    }

    public void restart() {
        currentTime = startingTime;
    }

    void Update()
    {
          currentTime -= 1 * Time.deltaTime;  

          if (isOver()) {
            currentTime = 0;
          }

          CountdownText.text = currentTime.ToString("0");
    }

    public int remainingTime() {
        return (int) currentTime;
    }

    public bool isOver() {
        if (currentTime <= 0)
            return true;
        return false;
    }
}
