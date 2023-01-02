using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInterpolation))]
public class ClientPlayer : MonoBehaviour
{
	private bool isMyPlayer;
	[SerializeField]
	private Transform lookPoint;
	[SerializeField]
	private Transform attackPoint;

	private bool isDead = false;
	private bool allowedOtherPlayerTick = false;

	[SerializeField]
	private Hammer hammer;

	[SerializeField]
	// private SkinnedMeshRenderer renderer;
	Renderer[] characterMaterials;

	[Space]
	[Header("Animation Smoothing")]
	[Range(0, 1f)]
	public float startAnimTime = 0.1f;
	[Range(0, 1f)]
	public float stopAnimTime = 0.15f;

	private Animator anim;
	private PlayerInterpolation playerInterpolation;

	private Queue<ReconciliationInfo> reconciliationHistory = new();
	private Color eyeColor;

	public int ClientTick { get; set; }

	public bool IsMyPlayer => isMyPlayer;

	public Animator Anim => anim;

	public Transform LookPoint => lookPoint;

	public Transform AttackPoint => attackPoint;

	[SerializeField]
	private UnityEvent<Vector3, int> playerAttackEvent;

	[SerializeField]
	private UnityEvent<PlayerInputModel, Quaternion> playerInputEvent;

	[SerializeField]
	private UnityEvent deadEvent;

	// Use this for initialization
	void Awake()
	{
		anim = GetComponent<Animator>();
		playerInterpolation = GetComponent<PlayerInterpolation>();
		eyeColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1.0f);
	}

	private void FixedUpdate()
	{
		if (isDead)
		{
			Anim.SetFloat("Blend", 0, stopAnimTime, Time.deltaTime);
			return;
		}

		if (IsMyPlayer)
		{
			ClientTick++;
			HandleInput();
		}
		else
		{
			if (allowedOtherPlayerTick)
			{
				ClientTick++;
			}
		}
	}

	void HandleInput()
	{
		if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
		{
			// Stop run animation when attacking
			Anim.SetFloat("Blend", 0, stopAnimTime, Time.deltaTime);
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
			// Only allow new attack when finishing previous attack
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

		// Use PlayerInterpolation to smooth out movement
		if (moveInputMagnitude > 0)
		{
			Debug.Log("movement = " + movement);
			Anim.SetFloat("Blend", moveInputMagnitude, startAnimTime, Time.deltaTime);

			PlayerInputModel playerInput = new PlayerInputModel(inputs, ClientTick);
			PlayerStateModel nextPlayerState = PlayerLogic.GetPlayerStateOfNextFrame(playerInput, playerInterpolation.CurrentPlayerState);
			playerInterpolation.SetFramePosition(nextPlayerState);
			playerInputEvent?.Invoke(playerInput, nextPlayerState.Rotation);
			Debug.Log("TimeTick: " + ClientTick + ", StateData: " + nextPlayerState.Position.ToString("F8"));
			reconciliationHistory.Enqueue(new ReconciliationInfo(ClientTick, nextPlayerState, playerInput));
		}
		else
		{
			Anim.SetFloat("Blend", 0, stopAnimTime, Time.deltaTime);
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
				if (Vector3.Distance(info.PlayerState.Position, position) > 0.05f)
				{
					Debug.Log("SERVER RECONCILIATION! server position = " + position + ", client position = " + info.PlayerState.Position);
					List<ReconciliationInfo> infos = reconciliationHistory.ToList();
					playerInterpolation.CurrentPlayerState.Position = position;
					playerInterpolation.CurrentPlayerState.Rotation = info.PlayerState.Rotation;
					transform.position = playerInterpolation.CurrentPlayerState.Position;
					transform.rotation = playerInterpolation.CurrentPlayerState.Rotation;

					for (int i = 0; i < infos.Count; i++)
					{
						PlayerStateModel playerState =
							PlayerLogic.GetPlayerStateOfNextFrame(infos[i].PlayerInput, playerInterpolation.CurrentPlayerState);
						playerInterpolation.SetFramePosition(playerState);
					}
				}
			}
		}
		else
		{
			allowedOtherPlayerTick = true;
			StartCoroutine(OtherPlayerUpdateTimeTick(time));
			playerInterpolation.SetFramePosition(new PlayerStateModel(position, Quaternion.Euler(rotation)));
			// Debug.Log("OnServerDataUpdate" + ClientTick + ", time = " + time);
		}
	}

	/**
	 * The time tick received from server is corresponding to the t = 1 in PlayerInterpolation,
	 * and t = 1 when time lasts for SERVER_FIXED_DELTA_TIME
	 */
	IEnumerator OtherPlayerUpdateTimeTick(int time)
	{
		yield return new WaitForSeconds(GameConstants.SERVER_FIXED_DELTA_TIME);
		ClientTick = time;
		allowedOtherPlayerTick = false;
		// Debug.Log("OtherPlayerUpdateTimeTick " + ClientTick);
	}

	public void Initialize(PlayerSpawnInfoModel playerSpawnInfo, bool isMyPlayer)
	{
		this.isMyPlayer = isMyPlayer;
		ClientTick = 0;
		playerInterpolation.CurrentPlayerState = new PlayerStateModel(playerSpawnInfo.Position, transform.rotation);
		playerInterpolation.PreviousPlayerState = new PlayerStateModel(playerSpawnInfo.Position, transform.rotation);

		characterMaterials = GetComponentsInChildren<Renderer>();
		Color mainColor = new Color(
			playerSpawnInfo.PlayerColor.x,
			playerSpawnInfo.PlayerColor.y,
			playerSpawnInfo.PlayerColor.z,
			1.0f
		);
		for (int i = 0; i < characterMaterials.Length; i++)
		{
			if (characterMaterials[i].transform.CompareTag("PlayerEyes"))
				characterMaterials[i].material.SetColor("_EmissionColor", eyeColor);
			else
				characterMaterials[i].material.SetColor("_Color", mainColor);
		}
	}

	public void OnBeingAttacked()
	{
		isDead = true;
		StartCoroutine(BeingAttackCoroutine());
	}


	IEnumerator BeingAttackCoroutine()
	{
		transform.localScale = new Vector3(1.0f, 0.2f, 1.0f);
		yield return new WaitForSeconds(0.1f);
		if (IsMyPlayer)
		{
			deadEvent?.Invoke();
		}
	}

	public void OnServerAttack()
	{
		if (!isMyPlayer)
		{
			Anim.SetTrigger("slash");
		}
	}
}
