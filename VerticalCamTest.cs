using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class VerticalCamTest : MonoBehaviour
{
	// Token: 0x06000B65 RID: 2917 RVA: 0x0003C026 File Offset: 0x0003A226
	private void Start()
	{
		this.mat = this.cam.projectionMatrix;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0003C03C File Offset: 0x0003A23C
	private void Update()
	{
		if (Input.GetKey(KeyCode.U))
		{
			ref Matrix4x4 ptr = ref this.mat;
			ptr[1, 2] = ptr[1, 2] + Time.deltaTime;
			this.cam.projectionMatrix = this.mat;
		}
		if (Input.GetKey(KeyCode.J))
		{
			ref Matrix4x4 ptr = ref this.mat;
			ptr[1, 2] = ptr[1, 2] - Time.deltaTime;
			this.cam.projectionMatrix = this.mat;
		}
	}

	// Token: 0x04000DB3 RID: 3507
	public Camera cam;

	// Token: 0x04000DB4 RID: 3508
	private Matrix4x4 mat;
}
