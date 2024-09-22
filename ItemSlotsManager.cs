using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020E RID: 526
public class ItemSlotsManager : MonoBehaviour
{
	// Token: 0x06000BAD RID: 2989 RVA: 0x0003D168 File Offset: 0x0003B368
	private void Awake()
	{
		for (int i = 0; i < this.itemSlider.Length; i++)
		{
			this.itemSlider[i] = new ItemSlider();
			this.itemSlider[i].image = this.itemSliderImage[i];
			this.itemSlider[i].rect = this.itemSliderImage[i].rectTransform;
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0003D1C4 File Offset: 0x0003B3C4
	public void SetSize(int size)
	{
		int num = 0;
		while (num < size && num < this.itemCover.Length)
		{
			if (num == 0)
			{
				this.itemCover[num].sprite = this.itemCoverLeftSprite;
			}
			else if (num == size - 1 || num == this.itemCover.Length - 1)
			{
				this.itemCover[num].sprite = this.itemCoverRightSprite;
			}
			else
			{
				this.itemCover[num].sprite = this.itemCoverCenterSprite;
			}
			num++;
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0003D23C File Offset: 0x0003B43C
	public void CollectItem(int slot, ItemObject item)
	{
		this.itemIcon[slot].enabled = false;
		this.itemSlider[this.currentSlider].image.sprite = item.itemSpriteSmall;
		base.StartCoroutine(this.SlideItem(this.itemSlider[this.currentSlider], -1, slot));
		this.currentSlider++;
		if (this.currentSlider >= this.itemSlider.Length)
		{
			this.currentSlider = 0;
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0003D2B8 File Offset: 0x0003B4B8
	public void LoseItem(int slot, ItemObject item)
	{
		this.itemSlider[this.currentSlider].image.sprite = item.itemSpriteSmall;
		base.StartCoroutine(this.SlideItem(this.itemSlider[this.currentSlider], slot, -1));
		this.currentSlider++;
		if (this.currentSlider >= this.itemSlider.Length)
		{
			this.currentSlider = 0;
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0003D323 File Offset: 0x0003B523
	private IEnumerator SlideItem(ItemSlider itemSlider, int start, int end)
	{
		itemSlider.rect.gameObject.SetActive(true);
		itemSlider.position = (float)((start + 1) * 40);
		float goal = (float)((end + 1) * 40);
		Vector2 position = default(Vector2);
		while (!Mathf.Approximately(itemSlider.position, goal))
		{
			float num = this.slideSpeed * Time.deltaTime;
			itemSlider.position = Mathf.Max(itemSlider.position - num, Mathf.Min(itemSlider.position + num, goal));
			position.x = Mathf.Round(itemSlider.position);
			itemSlider.rect.anchoredPosition = position;
			yield return null;
		}
		itemSlider.rect.gameObject.SetActive(false);
		if (end >= 0)
		{
			this.itemIcon[end].enabled = true;
		}
		yield break;
	}

	// Token: 0x04000E0B RID: 3595
	[SerializeField]
	private HudManager hud;

	// Token: 0x04000E0C RID: 3596
	public RectTransform rectTransform;

	// Token: 0x04000E0D RID: 3597
	private ItemSlider[] itemSlider = new ItemSlider[8];

	// Token: 0x04000E0E RID: 3598
	[SerializeField]
	private Image[] itemCover = new Image[9];

	// Token: 0x04000E0F RID: 3599
	[SerializeField]
	private Image[] itemIcon = new Image[9];

	// Token: 0x04000E10 RID: 3600
	[SerializeField]
	private Image[] itemSliderImage = new Image[8];

	// Token: 0x04000E11 RID: 3601
	[SerializeField]
	private Sprite itemCoverLeftSprite;

	// Token: 0x04000E12 RID: 3602
	[SerializeField]
	private Sprite itemCoverCenterSprite;

	// Token: 0x04000E13 RID: 3603
	[SerializeField]
	private Sprite itemCoverRightSprite;

	// Token: 0x04000E14 RID: 3604
	[SerializeField]
	private float slideSpeed = 40f;

	// Token: 0x04000E15 RID: 3605
	private int currentSlider;
}
