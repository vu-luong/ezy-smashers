using System;
using UnityEditor.Animations;
using UnityEngine;

public class Movement : MonoBehaviour
{

	public float Velocity;
	[Space]
	public float desiredRotationSpeed = 1f;
	public float allowPlayerRotation = 0.1f;

	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float StartAnimTime = 0.3f;
	[Range(0, 1f)]
	public float StopAnimTime = 0.15f;

	private Camera cam;
	private Animator anim;
	
	private Vector3 currentEulerAngles;

	// Use this for initialization
	void Start()
	{
		anim = this.GetComponent<Animator>();
		cam = Camera.main;

		Vector3 vec = new Vector3(0.8f, 0.3f, 0.1f);
		// Debug.Log("vec = " + vec);
		Debug.Log("quaternion = " + Quaternion.LookRotation(vec, Vector3.up).ToString("F10"));
		Quaternion res = Quaternion.LookRotation(vec, Vector3.up);
		Debug.Log("res.X = " + res.x);
		Debug.Log("res.Y = " + res.y);
		Debug.Log("res.Z = " + res.z);
		Debug.Log("res.W = " + res.w);
	}

	// Update is called once per frame
	// void Update()
	// {
	// 	InputMagnitude();
	// }


	private void FixedUpdate()
	{
		InputMagnitude();
	}

	void Move(Vector3 delta)
	{
		transform.position = transform.position + delta;
	}

	void PlayerMoveAndRotation(Vector3 movement)
	{
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		// Debug.Log("forward = " + forward);
		// Debug.Log("right = " + right);

		// var desiredMoveDirection = forward * movement.z + right * movement.x;
		var desiredMoveDirection = Vector3.forward * movement.z + Vector3.right * movement.x;

		Debug.Log("-------------------------------");
		Debug.Log("transform.rotation.euler = " + transform.rotation.eulerAngles.ToString("F4"));
		Debug.Log("transform.rotation = " + transform.rotation.ToString("F4"));
		Debug.Log("desiredMoveDirection = " + desiredMoveDirection.ToString("F4") + " " + Quaternion.LookRotation(desiredMoveDirection).ToString("F4"));
		Debug.Log("speed = " + desiredRotationSpeed.ToString("F4"));
		// Debug.Log("result = " + Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed).ToString("F4"));
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
		// transform.rotation = Quaternion.LookRotation(desiredMoveDirection);
		Debug.Log("result = " + transform.rotation.ToString("F4"));
		
		
		// currentEulerAngles += new Vector3(0, movement.x, 0) * desiredRotationSpeed;
		// transform.eulerAngles = currentEulerAngles;
		
		var temp = desiredMoveDirection * Time.deltaTime * Velocity;
		Move(temp);
	}

	void InputMagnitude()
	{
		bool[] inputs = new bool[6];
		inputs[0] = Input.GetKey(KeyCode.UpArrow);
		inputs[1] = Input.GetKey(KeyCode.LeftArrow);
		inputs[2] = Input.GetKey(KeyCode.DownArrow);
		inputs[3] = Input.GetKey(KeyCode.RightArrow);

		Vector3 movement = Vector3.zero;

		if (inputs[0])
		{
			movement += Vector3.forward;
		}
		if (inputs[1])
		{
			movement += Vector3.left;
		}
		if (inputs[2])
		{
			movement += Vector3.back;
		}
		if (inputs[3])
		{
			movement += Vector3.right;
		}

		if (Input.GetKeyDown("space"))
		{
			anim.SetTrigger("slash");
		}

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
		{
			return;
		}

		//Calculate the Input Magnitude
		var speed = new Vector2(movement.x, movement.z).sqrMagnitude;

		//Physically move player
		if (speed > allowPlayerRotation)
		{
			anim.SetFloat("Blend", speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation(movement);
		}
		else if (speed < allowPlayerRotation)
		{
			anim.SetFloat("Blend", speed, StopAnimTime, Time.deltaTime);
		}
	}
}
