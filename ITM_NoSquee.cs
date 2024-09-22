using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class ITM_NoSquee : Item
{
	// Token: 0x0600033F RID: 831 RVA: 0x00011064 File Offset: 0x0000F264
	public override bool Use(PlayerManager pm)
	{
		this.silencedCells.Add(pm.ec.CellFromPosition(pm.transform.position));
		DijkstraMap dijkstraMap = new DijkstraMap(pm.ec, PathType.Nav, Array.Empty<Transform>());
		dijkstraMap.Calculate(this.distance + 1, true, new IntVector2[]
		{
			pm.ec.CellFromPosition(pm.transform.position).position
		});
		this.silencedCells.AddRange(dijkstraMap.FoundCells());
		foreach (Cell cell in this.silencedCells)
		{
			cell.SetSilence(true);
			if (dijkstraMap.Value(cell.position) <= this.distance)
			{
				this.sparkleParticelEmitters.Add(Object.Instantiate<Transform>(this.sparkleParticlesPre, cell.ObjectBase));
			}
		}
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.sound);
		base.StartCoroutine(this.Timer(pm.ec, this.time));
		return true;
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00011194 File Offset: 0x0000F394
	private IEnumerator Timer(EnvironmentController ec, float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime * ec.EnvironmentTimeScale;
			yield return null;
		}
		foreach (Cell cell in this.silencedCells)
		{
			cell.SetSilence(false);
		}
		foreach (Transform transform in this.sparkleParticelEmitters)
		{
			Object.Destroy(transform.gameObject);
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000388 RID: 904
	private List<Transform> sparkleParticelEmitters = new List<Transform>();

	// Token: 0x04000389 RID: 905
	[SerializeField]
	private Transform sparkleParticlesPre;

	// Token: 0x0400038A RID: 906
	[SerializeField]
	private SoundObject sound;

	// Token: 0x0400038B RID: 907
	private List<Cell> silencedCells = new List<Cell>();

	// Token: 0x0400038C RID: 908
	[SerializeField]
	private float time = 300f;

	// Token: 0x0400038D RID: 909
	[SerializeField]
	private int distance = 4;
}
