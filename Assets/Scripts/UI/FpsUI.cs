using System;
using TMPro;
using UnityEngine;

namespace Portaless.UI
{
	public class FpsUI : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI fpsCounterText;
		[SerializeField] private float fpsRefreshRate = 1f;
		private float fpsCounter;
		private float timer;

		[SerializeField] private KeyCode toggleKey = KeyCode.F3;
		private string showFpsStateKey = "show_fps";

		private void Awake() {
			// Allow only one instance by destroying another ones
			DontDestroyOnLoad(this.gameObject);
			if (FindObjectsByType(GetType(), FindObjectsSortMode.None).Length > 1)
				Destroy(this.gameObject);

			LoadState();
		}

		private void Update() {
			if (Input.GetKeyDown(toggleKey))
				ToggleFPS();

			if (Time.time > timer) {
				fpsCounter = (int)(1f / Time.unscaledDeltaTime);
				fpsCounterText.text = fpsCounter + " FPS";
				timer += fpsRefreshRate;
			}
		}

		private void ToggleFPS() {
			SaveState();
			LoadState();
		}

		private void SaveState() {
			PlayerPrefs.SetInt(
				showFpsStateKey,
				Convert.ToInt32(!fpsCounterText.gameObject.activeSelf)
			);
		}

		private void LoadState() {
			fpsCounterText.gameObject.SetActive(
				Convert.ToBoolean(PlayerPrefs.GetInt(showFpsStateKey, 0))
			);
		}
	}
}
