using System;
using System.Collections.Generic;

// Token: 0x0200012B RID: 299
[Serializable]
public class SavedGameData
{
	// Token: 0x06000733 RID: 1843 RVA: 0x00024E34 File Offset: 0x00023034
	public SavedGameData()
	{
		this.suspendedCharacters = new List<Character>();
		this.levelId = 0;
		this.ytps = 0;
		this.lives = 2;
		this.seed = 0;
		this.version = 0;
		this.saveAvailable = false;
		this.fieldTripPlayed = false;
		this.foundMapTiles = new bool[0];
		this.mapAvailable = false;
		this.mapPurchased = false;
		this.johnnyHelped = false;
		this.mapSizeX = 0;
		this.mapSizeZ = 0;
	}

	// Token: 0x040007A3 RID: 1955
	public int[] items = new int[9];

	// Token: 0x040007A4 RID: 1956
	public int[] lockerItems = new int[3];

	// Token: 0x040007A5 RID: 1957
	public List<Character> suspendedCharacters = new List<Character>();

	// Token: 0x040007A6 RID: 1958
	public int levelId;

	// Token: 0x040007A7 RID: 1959
	public string levelName = "Obsolete";

	// Token: 0x040007A8 RID: 1960
	public int ytps;

	// Token: 0x040007A9 RID: 1961
	public int lives;

	// Token: 0x040007AA RID: 1962
	public int seed;

	// Token: 0x040007AB RID: 1963
	public int version;

	// Token: 0x040007AC RID: 1964
	public bool saveAvailable;

	// Token: 0x040007AD RID: 1965
	public bool fieldTripPlayed;

	// Token: 0x040007AE RID: 1966
	public bool[] foundMapTiles = new bool[0];

	// Token: 0x040007AF RID: 1967
	public bool mapAvailable;

	// Token: 0x040007B0 RID: 1968
	public bool mapPurchased;

	// Token: 0x040007B1 RID: 1969
	public bool johnnyHelped;

	// Token: 0x040007B2 RID: 1970
	public int mapSizeX;

	// Token: 0x040007B3 RID: 1971
	public int mapSizeZ;
}
