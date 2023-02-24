using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void SoundStop()
    {
        if (audio.isPlaying)
            audio.Pause();
        else
            audio.Play();
    }


}
