using UnityEngine;
using System.Collections;

public class CursedLauncher : MonoBehaviour {
	[SerializeField] private Transform pad;
	[SerializeField] private Vector3 launchedRot = new Vector3(60, 0, 0);
	[SerializeField] private float animationSpeed = 20f;
	[SerializeField] private float timeToHide = 2f;
	[SerializeField] private float flySpeed = 20f;

	[SerializeField] private bool launched;
	private Vector3 velocity;

	[SerializeField] private GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		pad.localRotation = Quaternion.Slerp(
			pad.localRotation,
			Quaternion.Euler(
				(launched) ? launchedRot : Vector3.zero
			),
			animationSpeed * Time.deltaTime
		);
	}

	IEnumerator IEHide() {
		yield return new WaitForSeconds(timeToHide);
		launched = false;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			launched = true;
			player.GetComponent<CharacterMotor>().AddMomentum((transform.forward + transform.up) * flySpeed);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player"))
			StartCoroutine(IEHide());
	}
}
