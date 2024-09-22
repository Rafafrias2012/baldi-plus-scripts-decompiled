using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000199 RID: 409
[Serializable]
public class OpenTileGroup
{
	// Token: 0x06000948 RID: 2376 RVA: 0x0003160C File Offset: 0x0002F80C
	public Cell randomOpenTile(Cell startTile)
	{
		this._possibleOpenCells.Clear();
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i].shape == TileShape.Open && this.cells[i] != startTile)
			{
				this._possibleOpenCells.Add(this.cells[i]);
			}
		}
		if (this._possibleOpenCells.Count > 0)
		{
			return this._possibleOpenCells[Random.Range(0, this._possibleOpenCells.Count)];
		}
		return this.cells[Random.Range(0, this.cells.Count)];
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x000316BC File Offset: 0x0002F8BC
	public bool CellIsExit(Cell cell)
	{
		using (List<OpenGroupExit>.Enumerator enumerator = this.exits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.cell == cell)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00031718 File Offset: 0x0002F918
	public bool HasExit(Cell cell, Direction direction)
	{
		foreach (OpenGroupExit openGroupExit in this.exits)
		{
			if (openGroupExit.cell == cell && openGroupExit.direction == direction)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00031780 File Offset: 0x0002F980
	public OpenGroupExit GetEntrance(Cell to, Cell from)
	{
		foreach (OpenGroupExit openGroupExit in this.exits)
		{
			if (openGroupExit.cell == to && openGroupExit.OutputCell(to.room.ec) == from)
			{
				return openGroupExit;
			}
		}
		return new OpenGroupExit(to, Direction.Null);
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x000317F8 File Offset: 0x0002F9F8
	public OpenGroupExit GetNearbyExit(Cell startCell, Vector3 position)
	{
		OpenGroupExit result = new OpenGroupExit(null, Direction.Null);
		float num = 0.1f;
		if (this.CellIsExit(startCell))
		{
			result.cell = startCell;
			Vector2 vector = new Vector2(position.x - startCell.FloorWorldPosition.x, position.z - startCell.FloorWorldPosition.z) / 5f;
			Direction direction = Direction.Null;
			Direction direction2 = Direction.Null;
			if (Mathf.Abs(vector.y) > num)
			{
				if (vector.y > 0f)
				{
					direction = Direction.North;
				}
				else
				{
					direction = Direction.South;
				}
			}
			if (Mathf.Abs(vector.x) > num)
			{
				if (vector.x > 0f)
				{
					direction2 = Direction.East;
				}
				else
				{
					direction2 = Direction.West;
				}
			}
			if (Mathf.Abs(vector.y) > Mathf.Abs(vector.x))
			{
				if (this.HasExit(startCell, direction))
				{
					result.direction = direction;
				}
				else if (this.HasExit(startCell, direction2))
				{
					result.direction = direction2;
				}
			}
			else if (this.HasExit(startCell, direction2))
			{
				result.direction = direction2;
			}
			else if (this.HasExit(startCell, direction))
			{
				result.direction = direction;
			}
		}
		return result;
	}

	// Token: 0x04000A32 RID: 2610
	public List<Cell> cells = new List<Cell>();

	// Token: 0x04000A33 RID: 2611
	public List<Cell> exitCells = new List<Cell>();

	// Token: 0x04000A34 RID: 2612
	public List<OpenGroupExit> exits = new List<OpenGroupExit>();

	// Token: 0x04000A35 RID: 2613
	private List<Cell> _possibleOpenCells = new List<Cell>();
}
