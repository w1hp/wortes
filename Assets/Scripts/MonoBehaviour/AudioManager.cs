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
        if (AudioMixer == null)
        {
            Debug.LogError("AudioMixer nie jest przypisany w AudioManager!");
            return;
        }

        volume = Mathf.Clamp(volume, 0.0001f, 1f); // Upewnij sie, ze volume nie jest 0 ani >1

        if (volume <= 0.0001f)
        {
            AudioMixer.SetFloat(name, -80f); // Calkowita cisza
        }
        else
        {
            float mappedVolume = Mathf.Log10(volume) * 20; // Przeksztalcenie 0-1 na decybele
            AudioMixer.SetFloat(name, mappedVolume);
        }

float result;
bool exists = AudioMixer.GetFloat(name, out result);
Debug.Log($"Ustawiono {name} na {volume} -> {(exists ? result : -80)} dB");

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
    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (AudioSource == null)
        {
            Debug.LogError("AudioSource nie jest przypisany w AudioManager!");
            return;
        }

        if (AudioSource.clip == null)
        {
            Debug.LogError("Brak przypisanego AudioClip do AudioSource!");
            return;
        }

        AudioSource.loop = true; // Powtarzanie muzyki w petli
        AudioSource.Play();
        Debug.Log("Muzyka zostala uruchomiona: " + AudioSource.clip.name);
    }






}
