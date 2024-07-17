using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class VolumeAudioManager : MonoBehaviour
{
    public AudioMixer masterVolumeMixer;

    [Header("Master")]
    public Slider masterSlider;
    public TMP_Text masterText;

    [Header("BGM")]
    public Slider bgmSlider;
    public TMP_Text bgmText;

    [Header("SFX")]
    public Slider sfxSlider;
    public TMP_Text sfxText;


    [Header("Button")]
    public Button applyButton; //SaveButton
    public Button resetButton;

    private void Start()
    {
        // Load saved settings or default to 100
        masterSlider.value = PlayerPrefsManager.Load("Master", 100f);
        bgmSlider.value = PlayerPrefsManager.Load("BGM", 100f);
        sfxSlider.value = PlayerPrefsManager.Load("SFX", 100f);

        // Apply loaded settings
        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();
    }
    public void SaveSettings()
    {
        PlayerPrefsManager.Save("Master", masterSlider.value);
        PlayerPrefsManager.Save("BGM", bgmSlider.value);
        PlayerPrefsManager.Save("SFX", sfxSlider.value);

        Debug.Log("Settings saved.");
    }
    public void ResetSettings()
    {
        masterSlider.value = 100f;
        bgmSlider.value = 100f;
        sfxSlider.value = 100f;

        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();

        Debug.Log("Settings reset to default.");
    }
    public void SetMasterVolume()
    {
        float volume = Mathf.Log10(masterSlider.value / 100f) * 20f;
        masterVolumeMixer.SetFloat("Master", volume);
        PlayerPrefsManager.Save("Master", masterSlider.value);

        int displayVolume = Mathf.RoundToInt(masterSlider.value);
        masterText.text = displayVolume.ToString();
    }
    public void SetBGMVolume()
    {
        float volume = Mathf.Log10(bgmSlider.value / 100f) * 20f;
        masterVolumeMixer.SetFloat("BGM", volume);
        PlayerPrefsManager.Save("BGM", bgmSlider.value);

        int displayVolume = Mathf.RoundToInt(bgmSlider.value);
        bgmText.text = displayVolume.ToString();
    }
    public void SetSFXVolume()
    {
        float volume = Mathf.Log10(sfxSlider.value / 100f) * 20f;
        masterVolumeMixer.SetFloat("SFX", volume);
        PlayerPrefsManager.Save("SFX", sfxSlider.value);

        int displayVolume = Mathf.RoundToInt(sfxSlider.value);
        sfxText.text = displayVolume.ToString();
    }
}
