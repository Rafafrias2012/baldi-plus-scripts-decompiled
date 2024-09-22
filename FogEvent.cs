using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class FogEvent : RandomEvent
{
	// Token: 0x06000269 RID: 617 RVA: 0x0000D718 File Offset: 0x0000B918
	public override void Begin()
	{
		base.Begin();
		base.StartCoroutine(this.FadeOnFog());
		this.audMan.QueueAudio(this.music);
		this.audMan.loop = true;
		this.ec.MaxRaycast = this.maxRaycast;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000D766 File Offset: 0x0000B966
	public override void End()
	{
		base.End();
		base.StartCoroutine(this.FadeOffFog());
		this.audMan.FadeOut(1f);
		this.ec.MaxRaycast = float.PositiveInfinity;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000D79B File Offset: 0x0000B99B
	public override void Pause()
	{
		base.Pause();
		this.audMan.Pause(true);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000D7AF File Offset: 0x0000B9AF
	public override void Unpause()
	{
		base.Unpause();
		this.audMan.Pause(false);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000D7C3 File Offset: 0x0000B9C3
	private IEnumerator FadeOnFog()
	{
		this.ec.AddFog(this.fog);
		this.fog.color = this.fogColor;
		this.fog.startDist = 5f;
		this.fog.maxDist = 100f;
		this.fog.strength = 0f;
		float fogStrength = 0f;
		while (fogStrength < 1f)
		{
			fogStrength += 0.25f * Time.deltaTime;
			this.fog.strength = fogStrength;
			this.ec.UpdateFog();
			yield return null;
		}
		fogStrength = 1f;
		this.fog.strength = fogStrength;
		this.ec.UpdateFog();
		yield break;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000D7D2 File Offset: 0x0000B9D2
	private IEnumerator FadeOffFog()
	{
		float fogStrength = 1f;
		this.fog.strength = fogStrength;
		this.ec.UpdateFog();
		while (fogStrength > 0f)
		{
			fogStrength -= 0.25f * Time.deltaTime;
			this.fog.strength = fogStrength;
			this.ec.UpdateFog();
			yield return null;
		}
		fogStrength = 0f;
		this.fog.strength = fogStrength;
		this.ec.UpdateFog();
		yield break;
	}

	// Token: 0x04000284 RID: 644
	private const float fogDensity = 0.05f;

	// Token: 0x04000285 RID: 645
	private const float fogSpeed = 0.25f;

	// Token: 0x04000286 RID: 646
	[SerializeField]
	private Color fogColor = Color.gray;

	// Token: 0x04000287 RID: 647
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000288 RID: 648
	[SerializeField]
	private SoundObject music;

	// Token: 0x04000289 RID: 649
	public Fog fog;

	// Token: 0x0400028A RID: 650
	[SerializeField]
	private float maxRaycast = 25f;
}
