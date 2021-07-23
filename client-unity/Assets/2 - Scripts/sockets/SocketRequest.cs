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
}
