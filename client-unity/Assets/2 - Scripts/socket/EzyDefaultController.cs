using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using Object = System.Object;

public class EzyDefaultController : MonoBehaviour
{
	private readonly List<Tuple<String, Object>> handlers = new();
	protected readonly EzyLogger logger;

	public EzyDefaultController()
	{
		logger = EzyLoggerFactory.getLogger(GetType());
	}

	protected void AddHandler<T>(String cmd, EzyAppProxyDataHandler<T> handler)
	{
		handlers.Add(
			new Tuple<String, Object>(
				cmd,
				EzyDefaultSocketManager.GetInstance()
					.AppProxy
					.on(cmd, handler)
			)
		);
	}

	private void OnDestroy()
	{
		logger.debug("OnDestroy");
		foreach (Tuple<String, Object> tuple in handlers)
		{
			EzyDefaultSocketManager.GetInstance()
				.AppProxy.unbind(tuple.Item1, tuple.Item2);
		}
	}
}
