using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{

	public float spawnEffectTime = 1;
	public float pause = 2;
	public AnimationCurve fadeIn;

	ParticleSystem ps;
	float timer = 0;
	Renderer _renderer;

	int shaderProperty;

	void Start()
	{
		shaderProperty = Shader.PropertyToID("_cutoff");
		_renderer = GetComponent<Renderer>();
		ps = GetComponentInChildren<ParticleSystem>();

		var main = ps.main;
		main.duration = spawnEffectTime;

		ps.Play();

	}

	void Update()
	{
		if (timer < spawnEffectTime + pause)
		{
			timer += Time.deltaTime;
			_renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
		}
		else
		{
			Destroy(gameObject);
			//ps.Play();
			//timer = 0;
		}
	}
}
