using UnityEngine;

public class EzyDefaultSocketEventProcessor : MonoBehaviour
{
    private static EzyDefaultSocketEventProcessor _instance;

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
        EzyDefaultSocketManager.GetInstance()
            .SocketProxy
            .getClient()
            .processEvents();
    }
}
