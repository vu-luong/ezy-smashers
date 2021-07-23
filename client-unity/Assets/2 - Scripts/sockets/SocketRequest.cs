using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.util;

public class SocketRequest : EzyLoggable
{
    private static readonly SocketRequest INSTANCE = new SocketRequest();

    public static SocketRequest getInstance()
    {
        return INSTANCE;
    }

    public void SendPluginInfoRequest(string pluginName)
    {
        var client = SocketProxy.getInstance().Client;

        var request = EzyEntityFactory.newArrayBuilder()
            .append(pluginName)
            .build();

        client.send(EzyCommand.PLUGIN_INFO, request);
    }
}
