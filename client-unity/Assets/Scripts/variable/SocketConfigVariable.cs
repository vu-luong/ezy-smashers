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

		public string ZoneName => zoneName;
		public string AppName => appName;
	}
}

