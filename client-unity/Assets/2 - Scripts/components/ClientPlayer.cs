using System.Collections.Generic;
using System.Linq;
using _2___Scripts.shared;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInterpolation))]
public class ClientPlayer : MonoBehaviour
{
	private string playerName;
	private bool isMyPlayer;

	[Space]
	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float startAnimTime = 0.1f;
	[Range(0, 1f)]
	public float stopAnimTime = 0.15f;

	private Animator anim;
	private PlayerInterpolation playerInterpolation;
	public static UnityAction<PlayerInputData, Quaternion> playerInputEvent;

	private Queue<ReconciliationInfo> reconciliationHistory = new Queue<ReconciliationInfo>();

	public int ClientTick { get; set; }

	public bool IsMyPlayer => isMyPlayer;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		playerInterpolation = GetComponent<PlayerInterpolation>();
		// playerInterpolation.CurrentData = new PlayerStateData(transform.position, transform.rotation);
		// playerInterpolation.PreviousData = new PlayerStateData(transform.position, transform.rotation);
	}

	private void FixedUpdate()
	{
		ClientTick++;
		if (IsMyPlayer)
		{
			InputMagnitude();
		}
	}

	private void Update()
	{
		if (IsMyPlayer)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && !anim.IsInTransition(0))
				{
					anim.SetTrigger("slash");
				}
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
			Debug.Log("movement = " + movement);
			anim.SetFloat("Blend", moveInputMagnitude, startAnimTime, Time.deltaTime);
			// PlayerMoveAndRotation(movement);
			PlayerInputData inputData = new PlayerInputData(inputs, ClientTick);
			PlayerStateData nextStateData = PlayerLogic.GetNextFrameData(inputData, playerInterpolation.CurrentData);
			playerInterpolation.SetFramePosition(nextStateData);
			playerInputEvent?.Invoke(inputData, nextStateData.Rotation);
			Debug.Log("TimeTick: " + ClientTick + ", StateData: " + nextStateData.Position.ToString("F8"));
			reconciliationHistory.Enqueue(new ReconciliationInfo(ClientTick, nextStateData, inputData));
		}
		else
		{
			anim.SetFloat("Blend", moveInputMagnitude, stopAnimTime, Time.deltaTime);
		}
	}

	public void OnServerDataUpdate(Vector3 position, Vector3 rotation, int time)
	{
		if (IsMyPlayer)
		{
			while (reconciliationHistory.Any() && reconciliationHistory.Peek().TimeTick < time)
			{
				reconciliationHistory.Dequeue();
			}

			if (reconciliationHistory.Any() && reconciliationHistory.Peek().TimeTick == time)
			{
				var info = reconciliationHistory.Dequeue();
				if (Vector3.Distance(info.StateData.Position, position) > 0.05f)
				{
					Debug.Log("SERVER RECONCILIATION! server position = " + position + ", client position = " + info.StateData.Position);
					List<ReconciliationInfo> infos = reconciliationHistory.ToList();
					playerInterpolation.CurrentData.Position = position;
					playerInterpolation.CurrentData.Rotation = info.StateData.Rotation;
					transform.position = playerInterpolation.CurrentData.Position;
					transform.rotation = playerInterpolation.CurrentData.Rotation;

					for (int i = 0; i < infos.Count; i++)
					{
						PlayerStateData u = PlayerLogic.GetNextFrameData(infos[i].InputData, playerInterpolation.CurrentData);
						playerInterpolation.SetFramePosition(u);
					}
				}
			}
		}
		else
		{
			playerInterpolation.SetFramePosition(new PlayerStateData(position, Quaternion.Euler(rotation)));
		}
	}
	public void Initialize(PlayerSpawnData playerSpawnData, bool isMyPlayer)
	{
		playerName = playerSpawnData.playerName;
		this.isMyPlayer = isMyPlayer;
		playerInterpolation.CurrentData = new PlayerStateData(playerSpawnData.position, transform.rotation);
		playerInterpolation.PreviousData = new PlayerStateData(playerSpawnData.position, transform.rotation);
	}
}
