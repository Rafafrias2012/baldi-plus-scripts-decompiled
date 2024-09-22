using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200005A RID: 90
public class Jumprope : MonoBehaviour
{
	// Token: 0x060001FC RID: 508 RVA: 0x0000B4E4 File Offset: 0x000096E4
	private void Start()
	{
		base.transform.position = this.player.cameraBase.position;
		this.player.plm.am.moveMods.Add(this.moveMod);
		this.player.jumpropes.Add(this);
		Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).UpdateTargets(base.transform, 24);
		this.ropeCanvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).canvasCam;
		this.ropeCanvas.transform.SetParent(null);
		this.ropeCanvas.transform.position = Vector3.zero;
		this.textCanvas.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).canvasCam;
		this.textCanvas.transform.SetParent(null);
		this.textCanvas.transform.position = Vector3.zero;
		if (!Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			this.textScaler.scaleFactor = (float)Mathf.RoundToInt((float)Singleton<PlayerFileManager>.Instance.resolutionY / 360f);
		}
		this.instructionsTmp.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Hud_Playtime_Instructions"), Singleton<InputManager>.Instance.GetInputButtonName("Interact"));
		this.UpdateCount();
		this.totalPoints = this.startVal;
		base.StartCoroutine(this.RopeTimer());
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000B670 File Offset: 0x00009870
	private void Update()
	{
		base.transform.position = this.player.cameraBase.position + Singleton<CoreGameManager>.Instance.GetCamera(0).transform.up * this.height;
		base.transform.rotation = this.player.cameraBase.rotation;
		if (Singleton<InputManager>.Instance.GetDigitalInput("Interact", true) && this.height <= 0f)
		{
			base.StartCoroutine(this.Jump());
		}
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000B704 File Offset: 0x00009904
	private void RopeDown()
	{
		if (this.height > this.jumpBuffer)
		{
			this.jumps++;
			this.playtime.Count(this.jumps);
		}
		else
		{
			this.jumps = 0;
			this.playtime.JumpropeHit();
			this.totalPoints += this.penaltyVal;
			if (this.totalPoints < 0)
			{
				this.totalPoints = 0;
			}
		}
		this.UpdateCount();
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000B77B File Offset: 0x0000997B
	private void UpdateCount()
	{
		this.countTmp.text = string.Format("{0}/{1}", this.jumps, this.maxJumps);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000B7A8 File Offset: 0x000099A8
	public void End(bool success)
	{
		this.playtime.EndJumprope(success);
		if (success)
		{
			Singleton<CoreGameManager>.Instance.AddPoints(this.totalPoints, this.player.playerNumber, true);
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000B7D8 File Offset: 0x000099D8
	public void Destroy()
	{
		this.player.plm.am.moveMods.Remove(this.moveMod);
		Singleton<CoreGameManager>.Instance.GetCamera(this.player.playerNumber).UpdateTargets(null, 24);
		this.player.jumpropes.Remove(this);
		Object.Destroy(this.textCanvas.gameObject);
		Object.Destroy(this.ropeCanvas.gameObject);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000B860 File Offset: 0x00009A60
	private IEnumerator Jump()
	{
		float velocity = this.initVelocity;
		this.moveMod.movementMultiplier = 0.25f;
		while (this.height >= 0f)
		{
			this.height += velocity * Time.deltaTime + 0.5f * this.accel * Time.deltaTime * Time.deltaTime;
			velocity += this.accel * Time.deltaTime;
			if (this.height > this.maxHeight)
			{
				this.maxHeight = this.height;
			}
			if (this.height <= 0f)
			{
				break;
			}
			yield return null;
		}
		this.height = 0f;
		this.moveMod.movementMultiplier = 0f;
		yield break;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000B86F File Offset: 0x00009A6F
	private IEnumerator RopeTimer()
	{
		while (this.jumps < this.maxJumps)
		{
			float delay = this.ropeDelay;
			while (delay > 0f)
			{
				delay -= Time.deltaTime;
				yield return null;
			}
			this.animator.Play("Jumprope", -1, 0f);
			float hitTime = this.ropeTime;
			while (hitTime > 0f)
			{
				hitTime -= Time.deltaTime;
				yield return null;
			}
			this.RopeDown();
		}
		while (this.height > 0f)
		{
			yield return null;
		}
		this.End(true);
		yield break;
	}

	// Token: 0x04000206 RID: 518
	[SerializeField]
	private Animator animator;

	// Token: 0x04000207 RID: 519
	[SerializeField]
	private MovementModifier moveMod = new MovementModifier(default(Vector3), 0f);

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private Canvas ropeCanvas;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	private Canvas textCanvas;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	private CanvasScaler textScaler;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	private TMP_Text instructionsTmp;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	private TMP_Text countTmp;

	// Token: 0x0400020D RID: 525
	public Playtime playtime;

	// Token: 0x0400020E RID: 526
	public PlayerManager player;

	// Token: 0x0400020F RID: 527
	public GameCamera gameCamera;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	private float ropeDelay = 1f;

	// Token: 0x04000211 RID: 529
	[SerializeField]
	private float ropeTime = 0.9f;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	private float jumpBuffer = 0.2f;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private float initVelocity = 2f;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private float accel = -2f;

	// Token: 0x04000215 RID: 533
	private float height;

	// Token: 0x04000216 RID: 534
	public float maxHeight;

	// Token: 0x04000217 RID: 535
	[SerializeField]
	private int maxJumps = 5;

	// Token: 0x04000218 RID: 536
	[SerializeField]
	private int startVal = 25;

	// Token: 0x04000219 RID: 537
	[SerializeField]
	private int penaltyVal = -5;

	// Token: 0x0400021A RID: 538
	private int jumps;

	// Token: 0x0400021B RID: 539
	private int totalPoints;
}
