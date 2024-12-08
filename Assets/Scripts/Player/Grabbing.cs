using Portaless.Input;
using UnityEngine;

namespace Portaless.Player
{
	// Literally Garbage To Me!
	public class Grabbing : MonoBehaviour {
		[Header("General")]
		[SerializeField] private Transform grabbingPoint;
		[SerializeField] private LayerMask layerMask;
		private Transform playerTransform;

		[Header("Values")]

		[Range(1, 16)]
		[SerializeField]
		private float maxDistance = 6f;

		[Range(1, 16)]
		[SerializeField]
		private float lerpSpeed;

		[Range(1, 16)]
		[SerializeField]
		public float rotationSpeed;

		[SerializeField] private float minLocalHeight = -0.3f;

		[Header("Grabbed")]
		[SerializeField] private GameObject grabbedObject;
		private Rigidbody grabbedRigidbody;

		private GameObject fixedGrabbingPointGameObject;
		private Vector3 localGrabbingPoint;

		private void Start() {
			playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

			fixedGrabbingPointGameObject = Instantiate(
				new GameObject("FixedGrabbingPoint"),
				grabbingPoint.position,
				grabbingPoint.rotation
			);
			fixedGrabbingPointGameObject.transform.SetParent(playerTransform);
		}

		private void Update() {
			Transform origin = Camera.main.transform;

			if (InputManager.Instance.Actions.Gameplay.Use.triggered) {
				// Dropping held object
				if (grabbedObject) {
					grabbedObject.GetComponent<Rigidbody>().useGravity = true;
					IgnoreGrabbedObjectCollision(false);
					grabbedObject = null;
					grabbedRigidbody = null;
					return; // Ignore rest of the code
				}

				// Grabbing object
				if (Physics.Raycast(origin.position, origin.forward, out var hit, maxDistance, layerMask)) {
					if (hit.rigidbody) {
						grabbedObject = hit.collider.gameObject;
						grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
						grabbedRigidbody.useGravity = false;
						IgnoreGrabbedObjectCollision(true);
					}
				}
			}

			if (grabbedObject) {
				localGrabbingPoint = playerTransform.InverseTransformPoint(grabbingPoint.position);

				grabbedObject.transform.position = Vector3.Lerp(
					grabbedObject.transform.position,
					new Vector3(
						grabbingPoint.position.x,
						(localGrabbingPoint.y >= minLocalHeight) ?
							grabbingPoint.position.y : fixedGrabbingPointGameObject.transform.position.y + minLocalHeight,
						grabbingPoint.position.z
					),
					lerpSpeed * Time.deltaTime
				);

				grabbedObject.transform.rotation = Quaternion.Lerp(
					grabbedObject.transform.rotation,
					playerTransform.rotation,
					rotationSpeed * Time.deltaTime
				);

				grabbedRigidbody.linearVelocity = Vector3.zero;
				grabbedRigidbody.angularVelocity = Vector3.zero;
			}

			Debug.DrawRay(origin.position, origin.forward * maxDistance, Color.green);
		}

		private void IgnoreGrabbedObjectCollision(bool ignore) {
			Physics.IgnoreCollision(
				grabbedObject.GetComponent<Collider>(),
				playerTransform.gameObject.GetComponent<Collider>(),
				ignore
			);
		}
	}
}
