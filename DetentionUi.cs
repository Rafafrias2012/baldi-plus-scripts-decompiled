using System;
using TMPro;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class DetentionUi : MonoBehaviour
{
	// Token: 0x06000B8A RID: 2954 RVA: 0x0003C7A4 File Offset: 0x0003A9A4
	private void Update()
	{
		this.time -= Time.deltaTime * this.ec.EnvironmentTimeScale;
		if (this.time <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
		if (this.roundedTime != Mathf.CeilToInt(this.time))
		{
			this.roundedTime = Mathf.CeilToInt(this.time);
			this.timer.text = this.roundedTime.ToString();
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0003C821 File Offset: 0x0003AA21
	public void Initialize(Camera cam, float time, EnvironmentController ec)
	{
		this.canvas.worldCamera = cam;
		this.time = time;
		this.ec = ec;
	}

	// Token: 0x04000DDB RID: 3547
	private EnvironmentController ec;

	// Token: 0x04000DDC RID: 3548
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000DDD RID: 3549
	[SerializeField]
	private TMP_Text timer;

	// Token: 0x04000DDE RID: 3550
	private float time;

	// Token: 0x04000DDF RID: 3551
	private int roundedTime;
}
