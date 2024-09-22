using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class ITM_Teleporter : Item
{
	// Token: 0x0600034E RID: 846 RVA: 0x0001185A File Offset: 0x0000FA5A
	public override bool Use(PlayerManager pm)
	{
		this.pm = pm;
		base.StartCoroutine(this.Teleporter());
		return true;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00011871 File Offset: 0x0000FA71
	private IEnumerator Teleporter()
	{
		this.pm.plm.Entity.SetInteractionState(false);
		this.pm.plm.Entity.SetFrozen(true);
		int teleports = Random.Range(12, 16);
		int teleportCount = 0;
		float baseTime = 0.2f;
		float currentTime = baseTime;
		float increaseFactor = 1.1f;
		while (teleportCount < teleports)
		{
			currentTime -= Time.deltaTime;
			if (currentTime <= 0f)
			{
				this.Teleport();
				teleportCount++;
				baseTime *= increaseFactor;
				currentTime = baseTime;
			}
			yield return null;
		}
		this.pm.plm.Entity.SetInteractionState(true);
		this.pm.plm.Entity.SetFrozen(false);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00011880 File Offset: 0x0000FA80
	private void Teleport()
	{
		this.pm.transform.position = this.pm.ec.RandomCell(false, false, true).FloorWorldPosition + Vector3.up * 5f;
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audTeleport);
	}

	// Token: 0x0400039B RID: 923
	[SerializeField]
	private SoundObject audTeleport;

	// Token: 0x0400039C RID: 924
	private List<Cell> tiles = new List<Cell>();
}
