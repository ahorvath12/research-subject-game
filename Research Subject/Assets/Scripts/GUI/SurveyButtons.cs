using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyButtons : MonoBehaviour
{
    public Image buttonImage;
    private int index = 0;

    public Sprite[] images;

    void Start()
    {
        index = 0;
        buttonImage.sprite = images[index];
    }

    public void UpdateButtonImage() {
        index++;
        if (index >= images.Length) {
            return;
        }
        buttonImage.sprite = images[index];
    }
}
