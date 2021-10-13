using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _2___Scripts.shared;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OfflinePlayerInterpolation))]
public class OfflinePlayer : MonoBehaviour
{
	[Space]
	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float startAnimTime = 0.1f;
	[Range(0, 1f)]
	public float stopAnimTime = 0.15f;

	private Animator anim;
	private OfflinePlayerInterpolation playerInterpolation;

	public int ClientTick { get; set; }


	public Animator Anim => anim;


	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		playerInterpolation = GetComponent<OfflinePlayerInterpolation>();
	}

	private void Start()
	{
		playerInterpolation.CurrentData = new PlayerStateData(transform.position, transform.rotation);
		playerInterpolation.PreviousData = new PlayerStateData(transform.position, transform.rotation);
	}

	private void FixedUpdate()
	{
		ClientTick++;
		HandleInput();
	}

	void HandleInput()
	{
		if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
		{
			return;
		}

		bool[] inputs = new bool[4];
		inputs[0] = Input.GetKey(KeyCode.UpArrow);
		inputs[1] = Input.GetKey(KeyCode.LeftArrow);
		inputs[2] = Input.GetKey(KeyCode.DownArrow);
		inputs[3] = Input.GetKey(KeyCode.RightArrow);

		bool attackInput = Input.GetKey(KeyCode.Space);

		if (attackInput) // Slash/smash attack
		{
			if (!Anim.IsInTransition(0))
			{
				Anim.SetTrigger("slash");
			}
		}

		Vector3 movement = InputUtils.ComputeMovementFromInput(inputs[0], inputs[1], inputs[2], inputs[3]);

		// Calculate the Input Magnitude
		var moveInputMagnitude = new Vector2(movement.x, movement.z).sqrMagnitude;

		// Physically move player
		if (moveInputMagnitude > 0)
		{
			Debug.Log("movement = " + movement);
			Anim.SetFloat("Blend", moveInputMagnitude, startAnimTime, Time.deltaTime);
			// PlayerMoveAndRotation(movement);
			PlayerInputData inputData = new PlayerInputData(inputs, ClientTick);
			PlayerStateData nextStateData = PlayerLogic.GetNextFrameData(inputData, playerInterpolation.CurrentData);
			playerInterpolation.SetFramePosition(nextStateData);
			Debug.Log("TimeTick: " + ClientTick + ", StateData: " + nextStateData.Position.ToString("F8"));
		}
		else
		{
			Anim.SetFloat("Blend", moveInputMagnitude, stopAnimTime, Time.deltaTime);
		}
	}
}
