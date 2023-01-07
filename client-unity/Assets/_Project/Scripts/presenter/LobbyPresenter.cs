using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPresenter : MonoBehaviour
{
	public void PlayerJoinedMmoRoom(int roomId)
	{
		RoomRepository.GetInstance().UpdatePlayingRoomId(roomId);
		SceneManager.LoadScene("GameLoungeScene");
	}
}
