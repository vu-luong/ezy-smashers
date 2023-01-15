using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.unity;
using UnityEngine;
using UnityEngine.Events;

public class LobbyController : EzyDefaultController
{
	[SerializeField]
	private UnityEvent<List<int>> mmoRoomIdListUpdateEvent;

	[SerializeField]
	private UnityEvent<int> playerJoinedMmoRoomEvent;
	
	private void Awake()
	{
		base.Awake();
		addHandler<EzyObject>(Commands.CREATE_MMO_ROOM, JoinRoom);
		addHandler<EzyArray>(Commands.GET_MMO_ROOM_ID_LIST, OnMMORoomIdListResponse);
		addHandler<EzyObject>(Commands.JOIN_MMO_ROOM, JoinRoom);
	}

	private void Start()
	{
		RefreshRoomIdList();
	}

	private void JoinRoom(EzyAppProxy appProxy, EzyObject data)
	{
		int roomId = data.get<int>("roomId");
		logger.debug("JoinRoom roomId = " + roomId);
		playerJoinedMmoRoomEvent?.Invoke(roomId);
	}

	private void OnMMORoomIdListResponse(EzyAppProxy appProxy, EzyArray data)
	{
		logger.debug("OnMMORoomIdListResponse " + data);
		EzyArray roomIdArray = data.get<EzyArray>(0);
		List<int> roomIds = new List<int>();
		for (int i = 0; i < roomIdArray.size(); ++i)
		{
			roomIds.Add(roomIdArray.get<int>(i));
		}
		logger.debug("OnMMORoomIdListResponse roomIds = " + string.Join(", ", roomIds));
		mmoRoomIdListUpdateEvent?.Invoke(roomIds);
	}

	#region public methods

	public void RefreshRoomIdList()
	{
		logger.debug("OnRefreshRoomIdList");
		SocketRequest.GetInstance().SendGetMMORoomIdListRequest();
	}

	public void OnCreateMMORoom()
	{
		logger.debug("OnCreateMMORoom");
		SocketRequest.GetInstance().SendCreateMMORoomRequest();
	}

	public void RequestJoinMMORoom(int roomId)
	{
		logger.debug("RequestJoinMMORoom: roomId = " + roomId);
		SocketRequest.GetInstance().SendJoinMMORoomRequest(roomId);
	}

	#endregion
}
