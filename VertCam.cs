using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class VertCam : MonoBehaviour
{
	// Token: 0x06000773 RID: 1907 RVA: 0x000260BE File Offset: 0x000242BE
	private void Awake()
	{
		this.ogGameMatrix = this.gameCam.camCom.projectionMatrix;
		this.ogBillboardMatrix = this.gameCam.billboardCam.projectionMatrix;
		this.matrix = this.ogGameMatrix;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000260F8 File Offset: 0x000242F8
	private void Update()
	{
		if (this.movementEnabled)
		{
			Singleton<InputManager>.Instance.GetAnalogInput(this.cameraInputData, out this._analogOut, out this._deltaOut);
			float num = this._analogOut.y * Time.deltaTime * Singleton<PlayerFileManager>.Instance.controllerCameraSensitivity + this._deltaOut.y * Singleton<PlayerFileManager>.Instance.mouseCameraSensitivity;
			this.matrix[1, 2] = Mathf.Clamp(this.matrix[1, 2] + num * this.sensitivity * Time.timeScale, -this.limit, this.limit);
			this.gameCam.camCom.projectionMatrix = this.matrix;
			this.gameCam.billboardCam.projectionMatrix = this.matrix;
			this.gameCam.overlayCam.projectionMatrix = this.matrix;
			return;
		}
		this.gameCam.camCom.projectionMatrix = this.ogGameMatrix;
		this.gameCam.billboardCam.projectionMatrix = this.ogBillboardMatrix;
		this.gameCam.overlayCam.projectionMatrix = this.matrix;
	}

	// Token: 0x04000827 RID: 2087
	[SerializeField]
	private GameCamera gameCam;

	// Token: 0x04000828 RID: 2088
	private Matrix4x4 matrix;

	// Token: 0x04000829 RID: 2089
	private Matrix4x4 ogGameMatrix;

	// Token: 0x0400082A RID: 2090
	private Matrix4x4 ogBillboardMatrix;

	// Token: 0x0400082B RID: 2091
	public AnalogInputData cameraInputData;

	// Token: 0x0400082C RID: 2092
	private Vector2 _analogOut;

	// Token: 0x0400082D RID: 2093
	private Vector2 _deltaOut;

	// Token: 0x0400082E RID: 2094
	[SerializeField]
	private float sensitivity = 0.01f;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	private float limit = 0.8f;

	// Token: 0x04000830 RID: 2096
	public bool movementEnabled;
}
