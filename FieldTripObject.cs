using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
[CreateAssetMenu(fileName = "FieldTrip", menuName = "Custom Assets/Field Trip Data Object", order = 4)]
public class FieldTripObject : ScriptableObject
{
	// Token: 0x04000CEA RID: 3306
	public RoomAssetPlacementData tripHub;

	// Token: 0x04000CEB RID: 3307
	public FieldTrips trip;

	// Token: 0x04000CEC RID: 3308
	public SoundObject intro;

	// Token: 0x04000CED RID: 3309
	public Cubemap skybox;

	// Token: 0x04000CEE RID: 3310
	public Direction spawnDirection;

	// Token: 0x04000CEF RID: 3311
	public Vector3 spawnPoint = new Vector3(165f, 5f, 325f);

	// Token: 0x04000CF0 RID: 3312
	public string animationName;
}
