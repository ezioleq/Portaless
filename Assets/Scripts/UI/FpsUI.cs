using UnityEngine;
using TMPro;
public class FpsUI : MonoBehaviour {
    public TextMeshProUGUI fpsCounterText;
    [SerializeField] private float fpsRefreshRate = 0.5f; // defines how often fps will be updated on HUD ;
    private float fpsCounter;
    private float timer;
 
    void Update() {
        if (Time.time > timer) {
            fpsCounter = (int) (1f / Time.unscaledDeltaTime);
            fpsCounterText.text = fpsCounter.ToString() + " FPS";
            timer += fpsRefreshRate;
        }
    }
}
