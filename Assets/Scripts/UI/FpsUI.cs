using System;
using Portaless.Input;
using TMPro;
using UnityEngine;

namespace Portaless.UI
{
	public class FpsUI : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI fpsCounterText;
		[SerializeField] private float fpsRefreshRate = 1f;
		private float fpsCounter;
		private float timer;

		private const string ShowFpsStateKey = "show_fps";

		private void Awake() {
			// Allow only one instance by destroying another ones
			DontDestroyOnLoad(this.gameObject);
			if (FindObjectsByType(GetType(), FindObjectsSortMode.None).Length > 1)
				Destroy(this.gameObject);

			LoadState();

		}

		private void Start()
		{
			InputManager.Instance.Actions.Gameplay.TogglePerformanceOverlay.performed += _ => ToggleFPS();
		}

		private void Update() {
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
				ShowFpsStateKey,
				Convert.ToInt32(!fpsCounterText.gameObject.activeSelf)
			);
		}

		private void LoadState() {
			fpsCounterText.gameObject.SetActive(
				Convert.ToBoolean(PlayerPrefs.GetInt(ShowFpsStateKey, 0))
			);
		}
	}
}
