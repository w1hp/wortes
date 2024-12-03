using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthPresenter : HealthPresenter
{
	public static BossHealthPresenter Singleton;


	void Start()
	{
		Singleton = this;
	}

}
