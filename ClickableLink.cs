using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class ClickableLink : MonoBehaviour, IClickable<int>
{
	// Token: 0x060005A8 RID: 1448 RVA: 0x0001C5DC File Offset: 0x0001A7DC
	private void Awake()
	{
		this.linkedClickable = this.link.GetComponent<IClickable<int>>();
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0001C5EF File Offset: 0x0001A7EF
	public void Clicked(int player)
	{
		this.linkedClickable.Clicked(player);
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0001C5FD File Offset: 0x0001A7FD
	public void ClickableSighted(int player)
	{
		this.linkedClickable.ClickableSighted(player);
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0001C60B File Offset: 0x0001A80B
	public void ClickableUnsighted(int player)
	{
		this.linkedClickable.ClickableUnsighted(player);
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0001C619 File Offset: 0x0001A819
	public bool ClickableHidden()
	{
		return this.linkedClickable.ClickableHidden();
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001C626 File Offset: 0x0001A826
	public bool ClickableRequiresNormalHeight()
	{
		return this.linkedClickable.ClickableRequiresNormalHeight();
	}

	// Token: 0x040005D0 RID: 1488
	[SerializeField]
	private MonoBehaviour link;

	// Token: 0x040005D1 RID: 1489
	private IClickable<int> linkedClickable;
}
