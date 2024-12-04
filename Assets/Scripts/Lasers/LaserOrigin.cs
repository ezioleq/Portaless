using Portaless.Player;
using UnityEngine;

namespace Portaless.Lasers
{
	public class LaserOrigin : MonoBehaviour {
		public bool Active = false;
		[SerializeField] private bool isReceiver = false;
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private LaserOrigin otherCube;

		private RaycastHit hit;
		private LineRenderer lineRenderer;
		private GameObject laserLight;

		[SerializeField] private float damagePerSecond = 50;
		[SerializeField] private float hurtRate = 0.3f;
		private float hurtTimer = 0;

		void Start() {
			lineRenderer = GetComponent<LineRenderer>();
			if (isReceiver) {
				if (gameObject.transform.Find("LaserLight"))
					laserLight = gameObject.transform.Find("LaserLight").gameObject;
			}
		}

		void Update() {
			if (Active) {
				lineRenderer.positionCount = 2;
				lineRenderer.SetPosition(0, transform.position);

				if (isReceiver)
					laserLight.SetActive(true);

				if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask)) {
					lineRenderer.SetPosition(1, hit.point);

					if (hit.collider.gameObject.CompareTag("Redirect")) {
						SetActiveOtherCube(false);
						otherCube = hit.collider.gameObject.GetComponent<LaserOrigin>();
					} else {
						SetActiveOtherCube(false);
						otherCube = null;
					}

					if (hit.collider.CompareTag("Player")) {
						hurtTimer += Time.deltaTime;

						if (hurtTimer >= hurtRate) {
							hit.collider.GetComponent<Health>().Hurt(damagePerSecond * hurtRate);
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
				if (isReceiver) {
					SetActiveOtherCube(false);
					if (laserLight)
						laserLight.SetActive(false);
				}
				otherCube = null;
			}
		}

		private void SetActiveOtherCube(bool active) {
			if (otherCube)
				otherCube.Active = active;
		}
	}
}
