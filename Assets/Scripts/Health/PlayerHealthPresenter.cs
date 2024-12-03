using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthPresenter : HealthPresenter
{
	public static PlayerHealthPresenter Singleton;


	void Start()
	{
		Singleton = this;
	}
}
