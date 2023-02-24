using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float maxTime;
    public float MaxTime => maxTime;

    [SerializeField] private UnityEvent _OnTimeOver;

    private float currentTime;
    public float CurrentTime => currentTime;

    private void Awake()
    {
        image = GetComponent<Image>();
        currentTime = 0;
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                _OnTimeOver.Invoke();
            }
            image.fillAmount = currentTime / maxTime;
        }
    }


    public void Restart()
    {
        currentTime = maxTime;
    }

    public void Stop()
    {
        currentTime = 0;
        image.fillAmount = 1;
    }

}
