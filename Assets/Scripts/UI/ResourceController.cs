using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _metalAmount;
	[SerializeField] private TextMeshProUGUI _fireAmount;
	[SerializeField] private TextMeshProUGUI _waterAmount;
	[SerializeField] private TextMeshProUGUI _earthAmount;
	[SerializeField] private TextMeshProUGUI _woodAmount;

	[SerializeField] private TextMeshProUGUI _metalTowerCost;
	[SerializeField] private TextMeshProUGUI _fireTowerCost;
	[SerializeField] private TextMeshProUGUI _waterTowerCost;
	[SerializeField] private TextMeshProUGUI _earthTowerCost;
	[SerializeField] private TextMeshProUGUI _woodTowerCost;

	[SerializeField] private Slider _metalReloadSlider;
	//[SerializeField] private Slider _fireReloadSlider;
	//[SerializeField] private Slider _waterReloadSlider;
	//[SerializeField] private Slider _earthReloadSlider;
	//[SerializeField] private Slider _woodReloadSlider;

	public static ResourceController Singleton;
	void Start()
    {
        Singleton = this;
	}

	public void UpdateReloadSlider(float value)
	{
		_metalReloadSlider.value = value;
		//_fireReloadSlider.value = 0;
		//_waterReloadSlider.value = 0;
		//_earthReloadSlider.value = 0;
		//_woodReloadSlider.value = 0;
	}

	public void UpdateResourceText(float metal, float fire, float water, float earth, float wood)
	{
		_metalAmount.text = FormatResourceText(metal);
		_fireAmount.text = FormatResourceText(fire);
		_waterAmount.text = FormatResourceText(water);
		_earthAmount.text = FormatResourceText(earth);
		_woodAmount.text = FormatResourceText(wood);
	}

	private string FormatResourceText(float value)
	{
		if (value >= 1000000)
			return $"{value / 1000000f:0.#}M";
		if (value >= 1000)
			return $"{value / 1000f:0.#}k";
		return value.ToString();
	}

	public void UpdateTowerCostText(float metal, float fire, float water, float earth, float wood)
	{
		_metalTowerCost.text = $"/ {metal}";
		_fireTowerCost.text = $"/ {fire}";
		_waterTowerCost.text = $"/ {water}";
		_earthTowerCost.text = $"/ {earth}";
		_woodTowerCost.text = $"/ {wood}";
	}
}
