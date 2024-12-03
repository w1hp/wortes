using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _timerText;

	[SerializeField] private GameObject bossHealthPanel;
	[SerializeField] private GameObject container;


	public static CountdownController Singleton;
    void Start()
    {
        Singleton = this;
    }

	public void UpdateTimer((float minutes, float seconds) time)
	{
		_timerText.text = string.Format("{0:00}:{1:00}", time.minutes, time.seconds);
	}
	public void UpdateBossHealthPanel(bool showBossHealthBar)
	{
		bossHealthPanel.SetActive(showBossHealthBar);
		container.SetActive(!showBossHealthBar);
	}
}
