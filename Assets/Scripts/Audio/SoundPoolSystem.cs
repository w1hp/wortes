using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct AudioSorurceComponent : IComponentData
{
	//public int SoundType;
	public AudioType Type;
}

public partial class SoundPoolSystem : SystemBase
{
	private AudioManager _audioManager;
	private AudioSource[] _audioSources;
	private Transform _mainCameraTransform;

	private NativeArray<float> _closestEntitiesSqDistance;
	private NativeArray<Entity> _closestEntities;
	private NativeArray<float3> _closestEntitiesPositions;
	private (Entity entity, AudioSource source)[] _entitiesWithActiveSounds;


	protected override void OnUpdate()
	{
		Initialize();

		if (_mainCameraTransform == null)
			_mainCameraTransform = Camera.main.transform;

		// Step 1: Find closest entities
		var closestEntitiesSqDistance = _closestEntitiesSqDistance;
		var closestEntities = _closestEntities;
		var closestEntitiesPositions = _closestEntitiesPositions;

		var cameraPosition = _mainCameraTransform.position;

		for (int i = 0; i < _audioManager.MaxAudioSources; i++)
			_closestEntitiesSqDistance[i] = float.MaxValue;

		float maxSqlDistInClosestSet = float.MaxValue;
		int maxSqlDistInClosestSetIndex = 0;
		float maxDistanceSq = _audioManager.MaxDistance * _audioManager.MaxDistance;

		foreach (var (transform, audioSource, entity) in SystemAPI.Query<LocalToWorld, AudioSorurceComponent>().WithEntityAccess())
		{
			float sqDist = math.distancesq(transform.Position, cameraPosition);

			if (sqDist < maxDistanceSq && sqDist < maxSqlDistInClosestSet)
			{
				closestEntitiesSqDistance[maxSqlDistInClosestSetIndex] = sqDist;
				closestEntities[maxSqlDistInClosestSetIndex] = entity;
				closestEntitiesPositions[maxSqlDistInClosestSetIndex] = transform.Position;

				for (int i = 0; i < closestEntitiesSqDistance.Length; i++)
				{
					if (_closestEntitiesSqDistance[i] > maxSqlDistInClosestSet)
					{
						maxSqlDistInClosestSet = _closestEntitiesSqDistance[i];
						maxSqlDistInClosestSetIndex = i;
					}
				}
			}
		}

		// Step 2: Reorder currently playing sound sources
		for (int i = 0; i < _audioManager.MaxAudioSources; i++)
		{
			if (_closestEntitiesSqDistance[i] == float.MaxValue)
				break;

			if (_closestEntities[i] == _entitiesWithActiveSounds[i].entity)
				continue;

			for (int j = 0; j < _audioManager.MaxAudioSources; j++)
			{
				if (_closestEntities[i] == _entitiesWithActiveSounds[j].entity)
					(_entitiesWithActiveSounds[i], _entitiesWithActiveSounds[j]) = (_entitiesWithActiveSounds[j], _entitiesWithActiveSounds[i]);
			}
		}

		// Step 3: Update sound sources positions
		for (int i = 0; i < _audioManager.MaxAudioSources; i++)
		{
			if (_closestEntitiesSqDistance[i] == float.MaxValue)
			{
				if (_entitiesWithActiveSounds[i].source != null)
					_entitiesWithActiveSounds[i].source.Stop();
			}
			else
			{
				if (_closestEntities[i] != _entitiesWithActiveSounds[i].entity)
				{
					//int soundType = SystemAPI.GetComponent<AudioSorurceComponent>(_closestEntities[i]).SoundType;
					AudioType type = SystemAPI.GetComponent<AudioSorurceComponent>(_closestEntities[i]).Type;
					_entitiesWithActiveSounds[i].entity = _closestEntities[i];
					//_entitiesWithActiveSounds[i].source.clip = _audioManager.AudioClips[soundType];
					foreach (var audioClipWithType in _audioManager.AudioClips)
					{
						if (audioClipWithType.Type == type)
						{
							_entitiesWithActiveSounds[i].source.clip = audioClipWithType.Clip;
							break;
						}
					}

					_entitiesWithActiveSounds[i].source.Play();
				}

				_entitiesWithActiveSounds[i].source.transform.position = _closestEntitiesPositions[i];
			}
		}

		if (_audioManager.ShowDebugLines)
		{
			for (int i = 0; i < _audioManager.MaxAudioSources; i++)
			{
				var source = _entitiesWithActiveSounds[i].source;

				if (_entitiesWithActiveSounds[i].source.isPlaying)
				{
					Debug.DrawLine(cameraPosition, source.transform.position, Color.red);
				}
			}
		}


	}

	protected override void OnDestroy()
	{
		if (_closestEntitiesSqDistance.IsCreated)
			_closestEntitiesSqDistance.Dispose();
		if (_closestEntities.IsCreated)
			_closestEntities.Dispose();
		if (_closestEntitiesPositions.IsCreated)
			_closestEntitiesPositions.Dispose();
	}

	private void Initialize()
	{
		if (_audioManager == null)
		{
			_audioManager = AudioManager.Instance;

			_closestEntitiesSqDistance = new NativeArray<float>(_audioManager.MaxAudioSources, Allocator.Persistent);
			_closestEntities = new NativeArray<Entity>(_audioManager.MaxAudioSources, Allocator.Persistent);
			_closestEntitiesPositions = new NativeArray<float3>(_audioManager.MaxAudioSources, Allocator.Persistent);
			_entitiesWithActiveSounds = new (Entity entity, AudioSource source)[_audioManager.MaxAudioSources];

			// Create audio pool
			for (int i = 0; i < _audioManager.MaxAudioSources; i++)
			{
				var audioSource = new GameObject("AudioSource" + i).AddComponent<AudioSource>();
				audioSource.maxDistance = _audioManager.MaxDistance;
				audioSource.outputAudioMixerGroup = _audioManager.Sfx;

				_entitiesWithActiveSounds[i].source = audioSource;
			}
		}
	}
}