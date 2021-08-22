using UnityEngine;

public class Movement : MonoBehaviour
{

	public float Velocity;
	[Space]
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	
	private Camera cam;

	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float StartAnimTime = 0.3f;
	[Range(0, 1f)]
	public float StopAnimTime = 0.15f;

	// Use this for initialization
	void Start()
	{
		anim = this.GetComponent<Animator>();
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update()
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

		var desiredMoveDirection = forward * movement.z + right * movement.x;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
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
		Speed = new Vector2(movement.x, movement.z).sqrMagnitude;

		//Physically move player
		if (Speed > allowPlayerRotation)
		{
			anim.SetFloat("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation(movement);
		}
		else if (Speed < allowPlayerRotation)
		{
			anim.SetFloat("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
