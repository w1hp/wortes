using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _woodText;
	[SerializeField] private TextMeshProUGUI _fireText;
	[SerializeField] private TextMeshProUGUI _waterText;
	[SerializeField] private TextMeshProUGUI _earthText;
	[SerializeField] private TextMeshProUGUI _metalText;

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

	public void UpdateResourceText(float wood, float fire, float water, float earth, float metal)
	{
		_woodText.text = $"{wood}";
		_fireText.text = $"{fire}";
		_waterText.text = $"{water}";
		_earthText.text = $"{earth}";
		_metalText.text = $"{metal}";
	}
}
