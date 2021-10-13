using UnityEngine;

public class Hammer : MonoBehaviour
{
	private bool hasEntered;
	
	private void OnCollisionEnter(Collision other)
	{
		// Check if is attacking --> update GameControllers.BeingAttackedList
		// When finish attack
		Debug.Log("OnCollisionEnter");
		Debug.Log(other.gameObject.name);
	}
}
