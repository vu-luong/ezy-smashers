using System;
using _2___Scripts.shared;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;

public class SocketRequest : EzyLoggable
{
	private static readonly SocketRequest INSTANCE = new SocketRequest();

	public static SocketRequest getInstance()
	{
		return INSTANCE;
	}

	public void SendAppAccessRequest()
	{
		var client = SocketProxy.getInstance().Client;
		var request = new EzyAppAccessRequest(SocketProxy.APP_NAME);
		client.send(request);
	}

	public void SendJoinLobbyRequest()
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.JOIN_LOBBY);
	}

	public void SendCreateMMORoomRequest()
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.CREATE_MMO_ROOM);
	}

	public void SendGetMMORoomIdListRequest()
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.GET_MMO_ROOM_ID_LIST);
	}

	public void SendGetMMORoomPlayersRequest()
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.GET_MMO_ROOM_PLAYERS);
	}

	public void SendJoinMMORoomRequest(int roomId)
	{
		var client = SocketProxy.getInstance().Client;
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("roomId", roomId)
			.build();
		client.getApp().send(Commands.JOIN_MMO_ROOM, data);
	}

	public void SendStartGameRequest()
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.START_GAME);
	}
	public void SendPlayerInputData(PlayerInputData inputData)
	{
		var client = SocketProxy.getInstance().Client;
		EzyArray data = EzyEntityFactory
			.newArrayBuilder()
			.append(inputData.KeyInputs)
			.build();
		client.getApp().send(Commands.PLAYER_INPUT_DATA, data);
	}
}
