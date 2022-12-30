using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

public class SocketRequest : EzyLoggable
{
	private static readonly SocketRequest INSTANCE = new();
	private EzyAppProxy appProxy;

	public static SocketRequest getInstance()
	{
		return INSTANCE;
	}

	public SocketRequest()
	{
		appProxy = SocketManager.GetInstance()
			.AppProxy;
	}

	public void SendJoinLobbyRequest()
	{
		appProxy.send(Commands.JOIN_LOBBY);
	}

	public void SendCreateMMORoomRequest()
	{
		appProxy.send(Commands.CREATE_MMO_ROOM);
	}

	public void SendGetMMORoomIdListRequest()
	{
		appProxy.send(Commands.GET_MMO_ROOM_ID_LIST);
	}

	public void SendGetMMORoomPlayersRequest()
	{
		appProxy.send(Commands.GET_MMO_ROOM_PLAYERS);
	}

	public void SendJoinMMORoomRequest(int roomId)
	{
		EzyObject data = EzyEntityFactory.newObjectBuilder()
			.append("roomId", roomId)
			.build();
		appProxy.send(Commands.JOIN_MMO_ROOM, data);
	}

	public void SendStartGameRequest()
	{
		appProxy.send(Commands.START_GAME);
	}

	public void SendPlayerInputData(PlayerInputData inputData, Vector3 nextRotation)
	{
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("t", inputData.Time)
			.append("k", inputData.KeyInputs)
			.append(
				"r",
				EzyEntityFactory.newArrayBuilder()
					.append(nextRotation.x)
					.append(nextRotation.y)
					.append(nextRotation.z)
					.build()
			)
			.build();
		appProxy.send(Commands.PLAYER_INPUT_DATA, data);
	}

	public void SendPlayerHit(string victimName, Vector3 attackPosition, int myClientTick, int otherClientTick)
	{
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("m", myClientTick)
			.append("o", otherClientTick)
			.append("v", victimName)
			.append(
				"p",
				EzyEntityFactory.newArrayBuilder()
					.append(attackPosition.x)
					.append(attackPosition.y)
					.append(attackPosition.z)
					.build()
			)
			.build();
		appProxy.send(Commands.PLAYER_HIT, data);
	}
	public void SendPlayerAttackData(Vector3 attackPosition, int clientTick)
	{
		appProxy.send(Commands.PLAYER_ATTACK_DATA);
	}
}
