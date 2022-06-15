using UnityEngine;
using UnityEngine.Events;

public class CubeButton : MonoBehaviour {
	[SerializeField] private bool activated;
	[SerializeField] private float animationSpeed = 5f;
	[SerializeField] private Vector3 offset = new Vector3(0, -0.12f, 0);

	[SerializeField] private UnityEvent onEnter = new UnityEvent();
	[SerializeField] private UnityEvent onExit = new UnityEvent();

	private GameObject activator;
	private Transform button;

	private Color previousColor;

	[SerializeField]
	[ColorUsage(true, true)]
	private Color activatedColor;

	void Start() {
		button = transform.Find("Button");
	}

	void Update() {
		button.localPosition = Vector3.Lerp(
			button.localPosition,
			(activated) ? offset : Vector3.zero,
			animationSpeed * Time.deltaTime
		);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CubeType>()) {
			activator = other.gameObject;
			onEnter.Invoke();

			if (other.GetComponent<CubeType>().Type == CubeTypes.Normal) {
				previousColor = other.transform.GetChild(3).GetComponent<Renderer>().materials[1].GetColor("_EmissionColor");
				other.transform.GetChild(3).GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", activatedColor);
			}

			activated = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject == activator) {
			activator = null;
			onExit.Invoke();

			if (other.GetComponent<CubeType>().Type == CubeTypes.Normal) {
				other.transform.GetChild(3).GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", previousColor);
			}

			activated = false;
		}
	}
}
