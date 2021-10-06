using _2___Scripts.shared;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInterpolation))]
public class Movement : MonoBehaviour
{
	[Space]
	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float startAnimTime = 0.1f;
	[Range(0, 1f)]
	public float stopAnimTime = 0.15f;

	private Animator anim;

	private PlayerInterpolation playerInterpolation;

	public static UnityAction<PlayerInputData> playerInputEvent;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		playerInterpolation = GetComponent<PlayerInterpolation>();
		playerInterpolation.CurrentData = new PlayerStateData(transform.position, transform.rotation);
		playerInterpolation.PreviousData = new PlayerStateData(transform.position, transform.rotation);
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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && !anim.IsInTransition(0))
			{
				anim.SetTrigger("slash");
			}
		}
	}

	void InputMagnitude()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
		{
			return;
		}

		bool[] inputs = new bool[6];
		inputs[0] = Input.GetKey(KeyCode.UpArrow);
		inputs[1] = Input.GetKey(KeyCode.LeftArrow);
		inputs[2] = Input.GetKey(KeyCode.DownArrow);
		inputs[3] = Input.GetKey(KeyCode.RightArrow);
		inputs[4] = Input.GetKeyDown(KeyCode.Space);

		Vector3 movement = InputUtils.ComputeMovementFromInput(inputs[0], inputs[1], inputs[2], inputs[3]);

		// Calculate the Input Magnitude
		var moveInputMagnitude = new Vector2(movement.x, movement.z).sqrMagnitude;

		// Physically move player
		if (moveInputMagnitude > 0)
		{
			// Debug.Log("movement = " + movement);
			anim.SetFloat("Blend", moveInputMagnitude, startAnimTime, Time.deltaTime);
			// PlayerMoveAndRotation(movement);
			PlayerInputData inputData = new PlayerInputData(inputs);
			PlayerStateData nextStateData = PlayerLogic.GetNextFrameData(inputData, playerInterpolation.CurrentData);
			playerInterpolation.SetFramePosition(nextStateData);
			playerInputEvent?.Invoke(inputData);
		}
		else
		{
			anim.SetFloat("Blend", moveInputMagnitude, stopAnimTime, Time.deltaTime);
		}
	}
}
