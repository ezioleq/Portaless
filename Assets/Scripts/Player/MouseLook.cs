using UnityEngine;

public class MouseLook : MonoBehaviour {
	private new Camera camera;
	private Health health;

	[Header("General")]
	public string mouseYAxis = "Mouse Y";
	public string mouseXAxis = "Mouse X";
	public float mouseYRotation;
	
	[Header("Values")]
	[Range(1, 10)]
	public float mouseSensitivity = 4.5f;
	[Range(0, 90)]
	public float maxYRotation = 90;
	[Range(-90, 0)]
	public float minYRotation = -90;

	[Header("Zoom")]
	public float zoomingFOV = 30f;
	public float zoomSpeed = 10f;
	private float _defaultFOV;
	
	[Header("Locks")]
	public bool lockMouse;
	public bool showMouse;
	
	private void Start() {
		camera = Camera.main;
		_defaultFOV = camera.fieldOfView;
		health = GetComponent<Health>();
	}

	private void Update() {
		if (!lockMouse && health.IsAlive())
			Mouse();
		
		if (Input.GetKeyDown(KeyCode.P)) showMouse = !showMouse;

		if (Input.GetMouseButton(2))
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomingFOV, zoomSpeed * Time.deltaTime);
		else
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _defaultFOV, zoomSpeed * Time.deltaTime);

		if (showMouse){
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			lockMouse = true;
		} else {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			lockMouse = false;
		}
	}

	public void Mouse() {
		float mouseX = Input.GetAxis(mouseXAxis) * mouseSensitivity;

		mouseYRotation += Input.GetAxis(mouseYAxis) * mouseSensitivity;
		mouseYRotation = Mathf.Clamp(mouseYRotation, minYRotation, maxYRotation);

		camera.transform.localEulerAngles = new Vector3(-mouseYRotation, 0, 0);
		transform.Rotate(0, mouseX, 0);
	}
}
