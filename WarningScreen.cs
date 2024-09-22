using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000145 RID: 325
public class WarningScreen : MonoBehaviour
{
	// Token: 0x06000778 RID: 1912 RVA: 0x00026400 File Offset: 0x00024600
	private void Start()
	{
		if (Singleton<InputManager>.Instance.SteamInputActive)
		{
			this.textBox.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Men_Warning"), Singleton<InputManager>.Instance.GetInputButtonName("MouseSubmit"));
			return;
		}
		this.textBox.text = string.Format(Singleton<LocalizationManager>.Instance.GetLocalizedText("Men_Warning"), "ANY BUTTON");
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0002646C File Offset: 0x0002466C
	private void Update()
	{
		if (Input.anyKeyDown || Singleton<InputManager>.Instance.GetDigitalInput("MouseSubmit", true) || Singleton<InputManager>.Instance.GetDigitalInput("Pause", true) || Singleton<InputManager>.Instance.AnyButton(true))
		{
			this.Advance();
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x000264AC File Offset: 0x000246AC
	private void Advance()
	{
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		this.blackCover.SetActive(true);
		base.StartCoroutine(this.WaitForTransition());
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x000264D7 File Offset: 0x000246D7
	private IEnumerator WaitForTransition()
	{
		yield return null;
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			this.audSource.volume = time;
			yield return null;
		}
		this.audSource.Stop();
		yield return null;
		SceneManager.LoadScene(this.scene);
		yield break;
	}

	// Token: 0x0400083D RID: 2109
	[SerializeField]
	private GameObject blackCover;

	// Token: 0x0400083E RID: 2110
	public TMP_Text textBox;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	private AudioSource audSource;

	// Token: 0x04000840 RID: 2112
	public string scene;
}
