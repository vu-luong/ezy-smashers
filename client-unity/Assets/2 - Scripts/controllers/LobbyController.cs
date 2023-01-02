using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : DefaultMonoBehaviour
{
	public UnityEvent<List<int>> mmoRoomIdListUpdateEvent;

	private void Start()
	{
		AddHandler<EzyObject>(Commands.CREATE_MMO_ROOM, JoinRoom);
		AddHandler<EzyArray>(Commands.GET_MMO_ROOM_ID_LIST, OnMMORoomIdListResponse);
		AddHandler<EzyObject>(Commands.JOIN_MMO_ROOM, JoinRoom);
		RefreshRoomIdList();
	}

	private void JoinRoom(EzyAppProxy appProxy, EzyObject data)
	{
		int roomId = data.get<int>("roomId");
		logger.debug("JoinRoom roomId = " + roomId);
		RoomManager.GetInstance().CurrentRoomId = roomId;
		SceneManager.LoadScene("GameLoungeScene");
	}

	private void OnMMORoomIdListResponse(EzyAppProxy appProxy, EzyArray data)
	{
		List<int> roomIdList = data.get<EzyArray>(0).toList<int>();
		logger.debug("OnMMORoomIdListResponse roomIdList = " + string.Join(", ", roomIdList));
		mmoRoomIdListUpdateEvent?.Invoke(roomIdList);
	}

	#region public methods

	public void RefreshRoomIdList()
	{
		logger.debug("OnRefreshRoomIdList");
		SocketRequest.getInstance().SendGetMMORoomIdListRequest();
	}

	public void OnCreateMMORoom()
	{
		logger.debug("OnCreateMMORoom");
		SocketRequest.getInstance().SendCreateMMORoomRequest();
	}

	public void RequestJoinMMORoom(int roomId)
	{
		SocketRequest.getInstance().SendJoinMMORoomRequest(roomId);
	}

	#endregion
}
