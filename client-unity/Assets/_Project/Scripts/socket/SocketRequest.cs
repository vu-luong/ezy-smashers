using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.unity;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

public sealed class SocketRequest : EzyLoggable
{
	private static readonly SocketRequest INSTANCE = new();

	private AbstractEzySocketManager SocketManager => EzySingletonSocketManager.GetInstance();

	public static SocketRequest GetInstance()
	{
		return INSTANCE;
	}

	public void SendJoinLobbyRequest()
	{
		SocketManager.Send(Commands.JOIN_LOBBY);
	}

	public void SendCreateMMORoomRequest()
	{
		SocketManager.Send(Commands.CREATE_MMO_ROOM);
	}

	public void SendGetMMORoomIdListRequest()
	{
		SocketManager.Send(Commands.GET_MMO_ROOM_ID_LIST);
	}

	public void SendGetMMORoomPlayersRequest()
	{
		SocketManager.Send(Commands.GET_MMO_ROOM_PLAYERS);
	}

	public void SendJoinMMORoomRequest(int roomId)
	{
		EzyObject data = EzyEntityFactory.newObjectBuilder()
			.append("roomId", roomId)
			.build();
		SocketManager.Send(Commands.JOIN_MMO_ROOM, data);
	}

	public void SendStartGameRequest()
	{
		SocketManager.Send(Commands.START_GAME);
	}

	public void SendPlayerInputData(PlayerInputModel playerInput, Vector3 nextRotation)
	{
		EzyObject data = EzyEntityFactory
			.newObjectBuilder()
			.append("t", playerInput.Time)
			.append("k", playerInput.KeyInputs)
			.append(
				"r",
				EzyEntityFactory.newArrayBuilder()
					.append(nextRotation.x)
					.append(nextRotation.y)
					.append(nextRotation.z)
					.build()
			)
			.build();
		SocketManager.Send(Commands.PLAYER_INPUT_DATA, data);
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
		SocketManager.Send(Commands.PLAYER_HIT, data);
	}
	public void SendPlayerAttackData(Vector3 attackPosition, int clientTick)
	{
		SocketManager.Send(Commands.PLAYER_ATTACK_DATA);
	}
}
