using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {
	private CharacterController cc;
	private Vector3 moveDirection = Vector3.zero;
	private float moveSpeed;
	private float gravityForce = 16;
	[SerializeField] private bool noclip;
	
	[Header("General")]
	public string verticalAxis = "Vertical";
	public string horizontalAxis = "Horizontal";
	public float walkSpeed = 4f;
	public float runSpeed = 6.5f;
	public float midairSpeed = 3;
	public float jumpForce = 5;
	public bool lockKeyboard;

	private void Start() {
		cc = gameObject.GetComponent<CharacterController>();
	}

	private void Update() {
		if (!lockKeyboard)
			Keyboard();
		if (Input.GetKeyDown(KeyCode.V))
			noclip = !noclip;
	}

	public void Keyboard() {
		moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		if (Input.GetKey(KeyCode.F2)) {
			String filename = $"capture-{DateTime.Now.ToString()}.png";
			ScreenCapture.CaptureScreenshot(filename);
			Debug.Log($"Saved screenshot {filename}");
		}

		if (!noclip) {
			GetComponent<CharacterController>().enabled = true;
			if (cc.isGrounded) {
				moveDirection = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis));
				moveDirection = Vector3.ClampMagnitude(moveDirection, 1);
				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection *= moveSpeed;
				if (Input.GetButton("Jump"))
					moveDirection.y = jumpForce;
			} else {
				moveDirection.x = Input.GetAxis(horizontalAxis) * midairSpeed;
				moveDirection.z = Input.GetAxis(verticalAxis) * midairSpeed;
				moveDirection = transform.TransformDirection(moveDirection);
			}

			moveDirection.y -= gravityForce * Time.deltaTime;
			cc.Move(moveDirection * Time.deltaTime);
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
}
