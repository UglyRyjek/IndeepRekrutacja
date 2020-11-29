using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UILevelSelection : MonoBehaviour
{
    [SerializeField] private LevelButton levelButtonPrefab;

 

    private void Start()
    {
        LoadLevelList();
    }


    private void LoadLevelList() // po szybkości przez instantiate
    {
        levelButtonPrefab.gameObject.SetActive(false);
        foreach (LevelInfo li in DataBase.DB.GetLevelsData())
        {
            if(li != null && li.scene != null)
            {
                LevelButton lb = Instantiate(levelButtonPrefab, levelButtonPrefab.transform.parent) as LevelButton;
                lb.gameObject.SetActive(true);
                lb.FillButton(li, () => LoadLevel(li));

            }
        }
    }


    private void LoadLevel(LevelInfo level)
    {
        DataBase.DB?.LoadLevelScene(level);
    }

}
