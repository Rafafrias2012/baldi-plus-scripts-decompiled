using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class ChalkEraser : Item
{
	// Token: 0x0600031E RID: 798 RVA: 0x0001032C File Offset: 0x0000E52C
	public override bool Use(PlayerManager pm)
	{
		IntVector2 gridPosition = IntVector2.GetGridPosition(pm.transform.position);
		this.ec = pm.ec;
		this.pos.x = (float)gridPosition.x * 10f + 5f;
		this.pos.z = (float)gridPosition.z * 10f + 5f;
		this.pos.y = this.ec.transform.position.y + 5f;
		base.transform.localPosition = this.pos;
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audUse);
		return true;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x000103DF File Offset: 0x0000E5DF
	private void Start()
	{
		base.StartCoroutine(this.Timer(this.setTime));
	}

	// Token: 0x06000320 RID: 800 RVA: 0x000103F4 File Offset: 0x0000E5F4
	private IEnumerator Timer(float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0001040C File Offset: 0x0000E60C
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			PlayerManager component = other.GetComponent<PlayerManager>();
			if (component != null)
			{
				this.currentPlayer = component;
				this.currentPlayer.SetInvisible(true);
			}
		}
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00010444 File Offset: 0x0000E644
	private void OnTriggerExit(Collider other)
	{
		if (this.currentPlayer != null && other.isTrigger)
		{
			PlayerManager component = other.GetComponent<PlayerManager>();
			if (component != null && component == this.currentPlayer)
			{
				this.currentPlayer.SetInvisible(false);
				this.currentPlayer = null;
			}
		}
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00010498 File Offset: 0x0000E698
	private void OnDestroy()
	{
		if (this.currentPlayer != null)
		{
			this.currentPlayer.SetInvisible(false);
			this.currentPlayer = null;
		}
	}

	// Token: 0x0400034E RID: 846
	public EnvironmentController ec;

	// Token: 0x0400034F RID: 847
	private PlayerManager currentPlayer;

	// Token: 0x04000350 RID: 848
	private Vector3 pos;

	// Token: 0x04000351 RID: 849
	[SerializeField]
	private SoundObject audUse;

	// Token: 0x04000352 RID: 850
	[SerializeField]
	private float setTime = 60f;
}
