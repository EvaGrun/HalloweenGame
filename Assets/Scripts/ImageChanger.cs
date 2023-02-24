using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    private Sprite firstSprite;
    [SerializeField] private Sprite newSprite;
    private bool change;
    private Image Img;

    private void Start()
    {
        Img = GetComponent<Image>();
        firstSprite = Img.sprite;
    }

    public void SpriteChang()
    {
        if (change)
        {
            Img.sprite = firstSprite;
        }
        else
        {
            Img.sprite = newSprite; 
        }

        change = !change;
    }

}
