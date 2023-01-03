using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPresenter : MonoBehaviour
{
	public void PlayerJoinedMmoRoom(int roomId)
	{
		RoomService.GetInstance().SetPlayingRoomId(roomId);
		SceneManager.LoadScene("GameLoungeScene");
	}
}
