using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameSettings : MonoBehaviour {
    public AudioMixer audioMixer;
    public TextMeshProUGUI volumeText;
    public TMP_Dropdown resolutionDropdown;

    private AudioSource[] audioSourcesList;
    private Resolution[] resolutions;

    public VolumeProfile hdrpVolume;// Need to assign this at runtime in the future...(same player, also same helper systems?)

    public TextMeshProUGUI bloomText, intensityText;

    public MouseLook cameraScript;

    void Start() {
        FindAllAudioSources();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
    }

    /* 
     * FindAllAudioSources() adds every Active audiosource in the scene and sets mixer to master
     * Will not work for audioSources that are not active. (Play on start, lights)
     */
    private void FindAllAudioSources() {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        audioSourcesList = audioSources;

        foreach (AudioSource audioSource in audioSourcesList) {
            //Debug.Log(audioSource.name);
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Test")[0];
        }
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", volume);
        volumeText.text = volume.ToString();
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBloom(float intensity) {
        if (!hdrpVolume.TryGet<Bloom>(out var bloom)) {
            bloom = hdrpVolume.Add<Bloom>(false);
        }
        bloomText.text = intensity.ToString();
        bloom.intensity.overrideState = true;
        bloom.intensity.value = intensity;
    }

    public void SetSensitivity(float senseValue) {
        cameraScript.mouseSensitivity = senseValue;
        intensityText.text = senseValue.ToString();
    }
}
