using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class AudioTest : MonoBehaviour
{
	// Token: 0x060004E4 RID: 1252 RVA: 0x000196C0 File Offset: 0x000178C0
	public void TestSingle()
	{
		this.audMan.PlaySingle(this.test1);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x000196D4 File Offset: 0x000178D4
	public void TestQueue()
	{
		this.audMan.QueueAudio(this.test_0);
		this.audMan.QueueAudio(this.test_1);
		this.audMan.QueueAudio(this.test_2);
		this.audMan.QueueAudio(this.test_3);
		this.audMan.QueueAudio(this.test_4);
		this.audMan.QueueAudio(this.test_5);
		this.audMan.QueueAudio(this.test_6);
		this.audMan.QueueAudio(this.test_7);
		this.audMan.QueueAudio(this.test_8);
		this.audMan.QueueAudio(this.test_9);
	}

	// Token: 0x0400052A RID: 1322
	public AudioManager audMan;

	// Token: 0x0400052B RID: 1323
	public SoundObject test1;

	// Token: 0x0400052C RID: 1324
	public SoundObject test_0;

	// Token: 0x0400052D RID: 1325
	public SoundObject test_1;

	// Token: 0x0400052E RID: 1326
	public SoundObject test_2;

	// Token: 0x0400052F RID: 1327
	public SoundObject test_3;

	// Token: 0x04000530 RID: 1328
	public SoundObject test_4;

	// Token: 0x04000531 RID: 1329
	public SoundObject test_5;

	// Token: 0x04000532 RID: 1330
	public SoundObject test_6;

	// Token: 0x04000533 RID: 1331
	public SoundObject test_7;

	// Token: 0x04000534 RID: 1332
	public SoundObject test_8;

	// Token: 0x04000535 RID: 1333
	public SoundObject test_9;
}
