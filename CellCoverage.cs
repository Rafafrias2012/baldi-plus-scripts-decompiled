using System;

// Token: 0x0200015E RID: 350
[Flags]
public enum CellCoverage
{
	// Token: 0x04000856 RID: 2134
	None = 0,
	// Token: 0x04000857 RID: 2135
	North = 1,
	// Token: 0x04000858 RID: 2136
	East = 2,
	// Token: 0x04000859 RID: 2137
	South = 4,
	// Token: 0x0400085A RID: 2138
	West = 8,
	// Token: 0x0400085B RID: 2139
	Up = 16,
	// Token: 0x0400085C RID: 2140
	Down = 32,
	// Token: 0x0400085D RID: 2141
	Center = 64
}
