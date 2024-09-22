using System;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class GameObjectUtilties
{
	// Token: 0x06000363 RID: 867 RVA: 0x00011C90 File Offset: 0x0000FE90
	public static string BuildPathFromGameObject(GameObject go, string path = "")
	{
		path = "/" + go.name + path;
		if (go.transform.parent != null)
		{
			path = GameObjectUtilties.BuildPathFromGameObject(go.transform.parent.gameObject, path);
		}
		return path;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x00011CDC File Offset: 0x0000FEDC
	public static string BuildPathFromObject(Object o, string path = "")
	{
		if (o is GameObject)
		{
			return GameObjectUtilties.BuildPathFromGameObject(o as GameObject, path);
		}
		if (o is Component)
		{
			return GameObjectUtilties.BuildPathFromGameObject((o as Component).gameObject, path) + "/" + o.GetType().Name;
		}
		Debug.LogWarning("ReferencedObject is not a GameObject nor a Component, so path cannot be built... will attempt to index by name and type instead");
		return o.name;
	}
}
