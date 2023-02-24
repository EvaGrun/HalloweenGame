using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sunset : MonoBehaviour
{
    Color[] sun = new Color[7];

    [SerializeField] private GameLogic LevelInfo;
    [SerializeField] private Image Img;


    void Start()
    {
        Img = GetComponent<Image>();
        sun[0] = new Color(0.9f, 0.9f, 0.7f, 1);
        sun[1] = new Color(0.9f, 1f, 0.6f, 1);
        sun[2] = new Color(1f, 0.7f, 0.4f, 1);
        sun[3] = new Color(1f, 0.5f, 0.4f, 1);
        sun[4] = new Color(0.8f, 0.3f, 0.4f, 1);
        sun[5] = new Color(0.5f, 0.4f, 0.6f, 1);
        sun[6] = new Color(0.3f, 0.2f, 0.4f, 1);
    }

    public void Update()
    {
        int level = LevelInfo.Level;
        if (0 < level && level < 3)
            Img.color = sun[0];
        if (2 < level && level < 5)
            Img.color = sun[1];
        if (4 < level && level < 7)
            Img.color = sun[2];
        if (6 < level && level < 9)
            Img.color = sun[3];
        if (8 < level && level < 11)
            Img.color = sun[4];
        if (10 < level && level < 13)
            Img.color = sun[5];
        //if (LevelInfo.winPanel.activeInHierarchy)
        //    Img.color = sun[6];


    }
}
