using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
	public AudioMixerGroup main;
	public AudioMixerGroup music;
	public AudioMixerGroup sfx;

	public AudioMixer audioMixer;

	public static AudioManager instance { get; private set; }

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void SetVolumeMaster(float volume)
	{
		SetVolume("volumeOfMaster", volume);
	}
	public void SetVolumeMusic(float volume)
	{
		SetVolume("volumeOfMusic", volume);
	}
	public void SetVolumeSFX(float volume)
	{
		SetVolume("volumeOfSFX", volume);
	}
	private void SetVolume(string name, float volume)
	{
		Math.Clamp(volume, -20f, 10f);
		audioMixer.SetFloat($"{name}", volume);
	}

}
