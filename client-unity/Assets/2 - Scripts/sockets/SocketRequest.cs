using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

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
	public void SendPlayerInputData(PlayerInputData inputData, Vector3 nextRotation)
	{
		var client = SocketProxy.getInstance().Client;
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("t", inputData.Time)
			.append("k", inputData.KeyInputs)
			.append("r",
			        EzyEntityFactory.newArrayBuilder()
				        .append(nextRotation.x)
				        .append(nextRotation.y)
				        .append(nextRotation.z)
				        .build()
			)
			.build();
		client.getApp().send(Commands.PLAYER_INPUT_DATA, data);
	}

	public void SendPlayerHit(string victimName, Vector3 attackPosition, int myClientTick, int otherClientTick)
	{
		var client = SocketProxy.getInstance().Client;
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("m", myClientTick)
			.append("o", otherClientTick)
			.append("v", victimName)
			.append("p",
			        EzyEntityFactory.newArrayBuilder()
				        .append(attackPosition.x)
				        .append(attackPosition.y)
				        .append(attackPosition.z)
				        .build()
			)
			.build();
		client.getApp().send(Commands.PLAYER_HIT, data);
	}
	public void SendPlayerAttackData(Vector3 attackPosition, int clientTick)
	{
		var client = SocketProxy.getInstance().Client;
		client.getApp().send(Commands.PLAYER_ATTACK_DATA);
	}

}
