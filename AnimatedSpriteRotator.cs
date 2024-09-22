using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class AnimatedSpriteRotator : MonoBehaviour
{
	// Token: 0x0600056A RID: 1386 RVA: 0x0001B8B4 File Offset: 0x00019AB4
	private void LateUpdate()
	{
		if (!this.bypassRotation && Singleton<CoreGameManager>.Instance.GetCamera(0) != null)
		{
			if (this.spriteMap[this.currentMapId].Sprite(this.currentSpriteId) != this.targetSprite)
			{
				bool flag = false;
				for (int i = 0; i < this.spriteMap.Length; i++)
				{
					for (int j = 0; j < this.spriteMap[i].SpriteCount; j += this.spriteMap[i].angleCount)
					{
						if (this.spriteMap[i].Sprite(j) == this.targetSprite)
						{
							this.currentMapId = i;
							this.currentSpriteId = j;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			this.angle = Vector3.SignedAngle(base.transform.forward, (Singleton<CoreGameManager>.Instance.GetCamera(0).transform.position.ZeroOutY() - base.transform.position.ZeroOutY()).normalized, -base.transform.up);
			if (this.angle < 0f)
			{
				this.angle = 360f + this.angle;
			}
			this.spriteIdOffset = Mathf.RoundToInt(this.angle / (360f / (float)this.spriteMap[this.currentMapId].angleCount));
			if (this.spriteIdOffset >= this.spriteMap[this.currentMapId].angleCount)
			{
				this.spriteIdOffset = 0;
			}
			this.renderer.sprite = this.spriteMap[this.currentMapId].Sprite(this.currentSpriteId + this.spriteIdOffset);
			return;
		}
		if (this.renderer.sprite != this.targetSprite)
		{
			this.renderer.sprite = this.targetSprite;
		}
	}

	// Token: 0x0400059D RID: 1437
	[SerializeField]
	private SpriteRenderer renderer;

	// Token: 0x0400059E RID: 1438
	public Sprite targetSprite;

	// Token: 0x0400059F RID: 1439
	[SerializeField]
	private SpriteRotationMap[] spriteMap = new SpriteRotationMap[0];

	// Token: 0x040005A0 RID: 1440
	private float angle;

	// Token: 0x040005A1 RID: 1441
	private int currentMapId;

	// Token: 0x040005A2 RID: 1442
	private int currentSpriteId;

	// Token: 0x040005A3 RID: 1443
	private int spriteIdOffset;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private bool bypassRotation;
}
