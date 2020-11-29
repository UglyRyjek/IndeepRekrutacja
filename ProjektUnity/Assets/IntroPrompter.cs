using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class IntroPrompter : MonoBehaviour
{
    [SerializeField] private Transform whole;
    [SerializeField] private Image placeForImage;
    [SerializeField] private Button onButton;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Text songText;


    public void DisplayIntro(LevelIntro intro, Action onClick)
    {
        whole.gameObject.SetActive(true);

        if(intro.image == null)
        {
            placeForImage.enabled = false;
        }
        else
        {
            placeForImage.enabled = true;
            placeForImage.sprite = intro.image;
        }

        onButton.onClick.RemoveAllListeners();
        onButton.onClick.AddListener(() => onClick?.Invoke());
        onButton.onClick.AddListener(() => HideIntro());

        buttonText.text = intro.okButtonText;
        titleText.text = intro.title;
        contentText.text = intro.content;
        songText.text = intro.songTitle;

    }


    public void HideIntro()
    {
        whole.gameObject.SetActive(false);
    }
}
