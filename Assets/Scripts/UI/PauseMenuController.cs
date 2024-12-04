using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
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

	public void SetPause(bool setPause)
	{
		_isPaused = setPause;
		Time.timeScale = setPause ? 0 : 1;
		Cursor.visible = setPause ? true : false;
		Cursor.lockState = setPause ? CursorLockMode.Confined : CursorLockMode.Locked;
#if UNITY_EDITOR
		Debug.Log($"Set pause: {setPause}");
#endif
	}
	public void SetPause(bool setPause, bool showCursor)
	{
		_isPaused = setPause;
		Time.timeScale = setPause ? 0 : 1;
		Cursor.visible = showCursor ? true : false;
		Cursor.lockState = showCursor ? CursorLockMode.Confined : CursorLockMode.Locked;
#if UNITY_EDITOR
		Debug.Log($"Set pause: {setPause}, show cursor: {showCursor}");
#endif
	}
	public void KillPlayerToExitGame()
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		var playerQuery = entityManager.CreateEntityQuery(typeof(PlayerTag));
		var playerArray = playerQuery.ToEntityArray(Allocator.Temp);
		foreach (var entity in playerArray)
		{
			entityManager.RemoveComponent<IsExistTag>(entity);
		}
		conteiner.SetActive(false);
	}

}