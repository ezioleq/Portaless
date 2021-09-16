using UnityEngine;

public class WallCamera : MonoBehaviour {
	private Transform playerTransform;
	private Transform body;
	public float speed = 10f;

	void Start() {
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		body = transform.GetChild(0).transform;
	}

	void Update() {
		Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - body.position);
		body.rotation = Quaternion.Slerp(body.rotation, targetRotation, speed * Time.deltaTime);
	}
}
