using UnityEngine;
using TMPro;

public class FpsUI : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI _fpsCounterText;
	// Defines how often fps will be updated on HUD
	[SerializeField] private float _fpsRefreshRate = 1f;
	private float _fpsCounter;
	private float _timer;
 
	private void Update() {
		if (Time.time > _timer) {
			_fpsCounter = (int)(1f / Time.unscaledDeltaTime);
			_fpsCounterText.text = _fpsCounter.ToString() + " FPS";
			_timer += _fpsRefreshRate;
		}
	}
}
