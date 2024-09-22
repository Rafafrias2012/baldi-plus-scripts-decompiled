using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class ITM_PortalPoster : Item
{
	// Token: 0x06000342 RID: 834 RVA: 0x000111E4 File Offset: 0x0000F3E4
	public override bool Use(PlayerManager pm)
	{
		if (!Physics.Raycast(pm.transform.position, Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.forward, out this.hit, pm.pc.reach, pm.pc.ClickLayers, QueryTriggerInteraction.Ignore))
		{
			Object.Destroy(base.gameObject);
			return false;
		}
		if (!this.hit.transform.CompareTag("Wall"))
		{
			Object.Destroy(base.gameObject);
			return false;
		}
		Direction direction = Directions.DirFromVector3(this.hit.transform.forward, 5f);
		Cell cell = pm.ec.CellFromPosition(IntVector2.GetGridPosition(this.hit.transform.position + this.hit.transform.forward * 5f));
		Cell cell2 = pm.ec.CellFromPosition(IntVector2.GetGridPosition(this.hit.transform.position + this.hit.transform.forward * -5f));
		if (pm.ec.ContainsCoordinates(IntVector2.GetGridPosition(this.hit.transform.position + this.hit.transform.forward * 5f)) && !cell.Null && cell.HasWallInDirection(direction.GetOpposite()) && !cell.WallHardCovered(direction.GetOpposite()) && pm.ec.ContainsCoordinates(IntVector2.GetGridPosition(this.hit.transform.position + this.hit.transform.forward * -5f)) && !cell2.Null && !cell2.WallHardCovered(direction))
		{
			pm.ec.BuildWindow(cell2, direction, this.windowObject, false);
			Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audYes);
			return true;
		}
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audNo);
		return false;
	}

	// Token: 0x0400038E RID: 910
	private RaycastHit hit;

	// Token: 0x0400038F RID: 911
	[SerializeField]
	private WindowObject windowObject;

	// Token: 0x04000390 RID: 912
	[SerializeField]
	private Material mask;

	// Token: 0x04000391 RID: 913
	[SerializeField]
	private Material overlay;

	// Token: 0x04000392 RID: 914
	[SerializeField]
	private SoundObject audYes;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	private SoundObject audNo;
}
