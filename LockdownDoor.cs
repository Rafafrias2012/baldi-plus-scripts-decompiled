using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class LockdownDoor : Door, IButtonReceiver
{
	// Token: 0x06000289 RID: 649 RVA: 0x0000E220 File Offset: 0x0000C420
	private void Awake()
	{
		if (this.setHeightOnStart)
		{
			this.originalHeight = this.door.transform.localPosition.y;
			this.setHeightOnStart = false;
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000E24C File Offset: 0x0000C44C
	public void Start()
	{
		this.InstantShut();
		EnvironmentController ec = this.ec;
		ec.OnEnvironmentBeginPlay = (EnvironmentController.EnvironmentBeginPlay)Delegate.Combine(ec.OnEnvironmentBeginPlay, new EnvironmentController.EnvironmentBeginPlay(this.InstantOpen));
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000E27B File Offset: 0x0000C47B
	public override void Shut()
	{
		base.Shut();
		if (this.moving)
		{
			base.StopCoroutine(this.doorMover);
		}
		this.doorMover = this.ShutAnimation();
		base.StartCoroutine(this.doorMover);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
	public override void Open(bool cancelTimer, bool makeNoise)
	{
		base.Open(cancelTimer, makeNoise);
		if (this.moving)
		{
			base.StopCoroutine(this.doorMover);
		}
		this.doorMover = this.OpenAnimation();
		base.StartCoroutine(this.doorMover);
		base.aTile.Mute(this.direction, false);
		base.bTile.Mute(this.direction.GetOpposite(), false);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000E31B File Offset: 0x0000C51B
	public override void Toggle(bool cancelTimer, bool makeNoise)
	{
		if (!this.moving)
		{
			base.Toggle(cancelTimer, makeNoise);
		}
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000E32D File Offset: 0x0000C52D
	private IEnumerator ShutAnimation()
	{
		this.moving = true;
		this.audMan.QueueAudio(this.doorLoop, true);
		this.audMan.SetLoop(true);
		while (this.door.transform.position.y > 0f)
		{
			yield return null;
			this.door.transform.position -= Vector3.up * (this.speed * Time.deltaTime * this.ec.EnvironmentTimeScale);
			if (this.door.transform.position.y <= this.collisionHeight && !this.collider.enabled)
			{
				this.collider.enabled = true;
			}
		}
		this.moving = false;
		this.collider.enabled = true;
		this.door.transform.position -= Vector3.up * (0f - this.door.transform.position.y);
		base.aTile.Mute(this.direction, true);
		base.bTile.Mute(this.direction.GetOpposite(), true);
		this.audMan.FlushQueue(true);
		this.audMan.PlaySingle(this.doorEnd);
		yield break;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000E33C File Offset: 0x0000C53C
	private IEnumerator OpenAnimation()
	{
		if (this.door.transform.position.y >= this.originalHeight)
		{
			this.door.transform.position -= Vector3.up * (this.door.transform.position.y - this.originalHeight);
			this.collider.enabled = false;
		}
		else
		{
			this.moving = true;
			this.audMan.QueueAudio(this.doorLoop, true);
			this.audMan.SetLoop(true);
			while (this.door.transform.position.y < this.originalHeight)
			{
				yield return null;
				this.door.transform.position += Vector3.up * (this.speed * Time.deltaTime * this.ec.EnvironmentTimeScale);
				if (this.door.transform.position.y > this.collisionHeight && this.collider.enabled)
				{
					this.collider.enabled = false;
				}
			}
			this.moving = false;
			this.collider.enabled = false;
			this.door.transform.position -= Vector3.up * (this.door.transform.position.y - this.originalHeight);
			this.audMan.FlushQueue(true);
			this.audMan.PlaySingle(this.doorEnd);
		}
		yield break;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000E34C File Offset: 0x0000C54C
	public void InstantShut()
	{
		base.Shut();
		Vector3 position = this.door.transform.position;
		position.y = 0f;
		this.door.transform.position = position;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000E390 File Offset: 0x0000C590
	public void InstantOpen()
	{
		base.Open(true, false);
		Vector3 position = this.door.transform.position;
		position.y = this.originalHeight;
		this.door.transform.position = position;
		this.collider.enabled = false;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
	public void ButtonPressed(bool val)
	{
		this.Toggle(true, false);
	}

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private MeshRenderer door;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private BoxCollider collider;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private SoundObject doorLoop;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private SoundObject doorEnd;

	// Token: 0x040002A7 RID: 679
	private IEnumerator doorMover;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	private float speed = 1f;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private float collisionHeight = 6f;

	// Token: 0x040002AA RID: 682
	public float originalHeight;

	// Token: 0x040002AB RID: 683
	[SerializeField]
	private bool setHeightOnStart = true;

	// Token: 0x040002AC RID: 684
	private bool moving;
}
