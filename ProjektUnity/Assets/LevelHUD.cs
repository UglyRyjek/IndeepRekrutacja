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
    [SerializeField] private Button subpanel_volume_mute;
    [SerializeField] private Button subpanel_volume_unmute;

    [SerializeField] private Button subpanel_options;
    [SerializeField] private Button subpanel_restart;

    [SerializeField] private UISimplePrompter simplePrompter;





    private void Start()
    {
        showUpperSupbanel.onClick.AddListener(ShowUpperPanel);
        hideUpperSubpanel.onClick.AddListener(HideUpperPanel);
        subpanel_volume_mute.onClick.AddListener(OnSupbanelVolume);
        subpanel_volume_unmute.onClick.AddListener(OnSupbanelVolume);
        subpanel_options.onClick.AddListener(OnSubpanelOptions);
        subpanel_restart.onClick.AddListener(OnSubpanelRestart);

        HideUpperPanel();
    }


    private void Update()
    {

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


    private void RestartLevel()
    {
        DataBase.DB?.ReloadLevel();
        //int loadedScenes = SceneManager.sceneCount;
        //SceneManager.GetAllScenes();
        //SceneManager.
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
}
