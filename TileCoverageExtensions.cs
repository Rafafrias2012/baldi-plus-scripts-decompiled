using System;

// Token: 0x02000161 RID: 353
public static class TileCoverageExtensions
{
	// Token: 0x060007FE RID: 2046 RVA: 0x00027C00 File Offset: 0x00025E00
	public static CellCoverage Rotated(this CellCoverage coverage, Direction direction)
	{
		int num = (int)(coverage & (CellCoverage.Up | CellCoverage.Down | CellCoverage.Center));
		return (CellCoverage)(Directions.RotateBin((int)(coverage & (CellCoverage.North | CellCoverage.East | CellCoverage.South | CellCoverage.West)), direction) | num);
	}
}
