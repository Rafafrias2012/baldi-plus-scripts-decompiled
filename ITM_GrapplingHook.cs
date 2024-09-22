using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class ITM_GrapplingHook : Item
{
	// Token: 0x06000325 RID: 805 RVA: 0x000104D0 File Offset: 0x0000E6D0
	public override bool Use(PlayerManager pm)
	{
		this.pm = pm;
		this.ec = pm.ec;
		base.transform.position = pm.transform.position;
		base.transform.rotation = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.rotation;
		this.entity.Initialize(this.ec, base.transform.position);
		this.entity.OnEntityMoveInitialCollision += this.OnEntityMoveCollision;
		pm.Am.moveMods.Add(this.moveMod);
		this.audMan.PlaySingle(this.audLaunch);
		if (this.uses > 0)
		{
			pm.itm.SetItem(this.allVersions[this.uses - 1], pm.itm.selectedItem);
			return false;
		}
		return true;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x000105B8 File Offset: 0x0000E7B8
	private void Update()
	{
		if (!this.locked)
		{
			this.entity.UpdateInternalMovement(base.transform.forward * this.speed * this.ec.EnvironmentTimeScale);
			this.time += Time.deltaTime * this.ec.EnvironmentTimeScale;
			if (this.time > 60f)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}
		else
		{
			this.entity.UpdateInternalMovement(Vector3.zero);
			if ((base.transform.position - this.pm.transform.position).magnitude <= this.stopDistance)
			{
				base.StartCoroutine(this.EndDelay());
			}
			this.moveMod.movementAddend = (base.transform.position - this.pm.transform.position).normalized * this.force;
			if (!this.snapped)
			{
				this.motorAudio.pitch = (this.force - this.initialForce) / 100f + 1f;
			}
			this.force += this.forceIncrease * Time.deltaTime;
			this.pressure = (base.transform.position - this.pm.transform.position).magnitude - (this.initialDistance - this.force);
			if (this.pressure > this.maxPressure && !this.snapped)
			{
				this.snapped = true;
				this.audMan.FlushQueue(true);
				this.audMan.QueueAudio(this.audSnap);
				this.motorAudio.Stop();
				this.lineRenderer.enabled = false;
				this.pm.Am.moveMods.Remove(this.moveMod);
				base.StartCoroutine(this.WaitForAudio());
			}
		}
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000107C0 File Offset: 0x0000E9C0
	private void LateUpdate()
	{
		this.positions[0] = base.transform.position;
		this.positions[1] = this.pm.transform.position - Vector3.up * 1f;
		this.lineRenderer.SetPositions(this.positions);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00010825 File Offset: 0x0000EA25
	private IEnumerator EndDelay()
	{
		float time = 0.25f;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		this.End();
		yield break;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00010834 File Offset: 0x0000EA34
	private void End()
	{
		this.pm.Am.moveMods.Remove(this.moveMod);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00010860 File Offset: 0x0000EA60
	private void OnEntityMoveCollision(RaycastHit hit)
	{
		if (this.layerMask.Contains(hit.collider.gameObject.layer) && !this.locked)
		{
			this.locked = true;
			this.entity.SetFrozen(true);
			this.force = this.initialForce;
			this.initialDistance = (base.transform.position - this.pm.transform.position).magnitude;
			this.audMan.PlaySingle(this.audClang);
			this.motorAudio.Play();
			this.cracks.rotation = Quaternion.LookRotation(hit.normal * -1f, Vector3.up);
			this.cracks.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00010939 File Offset: 0x0000EB39
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001093B File Offset: 0x0000EB3B
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0001093D File Offset: 0x0000EB3D
	private IEnumerator WaitForAudio()
	{
		while (this.audMan.audioDevice.isPlaying)
		{
			yield return null;
		}
		this.End();
		yield break;
	}

	// Token: 0x04000353 RID: 851
	private EnvironmentController ec;

	// Token: 0x04000354 RID: 852
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x04000355 RID: 853
	[SerializeField]
	private Entity entity;

	// Token: 0x04000356 RID: 854
	[SerializeField]
	private MovementModifier moveMod;

	// Token: 0x04000357 RID: 855
	[SerializeField]
	private ItemObject[] allVersions = new ItemObject[5];

	// Token: 0x04000358 RID: 856
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000359 RID: 857
	[SerializeField]
	private AudioSource motorAudio;

	// Token: 0x0400035A RID: 858
	[SerializeField]
	private SoundObject audLaunch;

	// Token: 0x0400035B RID: 859
	[SerializeField]
	private SoundObject audClang;

	// Token: 0x0400035C RID: 860
	[SerializeField]
	private SoundObject audSnap;

	// Token: 0x0400035D RID: 861
	[SerializeField]
	private Transform cracks;

	// Token: 0x0400035E RID: 862
	[SerializeField]
	private LayerMaskObject layerMask;

	// Token: 0x0400035F RID: 863
	private Vector3[] positions = new Vector3[2];

	// Token: 0x04000360 RID: 864
	[SerializeField]
	private float speed = 100f;

	// Token: 0x04000361 RID: 865
	[SerializeField]
	private float maxPressure = 100f;

	// Token: 0x04000362 RID: 866
	[SerializeField]
	private float initialForce = 20f;

	// Token: 0x04000363 RID: 867
	[SerializeField]
	private float forceIncrease = 5f;

	// Token: 0x04000364 RID: 868
	[SerializeField]
	private float stopDistance = 5f;

	// Token: 0x04000365 RID: 869
	public float force;

	// Token: 0x04000366 RID: 870
	public float pressure;

	// Token: 0x04000367 RID: 871
	public float initialDistance;

	// Token: 0x04000368 RID: 872
	public float time;

	// Token: 0x04000369 RID: 873
	public int uses;

	// Token: 0x0400036A RID: 874
	private bool locked;

	// Token: 0x0400036B RID: 875
	private bool snapped;
}
