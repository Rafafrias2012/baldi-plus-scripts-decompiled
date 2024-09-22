using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class ChalkboardBuilderFunction : RoomFunction
{
	// Token: 0x06000999 RID: 2457 RVA: 0x000333C4 File Offset: 0x000315C4
	public override void Build(LevelBuilder builder, Random rng)
	{
		base.Build(builder, rng);
		List<Cell> tilesOfShape = this.room.GetTilesOfShape(new List<TileShape>
		{
			TileShape.Single
		}, true);
		for (int i = 0; i < tilesOfShape.Count; i++)
		{
			if (!tilesOfShape[i].HasFreeWall)
			{
				tilesOfShape.RemoveAt(i);
				i--;
			}
		}
		if (tilesOfShape.Count > 0)
		{
			Cell cell = tilesOfShape[rng.Next(0, tilesOfShape.Count)];
			EnvironmentController ec = this.room.ec;
			WeightedSelection<PosterObject>[] items = this.chalkBoards;
			ec.BuildPoster(WeightedSelection<PosterObject>.ControlledRandomSelection(items, rng), cell, cell.RandomUncoveredDirection(rng));
		}
	}

	// Token: 0x04000AD3 RID: 2771
	[SerializeField]
	private WeightedPosterObject[] chalkBoards = new WeightedPosterObject[0];
}
