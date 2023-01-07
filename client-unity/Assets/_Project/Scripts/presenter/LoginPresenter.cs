using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPresenter : MonoBehaviour
{
	public void MyPlayerJoinedLobby(string playerName)
	{
		PlayerRepository.GetInstance()
			.UpdateMyPlayer(new PlayerModel(playerName, false));
		SceneManager.LoadScene("LobbyScene");
	}
}
