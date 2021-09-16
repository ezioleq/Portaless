using System;
using UnityEngine;

public class Screenshooter : MonoBehaviour {
	public KeyCode shotKey = KeyCode.F2;
	public int superSize = 2;

	void Start() {
		
	}

	void Update() {
		if (Input.GetKeyDown(shotKey)) {
			string filename = Application.dataPath + "/Screenshots/" + DateTime.Now;
			ScreenCapture.CaptureScreenshot(filename, superSize);
			Debug.Log("Saved screenshot to " + filename);
		}
	}
}
