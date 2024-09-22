using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class Looker : MonoBehaviour
{
	// Token: 0x06000437 RID: 1079 RVA: 0x0001652C File Offset: 0x0001472C
	private void Update()
	{
		for (int i = 0; i < this.npc.players.Count; i++)
		{
			this.playerSighted[i] = false;
		}
		this.visible = false;
		if (this.hasFov)
		{
			Debug.DrawRay(base.transform.position, Quaternion.AngleAxis(this.fieldOfView, Vector3.up) * base.transform.forward * 100f, Color.yellow);
			Debug.DrawRay(base.transform.position, Quaternion.AngleAxis(-this.fieldOfView, Vector3.up) * base.transform.forward * 100f, Color.yellow);
		}
		if (this.npc.ec.Active)
		{
			for (int j = 0; j < this.npc.players.Count; j++)
			{
				this.playerInSight[j] = false;
				this.Raycast(this.npc.players[j].transform, Mathf.Min((base.transform.position - this.npc.players[j].transform.position).magnitude, Mathf.Min(this.distance, this.npc.ec.MaxRaycast)), Singleton<CoreGameManager>.Instance.GetPlayer(j), this.layerMask, out this.playerSighted[j]);
				if (this.playerSighted[j])
				{
					this._angle = Vector3.Angle(-this._rayDir, Singleton<CoreGameManager>.Instance.GetCamera(j).transform.forward);
					float aspectRatio = (float)Singleton<PlayerFileManager>.Instance.resolutionX / (float)Singleton<PlayerFileManager>.Instance.resolutionY;
					float num = Camera.VerticalToHorizontalFieldOfView(Singleton<GlobalCam>.Instance.Cam.fieldOfView, aspectRatio);
					if (this._angle <= num / 2f + this.visibilityBuffer)
					{
						this.visible = true;
					}
					if (this.hasFov && Vector3.Angle(base.transform.forward, this.npc.players[j].transform.position - base.transform.position) > this.fieldOfView)
					{
						this.playerSighted[j] = false;
					}
					this.playerInSight[j] = this.playerSighted[j];
				}
			}
		}
		for (int k = 0; k < this.npc.players.Count; k++)
		{
			if (this.playerWasSighted[k])
			{
				if (!this.playerSighted[k])
				{
					this.playerWasSighted[k] = false;
					this.playerSighted[k] = false;
					this.npc.PlayerLost(this.npc.players[k]);
					this.npc.behaviorStateMachine.CurrentState.PlayerLost(this.npc.players[k]);
				}
			}
			else if (this.playerSighted[k])
			{
				this.playerWasSighted[k] = true;
				this.npc.PlayerSighted(this.npc.players[k]);
				this.npc.behaviorStateMachine.CurrentState.PlayerSighted(this.npc.players[k]);
			}
			if (this.playerInSight[k])
			{
				this.npc.PlayerInSight(this.npc.players[k]);
				this.npc.behaviorStateMachine.CurrentState.PlayerInSight(this.npc.players[k]);
			}
		}
		if (!this.wasVisible)
		{
			if (this.visible)
			{
				this.wasVisible = true;
				this.npc.Sighted();
			}
		}
		else if (!this.visible)
		{
			this.wasVisible = false;
			this.npc.Unsighted();
		}
		if (this.visible)
		{
			this.npc.InPlayerSight(this.npc.players[0]);
		}
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00016941 File Offset: 0x00014B41
	public void Raycast(Transform target, float rayDistance, out bool targetSighted)
	{
		this.Raycast(target, rayDistance, null, this.layerMask, out targetSighted);
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00016953 File Offset: 0x00014B53
	public void Raycast(Transform target, float rayDistance, LayerMask customMask, out bool targetSighted)
	{
		this.Raycast(target, rayDistance, null, customMask, out targetSighted);
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00016964 File Offset: 0x00014B64
	public void Raycast(Transform target, float rayDistance, PlayerManager player, LayerMask mask, out bool targetSighted)
	{
		targetSighted = false;
		this._castFailed = false;
		if ((base.transform.position - target.position).magnitude > rayDistance)
		{
			this._castFailed = true;
			return;
		}
		if (mask != (mask | 1 << target.gameObject.layer))
		{
			this._castFailed = true;
			return;
		}
		if (player != null && player.Invisible)
		{
			this._castFailed = true;
			return;
		}
		this._rayDir = target.position - base.transform.position;
		this.ray.origin = base.transform.position;
		this.ray.direction = this._rayDir;
		this.hitCount = Physics.RaycastNonAlloc(this.ray, this.hits, Mathf.Min(rayDistance, this.npc.ec.MaxRaycast), mask, QueryTriggerInteraction.Ignore);
		Debug.DrawLine(base.transform.position, base.transform.position + this._rayDir.normalized * rayDistance, Color.red);
		this._hitTransforms.Clear();
		for (int i = 0; i < this.hitCount; i++)
		{
			this._hitTransforms.Add(this.hits[i].transform);
			if (!this.hits[i].transform.CompareTag("Player") && !this.hits[i].transform.CompareTag("NPC") && this.hits[i].transform != target && !this.ignoredTransforms.Contains(this.hits[i].transform))
			{
				this._castFailed = true;
				break;
			}
		}
		if (this.hitCount == 32 && !this._castFailed)
		{
			Debug.LogWarning("Looker hit count reached the max of 32 and no blocking objects were detected! The buffer size may need to be increased.");
		}
		if (!this._castFailed)
		{
			targetSighted = true;
		}
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00016B78 File Offset: 0x00014D78
	public void Blink()
	{
		for (int i = 0; i < this.npc.players.Count; i++)
		{
			this.playerWasSighted[i] = false;
		}
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00016BA9 File Offset: 0x00014DA9
	public void IgnoreTransform(Transform toAdd)
	{
		this.ignoredTransforms.Add(toAdd);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00016BB8 File Offset: 0x00014DB8
	public bool PlayerInSight()
	{
		for (int i = 0; i < this.npc.players.Count; i++)
		{
			if (this.playerWasSighted[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00016BED File Offset: 0x00014DED
	public bool PlayerInSight(PlayerManager player)
	{
		return this.playerWasSighted[player.playerNumber];
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600043F RID: 1087 RVA: 0x00016BFC File Offset: 0x00014DFC
	public bool IsVisible
	{
		get
		{
			return this.visible;
		}
	}

	// Token: 0x040004A7 RID: 1191
	[SerializeField]
	private NPC npc;

	// Token: 0x040004A8 RID: 1192
	private Ray ray;

	// Token: 0x040004A9 RID: 1193
	private RaycastHit[] hits = new RaycastHit[32];

	// Token: 0x040004AA RID: 1194
	public List<Transform> _hitTransforms = new List<Transform>();

	// Token: 0x040004AB RID: 1195
	private List<Transform> ignoredTransforms = new List<Transform>();

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private LayerMask layerMask;

	// Token: 0x040004AD RID: 1197
	private Vector3 _rayDir;

	// Token: 0x040004AE RID: 1198
	public float distance = 10000f;

	// Token: 0x040004AF RID: 1199
	[SerializeField]
	private float visibilityBuffer = -0.5f;

	// Token: 0x040004B0 RID: 1200
	[SerializeField]
	private float fieldOfView = 180f;

	// Token: 0x040004B1 RID: 1201
	private float _angle;

	// Token: 0x040004B2 RID: 1202
	private int hitCount;

	// Token: 0x040004B3 RID: 1203
	private bool[] playerSighted = new bool[4];

	// Token: 0x040004B4 RID: 1204
	private bool[] playerWasSighted = new bool[4];

	// Token: 0x040004B5 RID: 1205
	private bool[] playerInSight = new bool[4];

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private bool hasFov;

	// Token: 0x040004B7 RID: 1207
	private bool visible;

	// Token: 0x040004B8 RID: 1208
	private bool wasVisible;

	// Token: 0x040004B9 RID: 1209
	private bool _castFailed;
}
