using System;
using System.Collections.Generic;

// Token: 0x020001A7 RID: 423
public class CharacterPostersRoomFunction : RoomFunction
{
	// Token: 0x0600099B RID: 2459 RVA: 0x00033474 File Offset: 0x00031674
	public override void Build(LevelBuilder builder, Random rng)
	{
		base.Build(builder, rng);
		List<Cell> tilesOfShape = this.room.GetTilesOfShape(new List<TileShape>
		{
			TileShape.Single,
			TileShape.Corner
		}, true);
		List<PosterObject> list = new List<PosterObject>();
		foreach (NPC npc in builder.Ec.npcsToSpawn)
		{
			list.Add(npc.Poster);
		}
		for (int i = 0; i < tilesOfShape.Count; i++)
		{
			if (!tilesOfShape[i].HasFreeWall)
			{
				tilesOfShape.RemoveAt(i);
				i--;
			}
		}
		while (tilesOfShape.Count > 0 && list.Count > 0)
		{
			int index = rng.Next(0, tilesOfShape.Count);
			int index2 = rng.Next(0, list.Count);
			this.room.ec.BuildPoster(list[index2], tilesOfShape[index], tilesOfShape[index].RandomUncoveredDirection(rng));
			list.RemoveAt(index2);
			if (!tilesOfShape[index].HasFreeWall)
			{
				tilesOfShape.RemoveAt(index);
			}
		}
	}
}
