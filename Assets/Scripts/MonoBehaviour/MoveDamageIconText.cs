using System.Collections;
using TMPro;
using UnityEngine;

public class MoveDamageIconText : MonoBehaviour
{
	[SerializeField] private float _moveDuration;
	[SerializeField] private float _moveHeight;

	private TextMeshProUGUI _text;

	private float _timer;
	private Vector3 _startPosition;
	private Vector3 _endPosition;

	private void Start()
	{
		_text = GetComponent<TextMeshProUGUI>();
		_startPosition = transform.position;
		_endPosition = _startPosition + Vector3.up * _moveHeight;
		StartCoroutine(MoveDamageText());
	}

	private IEnumerator MoveDamageText()
	{
		while (_timer < _moveDuration)
		{
			var t = EaseOutCubic(_timer, _moveDuration);

			transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
			_timer += Time.deltaTime;
			yield return null;
		}

		_timer = 0f;
		while (_timer < 0.5f)
		{
			var color = _text.color;
			var t = _timer / 0.5f;
			color.a = Mathf.Lerp(1, 0, t);
			_text.color = color;
			_timer += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}

	private static float EaseOutCubic(float time, float duration)
	{
		time /= duration;
		time--;
		return time * time * time + 1;
	}
}
