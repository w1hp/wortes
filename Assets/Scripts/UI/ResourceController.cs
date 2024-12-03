using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
	[SerializeField] private Text _woodText;
	[SerializeField] private Text _fireText;
	[SerializeField] private Text _waterText;
	[SerializeField] private Text _earthText;
	[SerializeField] private Text _metalText;

	public static ResourceController Singleton;
	void Start()
    {
        Singleton = this;
	}

    
    void Update()
    {
        
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
