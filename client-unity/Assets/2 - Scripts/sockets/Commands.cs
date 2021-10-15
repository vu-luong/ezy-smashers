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
    public const String ANOTHER_EXIT_MMO_ROOM = "anotherExitMMORoom";
    public const String START_GAME = "startGame";

    public const String PLAYER_INPUT_DATA = "i";
    public const String PLAYER_HIT = "h";
    public const String PLAYER_BEING_ATTACKED = "b";
    public const String PLAYER_ATTACK_DATA = "a";

    private Commands()
    {
    }
}
