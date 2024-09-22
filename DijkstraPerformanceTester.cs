using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class DijkstraPerformanceTester : MonoBehaviour
{
	// Token: 0x06000B5B RID: 2907 RVA: 0x0003BE86 File Offset: 0x0003A086
	private void Start()
	{
		this.ec = Singleton<CoreGameManager>.Instance.GetPlayer(0).ec;
		this.map = new DijkstraMap(this.ec, PathType.Nav, new Transform[]
		{
			base.transform
		});
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0003BEBF File Offset: 0x0003A0BF
	private void Update()
	{
		this.map.Calculate();
	}

	// Token: 0x04000DA9 RID: 3497
	private EnvironmentController ec;

	// Token: 0x04000DAA RID: 3498
	private DijkstraMap map;
}
