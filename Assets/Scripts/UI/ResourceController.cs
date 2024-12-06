using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _woodText;
	[SerializeField] private TextMeshProUGUI _fireText;
	[SerializeField] private TextMeshProUGUI _waterText;
	[SerializeField] private TextMeshProUGUI _earthText;
	[SerializeField] private TextMeshProUGUI _metalText;

	public static ResourceController Singleton;
	void Start()
    {
        Singleton = this;
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
