using System;
using UnityEngine;

[CreateAssetMenu]
public class SocketConfigVariable : GenericVariable<SocketConfigVariable.SocketConfigModel>
{
	[Serializable]
	public class SocketConfigModel
	{
		[SerializeField] private string zoneName;
		[SerializeField] private string appName;
		[SerializeField] private string host;
		[SerializeField] private int udpPort;

		public string ZoneName => zoneName;
		public string AppName => appName;
		public string Host => host;
		public int UdpPort => udpPort;
	}
}

