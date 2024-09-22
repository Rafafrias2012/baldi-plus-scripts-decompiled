using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000218 RID: 536
public class SubtitleManager : Singleton<SubtitleManager>
{
	// Token: 0x06000C00 RID: 3072 RVA: 0x0003F306 File Offset: 0x0003D506
	private void Update()
	{
		if (AudioListener.volume > 0f)
		{
			this.canvas.enabled = true;
			return;
		}
		this.canvas.enabled = false;
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0003F330 File Offset: 0x0003D530
	public void CreateSub(SoundObject file, AudioManager source, int sourceId, float distance, bool loop, Color color)
	{
		if (this.subsById[sourceId] != null)
		{
			Object.Destroy(this.subsById[sourceId]);
		}
		this.subsById[sourceId] = Object.Instantiate<GameObject>(this.subtitlePrefab, base.gameObject.transform);
		SubtitleController component = this.subsById[sourceId].GetComponent<SubtitleController>();
		component.subMan = this;
		component.sourceAudMan = source;
		component.soundTran = source.audioDevice.transform;
		if (source.positional && Singleton<CoreGameManager>.Instance != null && Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
		{
			component.camTran = Singleton<CoreGameManager>.Instance.GetCamera(0).transform;
			component.hasPosition = true;
		}
		component.soundObject = file;
		component.distance = distance;
		component.radius = this.canvasScaler.referenceResolution.y / 2.5f;
		component.contents = Singleton<LocalizationManager>.Instance.GetLocalizedText(file.soundKey, file.encrypted);
		component.currentKey = file.soundKey;
		component.loop = loop;
		component.color = color;
		component.duration = file.subDuration;
		component.Initialize();
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x0003F45F File Offset: 0x0003D65F
	public void DestroySub(int sourceId)
	{
		if (this.subsById[sourceId] != null)
		{
			Object.Destroy(this.subsById[sourceId]);
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x0003F480 File Offset: 0x0003D680
	public void DestroyAll()
	{
		for (int i = 0; i < this.subsById.Length; i++)
		{
			Object.Destroy(this.subsById[i]);
		}
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x0003F4AD File Offset: 0x0003D6AD
	public void Reverse()
	{
		this.reversed = !this.reversed;
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x0003F4BE File Offset: 0x0003D6BE
	public bool Reversed
	{
		get
		{
			return this.reversed;
		}
	}

	// Token: 0x04000E8B RID: 3723
	public Canvas canvas;

	// Token: 0x04000E8C RID: 3724
	public CanvasScaler canvasScaler;

	// Token: 0x04000E8D RID: 3725
	private GameObject[] subsById = new GameObject[256];

	// Token: 0x04000E8E RID: 3726
	public static int totalIds = 256;

	// Token: 0x04000E8F RID: 3727
	public GameObject subtitlePrefab;

	// Token: 0x04000E90 RID: 3728
	private bool reversed;
}
