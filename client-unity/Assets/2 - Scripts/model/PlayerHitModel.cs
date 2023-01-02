using UnityEngine;

public class PlayerHitModel
{
	public string VictimName { get; }
	public Vector3 AttackPosition { get; }
	public int AttackerTick { get; }
	public int VictimTick { get; }

	public PlayerHitModel(string victimName, Vector3 attackPosition, int attackerTick, int victimTick)
	{
		VictimName = victimName;
		AttackPosition = attackPosition;
		AttackerTick = attackerTick;
		VictimTick = victimTick;
	}
}