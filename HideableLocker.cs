using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class HideableLocker : MonoBehaviour, IClickable<int>
{
	// Token: 0x060003BB RID: 955 RVA: 0x00013410 File Offset: 0x00011610
	public void Clicked(int player)
	{
		Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.SetInteractionState(false);
		Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.SetFrozen(true);
		Singleton<CoreGameManager>.Instance.GetCamera(player).SetControllable(false);
		Singleton<CoreGameManager>.Instance.GetCamera(player).UpdateTargets(this.cameraTransform, 20);
		this.playerInLocker = Singleton<CoreGameManager>.Instance.GetPlayer(player);
		base.StartCoroutine(this.CameraReset(this.playerInLocker));
		Singleton<CoreGameManager>.Instance.GetPlayer(player).ec.MakeNoise(this.targetTransform.position, this.noiseVal);
		this.audMan.PlaySingle(this.audSlam);
		this.hud.gameObject.SetActive(true);
		this.hud.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(player).canvasCam;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00013503 File Offset: 0x00011703
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00013505 File Offset: 0x00011705
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00013507 File Offset: 0x00011707
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001350A File Offset: 0x0001170A
	public bool ClickableRequiresNormalHeight()
	{
		return true;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001350D File Offset: 0x0001170D
	private IEnumerator CameraReset(PlayerManager player)
	{
		Vector3 pos = player.transform.position;
		yield return null;
		while (player == this.playerInLocker)
		{
			player.ec.MakeNoise(this.targetTransform.position, this.noiseVal);
			if (!Singleton<CoreGameManager>.Instance.Paused && (Singleton<InputManager>.Instance.GetDigitalInput("Interact", true) || player.transform.position != pos))
			{
				player.plm.Entity.SetInteractionState(true);
				player.plm.Entity.SetFrozen(false);
				this.playerInLocker = null;
			}
			if (Singleton<CoreGameManager>.Instance.Paused)
			{
				this.hud.gameObject.SetActive(false);
			}
			else
			{
				this.hud.gameObject.SetActive(true);
			}
			yield return null;
		}
		player.ec.MakeNoise(this.targetTransform.position, this.noiseVal);
		this.audMan.PlaySingle(this.audSlam);
		Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber).UpdateTargets(null, 20);
		Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber).SetControllable(true);
		this.hud.gameObject.SetActive(false);
		player.RuleBreak("Lockers", this.guiltTime);
		yield break;
	}

	// Token: 0x040003E5 RID: 997
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040003E6 RID: 998
	[SerializeField]
	private SoundObject audSlam;

	// Token: 0x040003E7 RID: 999
	[SerializeField]
	private Canvas hud;

	// Token: 0x040003E8 RID: 1000
	private PlayerManager playerInLocker;

	// Token: 0x040003E9 RID: 1001
	[SerializeField]
	private Transform cameraTransform;

	// Token: 0x040003EA RID: 1002
	[SerializeField]
	private Transform targetTransform;

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	private float guiltTime = 1f;

	// Token: 0x040003EC RID: 1004
	[SerializeField]
	[Range(0f, 127f)]
	private int noiseVal = 78;

	// Token: 0x040003ED RID: 1005
	[SerializeField]
	[Range(0f, 127f)]
	private int noSqueeUses = 3;
}
