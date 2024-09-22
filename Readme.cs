using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class Readme : ScriptableObject
{
	// Token: 0x040003AA RID: 938
	public Texture2D icon;

	// Token: 0x040003AB RID: 939
	public string title;

	// Token: 0x040003AC RID: 940
	public Readme.Section[] sections;

	// Token: 0x040003AD RID: 941
	public bool loadedLayout;

	// Token: 0x02000340 RID: 832
	[Serializable]
	public class Section
	{
		// Token: 0x04001B40 RID: 6976
		public string heading;

		// Token: 0x04001B41 RID: 6977
		public string text;

		// Token: 0x04001B42 RID: 6978
		public string linkText;

		// Token: 0x04001B43 RID: 6979
		public string url;
	}
}
