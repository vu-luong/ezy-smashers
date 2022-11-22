using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
	public UnityEvent<List<int>> mmoRoomIdListUpdateEvent;

	private void Awake()
	{
		CreateRoomResponseHandler.action += JoinRoom;
		GetMMORoomIdListResponseHandler.action += OnMMORoomIdListResponse;
		JoinMMORoomResponseHandler.action += JoinRoom;
	}

	private void UnregisterEvents()
	{
		CreateRoomResponseHandler.action -= JoinRoom;
		GetMMORoomIdListResponseHandler.action -= OnMMORoomIdListResponse;
		JoinMMORoomResponseHandler.action -= JoinRoom;
	}

	private void Start()
	{
		OnRefreshRoomIdList();
	}

	private void JoinRoom(int roomId)
	{
		RoomManager.getInstance().CurrentRoomId = roomId;
		UnregisterEvents();
		SceneManager.LoadScene("GameLoungeScene");
	}

	private void OnMMORoomIdListResponse(List<int> roomIdList)
	{
		mmoRoomIdListUpdateEvent?.Invoke(roomIdList);
	}

	#region public methods

	public void OnRefreshRoomIdList()
	{
		Debug.Log("LobbyController: OnRefreshRoomIdList");
		SocketRequest.getInstance().SendGetMMORoomIdListRequest();
	}

	public void OnCreateMMORoom()
	{
		Debug.Log("LobbyController: OnCreateMMORoom!");
		SocketRequest.getInstance().SendCreateMMORoomRequest();
	}

	public void RequestJoinMMORoom(int roomId)
	{
		SocketRequest.getInstance().SendJoinMMORoomRequest(roomId);
	}

	#endregion
}
