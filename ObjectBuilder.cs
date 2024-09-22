using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class ObjectBuilder : MonoBehaviour
{
	// Token: 0x06000941 RID: 2369 RVA: 0x00031424 File Offset: 0x0002F624
	public virtual void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00031426 File Offset: 0x0002F626
	public virtual void Load(EnvironmentController ec, List<IntVector2> pos, List<Direction> dir)
	{
	}

	// Token: 0x04000A26 RID: 2598
	public Obstacle obstacle;
}
