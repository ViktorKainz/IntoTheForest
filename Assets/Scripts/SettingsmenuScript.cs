using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;


public class SettingsmenuScript : MonoBehaviour
{
	//public AudioMixer audioMixer;
	public TMPro.TMP_Dropdown resolutionDropdown;
	public TMPro.TMP_Dropdown qualityDropdown;
	public TMPro.TMP_Dropdown textureDropdown;
	public TMPro.TMP_Dropdown aaDropdown;
	public Slider volumeSlider;
	public Toggle fullScreen;
	float currentVolume;
	Resolution[] resolutions;

    void Start()
    {
	    //fullScreen.isOn = false;
	    
        resolutionDropdown.ClearOptions();
        qualityDropdown.ClearOptions();
        textureDropdown.ClearOptions();
        aaDropdown.ClearOptions();
        List<string> resOptions = new List<string>();
		resolutions = Screen.resolutions;
		Array.Reverse(resolutions);
		int currentResolutionIndex = 0;

		for(int i = 0; i < resolutions.Length; i++){
			string option = resolutions[i].width + " x " + resolutions[i].height;
			resOptions.Add(option);
			if (resolutions[i].width == Screen.currentResolution.width &&
			    resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
			
		}
		
		List<string> qualityOptions = new List<string>();
		qualityOptions.Add("Very Low");
		qualityOptions.Add("Low");
		qualityOptions.Add("Medium");
		qualityOptions.Add("High");
		qualityOptions.Add("Very High");
		
		List<string> textureOptions = new List<string>();
		textureOptions.Add("Very Low");
		textureOptions.Add("Low");
		textureOptions.Add("Medium");
		textureOptions.Add("High");
		textureOptions.Add("Very High");

		List<string> aaOptions = new List<string>();
		aaOptions.Add("Off");
		aaOptions.Add("2x");
		aaOptions.Add("4x");
		aaOptions.Add("8x");

		resolutionDropdown.AddOptions(resOptions);
		resolutionDropdown.RefreshShownValue();
		qualityDropdown.AddOptions(qualityOptions);
		qualityDropdown.RefreshShownValue();
		textureDropdown.AddOptions(textureOptions);
		textureDropdown.RefreshShownValue();
		aaDropdown.AddOptions(aaOptions);
		aaDropdown.RefreshShownValue();
		
		
		resolutionDropdown.GetComponent<GameObject>().layer= 5;
		
		
		

		resolutionDropdown.onValueChanged.AddListener(delegate
		{
			SetResolution();
		});
		
		qualityDropdown.onValueChanged.AddListener(delegate
		{
			SetQuality();
		});
		
		textureDropdown.onValueChanged.AddListener(delegate
		{
			SetTextureQuality();
		});
		
		aaDropdown.onValueChanged.AddListener(delegate
		{
			SetAntiAliasing();
		});
		
		LoadSettings();
		qualityDropdown.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SetVolume(float volume){
		//audioMixer.SetFloat("Volume",volume);
		currentVolume = volume;
	}

	public void SetFullscreen(bool isFullscreen){
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution()
	{
		int index = resolutionDropdown.value;
		Resolution resolution = resolutions[index];
		Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
		SaveSettings();
	}

	public void SetTextureQuality(){
		QualitySettings.masterTextureLimit = qualityDropdown.value;
		SaveSettings();
		//qualityDropdown.value = 6;
	}

	public void toggleFullScreen(bool fullscreen)
	{
		
	}

	public void SetAntiAliasing(){
		switch (aaDropdown.value)
		{
			case 1: QualitySettings.antiAliasing = 2;
				break;
			case 2: QualitySettings.antiAliasing = 4;
				break;
			case 3: QualitySettings.antiAliasing = 8;
				break;
			default: QualitySettings.antiAliasing = 0;
				break;
		}
		SaveSettings();
	}

	public void SetQuality(){
		QualitySettings.masterTextureLimit = qualityDropdown.value;
		SaveSettings();
	}

	public void SaveSettings(){
		PlayerPrefs.SetInt("QualitySettingsPreference",qualityDropdown.value);
		PlayerPrefs.SetInt("ResolutionPreference",resolutionDropdown.value);
		PlayerPrefs.SetInt("TextureQualityPreference",textureDropdown.value);
		PlayerPrefs.SetInt("AntiAliasingPreference",aaDropdown.value);
		PlayerPrefs.SetInt("FullscreenPreference",Convert.ToInt32(Screen.fullScreen));
		PlayerPrefs.SetFloat("VolumePreference",currentVolume);
	}

	public void LoadSettings(){
		if(PlayerPrefs.HasKey("QualitySettingsPreference")){
			qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingsPreference");
		}

		if(PlayerPrefs.HasKey("ResolutionPreference")){
			resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
		}

		if(PlayerPrefs.HasKey("TextureQualityPreference")){
			textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
		}

		if(PlayerPrefs.HasKey("AntiAliasingPreference")){
			aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
		}

		if(PlayerPrefs.HasKey("FullscreenPreference")){
			Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
		}

		if(PlayerPrefs.HasKey("VolumePreference")){
			volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
		}

		SaveSettings();
	}
}
