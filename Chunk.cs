using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class Chunk
{
	// Token: 0x06000803 RID: 2051 RVA: 0x00027C7C File Offset: 0x00025E7C
	public void Render(bool value)
	{
		if (value != this.rendering)
		{
			foreach (Renderer renderer in this.renderers)
			{
				renderer.enabled = value;
			}
			foreach (Cell cell in this.cells)
			{
				cell.Tile.MeshRenderer.enabled = (!cell.Hidden && value);
				foreach (Renderer renderer2 in cell.renderers)
				{
					renderer2.enabled = value;
				}
			}
		}
		this.rendering = value;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00027D78 File Offset: 0x00025F78
	public void AddCell(Cell cell)
	{
		this.cells.Add(cell);
		cell.SetChunk(this);
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00027D8D File Offset: 0x00025F8D
	public void AddRenderer(Renderer renderer)
	{
		this.renderers.Add(renderer);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00027D9B File Offset: 0x00025F9B
	public void AddPortal(IntVector2 position, Direction direction)
	{
		this.portals.Add(new CullingPortal(position, direction));
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00027DAF File Offset: 0x00025FAF
	public void SetId(int id)
	{
		this.id = id;
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000808 RID: 2056 RVA: 0x00027DB8 File Offset: 0x00025FB8
	public int Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x00027DC0 File Offset: 0x00025FC0
	public List<Cell> Cells
	{
		get
		{
			return this.cells;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600080A RID: 2058 RVA: 0x00027DC8 File Offset: 0x00025FC8
	public static int HallChunkSize
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600080B RID: 2059 RVA: 0x00027DCC File Offset: 0x00025FCC
	public bool Rendering
	{
		get
		{
			return this.rendering;
		}
	}

	// Token: 0x04000892 RID: 2194
	private List<Cell> cells = new List<Cell>();

	// Token: 0x04000893 RID: 2195
	private List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04000894 RID: 2196
	private RoomController room;

	// Token: 0x04000895 RID: 2197
	public List<CullingPortal> portals = new List<CullingPortal>();

	// Token: 0x04000896 RID: 2198
	private int id;

	// Token: 0x04000897 RID: 2199
	public bool[] visibleChunks = new bool[0];

	// Token: 0x04000898 RID: 2200
	public bool[] observingChunks = new bool[0];

	// Token: 0x04000899 RID: 2201
	private bool rendering = true;
}
