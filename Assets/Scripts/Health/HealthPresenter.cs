using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Entities;
using UnityEngine.UI;


public class HealthPresenter : MonoBehaviour
{
	[SerializeField] private Slider _healthSlider;
	//[SerializeField] private TextMeshProUGUI _healthText;

	public void UpdateView(int currentHealth, int maxHealth)
	{
		//if (_healthSlider != null && _healthText != null)
		//{
		//	_healthSlider.maxValue = maxHealth;
		//	_healthSlider.value = currentHealth;
		//	_healthText.text = $"HP: {currentHealth}";
		//}
		if (_healthSlider != null )
		{
			_healthSlider.maxValue = maxHealth;
			_healthSlider.value = currentHealth;
		}
	}
}

