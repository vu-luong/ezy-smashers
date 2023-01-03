using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPresenter : MonoBehaviour
{
	public void MyPlayerJoinedLobby(string playerName)
	{
		PlayerService.GetInstance()
			.UpdateMyPlayer(new PlayerModel(playerName, false));
		SceneManager.LoadScene("LobbyScene");
	}
}
