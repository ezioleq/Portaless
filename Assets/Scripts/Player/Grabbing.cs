using UnityEngine;

// Literally Garbage To Me!
public class Grabbing : MonoBehaviour {
	[Header("General")]
	public Transform grabbingPoint;
	public LayerMask layerMask;
	public KeyCode grabKey;
	private Transform _playerTransform;
	
	[Header("Values")]
	[Range(1, 16)]
	public float maxDistance = 6f;
	[Range(1, 16)]
	public float lerpSpeed;
	[Range(1, 16)]
	public float rotationSpeed;
	public float minLocalHeight = -0.3f;

	[Header("Grabbed")]
	[SerializeField]
	private GameObject _grabbedObject;
	private Rigidbody _grabbedRigidbody;

	private GameObject fixedGrabbingPointGameObject;
	private Vector3 localGrabbingPoint;

	private void Start() {
		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		fixedGrabbingPointGameObject = Instantiate(
			new GameObject("FixedGrabbingPoint"),
			grabbingPoint.position,
			grabbingPoint.rotation
		);
		fixedGrabbingPointGameObject.transform.SetParent(_playerTransform);
	}

	private void Update() {
		RaycastHit hit;
		Transform origin = Camera.main.transform;

		if (Input.GetKeyDown(grabKey)) {
			// Dropping held object
			if (_grabbedObject) {
				_grabbedObject.GetComponent<Rigidbody>().useGravity = true;
				IgnoreGrabbedObjectCollision(false);
				_grabbedObject = null;
				_grabbedRigidbody = null;
				return; // Ignore rest of the code
			}

			// Grabbing object
			if (Physics.Raycast(origin.position, origin.forward, out hit, maxDistance, layerMask)) {
				if (hit.rigidbody) {
					_grabbedObject = hit.collider.gameObject;
					_grabbedRigidbody = _grabbedObject.GetComponent<Rigidbody>();
					_grabbedRigidbody.useGravity = false;
					IgnoreGrabbedObjectCollision(true);
				}
			}
		}

		if (_grabbedObject) {
			localGrabbingPoint = _playerTransform.InverseTransformPoint(grabbingPoint.position);

			_grabbedObject.transform.position = Vector3.Lerp(
				_grabbedObject.transform.position,
				new Vector3(
					grabbingPoint.position.x,
					(localGrabbingPoint.y >= minLocalHeight) ?
						grabbingPoint.position.y : fixedGrabbingPointGameObject.transform.position.y + minLocalHeight,
					grabbingPoint.position.z
				),
				lerpSpeed * Time.deltaTime
			);

			_grabbedObject.transform.rotation = Quaternion.Lerp(
				_grabbedObject.transform.rotation,
				_playerTransform.rotation,
				rotationSpeed * Time.deltaTime
			);

			_grabbedRigidbody.velocity = Vector3.zero;
			_grabbedRigidbody.angularVelocity = Vector3.zero;
		}

		Debug.DrawRay(origin.position, origin.forward * maxDistance, Color.green);
	}

	private void IgnoreGrabbedObjectCollision(bool ignore) {
		Physics.IgnoreCollision(
			_grabbedObject.GetComponent<Collider>(),
			_playerTransform.gameObject.GetComponent<Collider>(),
			ignore
		);
	}
}
