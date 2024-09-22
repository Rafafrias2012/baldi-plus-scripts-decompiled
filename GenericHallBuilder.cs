using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class GenericHallBuilder : ObjectBuilder
{
	// Token: 0x060003DD RID: 989 RVA: 0x000142F6 File Offset: 0x000124F6
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		this.objectPlacer.Build(builder, room, cRng);
	}

	// Token: 0x04000411 RID: 1041
	[SerializeField]
	private ObjectPlacer objectPlacer;
}
