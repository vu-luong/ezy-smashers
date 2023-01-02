
using UnityEngine;

public class PlayerHitModel
{
	public string VictimName { get; set; }
	public Vector3 AttackPosition { get; set; }
	public int AttackerTick { get; set; }
	public int VictimTick { get; set; }

	public PlayerHitModel(string victimName, Vector3 attackPosition, int attackerTick, int victimTick)
	{
		VictimName = victimName;
		AttackPosition = attackPosition;
		AttackerTick = attackerTick;
		VictimTick = victimTick;
	}
}