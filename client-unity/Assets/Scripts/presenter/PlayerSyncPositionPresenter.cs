using UnityEngine;

public class PlayerSyncPositionPresenter : MonoBehaviour
{
	public void SyncPlayerPosition(PlayerSyncPositionModel model)
	{
		PlayerService.GetInstance()
			.GetPlayerByName(model.PlayerName)
			.OnServerDataUpdate(model.Position, model.Rotation, model.Time);
	}
}
