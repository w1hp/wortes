using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public enum AudioType
{
	Music,
	Sfx
}

[Serializable]
public struct AudioClipWithType
{
	[SerializeField] public AudioType Type;
	[SerializeField] public AudioClip Clip;
}
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	public float MaxDistance = 50f;
	public int MaxAudioSources = 10;
	public int ClosestEmitterPerClipCount = 3;

	public AudioMixerGroup Main;
	public AudioMixerGroup Music;
	public AudioMixerGroup Sfx;

	public AudioMixer AudioMixer;

	public AnimationCurve VolumeCurve;
	public AnimationCurve PitchCurve;

	public AudioClip LoopClip;
	public AudioClip MaxValClip;

	public AudioSource AudioSource;

	public bool ShowDebugLines;

	public AudioClipWithType[] AudioClips;





	//private readonly Dictionary<Entity, AudioReference> _audioReferences = new();




	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
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
		AudioMixer.SetFloat($"{name}", volume);
	}

	//public void UpdatePitchAndVolume(Entity entity, float velocity)
	//{
	//	if (_audioReferences.ContainsKey(entity) &&
	//		_audioReferences[entity].AudioSource.clip != _audioReferences[entity].AudioClip)
	//	{
	//		if (velocity >= 1f &&
	//			_audioReferences[entity].AudioSource.clip != MaxValClip)
	//		{
	//			ChangeAudio(_audioReferences[entity].AudioSource, MaxValClip);
	//		}

	//		else if (velocity < 1f &&
	//			_audioReferences[entity].AudioSource.clip != LoopClip)
	//		{
	//			ChangeAudio(_audioReferences[entity].AudioSource, LoopClip);
	//		}

	//		_audioReferences[entity].AudioSource.pitch = PitchCurve.Evaluate(velocity);
	//		_audioReferences[entity].AudioSource.volume = VolumeCurve.Evaluate(velocity);
	//	}
	//}
	private void ChangeAudio(AudioSource audioSource, AudioClip clip)
	{
		if (audioSource.clip != clip)
		{
			audioSource.clip = clip;
			audioSource.Play();
		}
	}
	//private struct AudioReference
	//{
	//	public GameObject GameObject;
	//	public AudioSource AudioSource;
	//	public AudioClip AudioClip;
	//}
}
