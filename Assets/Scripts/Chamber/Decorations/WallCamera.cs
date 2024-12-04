using UnityEngine;

namespace Portaless.Chamber.Decorations
{
	public class WallCamera : MonoBehaviour {
		[SerializeField] private float rotationSpeed = 6f;
		[SerializeField] private Vector3 rotationOffsets = Vector3.zero;
		[SerializeField] private Transform headBone;
		private Transform playerPosition;

		private void Start() {
			playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		}

		private void Update() {
			Quaternion targetRotation = Quaternion.LookRotation(playerPosition.position - headBone.position);

			targetRotation.eulerAngles = new Vector3(
				targetRotation.eulerAngles.x + rotationOffsets.x,
				targetRotation.eulerAngles.y + rotationOffsets.y,
				targetRotation.eulerAngles.z + rotationOffsets.z
			);

			headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
	}
}
