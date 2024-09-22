using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001D5 RID: 469
public class MainMenu : MonoBehaviour
{
	// Token: 0x06000A99 RID: 2713 RVA: 0x00038058 File Offset: 0x00036258
	private void OnEnable()
	{
		if (DateTime.Now.Month == 9 && DateTime.Now.Day == 9 && DateTime.Now.Year == 2024)
		{
			this.yes = Singleton<PlayerFileManager>.Instance.fileName.Contains("NINENINE");
			this.timeToDoIt = 0f;
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x000380C0 File Offset: 0x000362C0
	private void Start()
	{
		if (Singleton<PlayerFileManager>.Instance.clearedLevels[2])
		{
			this.seedInput.SetActive(true);
		}
		if (PlayerPrefs.GetInt("NotifChecked") < 11)
		{
			this.aboutNotif.SetActive(true);
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000380F8 File Offset: 0x000362F8
	private void Update()
	{
		if (this.yes)
		{
			this.timeToDoIt += Time.unscaledDeltaTime;
			if (this.timeToDoIt >= 99f && !this.doingIt)
			{
				this.DoIt();
			}
			if (Singleton<InputManager>.Instance.AnyButton(false) || CursorController.Instance.position != this.lastCursorPosition)
			{
				this.timeToDoIt = 0f;
				this.lastCursorPosition = CursorController.Instance.position;
			}
		}
		if (this.doingIt)
		{
			if (this.poolOf99.Count < 256)
			{
				RectTransform rectTransform = Object.Instantiate<RectTransform>(this.ninetyninePrefab, base.transform);
				rectTransform.anchoredPosition = new Vector2((float)Random.Range(-240, 240), (float)Random.Range(-180, 180));
				this.poolOf99.Enqueue(rectTransform);
				return;
			}
			RectTransform rectTransform2 = this.poolOf99.Dequeue();
			if (rectTransform2 == null)
			{
				rectTransform2 = Object.Instantiate<RectTransform>(this.ninetyninePrefab, base.transform);
			}
			rectTransform2.anchoredPosition = new Vector2((float)Random.Range(-240, 240), (float)Random.Range(-180, 180));
			this.poolOf99.Enqueue(rectTransform2);
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00038240 File Offset: 0x00036440
	private void DoIt()
	{
		this.doingIt = true;
		this.audioSource.clip = this.aud99;
		this.audioSource.outputAudioMixerGroup = null;
		this.audioSource.volume = 0.5f;
		this.audioSource.Play();
		base.StartCoroutine(this.Messenger(99f));
		CursorController.Instance.enabled = false;
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x000382A9 File Offset: 0x000364A9
	private IEnumerator Messenger(float time)
	{
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		RectTransform current = null;
		time = 5f;
		int currentMessage = 0;
		while (currentMessage < this.message.Length)
		{
			current = Object.Instantiate<RectTransform>(this.message[currentMessage], base.transform);
			while (time > 0f)
			{
				time -= Time.unscaledDeltaTime;
				yield return null;
			}
			currentMessage++;
			if (currentMessage < this.message.Length)
			{
				Object.Destroy(current.gameObject);
			}
			time = 5f;
			yield return null;
		}
		while (this.audioSource.isPlaying)
		{
			current.localScale += Vector3.one * Time.unscaledDeltaTime * 0.18f;
			yield return null;
		}
		Application.Quit();
		yield break;
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000382BF File Offset: 0x000364BF
	public void UpdateNotif()
	{
		this.aboutNotif.SetActive(false);
		PlayerPrefs.SetInt("NotifChecked", 11);
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x000382D9 File Offset: 0x000364D9
	public void ToggleSubtitles(bool state)
	{
		Singleton<PlayerFileManager>.Instance.subtitles = state;
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x000382E6 File Offset: 0x000364E6
	public void Quit()
	{
		this.audioSource.Play();
		base.StartCoroutine(this.WaitForAudio());
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00038300 File Offset: 0x00036500
	private IEnumerator WaitForAudio()
	{
		float volume = 0f;
		this.audioSource.outputAudioMixerGroup.audioMixer.GetFloat("voiceVol", out volume);
		while (this.audioSource.isPlaying && volume > -80f)
		{
			yield return null;
		}
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		this.blackCover.SetActive(true);
		yield return null;
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		yield return null;
		Application.Quit();
		yield break;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0003830F File Offset: 0x0003650F
	public void ActivateTransition(float duration)
	{
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, duration);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0003831D File Offset: 0x0003651D
	public void LoadCredits()
	{
		SceneManager.LoadScene("Credits");
	}

	// Token: 0x04000C25 RID: 3109
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000C26 RID: 3110
	[SerializeField]
	private GameObject seedInput;

	// Token: 0x04000C27 RID: 3111
	[SerializeField]
	private GameObject blackCover;

	// Token: 0x04000C28 RID: 3112
	public GameObject aboutNotif;

	// Token: 0x04000C29 RID: 3113
	public RectTransform[] message = new RectTransform[0];

	// Token: 0x04000C2A RID: 3114
	public Queue<RectTransform> poolOf99 = new Queue<RectTransform>();

	// Token: 0x04000C2B RID: 3115
	public RectTransform ninetyninePrefab;

	// Token: 0x04000C2C RID: 3116
	public AudioClip aud99;

	// Token: 0x04000C2D RID: 3117
	private Vector2 lastCursorPosition;

	// Token: 0x04000C2E RID: 3118
	private float timeToDoIt;

	// Token: 0x04000C2F RID: 3119
	private bool yes;

	// Token: 0x04000C30 RID: 3120
	private bool doingIt;
}
