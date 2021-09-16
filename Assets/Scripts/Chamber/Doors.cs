using UnityEngine;

public class Doors : MonoBehaviour {
	public bool activated;
	[ColorUsage(false, true)] public Color activatedColor;
	[ColorUsage(false, true)] public Color deactivatedColor;
	public Vector3 openOffset = new Vector3(1, 0, 0);
	public Vector3 closedOffset = new Vector3(0, -0.3724104f, 0);
	public float animationSpeed = 10f;

	private Transform _leftDoor;
	private Transform _rightDoor;
	private Material _identifier_mat;

	void Start() {
		_leftDoor = transform.GetChild(1);
		_rightDoor = transform.GetChild(2);
		_identifier_mat = _rightDoor.GetChild(0).GetComponent<Renderer>().materials[1];
	}

	void Update() {
		if (activated) {
			_leftDoor.localPosition = Vector3.Lerp(_leftDoor.localPosition, openOffset, animationSpeed * Time.deltaTime);
			_rightDoor.localPosition = Vector3.Lerp(
				_rightDoor.localPosition,
				new Vector3(openOffset.x * -1, openOffset.y, openOffset.z),
				animationSpeed * Time.deltaTime
			);
			_identifier_mat.SetColor("_EmissionColor", activatedColor);
		} else {
			_leftDoor.localPosition = Vector3.Lerp(_leftDoor.localPosition, closedOffset, animationSpeed * Time.deltaTime);
			_rightDoor.localPosition = Vector3.Lerp(_rightDoor.localPosition, closedOffset, animationSpeed * Time.deltaTime);
			_identifier_mat.SetColor("_EmissionColor", deactivatedColor);
		}
	}

	public void ChangeState(bool open) {
		activated = open;
	}
}
