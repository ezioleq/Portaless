using UnityEngine;

public class LaserRedirectionCube : MonoBehaviour {
	public LayerMask layerMask;
	public bool active = false;
	public LaserRedirectionCube otherCube;

	private RaycastHit hit;
	private LineRenderer lineRenderer;
	public GameObject laserLight;

	public float dps = 15;
	private float hurtTimer = 1;
	
	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
		if (gameObject.transform.Find("LaserLight"))
			laserLight = gameObject.transform.Find("LaserLight").gameObject;
	}

	void Update() {
		if (active) {
			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, transform.position);
			laserLight.SetActive(true);

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
				lineRenderer.SetPosition(1, transform.forward * 1000f);
				SetActiveOtherCube(false);
				otherCube = null;
			}

		} else {
			lineRenderer.positionCount = 0;
			SetActiveOtherCube(false);
			otherCube = null;
			laserLight.SetActive(false);
		}
	}

	private void SetActiveOtherCube(bool active) {
		if (otherCube)
			otherCube.active = active;
	}
}
