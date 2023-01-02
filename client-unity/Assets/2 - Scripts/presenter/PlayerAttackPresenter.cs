using UnityEngine;

public class PlayerAttackPresenter : MonoBehaviour
{
	public void PlayerBeingAttacked(string victimName)
	{
		PlayerService.GetInstance()
			.GetPlayerByName(victimName)
			.OnBeingAttacked();
	}

	public void OtherPlayerAttack(string attackerName)
	{
		PlayerService.GetInstance()
			.GetPlayerByName(attackerName)
			.OnServerAttack();
	}
}
