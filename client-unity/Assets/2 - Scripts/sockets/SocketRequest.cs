using System;
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

    public void sendAppAccessRequest()
    {
        var client = SocketProxy.getInstance().Client;
        var request = new EzyAppAccessRequest(SocketProxy.APP_NAME);
        client.send(request);
    }

    public void sendJoinLobbyRequest()
    {
        var client = SocketProxy.getInstance().Client;
        client.getApp().send(Commands.JOIN_LOBBY);
    }

    public void sendCreateMMORoomRequest()
    {
        var client = SocketProxy.getInstance().Client;
        client.getApp().send(Commands.CREATE_MMO_ROOM);
    }

    public void sendGetMMORoomIdListRequest()
    {
        var client = SocketProxy.getInstance().Client;
        client.getApp().send(Commands.GET_MMO_ROOM_ID_LIST);
    }

    public void sendGetMMORoomPlayersRequest()
    {
        var client = SocketProxy.getInstance().Client;
        client.getApp().send(Commands.GET_MMO_ROOM_PLAYERS);
    }

    public void sendJoinMMORoomRequest(int roomId)
    {
        var client = SocketProxy.getInstance().Client;
        EzyObject data = EzyEntityFactory
            .newObjectBuilder()
            .append("roomId", roomId)
            .build();
        client.getApp().send(Commands.JOIN_MMO_ROOM, data);
    }
}
