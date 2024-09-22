using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class ITM_PrincipalWhistle : Item
{
	// Token: 0x06000344 RID: 836 RVA: 0x0001141C File Offset: 0x0000F61C
	public override bool Use(PlayerManager pm)
	{
		foreach (NPC npc in pm.ec.Npcs)
		{
			if (npc.Character == Character.Principal)
			{
				npc.GetComponent<Principal>().WhistleReact(pm.transform.position);
			}
		}
		Singleton<CoreGameManager>.Instance.audMan.PlaySingle(this.audWhistle);
		Object.Destroy(base.gameObject);
		return true;
	}

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private SoundObject audWhistle;
}
