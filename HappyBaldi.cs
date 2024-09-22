using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class HappyBaldi : MonoBehaviour
{
	// Token: 0x060001E5 RID: 485 RVA: 0x0000B1CE File Offset: 0x000093CE
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && !this.activated)
		{
			this.Activate();
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000B1F0 File Offset: 0x000093F0
	private void Activate()
	{
		this.activated = true;
		base.StartCoroutine(this.SpawnWait());
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000B206 File Offset: 0x00009406
	public void PlayIntroAudio()
	{
		this.audMan.PlaySingle(this.audIntro);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000B219 File Offset: 0x00009419
	private IEnumerator SpawnWait()
	{
		yield return null;
		float time = 1f;
		int count = 9;
		while (this.audMan.QueuedAudioIsPlaying || Singleton<CoreGameManager>.Instance.Paused)
		{
			yield return null;
		}
		while (count >= 0)
		{
			this.audMan.QueueAudio(this.audCountdown[count]);
			if (Random.value * 100f < 1f)
			{
				this.animator.Play("BAL_CountPeek", -1, 0f);
			}
			else
			{
				this.animator.Play("BAL_CountNormal", -1, 0f);
			}
			while (time > 0f)
			{
				time -= Time.deltaTime * this.ec.NpcTimeScale * 0.5f;
				yield return null;
			}
			count--;
			time = 1f;
		}
		this.audMan.QueueAudio(this.audHere);
		yield return null;
		while (this.audMan.QueuedAudioIsPlaying || Singleton<CoreGameManager>.Instance.Paused)
		{
			yield return null;
		}
		Singleton<MusicManager>.Instance.StopMidi();
		Singleton<BaseGameManager>.Instance.BeginSpoopMode();
		this.ec.SpawnNPCs();
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
		{
			this.ec.GetBaldi().transform.position = base.transform.position;
		}
		else if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
		{
			this.ec.GetBaldi().Despawn();
		}
		this.ec.StartEventTimers();
		this.sprite.enabled = false;
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B228 File Offset: 0x00009428
	// (set) Token: 0x060001EA RID: 490 RVA: 0x0000B230 File Offset: 0x00009430
	public EnvironmentController Ec
	{
		get
		{
			return this.ec;
		}
		set
		{
			this.ec = value;
		}
	}

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private Animator animator;

	// Token: 0x040001F7 RID: 503
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private EnvironmentController ec;

	// Token: 0x040001F9 RID: 505
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x040001FA RID: 506
	[SerializeField]
	private SoundObject audIntro;

	// Token: 0x040001FB RID: 507
	[SerializeField]
	private SoundObject audHere;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private SoundObject[] audCountdown = new SoundObject[10];

	// Token: 0x040001FD RID: 509
	private bool activated;
}
