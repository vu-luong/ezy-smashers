using System;
using com.tvd12.ezyfoxserver.client.logger;
using UnityEngine;

[CreateAssetMenu]
public class SocketConfigVariable : GenericVariable<SocketConfigVariable.SocketConfigModel>
{
	[Serializable]
	public class SocketConfigModel
	{
		[SerializeField] private string zoneName;
		[SerializeField] private string appName;
		[SerializeField] private EzyLoggerLevel loggerLevel;

		public string ZoneName => zoneName;
		public string AppName => appName;
		public EzyLoggerLevel LoggerLevel => loggerLevel;
	}
}

