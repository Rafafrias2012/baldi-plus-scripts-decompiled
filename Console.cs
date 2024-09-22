using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class Console : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Awake()
	{
		this.mixerGroup = this.audioSource.outputAudioMixerGroup;
		this.currentLineDataId = this.startingLineDataId;
		this.errorMixInData1 = new float[this.errorMixIn1.samples];
		this.errorMixIn1.GetData(this.errorMixInData1, 0);
		this.errorMixInData2 = new float[this.errorMixIn2.samples];
		this.errorMixIn2.GetData(this.errorMixInData2, 0);
		AudioClip clip = AudioClip.Create("ErrorGlitch", 441000, 1, 8000, true, new AudioClip.PCMReaderCallback(this.OnAudioRead));
		this.faceSource.clip = clip;
		this.faceSource.loop = true;
		this.faceSource.pitch = Random.Range(0.01f, 1.5f);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002124 File Offset: 0x00000324
	private void Start()
	{
		this.faceMaterial.SetInt("_ColorGlitching", 1);
		this.faceMaterial.SetInt("_UseForcedGlitchColor", 1);
		this.faceMaterial.SetColor("_ForcedGlitchColor", Color.black);
		this.faceMaterial.SetFloat("_ColorGlitchPercent", 1f);
		this.faceMaterial.SetFloat("_NoisePercent", 0.5f);
		this.currentText = "";
		this.LoadLineData();
		this.faceSource.Play();
		Singleton<MusicManager>.Instance.QueueFile(this.ambience, true);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021C0 File Offset: 0x000003C0
	private void Update()
	{
		this.faceMaterial.SetFloat("_NoiseSeed", Random.Range(0f, 4096f));
		if (this.rapidGlitch)
		{
			this.RandomizeGlitchVal();
		}
		if (!this.paused && !this.prompting)
		{
			this.timeToNextCharacter -= Time.unscaledDeltaTime;
			if (this.timeToNextCharacter <= 0f)
			{
				this.LoadCharacter();
				while (this.timeToNextCharacter == 0f)
				{
					this.LoadCharacter();
				}
			}
		}
		if (this.prompting)
		{
			if (Input.anyKeyDown && this.currentPromptAnswer.Length < 128 && Input.inputString.Length > 0 && (char.IsLetter(Input.inputString, 0) || char.IsNumber(Input.inputString, 0)))
			{
				this.currentPromptAnswer += Input.inputString[0].ToString();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.currentPromptAnswer += " ";
			}
			if (this.currentPromptAnswer.Length > 0 && Input.GetKeyDown(KeyCode.Backspace))
			{
				this.currentPromptAnswer = this.currentPromptAnswer.Remove(this.currentPromptAnswer.Length - 1);
			}
			if (this.currentPromptAnswer.Length > 0 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
			{
				this.currentText += this.currentPromptAnswer;
				this.currentPromptAnswer = "";
				this.currentConsoleLineData.prompt = false;
				this.LoadLineData();
			}
			this.tmp.text = this.currentText + this.currentPromptAnswer + "_";
			return;
		}
		this.tmp.text = this.currentText;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002390 File Offset: 0x00000590
	private void LateUpdate()
	{
		this.audioSource.outputAudioMixerGroup = this.mixerGroup;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000023A4 File Offset: 0x000005A4
	public void LoadLineData()
	{
		this.prompting = false;
		if (this.currentLineDataId < this.lineData.Length)
		{
			if (this.currentConsoleLineData != null)
			{
				this.currentConsoleLineData.OnFinished.Invoke();
				if (this.currentConsoleLineData.prompt)
				{
					this.prompting = true;
				}
			}
			if (!this.prompting)
			{
				this.currentConsoleLineData = this.lineData[this.currentLineDataId];
				this.currentLinePosition = 0;
				this.currentLineDataId++;
				this.timeToNextCharacter = this.currentConsoleLineData.lineSpeed;
				if (this.currentConsoleLineData.newScreen)
				{
					this.clearScreen = true;
				}
				this.currentText += "\n\n";
				if (this.currentConsoleLineData.randomText.Length != 0)
				{
					ConsoleLineData consoleLineData = this.currentConsoleLineData;
					consoleLineData.text += this.currentConsoleLineData.randomText[Random.Range(0, this.currentConsoleLineData.randomText.Length)];
					return;
				}
			}
			else
			{
				this.currentText += "\n";
			}
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000024C0 File Offset: 0x000006C0
	public void LoadCharacter()
	{
		if (this.currentLinePosition < this.currentConsoleLineData.text.Length)
		{
			if (this.clearScreen)
			{
				this.currentText = "";
				this.clearScreen = false;
			}
			if (this.currentLinePosition == 0)
			{
				this.currentConsoleLineData.OnActivate.Invoke();
				this.currentText += string.Format("<color={0}>", this.currentConsoleLineData.color);
			}
			this.currentText += this.currentConsoleLineData.text[this.currentLinePosition].ToString();
			this.timeToNextCharacter = this.currentConsoleLineData.characterSpeed;
			if (this.currentConsoleLineData.characterSound.Length != 0)
			{
				this.audioManager.FlushQueue(true);
				this.audioManager.QueueRandomAudio(this.currentConsoleLineData.characterSound);
			}
			this.currentConsoleLineData.OnCharacter.Invoke();
			this.currentLinePosition++;
			if (this.currentLinePosition >= this.currentConsoleLineData.text.Length)
			{
				this.LoadLineData();
			}
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000025EC File Offset: 0x000007EC
	public void Pause(bool pause)
	{
		this.paused = pause;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000025F5 File Offset: 0x000007F5
	public void OpenPopup(bool open)
	{
		this.popupLineData = this.currentConsoleLineData;
		this.popup.gameObject.SetActive(open);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002614 File Offset: 0x00000814
	public void PopupAccepted(bool accepted)
	{
		this.OpenPopup(false);
		if (!accepted)
		{
			this.PulseFace();
			this.JumpToIndex(this.popupLineData.denyJump);
			return;
		}
		this.JumpToIndex(this.popupLineData.acceptJump);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002649 File Offset: 0x00000849
	public void JumpToIndex(int index)
	{
		this.currentLineDataId = index;
		this.LoadLineData();
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002658 File Offset: 0x00000858
	public void PulseFace()
	{
		base.StartCoroutine(this.FacePulser(3f, 0.4f));
		this.RandomizeStaticPitch();
		this.RandomizeGlitchVal();
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002680 File Offset: 0x00000880
	public void FlashFace()
	{
		this.rapidGlitch = true;
		this.faceMaterial.SetFloat("_ColorGlitchPercent", 0.9f);
		this.faceMaterial.SetFloat("_NoisePercent", 0.75f);
		this.faceSource.volume = 0.25f;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000026D0 File Offset: 0x000008D0
	public void IncreaseFace()
	{
		this.faceMaterial.SetFloat("_ColorGlitchPercent", this.faceMaterial.GetFloat("_ColorGlitchPercent") - 0.125f);
		this.faceMaterial.SetFloat("_NoisePercent", this.faceMaterial.GetFloat("_NoisePercent") - 0.05f);
		this.faceSource.volume = 1f - this.faceMaterial.GetFloat("_ColorGlitchPercent");
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000274C File Offset: 0x0000094C
	public void CalmFace()
	{
		this.faceMaterial.SetFloat("_ColorGlitchPercent", this.faceMaterial.GetFloat("_ColorGlitchPercent") + 0.125f);
		this.faceMaterial.SetFloat("_NoisePercent", this.faceMaterial.GetFloat("_NoisePercent") + 0.05f);
		this.faceSource.volume = 1f - this.faceMaterial.GetFloat("_ColorGlitchPercent");
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000027C8 File Offset: 0x000009C8
	public void ShowOtherFace()
	{
		this.faceMaterial.SetFloat("_ColorGlitchPercent", 1f);
		this.spriteRenderer.sprite = this.otherSprite;
		this.rapidGlitch = false;
		this.IncreaseFace();
		this.IncreaseFace();
		this.IncreaseFace();
		this.IncreaseFace();
		this.IncreaseFace();
		this.IncreaseFace();
		this.faceMaterial.SetFloat("_NoisePercent", 0.4f);
		this.RandomizeStaticPitch();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002841 File Offset: 0x00000A41
	public void RandomizeStaticPitch()
	{
		this.faceSource.pitch = Random.Range(0.01f, 1.5f);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000285D File Offset: 0x00000A5D
	public void RandomizeGlitchVal()
	{
		this.faceMaterial.SetFloat("_ColorGlitchVal", Random.Range(0f, 4096f));
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000287E File Offset: 0x00000A7E
	public void KillTheLights()
	{
		this.faceMaterial.SetFloat("_ColorGlitchPercent", 1f);
		this.faceSource.volume = 0f;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000028A5 File Offset: 0x00000AA5
	public void End()
	{
		Application.Quit();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000028B8 File Offset: 0x00000AB8
	private void OnAudioRead(float[] data)
	{
		Random random = new Random();
		int i = 0;
		while (i < data.Length)
		{
			while (i < data.Length)
			{
				if (random.NextDouble() > 0.800000011920929)
				{
					data[i] = 691.1504f * (float)this.audPosition / 44100f % 1f * 0.7f;
				}
				else
				{
					if (this.mixInPosition1 >= this.errorMixInData1.Length)
					{
						this.mixInPosition1 = 0;
					}
					if (this.mixInPosition2 >= this.errorMixInData2.Length)
					{
						this.mixInPosition2 = 0;
					}
					if (random.Next(0, 2) == 0)
					{
						data[i] = (this.errorMixInData1[this.mixInPosition1] + (float)random.NextDouble() * 0.05f - 0.025f) * 3f;
					}
					else
					{
						data[i] = this.errorMixInData2[this.mixInPosition2] + (float)random.NextDouble() * 0.3f - 0.15f;
					}
				}
				this.audPosition++;
				this.mixInPosition1++;
				this.mixInPosition2++;
				i++;
			}
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000029CF File Offset: 0x00000BCF
	private IEnumerator FacePulser(float speed, float intensity)
	{
		float value = 0f;
		while (value < 3.1415927f)
		{
			value += Time.unscaledDeltaTime * speed;
			this.faceMaterial.SetFloat("_ColorGlitchPercent", 1f - Mathf.Sin(value) * intensity);
			this.faceSource.volume = Mathf.Sin(value) * intensity;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000004 RID: 4
	public TMP_Text tmp;

	// Token: 0x04000005 RID: 5
	public AudioManager audioManager;

	// Token: 0x04000006 RID: 6
	public AudioSource audioSource;

	// Token: 0x04000007 RID: 7
	public AudioSource faceSource;

	// Token: 0x04000008 RID: 8
	public AudioClip errorMixIn1;

	// Token: 0x04000009 RID: 9
	public AudioClip errorMixIn2;

	// Token: 0x0400000A RID: 10
	public Image spriteRenderer;

	// Token: 0x0400000B RID: 11
	public Sprite otherSprite;

	// Token: 0x0400000C RID: 12
	public GameObject popup;

	// Token: 0x0400000D RID: 13
	public LoopingSoundObject ambience;

	// Token: 0x0400000E RID: 14
	private AudioMixerGroup mixerGroup;

	// Token: 0x0400000F RID: 15
	public ConsoleLineData[] lineData;

	// Token: 0x04000010 RID: 16
	private ConsoleLineData currentConsoleLineData;

	// Token: 0x04000011 RID: 17
	private ConsoleLineData popupLineData;

	// Token: 0x04000012 RID: 18
	public Material faceMaterial;

	// Token: 0x04000013 RID: 19
	private string currentText;

	// Token: 0x04000014 RID: 20
	private string currentPromptAnswer = "";

	// Token: 0x04000015 RID: 21
	private float[] errorMixInData1 = new float[0];

	// Token: 0x04000016 RID: 22
	private float[] errorMixInData2 = new float[0];

	// Token: 0x04000017 RID: 23
	private float timeToNextCharacter;

	// Token: 0x04000018 RID: 24
	public int startingLineDataId;

	// Token: 0x04000019 RID: 25
	private int currentLineDataId;

	// Token: 0x0400001A RID: 26
	private int currentLinePosition;

	// Token: 0x0400001B RID: 27
	private int audPosition;

	// Token: 0x0400001C RID: 28
	private int mixInPosition1;

	// Token: 0x0400001D RID: 29
	private int mixInPosition2;

	// Token: 0x0400001E RID: 30
	private bool paused;

	// Token: 0x0400001F RID: 31
	private bool rapidGlitch;

	// Token: 0x04000020 RID: 32
	private bool prompting;

	// Token: 0x04000021 RID: 33
	private bool clearScreen;
}
