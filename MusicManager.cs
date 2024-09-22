using System;
using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class MusicManager : Singleton<MusicManager>
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060006E9 RID: 1769 RVA: 0x00022F20 File Offset: 0x00021120
	// (remove) Token: 0x060006EA RID: 1770 RVA: 0x00022F54 File Offset: 0x00021154
	public static event MusicManager.MidiEventFunction OnMidiEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060006EB RID: 1771 RVA: 0x00022F88 File Offset: 0x00021188
	// (remove) Token: 0x060006EC RID: 1772 RVA: 0x00022FBC File Offset: 0x000211BC
	public static event MusicManager.MidiLoopFunction OnMidiLoop;

	// Token: 0x060006ED RID: 1773 RVA: 0x00022FF0 File Offset: 0x000211F0
	private void Start()
	{
		this.midiPlayer.MPTK_KeepNoteOff = true;
		this.midiSource.ignoreListenerPause = true;
		this.synthSource.ignoreListenerPause = true;
		this.fileSoure[0].ignoreListenerPause = true;
		this.fileSoure[1].ignoreListenerPause = true;
		AudioSource[] array = this.musicChannelSource;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ignoreListenerPause = true;
		}
		this.LoadCorruptionData(this.defaultCorruptionObject);
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00023068 File Offset: 0x00021268
	private void Update()
	{
		if (this.fileQueue.Count > 0)
		{
			if (!this.otherSourceQueued)
			{
				this.fileSoure[this.currentSource].loop = false;
				this.currentSource = 1 - this.currentSource;
				this.fileSoure[this.currentSource].clip = this.fileQueue[0].clip;
				this.fileSoure[this.currentSource].outputAudioMixerGroup = this.fileQueue[0].mixer;
				this.fileQueue.RemoveAt(0);
				this.fileSoure[this.currentSource].PlayScheduled((double)this.fileSoure[1 - this.currentSource].clip.samples / (double)this.fileSoure[1 - this.currentSource].clip.frequency - (double)this.fileSoure[1 - this.currentSource].timeSamples / (double)this.fileSoure[1 - this.currentSource].clip.frequency + AudioSettings.dspTime);
				this.otherSourceQueued = true;
				return;
			}
			if (!this.fileSoure[1 - this.currentSource].isPlaying)
			{
				this.otherSourceQueued = false;
				return;
			}
		}
		else
		{
			if (this.fileLoop)
			{
				this.fileSoure[this.currentSource].loop = true;
				this.fileLoop = false;
			}
			if (!this.fileSoure[this.currentSource].isPlaying)
			{
				this.filePlaying = false;
			}
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x000231E8 File Offset: 0x000213E8
	public void PlayMidi(string song, bool loop)
	{
		this.SetSpeed(1f);
		this.midiPlayer.transpose = 0;
		this.midiPlayer.MPTK_MidiName = song;
		this.midiPlayer.MPTK_RePlay();
		this.midiPlaying = true;
		this.midiPaused = false;
		for (int i = 0; i < this.midiPlayer.Channels.Length; i++)
		{
			this.midiPlayer.MPTK_ChannelEnableSet(i, true);
		}
		this.SetLoop(loop);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00023260 File Offset: 0x00021460
	public void QueueFile(LoopingSoundObject file, bool loop)
	{
		foreach (AudioClip clip in file.clips)
		{
			this.fileQueue.Add(new MusicFileData(clip, file.mixer));
		}
		this.SetFileLoop(loop);
		if (!this.filePlaying)
		{
			this.StartFileQueue();
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x000232B4 File Offset: 0x000214B4
	public void StartFileQueue()
	{
		if (this.fileQueue.Count > 0)
		{
			this.currentSource = 1 - this.currentSource;
			this.fileSoure[this.currentSource].clip = this.fileQueue[0].clip;
			this.fileSoure[this.currentSource].outputAudioMixerGroup = this.fileQueue[0].mixer;
			this.fileQueue.RemoveAt(0);
			this.fileSoure[this.currentSource].Play();
			this.filePlaying = true;
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00023348 File Offset: 0x00021548
	public void StopFile()
	{
		this.fileSoure[0].Stop();
		this.fileSoure[1].Stop();
		this.fileQueue.Clear();
		this.filePlaying = false;
		this.fileLoop = false;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002337D File Offset: 0x0002157D
	public void SetFileLoop(bool val)
	{
		this.fileLoop = val;
		if (this.fileQueue.Count == 0)
		{
			this.fileSoure[this.currentSource].loop = this.fileLoop;
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x000233AB File Offset: 0x000215AB
	public void PlaySoundEffect(SoundObject sound)
	{
		this.soundSource.PlaySingle(sound);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x000233BC File Offset: 0x000215BC
	public void PauseMidi(bool pause)
	{
		if (this.midiPlaying)
		{
			if (pause && !this.midiPaused)
			{
				this.midiPlayer.MPTK_Pause(-1f);
				this.midiPaused = true;
				return;
			}
			if (!pause && this.midiPaused)
			{
				this.midiPlayer.MPTK_UnPause();
				this.midiPaused = false;
			}
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00023411 File Offset: 0x00021611
	public void StopMidi()
	{
		this.midiPlayer.MPTK_Stop();
		this.midiPlayer.MPTK_Loop = false;
		this.StopModulation();
		this.midiPlaying = false;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00023437 File Offset: 0x00021637
	public void SetLoop(bool val)
	{
		this.midiPlayer.MPTK_Loop = val;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00023445 File Offset: 0x00021645
	public void SetSpeed(float speed)
	{
		this.midiPlayer.MPTK_Speed = speed;
		this.speed = speed;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002345A File Offset: 0x0002165A
	public void ModulateSpeed(float rate, float increase, float increaseIncrease, float limit, bool waitForLoop)
	{
		if (this.speedModulatorRunning)
		{
			base.StopCoroutine(this.speedModulator);
		}
		this.speedModulator = this.SpeedModulator(rate, increase, increaseIncrease, limit, waitForLoop);
		base.StartCoroutine(this.speedModulator);
		this.speedModulatorRunning = true;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00023497 File Offset: 0x00021697
	public void StopModulation()
	{
		if (this.speedModulatorRunning)
		{
			base.StopCoroutine(this.speedModulator);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x000234AD File Offset: 0x000216AD
	private IEnumerator SpeedModulator(float rate, float increase, float increaseIncrease, float limit, bool waitForLoop)
	{
		this.speed = this.midiPlayer.MPTK_Speed;
		while (this.speed < limit)
		{
			float time = rate;
			while (time > 0f)
			{
				if (!this.midiPlayer.MPTK_IsPaused)
				{
					time -= Time.deltaTime;
				}
				yield return null;
			}
			this.speed += increase;
			this.SetSpeed(this.speed);
			increase += increaseIncrease;
		}
		this.speed = limit;
		this.SetSpeed(limit);
		yield break;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000234D9 File Offset: 0x000216D9
	public void StartExponentialModulator()
	{
		if (this.speedModulatorRunning)
		{
			base.StopCoroutine(this.speedModulator);
		}
		this.speedModulator = this.ExponentialModulator();
		base.StartCoroutine(this.speedModulator);
		this.speedModulatorRunning = true;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002350F File Offset: 0x0002170F
	private IEnumerator ExponentialModulator()
	{
		float time = 0f;
		while (this.speed < 10f)
		{
			this.SetSpeed(this.speed);
			this.speed = 0.01f * Mathf.Pow(1.109f, 2f * time) + 0.4f;
			time += Time.deltaTime;
			yield return null;
		}
		this.speed = 10f;
		this.SetSpeed(this.speed);
		yield break;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00023520 File Offset: 0x00021720
	public void MidiEvent(List<MPTKEvent> midiEvents)
	{
		for (int i = 0; i < midiEvents.Count; i++)
		{
			if (MusicManager.OnMidiEvent != null)
			{
				MusicManager.OnMidiEvent(midiEvents[i]);
			}
			if (this.transitionWaiting && midiEvents[i].Channel == 14 && midiEvents[i].Command == MPTKCommand.NoteOff)
			{
				this.transPlayer.MPTK_UnPause();
				this.transPlayer.MPTK_TickCurrent = this.transPlayer.MPTK_TickLast;
				this.midiPlayer.MPTK_Stop();
				this.transitionWaiting = false;
				this.transitionStarting = true;
				this.transitionPlaying = true;
			}
			else if (this.transitionStarting && midiEvents[i].Command == MPTKCommand.NoteOn)
			{
				this.transitionStarting = false;
				this.transPlayer.MPTK_Loop = false;
			}
			if (this.midiCorrupted)
			{
				MPTKEvent mptkevent = midiEvents[i];
				MidiChannelCorruptionData midiChannelCorruptionData = this.currentCorruptionData.corruptions[mptkevent.Channel];
				if (midiChannelCorruptionData.useSoundSwap)
				{
					if (AudioManager.audioMemory[midiChannelCorruptionData.soundSwapVal] != null)
					{
						this.musicChannelSource[mptkevent.Channel].clip = AudioManager.audioMemory[midiChannelCorruptionData.soundSwapVal].soundClip;
						this.musicChannelSource[mptkevent.Channel].pitch = (float)mptkevent.Value / 60f;
						this.musicChannelSource[mptkevent.Channel].Play();
					}
				}
				else
				{
					foreach (MidiModData midiModData in midiChannelCorruptionData.mods)
					{
						mptkevent.MTPK_ModifySynthParameter(midiModData.modulationType, midiModData.val, midiModData.change);
					}
					this.midiSynth.MPTK_PlayEvent(mptkevent);
				}
			}
		}
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x000236FC File Offset: 0x000218FC
	public void MidiLoop()
	{
		if (MusicManager.OnMidiLoop != null)
		{
			MusicManager.OnMidiLoop(this.midiPlayer.MPTK_MidiName);
		}
		if (!this.midiPlayer.MPTK_Loop)
		{
			this.midiPlaying = false;
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002372E File Offset: 0x0002192E
	public void TransitionFinished()
	{
		this.midiPlayer.MPTK_RePlay();
		this.transPlayer.MPTK_RePlay();
		this.transPlayer.MPTK_Pause(-1f);
		this.transPlayer.MPTK_Loop = true;
		this.transitionPlaying = false;
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00023769 File Offset: 0x00021969
	public void SetCorruption(bool val)
	{
		this.midiCorrupted = val;
		if (val)
		{
			this.midiPlayer.MPTK_DirectSendToPlayer = false;
			return;
		}
		this.midiPlayer.MPTK_DirectSendToPlayer = true;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002378E File Offset: 0x0002198E
	public void LoadCorruptionData(MidiCorruptionObject corruptionObject)
	{
		this.currentCorruptionData.corruptions = corruptionObject.corruptions;
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x000237A1 File Offset: 0x000219A1
	public MidiFilePlayer MidiPlayer
	{
		get
		{
			return this.midiPlayer;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x000237A9 File Offset: 0x000219A9
	public bool MidiPlaying
	{
		get
		{
			return this.midiPlaying;
		}
	}

	// Token: 0x0400072A RID: 1834
	private IEnumerator speedModulator;

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private MidiFilePlayer midiPlayer;

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private MidiFilePlayer transPlayer;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private MidiStreamPlayer midiSynth;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private AudioSource midiSource;

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private AudioSource synthSource;

	// Token: 0x04000730 RID: 1840
	[SerializeField]
	private AudioSource[] fileSoure = new AudioSource[0];

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private AudioSource[] musicChannelSource = new AudioSource[16];

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private AudioManager soundSource;

	// Token: 0x04000733 RID: 1843
	public MidiCorruptionObject defaultCorruptionObject;

	// Token: 0x04000734 RID: 1844
	private MidiCorruptionData currentCorruptionData = new MidiCorruptionData();

	// Token: 0x04000735 RID: 1845
	private int currentSource;

	// Token: 0x04000736 RID: 1846
	private bool otherSourceQueued;

	// Token: 0x04000737 RID: 1847
	private bool midiCorrupted;

	// Token: 0x04000738 RID: 1848
	private List<MusicFileData> fileQueue = new List<MusicFileData>();

	// Token: 0x04000739 RID: 1849
	public MPTKEvent testEvent;

	// Token: 0x0400073A RID: 1850
	private float speed;

	// Token: 0x0400073B RID: 1851
	private bool speedModulatorRunning;

	// Token: 0x0400073C RID: 1852
	private bool transitionReady;

	// Token: 0x0400073D RID: 1853
	private bool transitionWaiting;

	// Token: 0x0400073E RID: 1854
	private bool transitionStarting;

	// Token: 0x0400073F RID: 1855
	private bool transitionPlaying;

	// Token: 0x04000740 RID: 1856
	private bool fileLoop;

	// Token: 0x04000741 RID: 1857
	private bool filePlaying;

	// Token: 0x04000742 RID: 1858
	private bool midiPlaying;

	// Token: 0x04000743 RID: 1859
	private bool midiPaused;

	// Token: 0x0200036A RID: 874
	// (Invoke) Token: 0x06001BFB RID: 7163
	public delegate void MidiEventFunction(MPTKEvent midiEvent);

	// Token: 0x0200036B RID: 875
	// (Invoke) Token: 0x06001BFF RID: 7167
	public delegate void MidiLoopFunction(string midiName);
}
