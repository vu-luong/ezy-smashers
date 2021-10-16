using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hammer : MonoBehaviour
{
	public ClientPlayer clientPlayer;
	private bool hasEntered;
	HashSet<string> playersBeingAttacked = new HashSet<string>();

	public UnityAction<string, Vector3, int, int> PlayerHitEvent { get; set; }

	private void OnCollisionEnter(Collision other)
	{
		if (!clientPlayer.IsMyPlayer) return;
		if (other.gameObject.CompareTag("Player"))
		{
			if (other.gameObject.GetComponent<ClientPlayer>().IsMyPlayer) return;
			if (!playersBeingAttacked.Contains(other.gameObject.name))
			{
				Debug.Log("OnCollisionEnter");
				Debug.Log(other.gameObject.name);
				playersBeingAttacked.Add(other.gameObject.name);
				// Invoke event
				PlayerHitEvent?.Invoke(other.gameObject.name,
				                       clientPlayer.AttackPoint.position,
				                       clientPlayer.ClientTick,
				                       other.gameObject.GetComponent<ClientPlayer>().ClientTick);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!clientPlayer.Anim.GetCurrentAnimatorStateInfo(0).IsName("Slash") && playersBeingAttacked.Count > 0)
		{
			playersBeingAttacked.Clear();
		}
	}
}
