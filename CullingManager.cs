using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class CullingManager : MonoBehaviour
{
	// Token: 0x06000816 RID: 2070 RVA: 0x00027F7A File Offset: 0x0002617A
	private void Update()
	{
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00027F7C File Offset: 0x0002617C
	private void LateUpdate()
	{
		if (Singleton<CoreGameManager>.Instance != null && Singleton<CoreGameManager>.Instance.GetCamera(0) != null && this.ec.ContainsCoordinates(IntVector2.GetGridPosition(Singleton<CoreGameManager>.Instance.GetCamera(0).transform.position)))
		{
			this.currentCell = this.ec.CellFromPosition(Singleton<CoreGameManager>.Instance.GetCamera(0).transform.position);
			if (this.currentCell.HasChunk && this.currentCell.Chunk.Id != this.currentChunkId)
			{
				this.currentChunkId = this.currentCell.Chunk.Id;
				this.chunkChanged = true;
			}
		}
		if (this.updatesQueued)
		{
			if (this.chunksToUpdate[this.currentChunkId])
			{
				this.CalculateOcclusionCullingForChunk(this.currentChunkId);
				this.chunksToUpdate[this.currentChunkId] = false;
				this.chunkChanged = true;
			}
			else
			{
				for (int i = 0; i <= this.chunksToUpdate.Length; i++)
				{
					if (i == this.chunksToUpdate.Length)
					{
						this.updatesQueued = false;
						break;
					}
					if (this.chunksToUpdate[i])
					{
						this.CalculateOcclusionCullingForChunk(i);
						this.chunksToUpdate[i] = false;
						break;
					}
				}
			}
		}
		if (this.active && !this.manualMode)
		{
			if (this.cullAlls > 0)
			{
				this.CullAll();
				return;
			}
			if (this.chunkChanged)
			{
				this.CullChunk(this.currentChunkId);
			}
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x000280F0 File Offset: 0x000262F0
	public void PrepareOcclusionCalculations()
	{
		int num = Mathf.CeilToInt((float)this.ec.levelSize.x / (float)Chunk.HallChunkSize);
		int num2 = Mathf.CeilToInt((float)this.ec.levelSize.z / (float)Chunk.HallChunkSize);
		this.hallChunk = new Chunk[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				this.hallChunk[i, j] = new Chunk();
				this.allChunks.Add(this.hallChunk[i, j]);
			}
		}
		for (int k = 0; k < this.ec.levelSize.x; k++)
		{
			for (int l = 0; l < this.ec.levelSize.z; l++)
			{
				Cell cell = this.ec.CellFromPosition(k, l);
				if (cell.TileLoaded && cell.room.type == RoomType.Hall)
				{
					this.hallChunk[Mathf.FloorToInt((float)k / (float)Chunk.HallChunkSize), Mathf.FloorToInt((float)l / (float)Chunk.HallChunkSize)].AddCell(cell);
				}
			}
		}
		if (this.ec.mainHall != null)
		{
			for (int m = 0; m < this.ec.mainHall.objectObject.transform.childCount; m++)
			{
				RendererContainer component = this.ec.mainHall.objectObject.transform.GetChild(m).GetComponent<RendererContainer>();
				if (component != null && this.ec.ContainsCoordinates(component.transform.position))
				{
					foreach (Renderer item in component.renderers)
					{
						this.ec.CellFromPosition(component.transform.position).renderers.Add(item);
					}
				}
			}
		}
		foreach (RoomController roomController in this.ec.rooms)
		{
			if (roomController.type != RoomType.Hall)
			{
				Chunk chunk = new Chunk();
				this.roomChunks.Add(chunk);
				foreach (Cell cell2 in roomController.cells)
				{
					chunk.AddCell(cell2);
				}
				this.allChunks.Add(chunk);
			}
			for (int num3 = 0; num3 < roomController.objectObject.transform.childCount; num3++)
			{
				RendererContainer component = roomController.objectObject.transform.GetChild(num3).GetComponent<RendererContainer>();
				if (component != null && this.ec.ContainsCoordinates(component.transform.position))
				{
					foreach (Renderer item2 in component.renderers)
					{
						this.ec.CellFromPosition(component.transform.position).renderers.Add(item2);
					}
				}
			}
		}
		this.chunksToUpdate = new bool[this.allChunks.Count];
		for (int num4 = 0; num4 < this.allChunks.Count; num4++)
		{
			this.allChunks[num4].SetId(num4);
			this.allChunks[num4].observingChunks = new bool[this.allChunks.Count];
		}
		for (int num5 = 0; num5 < this.allChunks.Count; num5++)
		{
			this.FindPortalsForChunk(num5);
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000284F8 File Offset: 0x000266F8
	public void FindPortalsForChunk(int chunkId)
	{
		this.allChunks[chunkId].portals.Clear();
		foreach (Cell cell in this.allChunks[chunkId].Cells)
		{
			Directions.FillOpenDirectionsFromBin(this._availableDirections, cell.ConstBin);
			foreach (Direction direction in this._availableDirections)
			{
				if (this.ec.CellFromPosition(cell.position + direction.ToIntVector2()).HasChunk && this.ec.CellFromPosition(cell.position + direction.ToIntVector2()).Chunk.Id != cell.Chunk.Id)
				{
					this.allChunks[chunkId].AddPortal(cell.position, direction);
				}
			}
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00028628 File Offset: 0x00026828
	public void CalculateOcclusionCullingForChunk(int chunkId)
	{
		Chunk chunk = this.allChunks[chunkId];
		bool[] array = new bool[this.allChunks.Count];
		array[chunk.Id] = true;
		for (int i = 0; i < this.allChunks.Count; i++)
		{
			this.allChunks[i].observingChunks[chunkId] = false;
		}
		foreach (CullingPortal cullingPortal in chunk.portals)
		{
			this.CalculateVisibleCells(cullingPortal.position, cullingPortal.direction);
			foreach (IntVector2 intVector in this._visibleCells)
			{
				if (!this.ec.CellFromPosition(intVector).Null)
				{
					array[this.ec.CellFromPosition(intVector).Chunk.Id] = true;
					this.ec.CellFromPosition(intVector).Chunk.observingChunks[chunkId] = true;
				}
			}
		}
		chunk.visibleChunks = array;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0002876C File Offset: 0x0002696C
	public void UpdateChunk(int chunkId)
	{
		if (this.active)
		{
			this.updatesQueued = true;
			this.FindPortalsForChunk(chunkId);
			this.chunksToUpdate[chunkId] = true;
			for (int i = 0; i < this.allChunks.Count; i++)
			{
				this.chunksToUpdate[i] = (this.chunksToUpdate[i] || this.allChunks[chunkId].observingChunks[i]);
			}
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x000287D6 File Offset: 0x000269D6
	public void SetActive(bool value)
	{
		this.active = value;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000287E0 File Offset: 0x000269E0
	public void CalculateVisibleCells(IntVector2 startPosition, Direction windowDirection)
	{
		this.potentialCells.Clear();
		this._visibleCells.Clear();
		this.stageOneCheckedCells = new bool[this.ec.levelSize.x, this.ec.levelSize.z];
		this.allStageOneCheckedCells = new bool[this.ec.levelSize.x, this.ec.levelSize.z];
		this.stageTwoCheckedCells = new bool[this.ec.levelSize.x, this.ec.levelSize.z];
		this.data = new CullingData[this.ec.levelSize.x, this.ec.levelSize.z];
		Queue<CullFill> queue = new Queue<CullFill>();
		queue.Enqueue(new CullFill(startPosition + windowDirection.ToIntVector2(), windowDirection.GetOpposite().ToBinary() | windowDirection.PerpendicularList()[0].ToBinary(), 1, 1));
		this.potentialCells.Add(startPosition + windowDirection.ToIntVector2());
		this.stageOneCheckedCells[startPosition.x + windowDirection.ToIntVector2().x, startPosition.z + windowDirection.ToIntVector2().z] = true;
		this.allStageOneCheckedCells[startPosition.x + windowDirection.ToIntVector2().x, startPosition.z + windowDirection.ToIntVector2().z] = true;
		this.data[startPosition.x + windowDirection.ToIntVector2().x, startPosition.z + windowDirection.ToIntVector2().z].Initalize();
		this.InitialLoop(queue, windowDirection);
		this.stageOneCheckedCells = new bool[this.ec.levelSize.x, this.ec.levelSize.z];
		queue.Enqueue(new CullFill(startPosition + windowDirection.ToIntVector2(), windowDirection.GetOpposite().ToBinary() | windowDirection.PerpendicularList()[1].ToBinary(), 1, 1));
		this.stageOneCheckedCells[startPosition.x + windowDirection.ToIntVector2().x, startPosition.z + windowDirection.ToIntVector2().z] = true;
		this.data[startPosition.x + windowDirection.ToIntVector2().x, startPosition.z + windowDirection.ToIntVector2().z].Initalize();
		this.InitialLoop(queue, windowDirection);
		Queue<IntVector2> queue2 = new Queue<IntVector2>();
		queue2.Enqueue(startPosition + windowDirection.ToIntVector2());
		while (queue2.Count > 0)
		{
			IntVector2 intVector = queue2.Dequeue();
			this.stageTwoCheckedCells[intVector.x, intVector.z] = true;
			if (this.data[intVector.x, intVector.z].sidewaysDistance == 0)
			{
				this.data[intVector.x, intVector.z].Initalize();
				this.data[intVector.x, intVector.z].disabledDirections = windowDirection.GetOpposite().ToBinary();
			}
			bool flag = Mathf.Atan2((float)(this.data[intVector.x, intVector.z].sidewaysDistance - 1), (float)(this.data[intVector.x, intVector.z].forwardDistance - 1)) >= this.data[intVector.x, intVector.z].forwardOccluderAngle;
			bool flag2 = Mathf.Atan2((float)this.data[intVector.x, intVector.z].forwardDistance, (float)(this.data[intVector.x, intVector.z].sidewaysDistance - 1)) >= this.data[intVector.x, intVector.z].sidewaysOccluderAngle;
			if (flag && flag2)
			{
				this._visibleCells.Add(intVector);
				Directions.FillOpenDirectionsFromBin(this._availableDirections, this.ec.CellFromPosition(intVector).ConstBin);
				foreach (Direction direction in this._availableDirections)
				{
					IntVector2 intVector2 = intVector + direction.ToIntVector2();
					if (this.ec.ContainsCoordinates(intVector2) && this.allStageOneCheckedCells[intVector2.x, intVector2.z] && !this.stageTwoCheckedCells[intVector2.x, intVector2.z])
					{
						queue2.Enqueue(intVector2);
						this.stageTwoCheckedCells[intVector2.x, intVector2.z] = true;
					}
				}
			}
		}
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00028CC4 File Offset: 0x00026EC4
	public void InitialLoop(Queue<CullFill> cullQueue, Direction windowDirection)
	{
		this.carryoverCells = new bool[this.ec.levelSize.x, this.ec.levelSize.z];
		while (cullQueue.Count > 0)
		{
			CullFill cullFill = cullQueue.Dequeue();
			this.data[cullFill.position.x, cullFill.position.z].forwardDistance = cullFill.forwardDistance;
			this.data[cullFill.position.x, cullFill.position.z].sidewaysDistance = cullFill.sidewaysDistance;
			this.data[cullFill.position.x, cullFill.position.z].disabledDirections = cullFill.disabledDirections;
			Directions.FillOpenDirectionsFromBin(this._availableDirections, cullFill.disabledDirections);
			foreach (Direction direction in this._availableDirections)
			{
				if (this.ec.CellFromPosition(cullFill.position).HasWallInDirection(direction))
				{
					if (direction == windowDirection)
					{
						this.data[cullFill.position.x, cullFill.position.z].UpdateForwardOccluder(cullFill.forwardDistance, cullFill.sidewaysDistance - 1);
						if (this.ec.ContainsCoordinates(cullFill.position + direction.ToIntVector2()))
						{
							this.data[cullFill.position.x + direction.ToIntVector2().x, cullFill.position.z + direction.ToIntVector2().z].UpdateSidewaysOccluder(cullFill.sidewaysDistance, cullFill.forwardDistance);
							this.carryoverCells[cullFill.position.x + direction.ToIntVector2().x, cullFill.position.z + direction.ToIntVector2().z] = true;
						}
					}
					else
					{
						this.data[cullFill.position.x, cullFill.position.z].UpdateSidewaysOccluder(cullFill.sidewaysDistance, cullFill.forwardDistance);
						if (this.ec.ContainsCoordinates(cullFill.position + direction.ToIntVector2()))
						{
							this.data[cullFill.position.x + direction.ToIntVector2().x, cullFill.position.z + direction.ToIntVector2().z].UpdateForwardOccluder(cullFill.forwardDistance, cullFill.sidewaysDistance - 1);
							this.carryoverCells[cullFill.position.x + direction.ToIntVector2().x, cullFill.position.z + direction.ToIntVector2().z] = true;
						}
					}
				}
			}
			Directions.FillOpenDirectionsFromBin(this._availableDirections, cullFill.disabledDirections | this.ec.CellFromPosition(cullFill.position).ConstBin);
			foreach (Direction direction2 in this._availableDirections)
			{
				CullingPropagationDirection cullingDirection = CullingPropagationDirection.Forward;
				if (direction2 != windowDirection)
				{
					cullingDirection = CullingPropagationDirection.Sideways;
				}
				if (this.ec.ContainsCoordinates(cullFill.position + direction2.ToIntVector2()) && this.CellShouldBeQueued(this.data[cullFill.position.x, cullFill.position.z], cullFill.position + direction2.ToIntVector2(), cullingDirection))
				{
					IntVector2 intVector = cullFill.position + direction2.ToIntVector2();
					if (direction2 == windowDirection)
					{
						cullQueue.Enqueue(new CullFill(intVector, cullFill.disabledDirections | direction2.GetOpposite().ToBinary(), cullFill.forwardDistance + 1, cullFill.sidewaysDistance));
					}
					else
					{
						cullQueue.Enqueue(new CullFill(intVector, cullFill.disabledDirections | direction2.GetOpposite().ToBinary(), cullFill.forwardDistance, cullFill.sidewaysDistance + 1));
					}
					this.potentialCells.Add(intVector);
					this.stageOneCheckedCells[intVector.x, intVector.z] = true;
					this.allStageOneCheckedCells[intVector.x, intVector.z] = true;
				}
			}
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0002916C File Offset: 0x0002736C
	public bool CellShouldBeQueued(CullingData newData, IntVector2 position, CullingPropagationDirection cullingDirection)
	{
		bool result = false;
		if (this.carryoverCells[position.x, position.z])
		{
			result = true;
			this.carryoverCells[position.x, position.z] = false;
		}
		else if (!this.stageOneCheckedCells[position.x, position.z])
		{
			this.data[position.x, position.z].Initalize();
			result = true;
		}
		if (this.data[position.x, position.z].NeedsUpdated(newData, cullingDirection))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0002920A File Offset: 0x0002740A
	public void SetCullAll(bool val)
	{
		if (val)
		{
			this.cullAlls++;
			return;
		}
		if (this.cullAlls > 0)
		{
			this.cullAlls--;
			this.chunkChanged = true;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0002923C File Offset: 0x0002743C
	private void CullAll()
	{
		for (int i = 0; i < this.allChunks.Count; i++)
		{
			this.allChunks[i].Render(false);
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00029274 File Offset: 0x00027474
	public void CullChunk(int chunkId)
	{
		for (int i = 0; i < this.allChunks[chunkId].visibleChunks.Length; i++)
		{
			this.allChunks[i].Render(this.allChunks[chunkId].visibleChunks[i]);
		}
		this.chunkChanged = false;
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x000292CA File Offset: 0x000274CA
	public void RenderChunk(int chunkId, bool value)
	{
		if (chunkId < this.allChunks.Count)
		{
			this.allChunks[chunkId].Render(value);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000824 RID: 2084 RVA: 0x000292EC File Offset: 0x000274EC
	public int TotalChunks
	{
		get
		{
			return this.allChunks.Count;
		}
	}

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private EnvironmentController ec;

	// Token: 0x040008A4 RID: 2212
	public Direction direction;

	// Token: 0x040008A5 RID: 2213
	public IntVector2 position;

	// Token: 0x040008A6 RID: 2214
	public List<IntVector2> potentialCells = new List<IntVector2>();

	// Token: 0x040008A7 RID: 2215
	public List<IntVector2> _visibleCells = new List<IntVector2>();

	// Token: 0x040008A8 RID: 2216
	private List<Direction> _availableDirections = new List<Direction>();

	// Token: 0x040008A9 RID: 2217
	public CullingData[,] data = new CullingData[0, 0];

	// Token: 0x040008AA RID: 2218
	private Chunk[,] hallChunk = new Chunk[0, 0];

	// Token: 0x040008AB RID: 2219
	private Cell currentCell;

	// Token: 0x040008AC RID: 2220
	public List<Chunk> allChunks = new List<Chunk>();

	// Token: 0x040008AD RID: 2221
	private List<Chunk> roomChunks = new List<Chunk>();

	// Token: 0x040008AE RID: 2222
	private int cullAlls;

	// Token: 0x040008AF RID: 2223
	private bool[,] stageOneCheckedCells = new bool[0, 0];

	// Token: 0x040008B0 RID: 2224
	private bool[,] allStageOneCheckedCells = new bool[0, 0];

	// Token: 0x040008B1 RID: 2225
	private bool[,] carryoverCells = new bool[0, 0];

	// Token: 0x040008B2 RID: 2226
	private bool[,] stageTwoCheckedCells = new bool[0, 0];

	// Token: 0x040008B3 RID: 2227
	public bool[] chunksToUpdate;

	// Token: 0x040008B4 RID: 2228
	private bool active;

	// Token: 0x040008B5 RID: 2229
	private bool manualMode;

	// Token: 0x040008B6 RID: 2230
	private bool chunkChanged;

	// Token: 0x040008B7 RID: 2231
	private bool updatesQueued;

	// Token: 0x040008B8 RID: 2232
	private int currentChunkId = -1;
}
