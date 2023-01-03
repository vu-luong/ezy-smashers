using UnityEngine;

public class PlayerAttackPresenter : MonoBehaviour
{
	public void PlayerBeingAttacked(string victimName)
	{
		PlayerRepository.GetInstance()
			.GetPlayerByName(victimName)
			.OnBeingAttacked();
	}

	public void OtherPlayerAttack(string attackerName)
	{
		PlayerRepository.GetInstance()
			.GetPlayerByName(attackerName)
			.OnServerAttack();
	}
}
