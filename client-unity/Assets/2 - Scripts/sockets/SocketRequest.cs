using System;
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
}
