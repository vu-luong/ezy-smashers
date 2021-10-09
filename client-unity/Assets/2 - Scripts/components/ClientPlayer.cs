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
	[SerializeField]
	private Transform lookPoint;
	[SerializeField]
	private Transform attackPoint;

	[Space]
	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float startAnimTime = 0.1f;
	[Range(0, 1f)]
	public float stopAnimTime = 0.15f;

	private Animator anim;
	private PlayerInterpolation playerInterpolation;
	public static UnityAction<PlayerInputData, Quaternion> playerInputEvent;
	public static UnityAction<Vector3, int> playerAttackEvent;

	private Queue<ReconciliationInfo> reconciliationHistory = new Queue<ReconciliationInfo>();

	public int ClientTick { get; set; }

	public bool IsMyPlayer => isMyPlayer;

	public Animator Anim => anim;

	public Transform LookPoint => lookPoint;

	public Transform AttackPoint => attackPoint;

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
			HandleInput();
		}
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
				playerAttackEvent?.Invoke(attackPoint.transform.position, ClientTick);
			}
			else
			{
				Debug.Log("Den day roi!!!!!" + Time.time);
				Debug.Log(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
				Debug.Log(Anim.IsInTransition(0));
				Debug.Log("isSlash = " + Anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"));
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
			playerInputEvent?.Invoke(inputData, nextStateData.Rotation);
			Debug.Log("TimeTick: " + ClientTick + ", StateData: " + nextStateData.Position.ToString("F8"));
			reconciliationHistory.Enqueue(new ReconciliationInfo(ClientTick, nextStateData, inputData));
		}
		else
		{
			Anim.SetFloat("Blend", moveInputMagnitude, stopAnimTime, Time.deltaTime);
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
