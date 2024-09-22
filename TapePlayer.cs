using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class TapePlayer : EnvironmentObject, IItemAcceptor
{
	// Token: 0x06000A5C RID: 2652 RVA: 0x000371E4 File Offset: 0x000353E4
	public override void LoadingFinished()
	{
		this.dijkstraMap = new DijkstraMap(this.ec, PathType.Const, new Transform[]
		{
			base.transform
		});
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00037208 File Offset: 0x00035408
	public void InsertItem(PlayerManager player, EnvironmentController ec)
	{
		ec.MakeSilent(this.time);
		this.audMan.PlaySingle(this.audInsert);
		this.audMan.PlaySingle(this.beep);
		this.active = true;
		base.StartCoroutine(this.Cooldown());
		this.dijkstraMap.QueueUpdate();
		this.dijkstraMap.Activate();
		if (this.changeOnUse)
		{
			this.spriteToChange.sprite = this.usedSprite;
		}
		if (ec.GetBaldi() != null)
		{
			ec.GetBaldi().Distract();
		}
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0003729F File Offset: 0x0003549F
	public bool ItemFits(Items checkItem)
	{
		return this.requiredItem == checkItem && !this.active;
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x000372B5 File Offset: 0x000354B5
	private IEnumerator Cooldown()
	{
		while (this.dijkstraMap.PendingUpdate)
		{
			yield return null;
		}
		using (List<NPC>.Enumerator enumerator = this.ec.Npcs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				NPC npc = enumerator.Current;
				if (npc.Navigator.enabled)
				{
					NavigationState_WanderFleeOverride navigationState_WanderFleeOverride = new NavigationState_WanderFleeOverride(npc, 31, this.dijkstraMap);
					this.fleeStates.Add(navigationState_WanderFleeOverride);
					npc.navigationStateMachine.ChangeState(navigationState_WanderFleeOverride);
				}
			}
			goto IL_D1;
		}
		IL_BA:
		yield return null;
		IL_D1:
		if (!this.audMan.QueuedAudioIsPlaying)
		{
			foreach (NavigationState_WanderFleeOverride navigationState_WanderFleeOverride2 in this.fleeStates)
			{
				navigationState_WanderFleeOverride2.End();
			}
			this.dijkstraMap.Deactivate();
			this.fleeStates.Clear();
			this.active = false;
			yield break;
		}
		goto IL_BA;
	}

	// Token: 0x04000BD1 RID: 3025
	[SerializeField]
	private Items requiredItem = Items.Tape;

	// Token: 0x04000BD2 RID: 3026
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000BD3 RID: 3027
	[SerializeField]
	private SoundObject beep;

	// Token: 0x04000BD4 RID: 3028
	[SerializeField]
	private SoundObject audInsert;

	// Token: 0x04000BD5 RID: 3029
	[SerializeField]
	private SpriteRenderer spriteToChange;

	// Token: 0x04000BD6 RID: 3030
	[SerializeField]
	private Sprite usedSprite;

	// Token: 0x04000BD7 RID: 3031
	private List<NavigationState_WanderFleeOverride> fleeStates = new List<NavigationState_WanderFleeOverride>();

	// Token: 0x04000BD8 RID: 3032
	private DijkstraMap dijkstraMap;

	// Token: 0x04000BD9 RID: 3033
	[SerializeField]
	private float time = 30f;

	// Token: 0x04000BDA RID: 3034
	[SerializeField]
	private bool changeOnUse;

	// Token: 0x04000BDB RID: 3035
	private bool active;
}
