using System;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class SpriteRotator : MonoBehaviour
{
	// Token: 0x0600075A RID: 1882 RVA: 0x00025D48 File Offset: 0x00023F48
	private void Start()
	{
		this.angleRange = (float)(360 / this.sprites.Length);
		this.cam = Camera.main.transform;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00025D70 File Offset: 0x00023F70
	private void Update()
	{
		float num = Mathf.Atan2(this.cam.position.z - base.transform.position.z, this.cam.position.x - base.transform.position.x) * 57.29578f;
		num += base.transform.eulerAngles.y;
		if (num < 0f)
		{
			num += 360f;
		}
		else if (num >= 360f)
		{
			num -= 360f;
		}
		int num2 = Mathf.RoundToInt(num / this.angleRange);
		while (num2 < 0 || num2 >= this.sprites.Length)
		{
			num2 += (int)((float)(-1 * this.sprites.Length) * Mathf.Sign((float)num2));
		}
		this.spriteRenderer.sprite = this.sprites[num2];
	}

	// Token: 0x0400080F RID: 2063
	private Transform cam;

	// Token: 0x04000810 RID: 2064
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private Sprite[] sprites = new Sprite[0];

	// Token: 0x04000812 RID: 2066
	private float angleRange;
}
