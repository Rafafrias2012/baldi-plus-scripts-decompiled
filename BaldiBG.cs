using System;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class BaldiBG : MonoBehaviour
{
	// Token: 0x06000B70 RID: 2928 RVA: 0x0003C23D File Offset: 0x0003A43D
	private void OnEnable()
	{
		this.UpdatePosition();
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0003C245 File Offset: 0x0003A445
	private void Update()
	{
		if (!Singleton<GlobalCam>.Instance.TransitionActive)
		{
			this.UpdatePosition();
		}
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0003C25C File Offset: 0x0003A45C
	private void UpdatePosition()
	{
		this.val += 48f * Time.unscaledDeltaTime * this.speed;
		this.time += Time.unscaledDeltaTime * this.speed;
		this.speed = Mathf.Min(1f, this.speed + Time.unscaledDeltaTime);
		if (this.val >= 192f)
		{
			this.val -= 192f;
		}
		this.height = Mathf.Sin(this.time) * 128f;
		this.pos.x = (float)Mathf.RoundToInt(this.val);
		this.pos.y = (float)Mathf.RoundToInt(this.height);
		this.faceAnchor.localPosition = this.pos;
	}

	// Token: 0x04000DBE RID: 3518
	public RectTransform faceAnchor;

	// Token: 0x04000DBF RID: 3519
	private Vector3 pos;

	// Token: 0x04000DC0 RID: 3520
	private float val;

	// Token: 0x04000DC1 RID: 3521
	private float height;

	// Token: 0x04000DC2 RID: 3522
	private float speed;

	// Token: 0x04000DC3 RID: 3523
	private float time;
}
