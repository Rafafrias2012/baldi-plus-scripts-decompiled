using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class LightController
{
	// Token: 0x06000924 RID: 2340 RVA: 0x00030AC8 File Offset: 0x0002ECC8
	public void Initialize(EnvironmentController ec, IntVector2 position)
	{
		this.ec = ec;
		this.position = position;
		this.color = ec.standardDarkLevel;
		this.level = Mathf.Max(new float[]
		{
			this.color.r,
			this.color.g,
			this.color.b
		});
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00030B2C File Offset: 0x0002ED2C
	public void AddSource(Cell source, int distance)
	{
		for (int i = 0; i < this.lightSources.Count; i++)
		{
			if (this.lightSources[i].source == source)
			{
				return;
			}
		}
		this.lightSources.Add(new LightData(source, distance));
		this.ec.QueueLightControllerForUpdate(this);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00030B84 File Offset: 0x0002ED84
	public void RemoveSource(Cell source)
	{
		for (int i = 0; i < this.lightSources.Count; i++)
		{
			if (this.lightSources[i].source == source)
			{
				this.lightSources.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00030BCC File Offset: 0x0002EDCC
	public void RegenerateLightSources()
	{
		this.ec._unmodifiedLightSources.Clear();
		this.ec._unmodifiedLightSources.AddRange(this.lightSources);
		foreach (LightData lightData in this.ec._unmodifiedLightSources)
		{
			this.ec.QueueLightSourceForRegenerate(lightData.source);
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00030C54 File Offset: 0x0002EE54
	public void UpdateLighting()
	{
		this.color = Color.black;
		foreach (LightData lightData in this.lightSources)
		{
			if (this.ec.lightMode == LightMode.Greatest)
			{
				this.color.r = Mathf.Max(this.color.r, lightData.source.CurrentColor.r * (1f - Mathf.Clamp((float)lightData.distance / (float)lightData.source.lightStrength, 0f, 1f)));
				this.color.g = Mathf.Max(this.color.g, lightData.source.CurrentColor.g * (1f - Mathf.Clamp((float)lightData.distance / (float)lightData.source.lightStrength, 0f, 1f)));
				this.color.b = Mathf.Max(this.color.b, lightData.source.CurrentColor.b * (1f - Mathf.Clamp((float)lightData.distance / (float)lightData.source.lightStrength, 0f, 1f)));
			}
			else if (this.ec.lightMode == LightMode.Additive)
			{
				this.color += lightData.source.CurrentColor * (1f - Mathf.Clamp((float)lightData.distance / (float)lightData.source.lightStrength, 0f, 1f));
			}
			else if (this.ec.lightMode == LightMode.Cumulative)
			{
				this.color += lightData.source.CurrentColor * (1f - Mathf.Clamp((float)lightData.distance / (float)lightData.source.lightStrength, 0f, 1f)) * (Color.white - this.color);
			}
		}
		this.color.r = Mathf.Lerp(this.ec.standardDarkLevel.r, 1f, this.color.r);
		this.color.g = Mathf.Lerp(this.ec.standardDarkLevel.g, 1f, this.color.g);
		this.color.b = Mathf.Lerp(this.ec.standardDarkLevel.b, 1f, this.color.b);
		this.level = Mathf.Max(new float[]
		{
			this.color.r,
			this.color.g,
			this.color.b
		});
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x00030F64 File Offset: 0x0002F164
	public Color Color
	{
		get
		{
			return this.color;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x00030F6C File Offset: 0x0002F16C
	public float Level
	{
		get
		{
			return this.level;
		}
	}

	// Token: 0x04000A17 RID: 2583
	private Color color = Color.white;

	// Token: 0x04000A18 RID: 2584
	public List<LightData> lightSources = new List<LightData>();

	// Token: 0x04000A19 RID: 2585
	private EnvironmentController ec;

	// Token: 0x04000A1A RID: 2586
	public IntVector2 position;

	// Token: 0x04000A1B RID: 2587
	private float level;
}
