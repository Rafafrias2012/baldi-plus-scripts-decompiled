using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x020000FC RID: 252
public class GameCamera : MonoBehaviour
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x0001D7FC File Offset: 0x0001B9FC
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		if (this.targetTra == null)
		{
			this.UpdateTargets(base.transform, this.targets.Length - 1);
		}
		this.initialMainMask = this.camCom.cullingMask;
		this.initialBillboardMask = this.billboardCam.cullingMask;
		this.initialCanvasMask = this.canvasCam.cullingMask;
		if (Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			this.camCom.targetTexture = this.authenticTexture;
			this.camCom.enabled = false;
			this.comCamData.cameraStack.Remove(this.canvasCam);
			this.canvasCam.targetTexture = this.authenticHudTexture;
			this.canvasCamData.renderType = CameraRenderType.Base;
			this.canvasCam.clearFlags = CameraClearFlags.Color;
			this.canvasCam.backgroundColor = Color.clear;
			this.authenticMode = true;
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001D8FC File Offset: 0x0001BAFC
	public void UpdateTargets(Transform trans, int val)
	{
		this.targets[val] = trans;
		for (int i = 0; i < this.targets.Length; i++)
		{
			if (this.targets[i] != null)
			{
				this.targetTra = this.targets[i];
				return;
			}
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001D944 File Offset: 0x0001BB44
	private void Start()
	{
		bool crtFilter = Singleton<PlayerFileManager>.Instance.crtFilter;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001D954 File Offset: 0x0001BB54
	private void LateUpdate()
	{
		int num = 0;
		if (this.controllable & Singleton<InputManager>.Instance.GetDigitalInput("LookBack", false))
		{
			num = 180;
		}
		if (this.targetTra == null)
		{
			this.UpdateTargets(base.transform, this.targets.Length - 1);
		}
		base.transform.position = this.targetTra.position + this.offestPos;
		this._offsetRot = Vector3.zero;
		for (int i = 0; i < this.cameraModifiers.Count; i++)
		{
			this._offsetRot += this.cameraModifiers[i].rotOffset;
		}
		if (this.matchTargetRotation)
		{
			base.transform.rotation = this.targetTra.rotation * Quaternion.Euler(0f + this._offsetRot.x, (float)num + this._offsetRot.y, 0f + this._offsetRot.z);
		}
		else
		{
			this._lookAtPos.x = this.targetTra.transform.position.x;
			this._lookAtPos.y = base.transform.position.y;
			this._lookAtPos.z = this.targetTra.transform.position.z;
			base.transform.LookAt(this._lookAtPos);
		}
		if (GameCamera.dijkstraMap != null && GameCamera.dijkstraMap.EnvironmentController != null && (GameCamera.dijkstraMap.UpdateIsNeeded() || GameCamera.recalculateDijkstraMap))
		{
			GameCamera.dijkstraMap.Calculate();
			GameCamera.recalculateDijkstraMap = false;
		}
		if (this.mapCam != null)
		{
			if (!Singleton<GlobalCam>.Instance.TransitionActive && Singleton<CoreGameManager>.Instance.sceneObject.usesMap)
			{
				if ((this.controllable && Singleton<InputManager>.Instance.GetDigitalInput("Map", false)) || (Singleton<CoreGameManager>.Instance.MapOpen && !Singleton<GlobalCam>.Instance.TransitionActive))
				{
					this.mapCam.enabled = true;
				}
				else
				{
					this.mapCam.enabled = false;
				}
			}
			else
			{
				this.mapCam.enabled = false;
			}
		}
		Shader.SetGlobalFloat("_CamRotZ", this.camCom.transform.rotation.eulerAngles.z);
		this.billboardCam.ResetCullingMatrix();
		this.billboardCam.fieldOfView = 120f;
		this._billboardCullingMatrix = this.billboardCam.cullingMatrix;
		this.billboardCam.fieldOfView = 60f;
		this.billboardCam.cullingMatrix = this._billboardCullingMatrix;
		if (this.authenticMode)
		{
			this.timeToNextAuthenticFrame -= Time.unscaledDeltaTime;
			if (this.timeToNextAuthenticFrame <= 0f)
			{
				this.timeToNextAuthenticFrame = 0.06666667f;
				this.camCom.Render();
			}
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0001DC42 File Offset: 0x0001BE42
	public static void RecalculateDijkstraMap()
	{
		GameCamera.recalculateDijkstraMap = true;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001DC4C File Offset: 0x0001BE4C
	public void StopRendering(bool val)
	{
		if (val)
		{
			this.camCom.cullingMask = 0;
			this.billboardCam.cullingMask = 0;
			this.canvasCam.cullingMask = 0;
			return;
		}
		this.camCom.cullingMask = this.initialMainMask;
		this.billboardCam.cullingMask = this.initialBillboardMask;
		this.canvasCam.cullingMask = this.initialCanvasMask;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0001DCC4 File Offset: 0x0001BEC4
	public void ReverseAudio()
	{
		this.audioReversed = !this.audioReversed;
		if (!this.audioReversed)
		{
			this.listenerTra.transform.localEulerAngles = Vector3.zero;
			return;
		}
		this.listenerTra.transform.localEulerAngles = Vector3.forward * 180f;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001DD1D File Offset: 0x0001BF1D
	public void SetControllable(bool value)
	{
		if (!value)
		{
			this.controlLocks++;
			this.controllable = value;
			return;
		}
		this.controlLocks--;
		if (this.controlLocks <= 0)
		{
			this.controlLocks = 0;
			this.controllable = value;
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0001DD5D File Offset: 0x0001BF5D
	public void ResetControllable()
	{
		this.controllable = true;
		this.controlLocks = 0;
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001DD6D File Offset: 0x0001BF6D
	public bool Controllable
	{
		get
		{
			return this.controllable;
		}
	}

	// Token: 0x04000602 RID: 1538
	public Camera mapCamPre;

	// Token: 0x04000603 RID: 1539
	public Camera camCom;

	// Token: 0x04000604 RID: 1540
	public Camera mapCam;

	// Token: 0x04000605 RID: 1541
	public Camera canvasCam;

	// Token: 0x04000606 RID: 1542
	public Camera overlayCam;

	// Token: 0x04000607 RID: 1543
	public Camera billboardCam;

	// Token: 0x04000608 RID: 1544
	[SerializeField]
	private RenderTexture authenticTexture;

	// Token: 0x04000609 RID: 1545
	[SerializeField]
	private RenderTexture authenticHudTexture;

	// Token: 0x0400060A RID: 1546
	[SerializeField]
	private UniversalAdditionalCameraData canvasCamData;

	// Token: 0x0400060B RID: 1547
	[SerializeField]
	private UniversalAdditionalCameraData comCamData;

	// Token: 0x0400060C RID: 1548
	public Transform[] targets = new Transform[32];

	// Token: 0x0400060D RID: 1549
	[SerializeField]
	private Transform listenerTra;

	// Token: 0x0400060E RID: 1550
	private Transform targetTra;

	// Token: 0x0400060F RID: 1551
	private Matrix4x4 _billboardCullingMatrix;

	// Token: 0x04000610 RID: 1552
	public Vector3 offestPos;

	// Token: 0x04000611 RID: 1553
	private Vector3 _offsetRot;

	// Token: 0x04000612 RID: 1554
	private Vector3 _eulerAngles;

	// Token: 0x04000613 RID: 1555
	private Vector3 _lookAtPos;

	// Token: 0x04000614 RID: 1556
	private LayerMask initialMainMask;

	// Token: 0x04000615 RID: 1557
	private LayerMask initialBillboardMask;

	// Token: 0x04000616 RID: 1558
	private LayerMask initialCanvasMask;

	// Token: 0x04000617 RID: 1559
	public static DijkstraMap dijkstraMap;

	// Token: 0x04000618 RID: 1560
	private float timeToNextAuthenticFrame;

	// Token: 0x04000619 RID: 1561
	public int camNum;

	// Token: 0x0400061A RID: 1562
	private int controlLocks;

	// Token: 0x0400061B RID: 1563
	public bool matchTargetRotation = true;

	// Token: 0x0400061C RID: 1564
	private bool controllable = true;

	// Token: 0x0400061D RID: 1565
	private bool audioReversed;

	// Token: 0x0400061E RID: 1566
	private bool authenticMode;

	// Token: 0x0400061F RID: 1567
	private static bool recalculateDijkstraMap;

	// Token: 0x04000620 RID: 1568
	public List<CameraModifier> cameraModifiers = new List<CameraModifier>();
}
