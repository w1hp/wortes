using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenuController : MonoBehaviour
{
	[SerializeField] private GameObject conteiner;
	private bool _isPaused = false;

	public static PauseMenuController Singleton { get; private set; }

	void Start()
	{
		Singleton = this;
		conteiner.SetActive(false);
	}
	//TODO: zmienic to kiedys na cos bardziej sensownego

	public void SetPause()
	{
		_isPaused = !_isPaused;
		conteiner.SetActive(_isPaused);
		Time.timeScale = _isPaused ? 0 : 1;
		Cursor.visible = _isPaused ? true : false;
		Cursor.lockState = _isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
#if UNITY_EDITOR
		Debug.Log($"Set pause: {_isPaused}");
#endif
	}

	public void SetPause(bool isPaused)
	{
		_isPaused = isPaused;
		Time.timeScale = isPaused ? 0 : 1;
		Cursor.visible = isPaused ? true : false;
		Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
#if UNITY_EDITOR
		Debug.Log($"Set pause: {isPaused}");
#endif
	}

}