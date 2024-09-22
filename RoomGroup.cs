using System;

// Token: 0x0200018E RID: 398
[Serializable]
public class RoomGroup
{
	// Token: 0x0600091F RID: 2335 RVA: 0x000308D8 File Offset: 0x0002EAD8
	public RoomGroup()
	{
		this.name = "";
		this.potentialRooms = new WeightedRoomAsset[0];
		this.wallTexture = new WeightedTexture2D[0];
		this.floorTexture = new WeightedTexture2D[0];
		this.ceilingTexture = new WeightedTexture2D[0];
		this.light = new WeightedTransform[0];
		this.stickToHallChance = 1f;
		this.minRooms = 5;
		this.maxRooms = 10;
	}

	// Token: 0x04000A07 RID: 2567
	public string name = "Rooms";

	// Token: 0x04000A08 RID: 2568
	public WeightedRoomAsset[] potentialRooms;

	// Token: 0x04000A09 RID: 2569
	public WeightedTexture2D[] wallTexture;

	// Token: 0x04000A0A RID: 2570
	public WeightedTexture2D[] floorTexture;

	// Token: 0x04000A0B RID: 2571
	public WeightedTexture2D[] ceilingTexture;

	// Token: 0x04000A0C RID: 2572
	public WeightedTransform[] light;

	// Token: 0x04000A0D RID: 2573
	public float stickToHallChance = 1f;

	// Token: 0x04000A0E RID: 2574
	public int minRooms = 5;

	// Token: 0x04000A0F RID: 2575
	public int maxRooms = 10;
}
