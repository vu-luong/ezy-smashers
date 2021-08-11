using System;
using com.tvd12.ezyfoxserver.client.request;

public sealed class Commands
{
    public const String SYNC_POSITION = "s";
    public const String SYNC_DATA = "syncData";
    public const String JOIN_LOBBY = "joinLobby";
    public const String CREATE_MMO_ROOM = "createMMORoom";
    public const String GET_MMO_ROOM_ID_LIST = "getMMORoomIdList";
    public const String GET_MMO_ROOM_PLAYERS = "getMMORoomPlayers";
    public const String JOIN_MMO_ROOM = "joinMMORoom";
    public const String ANOTHER_JOIN_MMO_ROOM = "anotherJoinMMORoom";

    private Commands()
    {
    }
}
