using System;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class MapIcon : MonoBehaviour
{
	// Token: 0x0600092D RID: 2349 RVA: 0x00030FA8 File Offset: 0x0002F1A8
	private void Awake()
	{
		this.transform = base.gameObject.transform;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00030FBC File Offset: 0x0002F1BC
	public void UpdatePosition(Map map)
	{
		this.transform.position = new Vector3(this.target.position.x / 10f - 0.5f, this.target.position.z / 10f - 0.5f, 0f);
		this.transform.parent = map.ClosestTileFromPosition(this.target.position).transform;
	}

	// Token: 0x04000A1E RID: 2590
	public SpriteRenderer spriteRenderer;

	// Token: 0x04000A1F RID: 2591
	public new Transform transform;

	// Token: 0x04000A20 RID: 2592
	public Transform target;
}
