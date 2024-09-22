using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class MathMachineNumber : MonoBehaviour, IClickable<int>
{
	// Token: 0x0600039F RID: 927 RVA: 0x00013158 File Offset: 0x00011358
	private void Start()
	{
		this.spriteInitPosition = this.sprite.localPosition;
		this.initialSpriteLayer = this.sprite.gameObject.layer;
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00013184 File Offset: 0x00011384
	private void Update()
	{
		if (!this.trackPlayer && this.currentOffset != this.targetOffset)
		{
			this.currentOffset = Mathf.MoveTowards(this.currentOffset, this.targetOffset, Time.deltaTime * 20f);
			this.sprite.localPosition = this.spriteInitPosition + Vector3.up * this.currentOffset;
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000131EF File Offset: 0x000113EF
	public void Clicked(int playerNumber)
	{
		this.mathMachine.NumberClicked(this.value, playerNumber);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00013203 File Offset: 0x00011403
	public void ClickableSighted(int player)
	{
		if (!this.trackPlayer)
		{
			this.targetOffset = -2f;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00013218 File Offset: 0x00011418
	public void ClickableUnsighted(int player)
	{
		if (!this.trackPlayer)
		{
			this.targetOffset = 0f;
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001322D File Offset: 0x0001142D
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00013230 File Offset: 0x00011430
	public bool ClickableRequiresNormalHeight()
	{
		return false;
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060003A6 RID: 934 RVA: 0x00013233 File Offset: 0x00011433
	public Balloon Floater
	{
		get
		{
			return this.floater;
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001323B File Offset: 0x0001143B
	public void TrackPlayer(PlayerManager player, int playerNumber)
	{
		this.trackPlayer = true;
		this.sprite.localPosition = this.spriteInitPosition;
		this.floater.Entity.SetTrigger(false);
		base.StartCoroutine(this.Tracker(player, playerNumber));
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00013278 File Offset: 0x00011478
	public void ReInit()
	{
		this.trackPlayer = false;
		this.Floater.Entity.SetTrigger(true);
		this.currentOffset = -2f;
		this.sprite.localPosition = this.spriteInitPosition + Vector3.up * this.currentOffset;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000132CE File Offset: 0x000114CE
	public void Disable()
	{
		this.entity.Enable(false);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x000132DC File Offset: 0x000114DC
	public void Pop()
	{
		if (!this.popping)
		{
			if (this.trackPlayer)
			{
				this.mathMachine.NumberDropped(0);
			}
			this.popping = true;
			this.sprite.gameObject.SetActive(false);
			this.floater.Stop();
			this.entity.Enable(false);
			this.audMan.PlaySingle(this.audPop);
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.PopWait());
				return;
			}
		}
		else if (!base.gameObject.activeInHierarchy)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00013378 File Offset: 0x00011578
	private IEnumerator Tracker(PlayerManager player, int playerNumber)
	{
		this.floater.enabled = false;
		this.sprite.localPosition = this.spriteHeldPosition;
		this.sprite.gameObject.layer = 29;
		while (this.trackPlayer)
		{
			base.transform.position = player.transform.position;
			base.transform.rotation = player.transform.rotation;
			if (player.plm.Entity.InteractionDisabled)
			{
				this.ReInit();
				this.mathMachine.NumberDropped(player.playerNumber);
				this.targetOffset = 0f;
			}
			yield return null;
		}
		this.floater.enabled = true;
		base.transform.eulerAngles = Vector3.zero;
		this.sprite.gameObject.layer = this.initialSpriteLayer;
		yield break;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001338E File Offset: 0x0001158E
	private IEnumerator PopWait()
	{
		yield return null;
		while (this.audMan.QueuedAudioIsPlaying)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001339D File Offset: 0x0001159D
	public void Use()
	{
		this.available = false;
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060003AE RID: 942 RVA: 0x000133A6 File Offset: 0x000115A6
	public bool Available
	{
		get
		{
			return this.available;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060003AF RID: 943 RVA: 0x000133AE File Offset: 0x000115AE
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060003B0 RID: 944 RVA: 0x000133B6 File Offset: 0x000115B6
	public bool Popping
	{
		get
		{
			return this.popping;
		}
	}

	// Token: 0x040003D6 RID: 982
	public MathMachine mathMachine;

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	private Balloon floater;

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	private int value;

	// Token: 0x040003D9 RID: 985
	private int initialSpriteLayer;

	// Token: 0x040003DA RID: 986
	[SerializeField]
	private Transform sprite;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	private Vector3 spriteHeldPosition;

	// Token: 0x040003DC RID: 988
	private Vector3 spriteInitPosition;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040003DE RID: 990
	[SerializeField]
	private SoundObject audPop;

	// Token: 0x040003DF RID: 991
	[SerializeField]
	private Entity entity;

	// Token: 0x040003E0 RID: 992
	private float targetOffset;

	// Token: 0x040003E1 RID: 993
	private float currentOffset;

	// Token: 0x040003E2 RID: 994
	public bool trackPlayer;

	// Token: 0x040003E3 RID: 995
	private bool available = true;

	// Token: 0x040003E4 RID: 996
	private bool popping;
}
