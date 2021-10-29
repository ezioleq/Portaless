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
		if (Input.GetKeyDown(KeyCode.V))
			noclip = !noclip;
		cc.Move(playerVelocity * Time.deltaTime);
	}
	private void FixedUpdate(){
		groundSpeed = Input.GetKey(KeyCode.LeftShift) ? maxWalkSpeed : maxRunSpeed;
		maxSpeed = cc.isGrounded ? groundSpeed : maxAirSpeed; //maxSpeed differs on the ground and in the air
		if(cc.isGrounded) groundTimer += Time.fixedDeltaTime; //delay ground detection to allow bhopping
		else groundTimer = 0;
		if(groundTimer > 0.08f){
			if(!Input.GetMouseButton(0))playerVelocity = GroundAccelerate(playerVelocity, moveDirection, moveSpeed); //on the ground use GroundAccelerate (friction)
			if(Input.GetMouseButton(0)){playerVelocity = AirAccelerate(playerVelocity, moveDirection, midairSpeed);
			playerVelocity.y -= gravityForce * Time.deltaTime;
			}
		}else{
			playerVelocity = AirAccelerate(playerVelocity, moveDirection, midairSpeed); //in the air use AirAccelerate (no friction)
			playerVelocity.y -= gravityForce * Time.deltaTime; //apply gravity 
		}
	}

	public void Keyboard() {
		moveDirection = (Input.GetAxisRaw(horizontalAxis) * transform.right + Input.GetAxisRaw(verticalAxis) * transform.forward).normalized; //calculate moveDirection 
		moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		if (Input.GetKey(KeyCode.F2)) {
			String filename = $"capture-{DateTime.Now.ToString()}.png";
			ScreenCapture.CaptureScreenshot(filename);
			Debug.Log($"Saved screenshot {filename}");
		}

		if (!noclip) {
			GetComponent<CharacterController>().enabled = true;
			if (cc.isGrounded) {
				if (Input.GetButton("Jump")){
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
	private Vector3 Accelerate(Vector3 currentVelocity, Vector3 direction, float acceleration){
		float currentSpeed = Vector3.Dot(new Vector3(currentVelocity.x, 0, currentVelocity.z), direction); //calculate dot product of velocity and move direction 
        	float addSpeed = acceleration * Time.fixedDeltaTime; //calculate acceleration
		if(currentSpeed + addSpeed > maxSpeed) addSpeed = Mathf.Clamp(maxSpeed - currentSpeed, 0, maxSpeed); //don't accelerate if current speed is equal or exceeds max speed
		return new Vector3(currentVelocity.x, playerVelocity.y, currentVelocity.z) + addSpeed * direction; // return velocity + acceleration 
	}
	private Vector3 GroundAccelerate(Vector3 currentVelocity, Vector3 direction, float acceleration){
        	currentVelocity = ApplyFriction(currentVelocity, drag); 
        	return Accelerate(currentVelocity, direction, acceleration);
    	}	
    	private Vector3 AirAccelerate(Vector3 currentVelocity, Vector3 direction, float acceleration){
        	return Accelerate(currentVelocity, direction, acceleration);
    	}


	private Vector3 ApplyFriction(Vector3 currentVelocity, float friction){
        	return currentVelocity * (1 / (friction + 1)); 
    	}
	private void OnControllerColliderHit(ControllerColliderHit collision){
	RaycastHit stepCast;
	float stepCastDepth = 0.1f;
	if(!Physics.Raycast(transform.position - (cc.height/2) * transform.up + cc.stepOffset * transform.up, new Vector3(playerVelocity.x, 0, playerVelocity.z).normalized, out stepCast, cc.radius/2+stepCastDepth, layerMask, QueryTriggerInteraction.Ignore)){ //check if player is able to step
		float momentum = Vector3.Dot(playerVelocity, collision.normal); //calculate velocity product in direction of collision normal
		if(momentum < 0) playerVelocity -= collision.normal * momentum; //subtract it from velocity (only if calculated momentum points against the normal)
		if(collision.normal.y > 0)playerVelocity -= collision.normal * adhesionForce; //apply additional adhesion force (required for cc to detect collisions properly)
	}
	}
	public void AddMomentum(Vector3 momentum){
		playerVelocity += momentum;
	}
}
