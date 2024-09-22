using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class PlaceholderWinManager : BaseGameManager
{
	// Token: 0x0600067B RID: 1659 RVA: 0x00020858 File Offset: 0x0001EA58
	public override void Initialize()
	{
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
		{
			Object.Destroy(Singleton<ElevatorScreen>.Instance.gameObject);
			Singleton<CoreGameManager>.Instance.Quit();
			Object.Destroy(base.gameObject);
			return;
		}
		Singleton<CoreGameManager>.Instance.SpawnPlayers(this.ec);
		Singleton<CoreGameManager>.Instance.SaveEnabled = false;
		Singleton<CoreGameManager>.Instance.readyToStart = true;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000208C0 File Offset: 0x0001EAC0
	public override void BeginPlay()
	{
		Time.timeScale = 1f;
		AudioListener.pause = false;
		Singleton<MusicManager>.Instance.PlayMidi("DanceV0_5", false);
		this.dancingBaldi.gameObject.SetActive(true);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).Am.moveMods.Add(this.moveMod);
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Disable(true);
		base.StartCoroutine(this.FreakOut());
		for (int i = 0; i < this.balloonCount; i++)
		{
			Object.Instantiate<Balloon>(this.balloonPre[Random.Range(0, this.balloonPre.Length)], this.ec.transform).Initialize(this.ec.rooms[0]);
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0002098D File Offset: 0x0001EB8D
	private IEnumerator FreakOut()
	{
		float time = Random.Range(25f, 35f);
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		time = 2f;
		float swapTime = 0.5f;
		Shader.SetGlobalInt("_ColorGlitching", 1);
		Shader.SetGlobalFloat("_ColorGlitchVal", Random.Range(0f, 4096f));
		Shader.SetGlobalFloat("_ColorGlitchPercent", 1f);
		Shader.SetGlobalInt("_SpriteColorGlitching", 1);
		Shader.SetGlobalFloat("_SpriteColorGlitchVal", Random.Range(0f, 1f));
		Shader.SetGlobalFloat("_SpriteColorGlitchPercent", 1f);
		this.dancingBaldi.glitching = true;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			swapTime -= Time.deltaTime;
			if (swapTime <= 0f)
			{
				if (!Singleton<PlayerFileManager>.Instance.reduceFlashing)
				{
					Shader.SetGlobalFloat("_ColorGlitchVal", Random.Range(0f, 1f));
					Shader.SetGlobalFloat("_SpriteColorGlitchVal", Random.Range(0f, 1f));
				}
				swapTime = 0.5f;
			}
			this.dancingBaldi.SwapSprites();
			yield return null;
		}
		if (Singleton<PlayerFileManager>.Instance.reduceFlashing)
		{
			Shader.SetGlobalInt("_SpriteColorGlitching", 0);
		}
		this.endingError.SetActive(true);
		this.dancingBaldi.CrashSound();
		while (!Input.anyKeyDown && !Singleton<InputManager>.Instance.GetDigitalInput("MouseSubmit", true) && !Singleton<InputManager>.Instance.GetDigitalInput("Pause", true) && !Singleton<InputManager>.Instance.AnyButton(true))
		{
			yield return null;
		}
		this.endingError.SetActive(false);
		this.blackScreen.SetActive(true);
		this.dancingBaldi.crashAudioSource.Stop();
		this.dancingBaldi.glitching = false;
		Singleton<CoreGameManager>.Instance.Quit();
		yield break;
	}

	// Token: 0x04000699 RID: 1689
	private MovementModifier moveMod = new MovementModifier(default(Vector3), 0f);

	// Token: 0x0400069A RID: 1690
	[SerializeField]
	private BaldiDance dancingBaldi;

	// Token: 0x0400069B RID: 1691
	[SerializeField]
	private GameObject endingError;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private GameObject blackScreen;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private Balloon[] balloonPre = new Balloon[0];

	// Token: 0x0400069E RID: 1694
	public int balloonCount = 4;
}
