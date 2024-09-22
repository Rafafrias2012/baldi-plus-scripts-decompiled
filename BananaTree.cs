using System;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class BananaTree : EnvironmentObject
{
	// Token: 0x060007A9 RID: 1961 RVA: 0x00026B70 File Offset: 0x00024D70
	public override void LoadingFinished()
	{
		base.LoadingFinished();
		int num = Random.Range(this.minBananas, this.maxBananas + 1);
		for (int i = 0; i < num; i++)
		{
			ITM_NanaPeel itm_NanaPeel = Object.Instantiate<ITM_NanaPeel>(this.bananaPrefab);
			Vector2 insideUnitCircle = Random.insideUnitCircle;
			Vector3 vector = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			float d = Random.Range(5f, this.radius);
			if (this.ec.ContainsCoordinates(base.transform.position + vector * d) && this.ec.CellFromPosition(base.transform.position + vector * d).room == this.ec.CellFromPosition(base.transform.position).room)
			{
				itm_NanaPeel.Spawn(this.ec, base.transform.position + vector * d, vector, 0f);
			}
			else if (this.ec.ContainsCoordinates(base.transform.position - vector * d) && this.ec.CellFromPosition(base.transform.position - vector * d).room == this.ec.CellFromPosition(base.transform.position).room)
			{
				itm_NanaPeel.Spawn(this.ec, base.transform.position - vector * d, vector, 0f);
			}
		}
	}

	// Token: 0x04000851 RID: 2129
	[SerializeField]
	private ITM_NanaPeel bananaPrefab;

	// Token: 0x04000852 RID: 2130
	[SerializeField]
	private float radius = 50f;

	// Token: 0x04000853 RID: 2131
	[SerializeField]
	private int minBananas = 3;

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private int maxBananas = 8;
}
