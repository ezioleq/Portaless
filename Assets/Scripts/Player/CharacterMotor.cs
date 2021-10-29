using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {
	private CharacterController cc;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 playerVelocity = Vector3.zero;
	private float moveSpeed;
	private float gravityForce = 16;
	[SerializeField] private bool noclip;
	
	[Header("General")]
	public string verticalAxis = "Vertical";
	public string horizontalAxis = "Horizontal";
	public float walkSpeed = 4f;
	public float runSpeed = 6.5f;
	public float midairSpeed = 3;
	float maxSpeed = 0;
	float groundSpeed = 0;
	public float maxAirSpeed = 3;
	public float maxWalkSpeed = 20;
	public float maxRunSpeed = 30;
	public float drag = 0;
	public float jumpForce = 5;
	float adhesionForce = 0.4f;
	public bool lockKeyboard;
	float groundTimer;
	private int layerMask;

	private void Start() {
		cc = gameObject.GetComponent<CharacterController>();
		layerMask = LayerMask.GetMask("Player");
		layerMask = ~layerMask;
	}

	private void Update() {
		if (!lockKeyboard)
			Keyboard();
		
		if (!noclip)
			cc.Move(playerVelocity * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.V))
			noclip = !noclip;
	}

	private void FixedUpdate() {
		groundSpeed = Input.GetKey(KeyCode.LeftShift) ? maxWalkSpeed : maxRunSpeed;
		// maxSpeed differs on the ground and in the air
		maxSpeed = cc.isGrounded ? groundSpeed : maxAirSpeed;
		
		if (cc.isGrounded)
			// delay ground detection to allow bhopping
			groundTimer += Time.fixedDeltaTime;
		else
			groundTimer = 0;
		
		if (groundTimer > 0.08f) {
			// on the ground use GroundAccelerate (friction)
			if (!Input.GetMouseButton(0))
				playerVelocity = GroundAccelerate(playerVelocity, moveDirection, moveSpeed);

			if (Input.GetMouseButton(0)) {
				playerVelocity = AirAccelerate(playerVelocity, moveDirection, midairSpeed);
				playerVelocity.y -= gravityForce * Time.deltaTime;
			}
		} else {
			// in the air use AirAccelerate (no friction)
			playerVelocity = AirAccelerate(playerVelocity, moveDirection, midairSpeed);
			// apply gravity 
			playerVelocity.y -= gravityForce * Time.deltaTime;
		}
	}

	public void Keyboard() {
		// calculate moveDirection 
		moveDirection = (
			Input.GetAxisRaw(horizontalAxis) * transform.right +
			Input.GetAxisRaw(verticalAxis) * transform.forward
		).normalized;
		moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		if (!noclip) {
			GetComponent<CharacterController>().enabled = true;
			if (cc.isGrounded) {
				if (Input.GetButton("Jump")) {
					playerVelocity.y = Mathf.Clamp(playerVelocity.y, 0, Mathf.Infinity);
					playerVelocity.y += jumpForce;
				}		
			} 
			
		} else {
			GetComponent<CharacterController>().enabled = false;
			transform.Translate(0, 0, Input.GetAxis(verticalAxis) * moveSpeed * Time.deltaTime, Space.Self);
			transform.Translate(Input.GetAxis(horizontalAxis) * moveSpeed * Time.deltaTime, 0, 0, Space.Self);

			if (Input.GetButton("Jump"))
				transform.Translate(0, moveSpeed * Time.deltaTime, 0, Space.World);

			if (Input.GetKey(KeyCode.LeftControl))
				transform.Translate(0, -moveSpeed * Time.deltaTime, 0, Space.World);
		}
	}

	private Vector3 Accelerate(Vector3 currentVelocity, Vector3 direction, float acceleration) {
		// calculate dot product of velocity and move direction 
		float currentSpeed = Vector3.Dot(new Vector3(currentVelocity.x, 0, currentVelocity.z), direction);
		// calculate acceleration
        float addSpeed = acceleration * Time.fixedDeltaTime;

		if(currentSpeed + addSpeed > maxSpeed)
			// don't accelerate if current speed is equal or exceeds max speed
			addSpeed = Mathf.Clamp(maxSpeed - currentSpeed, 0, maxSpeed);
		
		// return velocity + acceleration 
		return new Vector3(currentVelocity.x, playerVelocity.y, currentVelocity.z) + addSpeed * direction;
	}

	private Vector3 GroundAccelerate(Vector3 currentVelocity, Vector3 direction, float acceleration) {
       	currentVelocity = ApplyFriction(currentVelocity, drag); 
       	return Accelerate(currentVelocity, direction, acceleration);
    }	
    
	private Vector3 AirAccelerate(Vector3 currentVelocity, Vector3 direction, float acceleration) {
       	return Accelerate(currentVelocity, direction, acceleration);
    }

	private Vector3 ApplyFriction(Vector3 currentVelocity, float friction) {
       	return currentVelocity * (1 / (friction + 1)); 
    }

	private void OnControllerColliderHit(ControllerColliderHit collision) {
		RaycastHit stepCast;
		float stepCastDepth = 0.1f;

		// check if player is able to step
		if (!Physics.Raycast(
			transform.position - (cc.height/2) * transform.up + cc.stepOffset * transform.up,
			new Vector3(playerVelocity.x, 0, playerVelocity.z).normalized,
			out stepCast,
			cc.radius/2+stepCastDepth,
			layerMask,
			QueryTriggerInteraction.Ignore)) 
		{

			// calculate velocity product in direction of collision normal
			float momentum = Vector3.Dot(playerVelocity, collision.normal);
			// subtract it from velocity (only if calculated momentum points against the normal)
			if (momentum < 0) playerVelocity -= collision.normal * momentum;
			// apply additional adhesion force (required for cc to detect collisions properly)
			if (collision.normal.y > 0) playerVelocity -= collision.normal * adhesionForce;
		}
	}

	public void AddMomentum(Vector3 momentum) {
		playerVelocity += momentum;
	}
}
