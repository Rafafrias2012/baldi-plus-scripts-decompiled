using System;
using UnityEngine.Events;

// Token: 0x02000004 RID: 4
[Serializable]
public class ConsoleLineData
{
	// Token: 0x04000022 RID: 34
	public string text;

	// Token: 0x04000023 RID: 35
	public string[] randomText;

	// Token: 0x04000024 RID: 36
	public float characterSpeed;

	// Token: 0x04000025 RID: 37
	public float lineSpeed;

	// Token: 0x04000026 RID: 38
	public ConsoleTextType textType;

	// Token: 0x04000027 RID: 39
	public string color = "white";

	// Token: 0x04000028 RID: 40
	public int acceptJump;

	// Token: 0x04000029 RID: 41
	public int denyJump;

	// Token: 0x0400002A RID: 42
	public bool prompt;

	// Token: 0x0400002B RID: 43
	public bool newScreen;

	// Token: 0x0400002C RID: 44
	public SoundObject[] characterSound;

	// Token: 0x0400002D RID: 45
	public UnityEvent OnActivate;

	// Token: 0x0400002E RID: 46
	public UnityEvent OnFinished;

	// Token: 0x0400002F RID: 47
	public UnityEvent OnCharacter;
}
