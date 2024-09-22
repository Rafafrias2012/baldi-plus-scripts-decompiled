using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class LanternMode : MonoBehaviour
{
	// Token: 0x060008C2 RID: 2242 RVA: 0x0002CD90 File Offset: 0x0002AF90
	public void Initialize(EnvironmentController ec)
	{
		ec.lightingOverride = true;
		this.ec = ec;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0002CDA0 File Offset: 0x0002AFA0
	public void AddSource(Transform trans, float strength, Color color)
	{
		LanternSource lanternSource = new LanternSource();
		lanternSource.transform = trans;
		lanternSource.strength = strength;
		lanternSource.color = color;
		this.sources.Add(lanternSource);
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0002CDD4 File Offset: 0x0002AFD4
	private void Update()
	{
		if (this.ec != null)
		{
			for (int i = 0; i < this.ec.levelSize.x; i++)
			{
				for (int j = 0; j < this.ec.levelSize.z; j++)
				{
					this._color = Color.black;
					this._position.x = (float)i * 10f + 5f;
					this._position.z = (float)j * 10f + 5f;
					this._position.y = 5f;
					foreach (LanternSource lanternSource in this.sources)
					{
						this._distance = Vector3.Distance(lanternSource.transform.position, this._position) / 10f;
						this._color += lanternSource.color * (1f - Mathf.Clamp(this._distance / lanternSource.strength, 0f, 1f)) * (Color.white - this._color);
					}
					this._updatePos.x = i;
					this._updatePos.z = j;
					Singleton<CoreGameManager>.Instance.UpdateLighting(this._color, this._updatePos);
				}
			}
		}
	}

	// Token: 0x0400094C RID: 2380
	private EnvironmentController ec;

	// Token: 0x0400094D RID: 2381
	private List<LanternSource> sources = new List<LanternSource>();

	// Token: 0x0400094E RID: 2382
	private Color _color;

	// Token: 0x0400094F RID: 2383
	private Vector3 _position;

	// Token: 0x04000950 RID: 2384
	private IntVector2 _updatePos;

	// Token: 0x04000951 RID: 2385
	private float _distance;
}
