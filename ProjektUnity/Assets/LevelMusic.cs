using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioSource audioSource;
    private float baseVolumee = 0.15f;
    [SerializeField] private List<Soundbar> soundBars = new List<Soundbar>();

    private float CalculateCurrentVolume()
    {
        if (soundBars.Count == 0)
        {
            return 1f;
        }

        float partVolume = (1f - baseVolumee) / soundBars.Count;
        float result = baseVolumee;
        foreach (var item in soundBars)
        {
            if (item.isPlaying)
            {
                result += partVolume;
            }
        }

        return result;
    }

    private void Start()
    {
        if (soundBars.Count == 0)
        {
            soundBars = FindObjectsOfType<Soundbar>().ToList();
        }

        LevelInfo li = DataBase.DB.GetCurrentLevel();
        if (li != null)
        {
            audioSource.clip = li.music;
        }
        else
        {
            audioSource.clip = music;
        }

        
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.volume = CalculateCurrentVolume();
    }
}

