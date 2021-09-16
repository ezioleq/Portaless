using UnityEngine;
using UnityEngine.Events;

public class CubeButton : MonoBehaviour {
	public bool activated;
	public float animationSpeed = 5f;
	public UnityEvent onEnter = new UnityEvent();
	public UnityEvent onExit = new UnityEvent();
	private GameObject _activator;
	private Vector3 _offset = new Vector3(0, -0.12f, 0);
	private Transform button;
	
	private Color prevColor;
	[ColorUsage(true, true)]
	public Color activatedColor;

	void Start() {
		button = transform.Find("Button");
	}

	void Update() {
		if (activated) {
			button.localPosition = Vector3.Lerp(button.localPosition, _offset, animationSpeed * Time.deltaTime);
		} else {
			button.localPosition = Vector3.Lerp(button.localPosition, Vector3.zero, animationSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CubeType>()) {
			_activator = other.gameObject;
			onEnter.Invoke();

			if (other.GetComponent<CubeType>().type == CubeTypes.Normal) {
				prevColor = other.transform.GetChild(3).GetComponent<Renderer>().materials[1].GetColor("_EmissionColor");
				other.transform.GetChild(3).GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", activatedColor);
			}
			
			activated = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject == _activator) {
			_activator = null;
			onExit.Invoke();
			
			if (other.GetComponent<CubeType>().type == CubeTypes.Normal) {
				other.transform.GetChild(3).GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", prevColor);
			}

			activated = false;
		}
	}
}
