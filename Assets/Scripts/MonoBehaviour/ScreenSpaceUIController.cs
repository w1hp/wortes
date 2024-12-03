using System;
using System.Collections;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class ScreenSpaceUIController : MonoBehaviour
{
	public static ScreenSpaceUIController Singleton;

	void Start()
	{
		Singleton = this;
	}

	private void Update()
	{
		
	}
}