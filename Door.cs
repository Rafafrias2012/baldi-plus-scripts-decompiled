using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001BC RID: 444
public class Door : TileBasedObject
{
	// Token: 0x060009FD RID: 2557 RVA: 0x00035784 File Offset: 0x00033984
	public virtual void Open(bool cancelTimer, bool makeNoise)
	{
		if (!this.locked)
		{
			if (makeNoise && !this.open)
			{
				this.ec.MakeNoise(base.transform.position + base.transform.forward * this.positionOffset, this.noiseValue);
			}
			this.open = true;
			if (this.closeBlocks)
			{
				this.Block(false);
			}
			if (this.shutTimerInst != null && cancelTimer)
			{
				base.StopCoroutine(this.shutTimerInst);
			}
		}
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0003580C File Offset: 0x00033A0C
	public virtual void OpenTimed(float time, bool makeNoise)
	{
		if (!this.locked)
		{
			this.Open(false, makeNoise);
			if (time > this.shutTime)
			{
				if (this.shutTimerInst != null)
				{
					base.StopCoroutine(this.shutTimerInst);
				}
				this.shutTimerInst = this.ShutTimer(time);
				base.StartCoroutine(this.shutTimerInst);
			}
		}
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00035860 File Offset: 0x00033A60
	public virtual void Shut()
	{
		this.open = false;
		if (this.closeBlocks)
		{
			this.Block(true);
		}
		if (this.shutTime > 0f)
		{
			base.StopCoroutine(this.shutTimerInst);
			this.shutTime = 0f;
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0003589C File Offset: 0x00033A9C
	public IEnumerator ShutTimer(float time)
	{
		this.shutTime = time;
		while (this.shutTime > 0f)
		{
			this.shutTime -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.Shut();
		yield break;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000358B2 File Offset: 0x00033AB2
	public virtual void Lock(bool cancelTimer)
	{
		this.locked = true;
		if (this.lockBlocks)
		{
			this.Block(true);
		}
		if (this.unlockTimerInst != null && cancelTimer)
		{
			base.StopCoroutine(this.unlockTimerInst);
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000358E3 File Offset: 0x00033AE3
	public virtual void LockTimed(float time)
	{
		this.Lock(false);
		if (time > this.unlockTime)
		{
			if (this.unlockTimerInst != null)
			{
				base.StopCoroutine(this.unlockTimerInst);
			}
			this.unlockTimerInst = this.unlockTimer(time);
			base.StartCoroutine(this.unlockTimerInst);
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00035923 File Offset: 0x00033B23
	public virtual void Unlock()
	{
		this.locked = false;
		if (this.lockBlocks)
		{
			this.Block(false);
		}
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0003593B File Offset: 0x00033B3B
	public virtual void Toggle(bool cancelTimer, bool makeNoise)
	{
		if (this.open)
		{
			this.Shut();
			return;
		}
		this.Open(cancelTimer, makeNoise);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00035954 File Offset: 0x00033B54
	public IEnumerator unlockTimer(float time)
	{
		this.unlockTime = time;
		while (this.unlockTime > 0f)
		{
			this.unlockTime -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		this.Unlock();
		yield break;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0003596A File Offset: 0x00033B6A
	public virtual void Block(bool block)
	{
		this.aTile.Block(this.direction, block);
		this.bTile.Block(this.direction.GetOpposite(), block);
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000A07 RID: 2567 RVA: 0x00035995 File Offset: 0x00033B95
	public Cell aTile
	{
		get
		{
			return this.ec.cells[this.position.x, this.position.z];
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x000359BD File Offset: 0x00033BBD
	public Cell bTile
	{
		get
		{
			return this.ec.cells[this.position.x + this.bOffset.x, this.position.z + this.bOffset.z];
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000359FD File Offset: 0x00033BFD
	public bool IsOpen
	{
		get
		{
			return this.open;
		}
	}

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	protected float positionOffset = 5f;

	// Token: 0x04000B5E RID: 2910
	private IEnumerator shutTimerInst;

	// Token: 0x04000B5F RID: 2911
	private IEnumerator unlockTimerInst;

	// Token: 0x04000B60 RID: 2912
	private float shutTime;

	// Token: 0x04000B61 RID: 2913
	private float unlockTime;

	// Token: 0x04000B62 RID: 2914
	[Range(0f, 127f)]
	public int noiseValue;

	// Token: 0x04000B63 RID: 2915
	public bool makesNoise = true;

	// Token: 0x04000B64 RID: 2916
	public bool closeBlocks;

	// Token: 0x04000B65 RID: 2917
	public bool lockBlocks = true;

	// Token: 0x04000B66 RID: 2918
	public bool open;

	// Token: 0x04000B67 RID: 2919
	public bool locked;
}
