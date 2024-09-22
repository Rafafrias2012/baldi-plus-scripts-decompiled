using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class AudioManager : MonoBehaviour
{
	// Token: 0x06000577 RID: 1399 RVA: 0x0001BBD2 File Offset: 0x00019DD2
	private void Awake()
	{
		if (!this.disableSubtitles)
		{
			this.sourceId = AudioManager.totalIds;
			AudioManager.totalIds++;
			if (AudioManager.totalIds >= SubtitleManager.totalIds)
			{
				AudioManager.totalIds = 0;
			}
		}
		this.VirtualAwake();
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0001BC0B File Offset: 0x00019E0B
	protected virtual void VirtualAwake()
	{
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001BC10 File Offset: 0x00019E10
	private void Update()
	{
		if (this.audioDevice.isPlaying || this.playingOneShotAudio)
		{
			if (!this.useUnscaledPitch)
			{
				this.audioDevice.pitch = Time.timeScale * this.pitchModifier;
			}
			else
			{
				this.audioDevice.pitch = this.pitchModifier;
			}
			double num = AudioSettings.dspTime - this.previousDspTime;
			this.remainingOneShotTime -= num * (double)this.audioDevice.pitch;
			if (this.remainingOneShotTime <= 0.0)
			{
				this.playingOneShotAudio = false;
			}
		}
		this.VirtualUpdate();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0001BCAA File Offset: 0x00019EAA
	protected virtual void VirtualUpdate()
	{
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001BCAC File Offset: 0x00019EAC
	private void Start()
	{
		if (this.ignoreListenerPause)
		{
			this.audioDevice.ignoreListenerPause = true;
		}
		if (this.soundOnStart.Length != 0)
		{
			if (this.loopOnStart)
			{
				this.SetLoop(true);
				this.maintainLoop = true;
			}
			for (int i = 0; i < this.soundOnStart.Length; i++)
			{
				this.QueueAudio(this.soundOnStart[i], i == 0);
			}
			if (this.randomizeStartPosition)
			{
				this.audioDevice.timeSamples = Random.Range(0, this.audioDevice.clip.samples);
			}
		}
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001BD3A File Offset: 0x00019F3A
	private void LateUpdate()
	{
		this.VirtualLateUpdate();
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0001BD42 File Offset: 0x00019F42
	protected virtual void VirtualLateUpdate()
	{
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0001BD44 File Offset: 0x00019F44
	protected virtual void UpdateAudioDeviceVolume()
	{
		this.audioDevice.volume = this.volumeMultiplier;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0001BD57 File Offset: 0x00019F57
	private IEnumerator PlayAudio()
	{
		this.ienumeratorRunning = true;
		while (AudioListener.pause)
		{
			if (this.audioDevice.ignoreListenerPause)
			{
				break;
			}
			yield return null;
		}
		while (this.filesQueued > 0 || this.audioDevice.isPlaying || (AudioListener.pause && !this.audioDevice.ignoreListenerPause))
		{
			if (!this.useUnscaledPitch)
			{
				this.audioDevice.pitch = Time.timeScale * this.pitchModifier;
			}
			else
			{
				this.audioDevice.pitch = this.pitchModifier;
			}
			if ((!AudioListener.pause || this.audioDevice.ignoreListenerPause) && !this.audioDevice.isPlaying)
			{
				this.PlayQueue();
			}
			yield return null;
		}
		this.ienumeratorRunning = false;
		yield break;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0001BD68 File Offset: 0x00019F68
	public virtual void PlaySingle(SoundObject file)
	{
		if (file.mixerOverride != null)
		{
			this.audioDevice.outputAudioMixerGroup = file.mixerOverride;
		}
		else
		{
			this.audioDevice.outputAudioMixerGroup = Singleton<PlayerFileManager>.Instance.mixer[(int)file.soundType];
		}
		this.volumeMultiplier = file.volumeMultiplier;
		this.UpdateAudioDeviceVolume();
		if (file.soundType == SoundType.Voice)
		{
			this._oneShotClip = Singleton<LocalizationManager>.Instance.GetLocalizedAudioClip(file);
			this.audioDevice.PlayOneShot(this._oneShotClip, Singleton<PlayerFileManager>.Instance.volume[(int)file.soundType]);
		}
		else
		{
			this._oneShotClip = file.soundClip;
			this.audioDevice.PlayOneShot(this._oneShotClip);
		}
		this.playingOneShotAudio = true;
		this.remainingOneShotTime = (double)Mathf.Max((float)this.remainingOneShotTime, this._oneShotClip.length);
		if (!this.disableSubtitles)
		{
			if (this.overrideSubtitleColor)
			{
				this.CreateSubtitle(file, false, this.subtitleColor);
			}
			else
			{
				this.CreateSubtitle(file, false, file.color);
			}
		}
		this.QueueMemory(file);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0001BE78 File Offset: 0x0001A078
	public void QueueAudio(SoundObject file)
	{
		this.QueueAudio(file, false);
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0001BE84 File Offset: 0x0001A084
	public void QueueAudio(SoundObject file, bool playImmediately)
	{
		if (this.filesQueued < this.soundQueue.Length)
		{
			this.soundQueue[this.filesQueued] = file;
			this.filesQueued++;
		}
		else
		{
			Debug.LogWarning("Sound queue on Audio Manager " + this.sourceId.ToString() + " full.");
		}
		if (!this.maintainLoop)
		{
			this.loop = false;
		}
		if (!this.ienumeratorRunning)
		{
			base.StartCoroutine(this.PlayAudio());
			this.ienumeratorRunning = true;
		}
		if (playImmediately && this.filesQueued > 0)
		{
			this.PlayQueue();
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0001BF1B File Offset: 0x0001A11B
	public void QueueRandomAudio(SoundObject[] sounds)
	{
		this.QueueAudio(sounds[Random.Range(0, sounds.Length)]);
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0001BF2E File Offset: 0x0001A12E
	public void PlayRandomAudio(SoundObject[] sounds)
	{
		this.PlaySingle(sounds[Random.Range(0, sounds.Length)]);
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0001BF44 File Offset: 0x0001A144
	public void FlushQueue(bool endCurrent)
	{
		for (int i = 0; i < this.soundQueue.Length; i++)
		{
			this.soundQueue[i] = null;
		}
		this.filesQueued = 0;
		if (endCurrent)
		{
			this.audioDevice.Stop();
			base.StopCoroutine(this.PlayAudio());
			this.ienumeratorRunning = false;
			Singleton<SubtitleManager>.Instance.DestroySub(this.sourceId);
		}
		this.loop = false;
		this.audioDevice.loop = false;
		this.audioDevice.clip = null;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001BFC4 File Offset: 0x0001A1C4
	public virtual void PlayQueue()
	{
		if (this.soundQueue.Length != 0 && this.soundQueue[0] != null)
		{
			if (this.soundQueue[0].soundType == SoundType.Voice)
			{
				this.audioDevice.clip = Singleton<LocalizationManager>.Instance.GetLocalizedAudioClip(this.soundQueue[0]);
			}
			else
			{
				this.audioDevice.clip = this.soundQueue[0].soundClip;
			}
			if (this.soundQueue[0].mixerOverride != null)
			{
				this.audioDevice.outputAudioMixerGroup = this.soundQueue[0].mixerOverride;
			}
			else
			{
				this.audioDevice.outputAudioMixerGroup = Singleton<PlayerFileManager>.Instance.mixer[(int)this.soundQueue[0].soundType];
			}
			this.volumeMultiplier = this.soundQueue[0].volumeMultiplier;
			this.UpdateAudioDeviceVolume();
			this.audioDevice.time = 0f;
			this.audioDevice.Play();
			if (!this.disableSubtitles)
			{
				if (this.overrideSubtitleColor)
				{
					this.CreateSubtitle(this.soundQueue[0], this.loop, this.subtitleColor);
				}
				else
				{
					this.CreateSubtitle(this.soundQueue[0], this.loop, this.soundQueue[0].color);
				}
			}
			this.QueueMemory(this.soundQueue[0]);
			for (int i = 0; i < this.filesQueued - 1; i++)
			{
				this.soundQueue[i] = this.soundQueue[i + 1];
			}
			this.soundQueue[this.filesQueued - 1] = null;
			this.filesQueued--;
			this.SetLoop(this.loop);
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0001C168 File Offset: 0x0001A368
	private void CreateSubtitle(SoundObject soundObject, bool loopSub, Color color)
	{
		if (soundObject.subtitle)
		{
			if (Singleton<CoreGameManager>.Instance != null && this.positional)
			{
				for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
				{
					Singleton<SubtitleManager>.Instance.CreateSub(soundObject, this, this.sourceId, this.audioDevice.maxDistance, loopSub, color);
				}
				return;
			}
			Singleton<SubtitleManager>.Instance.CreateSub(soundObject, this, this.sourceId, this.audioDevice.maxDistance, loopSub, color);
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001C1E6 File Offset: 0x0001A3E6
	public virtual float GetSubtitleScale(Transform cameraTransform)
	{
		return Mathf.Max(1f - Vector3.Distance(cameraTransform.position, base.transform.position) / this.audioDevice.maxDistance, 0f);
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001C21A File Offset: 0x0001A41A
	public void FadeOut(float fadeTime)
	{
		if (this.fadeControl != null)
		{
			base.StopCoroutine(this.fadeControl);
		}
		this.fadeControl = this.FadeOutCoroutine(fadeTime);
		base.StartCoroutine(this.fadeControl);
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0001C24A File Offset: 0x0001A44A
	private IEnumerator FadeOutCoroutine(float time)
	{
		float initialVal = time;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			this.volumeMultiplier = time / initialVal;
			this.UpdateAudioDeviceVolume();
			yield return null;
		}
		this.FlushQueue(true);
		this.volumeMultiplier = 0f;
		this.UpdateAudioDeviceVolume();
		yield break;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001C260 File Offset: 0x0001A460
	public void SetLoop(bool val)
	{
		this.loop = val;
		if (this.filesQueued == 0 && val)
		{
			this.audioDevice.loop = true;
			return;
		}
		this.audioDevice.loop = false;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001C28F File Offset: 0x0001A48F
	public void Pause(bool pause)
	{
		if (pause)
		{
			this.audioDevice.Pause();
			return;
		}
		this.audioDevice.Play();
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001C2AB File Offset: 0x0001A4AB
	public bool QueuedAudioIsPlaying
	{
		get
		{
			return this.ienumeratorRunning || this.audioDevice.isPlaying;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001C2C2 File Offset: 0x0001A4C2
	public bool AnyAudioIsPlaying
	{
		get
		{
			return this.ienumeratorRunning || this.audioDevice.isPlaying || this.playingOneShotAudio;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001C2E1 File Offset: 0x0001A4E1
	public bool QueuedUp
	{
		get
		{
			return this.filesQueued != 0;
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001C2EC File Offset: 0x0001A4EC
	private void QueueMemory(SoundObject sound)
	{
		if (sound.addToMemory)
		{
			for (int i = 0; i < AudioManager.audioMemory.Length; i++)
			{
				if (sound == AudioManager.audioMemory[i])
				{
					return;
				}
			}
			AudioManager.audioMemory[AudioManager.memoryPos] = sound;
			AudioManager.memoryPos++;
			if (AudioManager.memoryPos >= AudioManager.audioMemory.Length)
			{
				AudioManager.memoryPos = 0;
			}
		}
	}

	// Token: 0x040005A8 RID: 1448
	public AudioSource audioDevice;

	// Token: 0x040005A9 RID: 1449
	public static SoundObject[] audioMemory = new SoundObject[32];

	// Token: 0x040005AA RID: 1450
	public SoundObject[] soundQueue = new SoundObject[64];

	// Token: 0x040005AB RID: 1451
	private AudioClip _oneShotClip;

	// Token: 0x040005AC RID: 1452
	private double previousDspTime;

	// Token: 0x040005AD RID: 1453
	private double remainingOneShotTime;

	// Token: 0x040005AE RID: 1454
	public float volumeModifier = 1f;

	// Token: 0x040005AF RID: 1455
	public float pitchModifier = 1f;

	// Token: 0x040005B0 RID: 1456
	protected float volumeMultiplier = 1f;

	// Token: 0x040005B1 RID: 1457
	public int filesQueued;

	// Token: 0x040005B2 RID: 1458
	public int sourceId;

	// Token: 0x040005B3 RID: 1459
	public static int totalIds;

	// Token: 0x040005B4 RID: 1460
	public static int memoryPos;

	// Token: 0x040005B5 RID: 1461
	public bool useUnscaledPitch;

	// Token: 0x040005B6 RID: 1462
	public bool loop;

	// Token: 0x040005B7 RID: 1463
	public bool maintainLoop;

	// Token: 0x040005B8 RID: 1464
	public bool ignoreListenerPause;

	// Token: 0x040005B9 RID: 1465
	public bool positional = true;

	// Token: 0x040005BA RID: 1466
	public bool overrideCaptionPosition;

	// Token: 0x040005BB RID: 1467
	protected bool playingOneShotAudio;

	// Token: 0x040005BC RID: 1468
	private bool ienumeratorRunning;

	// Token: 0x040005BD RID: 1469
	private IEnumerator fadeControl;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private SoundObject[] soundOnStart = new SoundObject[0];

	// Token: 0x040005BF RID: 1471
	[SerializeField]
	private bool loopOnStart;

	// Token: 0x040005C0 RID: 1472
	[SerializeField]
	private bool randomizeStartPosition;

	// Token: 0x040005C1 RID: 1473
	[SerializeField]
	private bool disableSubtitles;

	// Token: 0x040005C2 RID: 1474
	[SerializeField]
	private bool overrideSubtitleColor;

	// Token: 0x040005C3 RID: 1475
	[SerializeField]
	private Color subtitleColor = Color.white;

	// Token: 0x040005C4 RID: 1476
	public Vector3 captionPosition;

	// Token: 0x040005C5 RID: 1477
	public Vector2 captionAnchor = new Vector2(0.5f, 0.5f);
}
