using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class MysteryDoor : MonoBehaviour
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600029B RID: 667 RVA: 0x0000E6CF File Offset: 0x0000C8CF
	public Door Door
	{
		get
		{
			return this.door;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600029C RID: 668 RVA: 0x0000E6D7 File Offset: 0x0000C8D7
	public MeshRenderer Cover
	{
		get
		{
			return this.cover;
		}
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
	public void HideDoor(bool hide)
	{
		GameObject[] array = this.doorObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!hide);
		}
		this.doorCollider.enabled = !hide;
		this.Cover.gameObject.SetActive(hide);
	}

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private StandardDoor door;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	private MeshRenderer cover;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	private GameObject[] doorObjects = new GameObject[0];

	// Token: 0x040002BB RID: 699
	[SerializeField]
	private BoxCollider doorCollider;
}
