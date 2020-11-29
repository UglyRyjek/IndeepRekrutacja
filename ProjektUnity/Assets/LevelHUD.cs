using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelHUD : MonoBehaviour
{
    [SerializeField] private Transform wholeHUD;

    [SerializeField] private Transform upperSubpane;
    [SerializeField] private Button showUpperSupbanel;
    [SerializeField] private Button hideUpperSubpanel;
    [SerializeField] private Button exitToMenu;

    [SerializeField] private Button subpanel_volume_mute;
    [SerializeField] private Button subpanel_volume_unmute;

    [SerializeField] private Button subpanel_options;
    [SerializeField] private Button subpanel_restart;

    [SerializeField] private UISimplePrompter simplePrompter;

    [SerializeField] private Image levelProgresBackground;
    [SerializeField] private Image levelProgresFill;



    private LevelManager levelManager;


    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelProgresBackground.gameObject.SetActive(levelManager != null);
        levelProgresFill.gameObject.SetActive(levelManager != null);
        levelProgresFill.fillAmount = 0f;

        showUpperSupbanel.onClick.AddListener(ShowUpperPanel);
        hideUpperSubpanel.onClick.AddListener(HideUpperPanel);
        subpanel_volume_mute.onClick.AddListener(OnSupbanelVolume);
        subpanel_volume_unmute.onClick.AddListener(OnSupbanelVolume);
        subpanel_options.onClick.AddListener(OnSubpanelOptions);
        subpanel_restart.onClick.AddListener(OnSubpanelRestart);
        exitToMenu.onClick.AddListener(OnExitToMenu);
        HideUpperPanel();
    }


    private void Update()
    {
        HandleLevelProgressBar();
    }


    private void ShowUpperPanel()
    {
        upperSubpane.gameObject.SetActive(true);
        showUpperSupbanel.gameObject.SetActive(false);
        hideUpperSubpanel.gameObject.SetActive(true);
    }


    private void HideUpperPanel()
    {
        upperSubpane.gameObject.SetActive(false);
        showUpperSupbanel.gameObject.SetActive(true);
        hideUpperSubpanel.gameObject.SetActive(false);
    }


    private void OnSubpanelRestart()
    {
        simplePrompter.DisplayPrompter("RESTART", "Restart level?", RestartLevel, "Yes");
    }


    private void OnExitToMenu()
    {
        simplePrompter.DisplayPrompter("EXIT", "Wanna quit to level selection?", GoToMainMenu, "Sure", DoNothing, "Nope");
    }


    private void RestartLevel()
    {
        DataBase.DB?.ReloadLevel();
        //int loadedScenes = SceneManager.sceneCount;
        //SceneManager.GetAllScenes();
        //SceneManager.
    }


    private void GoToMainMenu()
    {
        DataBase.DB.LoadMainMenu();
    }


    private void DoNothing()
    {

    }


    private void OnSubpanelOptions()
    {
        simplePrompter.DisplayPrompter("OPTIONS", "lore ipsum?", null, "");
    }


    private void OnSupbanelVolume()
    {
        bool state = subpanel_volume_unmute.gameObject.activeInHierarchy;

        AudioListener.volume = state ? 0f : 1f;
        subpanel_volume_mute.gameObject.SetActive(state);
        subpanel_volume_unmute.gameObject.SetActive(!state);
    }


    private void HandleLevelProgressBar()
    {
        if (levelManager)
        {
            levelProgresFill.fillAmount = Mathf.MoveTowards(levelProgresFill.fillAmount, levelManager.GetLevelProgressionNormalized(), 3f * Time.deltaTime);
        }
    }
}
