using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class RotoHall : EnvironmentObject, IButtonReceiver
{
	// Token: 0x060003EB RID: 1003 RVA: 0x000147CA File Offset: 0x000129CA
	public void ButtonPressed(bool val)
	{
		this.Rotate();
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x000147D4 File Offset: 0x000129D4
	private void Rotate()
	{
		if (!this.moving)
		{
			bool flag;
			Direction nextDirection = this.GetNextDirection(1, out flag);
			if (flag)
			{
				base.StartCoroutine(this.Rotator(1, nextDirection));
				return;
			}
			base.StartCoroutine(this.Rotator(-1, nextDirection));
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00014818 File Offset: 0x00012A18
	private Direction GetNextDirection(int val, out bool clockwise)
	{
		int num = (int)this.currentDir;
		int num2 = 0;
		while (num == (int)this.currentDir || (!this.CheckDirections((Direction)num) && num2 < 4))
		{
			num += val;
			if (num > 3)
			{
				num = 0;
			}
			else if (num < 0)
			{
				num = 3;
			}
			num2++;
		}
		clockwise = (num2 <= 1);
		return (Direction)num;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00014868 File Offset: 0x00012A68
	private void Block()
	{
		foreach (Direction direction in Directions.All())
		{
			this.tile.Block(direction, true);
			this.tile.Mute(direction, true);
			Cell cell = this.ec.CellFromPosition(this.tile.position + direction.ToIntVector2());
			if (cell != null && cell.ConstNavigable(direction.GetOpposite()))
			{
				cell.Block(direction.GetOpposite(), true);
				cell.Mute(direction.GetOpposite(), true);
			}
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001491C File Offset: 0x00012B1C
	private void UnblockAll()
	{
		foreach (Direction direction in Directions.All())
		{
			this.tile.Block(direction, false);
			this.tile.Mute(direction, false);
			Cell cell = this.ec.CellFromPosition(this.tile.position + direction.ToIntVector2());
			if (cell != null && cell.ConstNavigable(direction.GetOpposite()))
			{
				cell.Block(direction.GetOpposite(), false);
				cell.Mute(direction.GetOpposite(), false);
			}
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x000149D0 File Offset: 0x00012BD0
	private void Unblock()
	{
		this._dirsToUnblock.Clear();
		if (this.cylinderShape == CylinderShape.Straight)
		{
			this._dirsToUnblock.Add(this.currentDir);
			this._dirsToUnblock.Add(this.currentDir.GetOpposite());
		}
		else
		{
			this._dirsToUnblock.Add(this.currentDir);
			int num = (int)(this.currentDir + 1);
			if (num > 3)
			{
				num = 0;
			}
			this._dirsToUnblock.Add((Direction)num);
		}
		foreach (Direction direction in this._dirsToUnblock)
		{
			Cell cell = this.ec.CellFromPosition(this.tile.position + direction.ToIntVector2());
			if (cell != null && cell.ConstNavigable(direction.GetOpposite()))
			{
				this.tile.Block(direction, false);
				this.tile.Mute(direction, false);
				cell.Block(direction.GetOpposite(), false);
				cell.Mute(direction.GetOpposite(), false);
			}
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00014AEC File Offset: 0x00012CEC
	private bool CheckDirections(Direction dir)
	{
		this._dirsToUnblock.Clear();
		if (this.cylinderShape == CylinderShape.Straight)
		{
			this._dirsToUnblock.Add(dir);
			this._dirsToUnblock.Add(dir.GetOpposite());
		}
		else
		{
			this._dirsToUnblock.Add(dir);
			int num = (int)(dir + 1);
			if (num > 3)
			{
				num = 0;
			}
			this._dirsToUnblock.Add((Direction)num);
		}
		foreach (Direction dir2 in this._dirsToUnblock)
		{
			if (!this.ec.CellFromPosition(this.tile.position).ConstNavigable(dir2))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00014BB4 File Offset: 0x00012DB4
	private IEnumerator Rotator(int spinVal, Direction targetDir)
	{
		this.moving = true;
		this.UnblockAll();
		this.Block();
		this.audMan.FlushQueue(true);
		this.audMan.QueueAudio(this.audTurn);
		this.entitySpinner.SetSpeed(this.speed * (float)spinVal);
		bool keepMoving = true;
		float previousAngle = this.cylinder.transform.eulerAngles.y;
		while (keepMoving)
		{
			this._rotation = this.cylinder.transform.eulerAngles;
			this._rotation.y = this._rotation.y + this.speed * (float)spinVal * this.ec.EnvironmentTimeScale * Time.deltaTime;
			switch (targetDir)
			{
			case Direction.North:
				if ((spinVal == 1 && this._rotation.y >= 360f) || (spinVal == -1 && this._rotation.y <= 0f))
				{
					this._rotation.y = 0f;
					keepMoving = false;
				}
				break;
			case Direction.East:
				if ((spinVal == 1 && this._rotation.y >= 90f && (this._rotation.y <= previousAngle || previousAngle < 90f)) || (spinVal == -1 && this._rotation.y <= 90f && (this._rotation.y >= previousAngle || previousAngle > 90f)))
				{
					this._rotation.y = 90f;
					keepMoving = false;
				}
				break;
			case Direction.South:
				if ((spinVal == 1 && this._rotation.y >= 180f && (this._rotation.y <= previousAngle || previousAngle < 180f)) || (spinVal == -1 && this._rotation.y <= 180f && (this._rotation.y >= previousAngle || previousAngle > 180f)))
				{
					this._rotation.y = 180f;
					keepMoving = false;
				}
				break;
			case Direction.West:
				if ((spinVal == 1 && this._rotation.y >= 270f && (this._rotation.y <= previousAngle || previousAngle < 270f)) || (spinVal == -1 && this._rotation.y <= 270f && (this._rotation.y >= previousAngle || previousAngle > 270f)))
				{
					this._rotation.y = 270f;
					keepMoving = false;
				}
				break;
			}
			if (this._rotation.y >= 360f)
			{
				this._rotation.y = this._rotation.y - 360f;
			}
			else if (this._rotation.y < 0f)
			{
				this._rotation.y = this._rotation.y + 360f;
			}
			this.cylinder.transform.eulerAngles = this._rotation;
			yield return null;
		}
		this.currentDir = targetDir;
		this.Unblock();
		this.moving = false;
		this.entitySpinner.SetSpeed(0f);
		yield break;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00014BD4 File Offset: 0x00012DD4
	public void Setup(Direction dir, MeshRenderer newCylinder, CylinderShape shape, Cell newTile, bool spinClockwise)
	{
		this.currentDir = dir;
		if (this.cylinder != null)
		{
			Object.Destroy(this.cylinder);
		}
		this.cylinder = Object.Instantiate<MeshRenderer>(newCylinder, base.transform);
		this.cylinderShape = shape;
		this.tile = newTile;
		bool flag;
		this.currentDir = this.GetNextDirection(1, out flag);
		this.cylinder.transform.rotation = this.currentDir.ToRotation();
		this.Block();
		this.Unblock();
	}

	// Token: 0x04000422 RID: 1058
	[SerializeField]
	private MeshRenderer cylinder;

	// Token: 0x04000423 RID: 1059
	[SerializeField]
	private EntitySpinner entitySpinner;

	// Token: 0x04000424 RID: 1060
	private Cell tile;

	// Token: 0x04000425 RID: 1061
	private Vector3 _rotation;

	// Token: 0x04000426 RID: 1062
	[SerializeField]
	private CylinderShape cylinderShape;

	// Token: 0x04000427 RID: 1063
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000428 RID: 1064
	[SerializeField]
	private SoundObject audTurn;

	// Token: 0x04000429 RID: 1065
	[SerializeField]
	private Direction currentDir;

	// Token: 0x0400042A RID: 1066
	private List<Direction> _dirsToUnblock = new List<Direction>();

	// Token: 0x0400042B RID: 1067
	[SerializeField]
	private float speed = 18f;

	// Token: 0x0400042C RID: 1068
	private bool moving;
}
