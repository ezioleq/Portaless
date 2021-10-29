using UnityEngine;

public class WallCamera : MonoBehaviour {
	[SerializeField] private float _rotationSpeed = 6f;
	[SerializeField] private Vector3 _rotationOffsets = Vector3.zero;
	[SerializeField] private Transform _headBone;
	private Transform _playerPosition;

	private void Start() {
		_playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update() {
		Quaternion targetRotation = Quaternion.LookRotation(_playerPosition.position - _headBone.position);
		targetRotation.eulerAngles = new Vector3(
			targetRotation.eulerAngles.x + _rotationOffsets.x,
			targetRotation.eulerAngles.y + _rotationOffsets.y,
			targetRotation.eulerAngles.z + _rotationOffsets.z
		);
		_headBone.rotation = Quaternion.Slerp(_headBone.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
	}
}
