using System;
using TMPro;
using UnityEngine;

// Token: 0x020000DA RID: 218
public class SaveViewer : MonoBehaviour
{
	// Token: 0x060004FD RID: 1277 RVA: 0x00019BAC File Offset: 0x00017DAC
	private void Update()
	{
		if (Singleton<PlayerFileManager>.Instance != null)
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.currentType++;
				this.currentThing = 0;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.currentType--;
				this.currentThing = 0;
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.currentThing++;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.currentThing--;
			}
			switch (this.currentType)
			{
			case 0:
			{
				this.type.text = "Characters";
				TMP_Text tmp_Text = this.thing;
				Character character = (Character)this.currentThing;
				tmp_Text.text = character.ToString();
				if (Singleton<PlayerFileManager>.Instance.foundChars[this.currentThing])
				{
					this.value.text = "Yes";
					return;
				}
				this.value.text = "No";
				return;
			}
			case 1:
			{
				this.type.text = "Events";
				TMP_Text tmp_Text2 = this.thing;
				RandomEventType randomEventType = (RandomEventType)this.currentThing;
				tmp_Text2.text = randomEventType.ToString();
				if (Singleton<PlayerFileManager>.Instance.foundEvnts[this.currentThing])
				{
					this.value.text = "Yes";
					return;
				}
				this.value.text = "No";
				return;
			}
			case 2:
			{
				this.type.text = "Items";
				TMP_Text tmp_Text3 = this.thing;
				Items items = (Items)this.currentThing;
				tmp_Text3.text = items.ToString();
				if (Singleton<PlayerFileManager>.Instance.foundItems[this.currentThing])
				{
					this.value.text = "Yes";
					return;
				}
				this.value.text = "No";
				return;
			}
			case 3:
			{
				this.type.text = "Obstacles";
				TMP_Text tmp_Text4 = this.thing;
				Obstacle obstacle = (Obstacle)this.currentThing;
				tmp_Text4.text = obstacle.ToString();
				if (Singleton<PlayerFileManager>.Instance.foundObstcls[this.currentThing])
				{
					this.value.text = "Yes";
					return;
				}
				this.value.text = "No";
				return;
			}
			case 4:
			{
				this.type.text = "Field Trips";
				TMP_Text tmp_Text5 = this.thing;
				FieldTrips fieldTrips = (FieldTrips)this.currentThing;
				tmp_Text5.text = fieldTrips.ToString();
				if (Singleton<PlayerFileManager>.Instance.foundTrips[this.currentThing])
				{
					this.value.text = "Yes";
					return;
				}
				this.value.text = "No";
				break;
			}
			default:
				return;
			}
		}
	}

	// Token: 0x04000543 RID: 1347
	public TMP_Text type;

	// Token: 0x04000544 RID: 1348
	public TMP_Text thing;

	// Token: 0x04000545 RID: 1349
	public TMP_Text value;

	// Token: 0x04000546 RID: 1350
	public int currentType;

	// Token: 0x04000547 RID: 1351
	public int currentThing;
}
