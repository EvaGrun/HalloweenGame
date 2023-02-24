using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButton : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Timer timer;


    private void Update()
    {
        if (timer.CurrentTime == 0)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
}
