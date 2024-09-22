using System;
using System.Collections;
using MidiPlayerTK;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class BaldiDance : MonoBehaviour
{
	// Token: 0x06000AEC RID: 2796 RVA: 0x0003996C File Offset: 0x00037B6C
	private void OnEnable()
	{
		MusicManager.OnMidiEvent += this.MidiEvent;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0003997F File Offset: 0x00037B7F
	private void OnDisable()
	{
		MusicManager.OnMidiEvent -= this.MidiEvent;
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00039994 File Offset: 0x00037B94
	private void Update()
	{
		if (this.glitching)
		{
			this._pos = base.transform.position;
			this._pos.y = 1.83f;
			this._pos.z = Random.Range(0f, 27.5f);
			base.transform.position = this._pos;
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x000399F5 File Offset: 0x00037BF5
	public void MidiEvent(MPTKEvent midiEvent)
	{
		if (midiEvent.Channel == 9 && midiEvent.Command == MPTKCommand.NoteOn)
		{
			this.SwapSprites();
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00039A14 File Offset: 0x00037C14
	public void SwapSprites()
	{
		if (this.dancePose)
		{
			this.baldiSprite.sprite = this.danceSprites[0];
		}
		else
		{
			this.baldiSprite.sprite = this.danceSprites[1];
			this.audMan.PlaySingle(this.audSong[this.currentLine]);
			this.currentLine++;
			if (this.currentLine >= this.audSong.Length)
			{
				this.currentLine = 0;
			}
		}
		this.dancePose = !this.dancePose;
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00039AA0 File Offset: 0x00037CA0
	public void CrashSound()
	{
		this.audMan.audioDevice.Stop();
		this.crashAudioSource.clip = Singleton<LocalizationManager>.Instance.GetLocalizedAudioClip(this.audSong[Random.Range(0, this.audSong.Length)]);
		this.crashAudioSource.Play();
		float num = Random.Range(0f, this.crashAudioSource.clip.length);
		this.crashAudioSource.time = num;
		base.StartCoroutine(this.CrashSound(num));
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00039B27 File Offset: 0x00037D27
	private IEnumerator CrashSound(float sampleTime)
	{
		while (this.glitching)
		{
			this.crashAudioSource.time = sampleTime;
			yield return null;
		}
		this.crashAudioSource.Stop();
		yield break;
	}

	// Token: 0x04000C80 RID: 3200
	private bool dancePose;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000C82 RID: 3202
	public AudioSource crashAudioSource;

	// Token: 0x04000C83 RID: 3203
	[SerializeField]
	private SoundObject[] audSong = new SoundObject[8];

	// Token: 0x04000C84 RID: 3204
	[SerializeField]
	private SpriteRenderer baldiSprite;

	// Token: 0x04000C85 RID: 3205
	[SerializeField]
	private Sprite[] danceSprites = new Sprite[2];

	// Token: 0x04000C86 RID: 3206
	private Vector3 _pos;

	// Token: 0x04000C87 RID: 3207
	private int currentLine;

	// Token: 0x04000C88 RID: 3208
	public bool glitching;
}
