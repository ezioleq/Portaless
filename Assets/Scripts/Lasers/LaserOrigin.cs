using UnityEngine;

public class LaserOrigin : MonoBehaviour {
	public LayerMask layerMask;
	public bool active = true;
	public LaserRedirectionCube otherCube;
	
	private RaycastHit hit;
	private LineRenderer lineRenderer;

	public float dps = 15;
	private float hurtTimer = 1;

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update() {
		if (active) {
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, transform.position);

			if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask)) {
				lineRenderer.SetPosition(1, hit.point);

				if (hit.collider.gameObject.CompareTag("Redirect")) {
					SetActiveOtherCube(false);
					otherCube = hit.collider.gameObject.GetComponent<LaserRedirectionCube>();
				} else {
					SetActiveOtherCube(false);
					otherCube = null;
				}

				if (hit.collider.CompareTag("Player")) {
					hurtTimer += Time.deltaTime;

					if (hurtTimer >= 0.3) {
						hit.collider.GetComponent<Health>().Hurt(10);
						hit.collider.GetComponent<CharacterController>().Move(
							new Vector3(
								Random.Range(-0.2f, 0.2f),
								Random.Range(0.05f, 0.2f),
								Random.Range(-0.2f, 0.2f)
							)
						);
						hurtTimer = 0;
					}
				}

				SetActiveOtherCube(true);

			} else {
				lineRenderer.SetPosition(1, transform.forward * 100f);
				SetActiveOtherCube(false);
				otherCube = null;
			}
		} else {
			lineRenderer.positionCount = 0;
			otherCube = null;
		}
	}

	private void SetActiveOtherCube(bool active) {
		if (otherCube)
			otherCube.active = active;
	}
}
