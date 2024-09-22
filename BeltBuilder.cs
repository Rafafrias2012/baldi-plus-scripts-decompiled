using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class BeltBuilder : ObjectBuilder
{
	// Token: 0x060003CB RID: 971 RVA: 0x00013A94 File Offset: 0x00011C94
	public override void Build(EnvironmentController ec, LevelBuilder builder, RoomController room, Random cRng)
	{
		List<List<Cell>> list = ec.FindHallways();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Count < this.minHallSize)
			{
				list.RemoveAt(i);
				i--;
			}
		}
		if (list.Count <= 0)
		{
			Debug.LogWarning("No halls were found for the conveyor belt builder.");
			return;
		}
		List<Cell> list2 = list[cRng.Next(0, list.Count)];
		this.beltManager = Object.Instantiate<BeltManager>(this.beltManagerPre, room.transform);
		this.beltManager.Initialize();
		Direction dir;
		if ((list2[0].position - list2[list2.Count - 1].position).x == 0)
		{
			if (cRng.Next(0, 2) == 0)
			{
				dir = Direction.North;
			}
			else
			{
				dir = Direction.South;
			}
		}
		else if (cRng.Next(0, 2) == 0)
		{
			dir = Direction.East;
		}
		else
		{
			dir = Direction.West;
		}
		Cell cell = null;
		List<Cell> list3 = new List<Cell>();
		foreach (Cell cell2 in list2)
		{
			if (!cell2.HardCoverageFits(this.coverage))
			{
				break;
			}
			list3.Add(cell2);
			cell = cell2;
		}
		if (cell != null)
		{
			list2[0].position - cell.position;
			if ((float)cRng.NextDouble() * 100f < this.buttonChance)
			{
				GameButton.BuildInArea(ec, list2[0].position, cell.position, this.buttonRange, this.beltManager.gameObject, this.buttonPre, cRng);
			}
			this.Build(ec, list3, dir, this.beltManager);
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00013C50 File Offset: 0x00011E50
	public override void Load(EnvironmentController ec, List<IntVector2> pos, List<Direction> dir)
	{
		base.Load(ec, pos, dir);
		if (pos.Count >= 2 && dir.Count >= 1)
		{
			List<Cell> list = new List<Cell>();
			IntVector2 intVector = IntVector2.CombineLowest(pos[0], pos[1]);
			IntVector2 intVector2 = IntVector2.CombineGreatest(pos[0], pos[1]);
			for (int i = intVector.x; i <= intVector2.x; i++)
			{
				for (int j = intVector.z; j <= intVector2.z; j++)
				{
					IntVector2 position;
					position.x = i;
					position.z = j;
					if (ec.CellFromPosition(position) != null)
					{
						list.Add(ec.CellFromPosition(position));
					}
				}
			}
			this.beltManager = Object.Instantiate<BeltManager>(this.beltManagerPre, list[0].room.transform);
			this.beltManager.Initialize();
			this.Build(ec, list, dir[0], this.beltManager);
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00013D4C File Offset: 0x00011F4C
	private void Build(EnvironmentController ec, List<Cell> tilesToBuild, Direction dir, BeltManager beltManager)
	{
		beltManager.SetDirection(dir);
		IntVector2 position = new IntVector2(ec.levelSize.x, ec.levelSize.z);
		IntVector2 position2 = new IntVector2(0, 0);
		foreach (Cell cell in tilesToBuild)
		{
			if (cell.position.x <= position.x && cell.position.z <= position.z)
			{
				position = cell.position;
			}
			if (cell.position.x >= position2.x && cell.position.z >= position2.z)
			{
				position2 = cell.position;
			}
			this.BuildBelt(cell, dir);
		}
		Vector3 vector = new Vector3(((float)position2.x - (float)position.x) / 2f + (float)position.x, 5f, ((float)position2.z - (float)position.z) / 2f + (float)position.z);
		vector.x = vector.x * 10f + 5f;
		vector.z = vector.z * 10f + 5f;
		beltManager.transform.position = vector;
		Vector3 size = new Vector3((float)(position2.x - position.x) * 10f + 10f, 10f, (float)(position2.z - position.z) * 10f + 10f);
		beltManager.BoxCollider.size = size;
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00013F00 File Offset: 0x00012100
	private void BuildBelt(Cell tile, Direction dir)
	{
		MeshRenderer meshRenderer = Object.Instantiate<MeshRenderer>(this.beltPre, tile.TileTransform);
		meshRenderer.transform.rotation = dir.ToRotation();
		meshRenderer.transform.eulerAngles += Vector3.right * 90f;
		tile.HardCover(this.coverage);
		this.beltManager.AddBelt(meshRenderer);
	}

	// Token: 0x040003FE RID: 1022
	[SerializeField]
	private BeltManager beltManagerPre;

	// Token: 0x040003FF RID: 1023
	private BeltManager beltManager;

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private GameButton buttonPre;

	// Token: 0x04000401 RID: 1025
	[SerializeField]
	private MeshRenderer beltPre;

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private CellCoverage coverage = CellCoverage.Down;

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	[Range(0f, 100f)]
	private float buttonChance;

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private int buttonRange = 3;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private int minHallSize = 6;
}
