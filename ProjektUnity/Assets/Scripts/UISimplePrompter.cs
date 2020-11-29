using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UISimplePrompter : MonoBehaviour
{
    [SerializeField] private Transform wholePrompter;
    [SerializeField] private Text title;
    [SerializeField] private Text content;
    [SerializeField] private Button buttonLeft;
    private Text buttonLeftText;
    [SerializeField] private Button buttonRight;
    private Text buttonRightText;




    private void Start()
    {
        buttonLeftText = buttonLeft.GetComponentInChildren<Text>();
        buttonRightText = buttonRight.GetComponentInChildren<Text>();
        HidePrompter();
    }


    private void ClearPrompter()
    {
        buttonRight.onClick.RemoveAllListeners();
        buttonRight.onClick.AddListener(HidePrompter);
        buttonLeft.onClick.RemoveAllListeners();
        buttonLeft.onClick.AddListener(HidePrompter);

    }


    public void DisplayPrompter(string titleText, string contentText, Action onRightButton, string rightActionText)
    {
        wholePrompter.gameObject.SetActive(true);

        title.text = titleText;
        content.text = contentText;

        ClearPrompter();

        buttonLeft.gameObject.SetActive(true);

        if (onRightButton != null)
        {
            buttonRight.onClick.AddListener(() => onRightButton?.Invoke());
            buttonRightText.text = rightActionText;
        }
        else
        {
            buttonRightText.text = "Ok";
        }

        buttonLeft.gameObject.SetActive(false);
    }


    public void DisplayPrompter(string titleText, string contentText, Action onRightButton, string rightActionText, Action onLeftButton, string leftActionText)
    {
        wholePrompter.gameObject.SetActive(true);

        title.text = titleText;
        content.text = contentText;

        ClearPrompter();


        buttonLeft.gameObject.SetActive(true);

        if (onRightButton != null)
        {
            buttonRight.onClick.AddListener(() => onRightButton?.Invoke());
            buttonRightText.text = rightActionText;
        }
        else
        {
            buttonRightText.text = "Ok";
        }

        if (onLeftButton != null)
        {
            buttonLeft.gameObject.SetActive(true);
            buttonLeft.onClick.AddListener(() => onLeftButton?.Invoke());
            buttonLeftText.text = leftActionText;
        }
        else
        {
            buttonLeft.gameObject.SetActive(false);
        }
    }



    public void HidePrompter()
    {
        wholePrompter.gameObject.SetActive(false);
    }

}

