using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private Transform starts3;
    [SerializeField] private Transform starts2;
    [SerializeField] private Transform starts1;
    [SerializeField] private Transform starts0;
    [SerializeField] private Text main;
    [SerializeField] private Text subText;




    public void FillButton(LevelInfo level, Action action)
    {
        button.onClick.AddListener(() => action?.Invoke());
        iconImage.sprite = level.levelIcon != null ? level.levelIcon : iconImage.sprite;

        starts3.gameObject.SetActive(level.levelRecord.shootsUsed == 1);
        starts2.gameObject.SetActive(level.levelRecord.shootsUsed == 2);
        starts1.gameObject.SetActive(level.levelRecord.shootsUsed == 3);
        starts0.gameObject.SetActive(level.levelRecord.shootsUsed > 3);

        main.text = level.name;
        subText.text = "Last completed " + level.levelRecord.dateOfCompletion;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
