using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class VolumSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;


    [SerializeField] private TMP_Text musicPercentText;
    [SerializeField] private TMP_Text sfxPercentText;


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

    }
    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        UpdateSFXPercent(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        UpdateMusicPercent(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
    private void UpdateMusicPercent(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        musicPercentText.text = percent + "%";
    }

    private void UpdateSFXPercent(float value)
    {
        int percent = Mathf.RoundToInt(value * 100f);
        sfxPercentText.text = percent + "%";
    }

}
