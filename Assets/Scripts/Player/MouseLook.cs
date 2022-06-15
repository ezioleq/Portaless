using UnityEngine;

public class MouseLook : MonoBehaviour {
	private new Camera camera;
	private Health health;

	[Header("General")]
	[SerializeField] private string mouseYAxis = "Mouse Y";
	[SerializeField] private string mouseXAxis = "Mouse X";
	[SerializeField] private float mouseYRotation;

	[Header("Values")]
	[Range(1, 10)] public float MouseSensitivity = 4.5f;

	[Range(0, 90)]
	[SerializeField]
	private float maxYRotation = 90;

	[Range(-90, 0)]
	[SerializeField]
	private float minYRotation = -90;

	[Header("Zoom")]
	[SerializeField] private float zoomingFOV = 30f;
	[SerializeField] private float zoomSpeed = 10f;
	private float defaultFOV;

	[Header("Locks")]
	public bool LockMouse;
	public bool ShowMouse;

	private void Start() {
		camera = Camera.main;
		defaultFOV = camera.fieldOfView;
		health = GetComponent<Health>();
	}

	private void Update() {
		if (!LockMouse && health.IsAlive())
			Mouse();

		if (Input.GetKeyDown(KeyCode.P)) ShowMouse = !ShowMouse;

		if (Input.GetMouseButton(2))
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomingFOV, zoomSpeed * Time.deltaTime);
		else
			camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFOV, zoomSpeed * Time.deltaTime);

		if (ShowMouse){
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			LockMouse = true;
		} else {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			LockMouse = false;
		}
	}

	public void Mouse() {
		float mouseX = Input.GetAxis(mouseXAxis) * MouseSensitivity;

		mouseYRotation += Input.GetAxis(mouseYAxis) * MouseSensitivity;
		mouseYRotation = Mathf.Clamp(mouseYRotation, minYRotation, maxYRotation);

		camera.transform.localEulerAngles = new Vector3(-mouseYRotation, 0, 0);
		transform.Rotate(0, mouseX, 0);
	}
}
