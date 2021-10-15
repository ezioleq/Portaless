using UnityEngine;
using TMPro;
public class FpsUI : MonoBehaviour {
    public TextMeshProUGUI fpsCounterText;
    float fpsCounter;
    float refreshTime = 0.5f;
    void Update() {
        fpsCounter = (int)(1f / Time.unscaledDeltaTime);
        if (Time.time > refreshTime) {
            fpsCounterText.text = fpsCounter.ToString();
            refreshTime += 0.5f;
        }
        
        
    }
}