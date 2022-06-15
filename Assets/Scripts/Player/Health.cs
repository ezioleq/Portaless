using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {
	public int MaxHealth { get; private set; } = 100;

	[SerializeField] private float health;
	private bool isAlive = true;

	[SerializeField] private int regenRate = 5;
	[SerializeField] private float minTimeAfterHit = 5;
	[SerializeField] private KeyCode emergencySuicideKey = KeyCode.R;
	[SerializeField] private float timeToRespawn = 2f;

	private GameObject cameraObject;
	[SerializeField] private Image hurtSprite;

	private float regenTimer;
	private float respawnTimer;
	private float lastHitTimer;

	private Transform playerTransform;

	private void Start() {
		health = MaxHealth;
		cameraObject = Camera.main.gameObject;
	}

	private void Update() {
		if (GetHealth() > MaxHealth)
			health = MaxHealth;
		else if (GetHealth() <= 0)
			Kill();

		if (GetHealth() < MaxHealth)
			lastHitTimer += Time.deltaTime;

		if (Input.GetKeyDown(emergencySuicideKey))
			Kill();

		if (!isAlive) {
			respawnTimer += Time.deltaTime;

			if (respawnTimer >= timeToRespawn && Input.anyKey)
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		hurtSprite.color = new Color(1f, 1f, 1f, 1f - (health / MaxHealth));

		if (isAlive && lastHitTimer >= minTimeAfterHit && health < MaxHealth) {
			regenTimer += Time.deltaTime;

			if (regenTimer >= 1) {
				health += regenRate;
				regenTimer = 0;
			}
		}
	}

	public float GetHealth() => health;
	public bool IsAlive() => isAlive;

	public void Hurt(float amount) {
		// You can't hurt me if I'm already dead...
		if (isAlive) {
			health -= amount;
			lastHitTimer = 0;
		}
	}

	public void Kill() {
		if (!isAlive)
			return;

		isAlive = false;
		health = 0;

		cameraObject.AddComponent<SphereCollider>().radius = 0.4f;
		Physics.IgnoreCollision(
			gameObject.GetComponent<Collider>(),
			cameraObject.GetComponent<Collider>(),
			true
		);
		cameraObject.AddComponent<Rigidbody>();
		cameraObject.transform.parent = null;
	}
}
