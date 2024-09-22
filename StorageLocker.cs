using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class StorageLocker : EnvironmentObject, IClickable<int>
{
	// Token: 0x060003F7 RID: 1015 RVA: 0x00014E1C File Offset: 0x0001301C
	private void Start()
	{
		for (int i = 0; i < this.pickup.Length; i++)
		{
			this.pickup[i].AssignItem(Singleton<CoreGameManager>.Instance.currentLockerItems[i]);
			this.pickup[i].OnItemCollected += this.UpdateContents;
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00014E70 File Offset: 0x00013070
	private void UpdateContents(Pickup collectedPickup, int player)
	{
		for (int i = 0; i < this.pickup.Length; i++)
		{
			Singleton<CoreGameManager>.Instance.currentLockerItems[i] = this.pickup[i].item;
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00014EA9 File Offset: 0x000130A9
	public void Clicked(int player)
	{
		this.Toggle();
		if (this.open)
		{
			base.StartCoroutine(this.ShutWhenPlayerLeaves(Singleton<CoreGameManager>.Instance.GetPlayer(player)));
			return;
		}
		base.StopAllCoroutines();
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00014ED8 File Offset: 0x000130D8
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00014EDA File Offset: 0x000130DA
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00014EDC File Offset: 0x000130DC
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00014EDF File Offset: 0x000130DF
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00014EE4 File Offset: 0x000130E4
	private void Toggle()
	{
		this.open = !this.open;
		BoxCollider[] array = this.openBoxCollider;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.open;
		}
		array = this.closedBoxCollider;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = !this.open;
		}
		Pickup[] array2;
		if (this.open)
		{
			this.meshFilter.mesh = this.openMesh;
			this.audMan.PlaySingle(this.audOpen);
			array2 = this.pickup;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.SetActive(true);
			}
			return;
		}
		this.ec.MakeNoise(base.transform.position, this.noiseVal);
		this.meshFilter.mesh = this.closedMesh;
		this.audMan.PlaySingle(this.audClose);
		array2 = this.pickup;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00014FF8 File Offset: 0x000131F8
	private IEnumerator ShutWhenPlayerLeaves(PlayerManager player)
	{
		while (Vector3.Distance(base.transform.position, player.transform.position) < this.maxPlayerDistance)
		{
			player.RuleBreak("Lockers", 1f);
			yield return null;
		}
		if (this.open)
		{
			this.Toggle();
		}
		yield break;
	}

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000435 RID: 1077
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000436 RID: 1078
	[SerializeField]
	private SoundObject audOpen;

	// Token: 0x04000437 RID: 1079
	[SerializeField]
	private SoundObject audClose;

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private BoxCollider[] closedBoxCollider = new BoxCollider[0];

	// Token: 0x04000439 RID: 1081
	[SerializeField]
	private BoxCollider[] openBoxCollider = new BoxCollider[0];

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	private Pickup[] pickup = new Pickup[3];

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	private Mesh closedMesh;

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	private Mesh openMesh;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private float maxPlayerDistance = 30f;

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	[Range(0f, 127f)]
	private int noiseVal = 78;

	// Token: 0x0400043F RID: 1087
	private bool open;
}
