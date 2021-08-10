using System;
using com.tvd12.ezyfoxserver.client.request;

public sealed class Commands
{
    public const String SYNC_POSITION = "s";
    public const String SYNC_DATA = "syncData";
    public const String JOIN_LOBBY = "joinLobby";
    public const String CREATE_MMO_ROOM = "createMMORoom";
    public const String GET_MMO_ROOM_NAMES = "getMMORoomNames";

    private Commands()
    {
    }
}

