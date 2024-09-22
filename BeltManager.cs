using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class BeltManager : MonoBehaviour, IButtonReceiver
{
	// Token: 0x060003D0 RID: 976 RVA: 0x00013F8C File Offset: 0x0001218C
	private void Awake()
	{
		if (this.belts.Count > 0)
		{
			this.newMaterial = new Material(this.sourceMaterial);
			this.textureSlider.material = this.newMaterial;
			foreach (MeshRenderer meshRenderer in this.belts)
			{
				meshRenderer.sharedMaterial = this.newMaterial;
			}
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00014014 File Offset: 0x00012214
	public void Initialize()
	{
		this.newMaterial = new Material(this.sourceMaterial);
		this.textureSlider.material = this.newMaterial;
		this.SetSpeed(this.speed);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00014044 File Offset: 0x00012244
	private void OnTriggerEnter(Collider other)
	{
		Entity component = other.GetComponent<Entity>();
		if (other.isTrigger && component != null && other.name != "CloudyCopter(Clone)" && (!other.CompareTag("NPC") || !other.GetComponent<NPC>().IgnoreBelts || this.affectAll))
		{
			ActivityModifier externalActivity = component.ExternalActivity;
			this.currentActMods.Add(externalActivity);
			externalActivity.moveMods.Add(this.moveMod);
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000140C4 File Offset: 0x000122C4
	private void OnTriggerExit(Collider other)
	{
		Entity component = other.GetComponent<Entity>();
		if (other.isTrigger && component != null && other.name != "CloudyCopter(Clone)" && (!other.CompareTag("NPC") || !other.GetComponent<NPC>().IgnoreBelts || this.affectAll))
		{
			ActivityModifier externalActivity = component.ExternalActivity;
			this.currentActMods.Remove(externalActivity);
			externalActivity.moveMods.Remove(this.moveMod);
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00014144 File Offset: 0x00012344
	public void SetDirection(Direction dir)
	{
		this.dir = dir;
		this.moveMod.movementAddend.x = (float)dir.ToIntVector2().x * this.speed;
		this.moveMod.movementAddend.z = (float)dir.ToIntVector2().z * this.speed;
		foreach (MeshRenderer meshRenderer in this.belts)
		{
			meshRenderer.transform.rotation = dir.ToRotation();
			meshRenderer.transform.eulerAngles += Vector3.right * 90f;
		}
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00014214 File Offset: 0x00012414
	public void SetSpeed(float val)
	{
		this.speed = val;
		this.textureSlider.speed.y = -this.speed / 10f;
		this.SetDirection(this.dir);
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00014246 File Offset: 0x00012446
	public void AddBelt(MeshRenderer belt)
	{
		this.belts.Add(belt);
		belt.sharedMaterial = this.newMaterial;
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014260 File Offset: 0x00012460
	public BoxCollider BoxCollider
	{
		get
		{
			return this.boxCollider;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00014268 File Offset: 0x00012468
	public void ButtonPressed(bool val)
	{
		this.SetDirection(this.dir.GetOpposite());
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001427B File Offset: 0x0001247B
	private void OnDisable()
	{
		while (this.currentActMods.Count > 0)
		{
			this.currentActMods[0].moveMods.Remove(this.moveMod);
			this.currentActMods.RemoveAt(0);
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060003DA RID: 986 RVA: 0x000142B6 File Offset: 0x000124B6
	public TextureSlider TextureSlider
	{
		get
		{
			return this.textureSlider;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060003DB RID: 987 RVA: 0x000142BE File Offset: 0x000124BE
	public float Speed
	{
		get
		{
			return this.speed;
		}
	}

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000407 RID: 1031
	[SerializeField]
	private List<MeshRenderer> belts = new List<MeshRenderer>();

	// Token: 0x04000408 RID: 1032
	[SerializeField]
	private Direction dir;

	// Token: 0x04000409 RID: 1033
	private List<ActivityModifier> currentActMods = new List<ActivityModifier>();

	// Token: 0x0400040A RID: 1034
	[SerializeField]
	private TextureSlider textureSlider;

	// Token: 0x0400040B RID: 1035
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x0400040C RID: 1036
	[SerializeField]
	private Material sourceMaterial;

	// Token: 0x0400040D RID: 1037
	public Material newMaterial;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private float speed = 5f;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private bool affectAll;

	// Token: 0x04000410 RID: 1040
	[SerializeField]
	private bool affectNonGrounded = true;
}
