using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class LockdownEvent : RandomEvent
{
	// Token: 0x06000294 RID: 660 RVA: 0x0000E410 File Offset: 0x0000C610
	public override void AfterUpdateSetup()
	{
		this.doors.Clear();
		this.setDoors = this.crng.Next(this.minDoors, this.maxDoors);
		List<List<Cell>> list = this.ec.FindHallways();
		int num = 0;
		while (num < this.setDoors && list.Count > 0)
		{
			int index = this.crng.Next(0, list.Count);
			for (int i = 0; i < list[index].Count; i++)
			{
				if (list[index][i].HasAnyHardCoverage || list[index][i].shape != TileShape.Straight)
				{
					list[index].RemoveAt(i);
					i--;
				}
			}
			if (list[index].Count > 0)
			{
				int index2 = this.crng.Next(0, list[index].Count);
				Direction direction = Directions.OpenDirectionsFromBin(list[index][index2].ConstBin)[this.crng.Next(0, Directions.OpenDirectionsFromBin(list[index][index2].ConstBin).Count)];
				LockdownDoor lockdownDoor = Object.Instantiate<LockdownDoor>(this.doorPre, list[index][index2].TileTransform);
				lockdownDoor.transform.rotation = direction.ToRotation();
				this.ec.SetupDoor(lockdownDoor, list[index][index2], direction);
				list[index][index2].HardCoverEntirely();
				this.doors.Add(lockdownDoor);
				num++;
				list.RemoveAt(index);
			}
			else
			{
				list.RemoveAt(index);
			}
		}
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000E5C9 File Offset: 0x0000C7C9
	public override void Begin()
	{
		base.Begin();
		base.StartCoroutine(this.Countdown());
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
	public override void End()
	{
		base.End();
		foreach (Door door in this.doors)
		{
			door.Open(true, false);
		}
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000E638 File Offset: 0x0000C838
	private IEnumerator Countdown()
	{
		float time = 15f;
		int seconds = 10;
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		while (seconds > 0)
		{
			while (time > 0f)
			{
				time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
				yield return null;
			}
			time = 2f;
			seconds--;
			this.audMan.PlaySingle(this.audCountdown[seconds]);
		}
		base.StartCoroutine(this.ShutAllSequence());
		yield break;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000E647 File Offset: 0x0000C847
	private IEnumerator ShutAllSequence()
	{
		float time = 0f;
		foreach (Door door in this.doors)
		{
			door.Shut();
			time = this.timeBetweenDoors;
			while (time > 0f)
			{
				time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
				yield return null;
			}
		}
		List<Door>.Enumerator enumerator = default(List<Door>.Enumerator);
		foreach (Door door2 in this.doors)
		{
			door2.Block(false);
			if (this.ec.TrapCheck(door2.aTile))
			{
				this.trappedDoors.Add(door2);
			}
			door2.Block(true);
		}
		if (this.trappedDoors.Count > 0)
		{
			base.StartCoroutine(this.DoorRandomizer());
		}
		yield break;
		yield break;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000E656 File Offset: 0x0000C856
	private IEnumerator DoorRandomizer()
	{
		while (this.remainingTime > base.EventTime - this.randomizerBuffer)
		{
			yield return null;
		}
		float cool = 0f;
		while (this.remainingTime > this.randomizerBuffer)
		{
			cool -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			if (cool <= 0f)
			{
				this.trappedDoors[this.crng.Next(0, this.trappedDoors.Count)].Toggle(false, false);
				cool = this.randomizerCool;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040002AD RID: 685
	[SerializeField]
	private LockdownDoor doorPre;

	// Token: 0x040002AE RID: 686
	[SerializeField]
	private List<Door> doors = new List<Door>();

	// Token: 0x040002AF RID: 687
	public List<Door> trappedDoors = new List<Door>();

	// Token: 0x040002B0 RID: 688
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	private SoundObject[] audCountdown = new SoundObject[10];

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	private int minDoors = 10;

	// Token: 0x040002B3 RID: 691
	[SerializeField]
	private int maxDoors = 15;

	// Token: 0x040002B4 RID: 692
	private int setDoors;

	// Token: 0x040002B5 RID: 693
	[SerializeField]
	private float randomizerBuffer = 10f;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	private float randomizerCool = 5f;

	// Token: 0x040002B7 RID: 695
	[SerializeField]
	private float timeBetweenDoors = 0.5f;
}
