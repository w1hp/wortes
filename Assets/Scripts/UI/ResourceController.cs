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

	[SerializeField] private TextMeshProUGUI _selectedTowerCost;

	[SerializeField] private RectTransform _metalTransform;
	[SerializeField] private RectTransform _fireTransform;
	[SerializeField] private RectTransform _waterTransform;
	[SerializeField] private RectTransform _earthTransform;
	[SerializeField] private RectTransform _woodTransform;
	private RectTransform _lastSelectedElementTransform;

	[SerializeField] private Slider _metalReloadSlider;
	//[SerializeField] private Slider _fireReloadSlider;
	//[SerializeField] private Slider _waterReloadSlider;
	//[SerializeField] private Slider _earthReloadSlider;
	//[SerializeField] private Slider _woodReloadSlider;

	public static ResourceController Singleton;
	void Start()
    {
        Singleton = this;
		_lastSelectedElementTransform = _metalTransform;
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

	public void UpdateTowerCostText(float cost)
	{
		_selectedTowerCost.text = $"- {cost}";
	}
	public void UpdateSelectedTower(ElementType type)
	{
		_lastSelectedElementTransform.anchoredPosition -= new Vector2(0, 20);
		switch (type)
		{
			case ElementType.Metal:
				_lastSelectedElementTransform = _metalTransform;
				break;
			case ElementType.Fire:
				_lastSelectedElementTransform = _fireTransform;
				break;
			case ElementType.Water:
				_lastSelectedElementTransform = _waterTransform;
				break;
			case ElementType.Earth:
				_lastSelectedElementTransform = _earthTransform;
				break;
			case ElementType.Wood:
				_lastSelectedElementTransform = _woodTransform;
				break;
		}
		_lastSelectedElementTransform.anchoredPosition += new Vector2(0, 20);
	}
}
