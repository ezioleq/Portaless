using UnityEngine;

public class Doors : MonoBehaviour {
	[SerializeField] private bool activated;

	[SerializeField]
	[ColorUsage(false, true)]
	private Color activatedColor;

	[SerializeField]
	[ColorUsage(false, true)]
	private Color deactivatedColor;

	[SerializeField] private Vector3 openOffset = new Vector3(1, 0, 0);
	[SerializeField] private Vector3 closedOffset = new Vector3(0, -0.3724104f, 0);
	[SerializeField] private float animationSpeed = 10f;

	private Transform leftDoor;
	private Transform rightDoor;
	private Material identifierMaterial;

	void Start() {
		leftDoor = transform.GetChild(1);
		rightDoor = transform.GetChild(2);
		identifierMaterial = rightDoor.GetChild(0).GetComponent<Renderer>().materials[1];
	}

	void Update() {
		if (activated) {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, openOffset, animationSpeed * Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(
				rightDoor.localPosition,
				new Vector3(openOffset.x * -1, openOffset.y, openOffset.z),
				animationSpeed * Time.deltaTime
			);
			identifierMaterial.SetColor("_EmissionColor", activatedColor);
		} else {
			leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, closedOffset, animationSpeed * Time.deltaTime);
			rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, closedOffset, animationSpeed * Time.deltaTime);
			identifierMaterial.SetColor("_EmissionColor", deactivatedColor);
		}
	}

	public void ChangeState(bool open) {
		activated = open;
	}
}
