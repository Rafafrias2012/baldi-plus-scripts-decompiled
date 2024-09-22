using System;

// Token: 0x0200012A RID: 298
[Serializable]
internal class PlayerFileData
{
	// Token: 0x04000786 RID: 1926
	public float saveVersion;

	// Token: 0x04000787 RID: 1927
	public Language subLanguage;

	// Token: 0x04000788 RID: 1928
	public Language textureLanguage;

	// Token: 0x04000789 RID: 1929
	public Language speechLangauge;

	// Token: 0x0400078A RID: 1930
	public float mouseCameraSensitivity = 2f;

	// Token: 0x0400078B RID: 1931
	public float mouseCursorSensitivity = 1f;

	// Token: 0x0400078C RID: 1932
	public float controllerCameraSensitivity = 400f;

	// Token: 0x0400078D RID: 1933
	public float controllerCursorSensitivity = 400f;

	// Token: 0x0400078E RID: 1934
	public float[] volume = new float[3];

	// Token: 0x0400078F RID: 1935
	public int resolutionX;

	// Token: 0x04000790 RID: 1936
	public int resolutionY;

	// Token: 0x04000791 RID: 1937
	public bool rumble;

	// Token: 0x04000792 RID: 1938
	public bool analogMovement;

	// Token: 0x04000793 RID: 1939
	public bool antiAnnoyance;

	// Token: 0x04000794 RID: 1940
	public bool subtitles;

	// Token: 0x04000795 RID: 1941
	public bool crtFilter;

	// Token: 0x04000796 RID: 1942
	public bool fullscreen = true;

	// Token: 0x04000797 RID: 1943
	public bool vsync = true;

	// Token: 0x04000798 RID: 1944
	public bool pixelFilter = true;

	// Token: 0x04000799 RID: 1945
	public bool reduceFlashing;

	// Token: 0x0400079A RID: 1946
	public bool[] foundChars = new bool[32];

	// Token: 0x0400079B RID: 1947
	public bool[] foundEvnts = new bool[32];

	// Token: 0x0400079C RID: 1948
	public bool[] foundItems = new bool[64];

	// Token: 0x0400079D RID: 1949
	public bool[] foundObstcls = new bool[64];

	// Token: 0x0400079E RID: 1950
	public bool[] foundTrips = new bool[16];

	// Token: 0x0400079F RID: 1951
	public bool[] wonChallenges = new bool[64];

	// Token: 0x040007A0 RID: 1952
	public bool[] clearedLevels = new bool[16];

	// Token: 0x040007A1 RID: 1953
	public AchievementData achievementData;

	// Token: 0x040007A2 RID: 1954
	public SavedGameData savedGameData;
}
