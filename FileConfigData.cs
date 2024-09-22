using System;
using System.Collections.Generic;

// Token: 0x0200012C RID: 300
[Serializable]
public class FileConfigData
{
	// Token: 0x040007B4 RID: 1972
	public float saveVersion;

	// Token: 0x040007B5 RID: 1973
	public Language language;

	// Token: 0x040007B6 RID: 1974
	public float mouseCameraSensitivity = 2f;

	// Token: 0x040007B7 RID: 1975
	public float mouseCursorSensitivity = 1f;

	// Token: 0x040007B8 RID: 1976
	public float controllerCameraSensitivity = 400f;

	// Token: 0x040007B9 RID: 1977
	public float controllerCursorSensitivity = 400f;

	// Token: 0x040007BA RID: 1978
	public float[] volume = new float[3];

	// Token: 0x040007BB RID: 1979
	public int resolutionX;

	// Token: 0x040007BC RID: 1980
	public int resolutionY;

	// Token: 0x040007BD RID: 1981
	public bool rumble;

	// Token: 0x040007BE RID: 1982
	public bool analogMovement;

	// Token: 0x040007BF RID: 1983
	public bool antiAnnoyance;

	// Token: 0x040007C0 RID: 1984
	public bool subtitles;

	// Token: 0x040007C1 RID: 1985
	public bool crtFilter;

	// Token: 0x040007C2 RID: 1986
	public bool fullscreen = true;

	// Token: 0x040007C3 RID: 1987
	public bool vsync = true;

	// Token: 0x040007C4 RID: 1988
	public bool pixelFilter = true;

	// Token: 0x040007C5 RID: 1989
	public List<ControllerData> controllers;
}
