
using UnityEngine;

public class Movement: MonoBehaviour {

    public float Velocity;
    [Space]

    
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	// public CharacterController controller;
	public bool isGrounded;

    [Header("Animation Smoothing")]
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		// controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		var InputX = Input.GetAxis ("Horizontal");
		var InputZ = Input.GetAxis ("Vertical");
		desiredMoveDirection = forward * InputZ + right * InputX;
		
		InputMagnitude ();
    }

	void Move(Vector3 delta)
	{
		// transform.Translate(delta);
		transform.position = transform.position + delta;
	}

    void PlayerMoveAndRotation(Vector3 movement) {
		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();
		
		var InputX = Input.GetAxis ("Horizontal");
		var InputZ = Input.GetAxis ("Vertical");
		desiredMoveDirection = forward * InputZ + right * InputX;
		
		Debug.Log("desiredMoveDirection = " + desiredMoveDirection);
		Debug.Log("new value = " + (forward * movement.z + right * movement.x));

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
			var temp = desiredMoveDirection * Time.deltaTime * Velocity;
			Move(temp);
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

	void InputMagnitude() {
		bool[]inputs = new bool[6];
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


		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash")) {
			return;
        }

		//Calculate Input Vectors
		// var InputX = Input.GetAxis ("Horizontal");
		// var InputZ = Input.GetAxis ("Vertical");

		//Calculate the Input Magnitude
		Speed = new Vector2(movement.x, movement.z).sqrMagnitude;

        //Physically move player

		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation (movement);
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
