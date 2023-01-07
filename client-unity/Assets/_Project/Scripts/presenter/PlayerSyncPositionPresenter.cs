using System.Collections.Generic;
using UnityEngine;

public class PlayerSyncPositionPresenter : MonoBehaviour
{
	private Dictionary<string, Queue<ReconciliationModel>> reconciliationHistoryByPlayerName;

	public void SyncPlayerPosition(PlayerSyncPositionModel model)
	{
		PlayerRepository.GetInstance()
			.GetPlayerByName(model.PlayerName)
			.OnServerDataUpdate(model.Position, model.Rotation, model.Time);
	}
}
