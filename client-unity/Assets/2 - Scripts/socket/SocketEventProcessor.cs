using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;

public class SocketEventProcessor : MonoBehaviour
{
    private static SocketEventProcessor _instance;

    private void Awake()
    {
        // If go back to current scene, don't make duplication
        if (_instance != null)
        {
            Destroy(gameObject);
        } else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Main thread pulls data from socket
        SocketProxyManager.getInstance()
            .getDefaultSocketProxy()
            .getClient()
            .processEvents();
    }
}
