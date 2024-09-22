using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000196 RID: 406
public class MirrorMode : MonoBehaviour
{
	// Token: 0x0600093C RID: 2364 RVA: 0x000312B8 File Offset: 0x0002F4B8
	public void Initialize()
	{
		this.active = true;
		this.cameraToMirror[0] = Singleton<CoreGameManager>.Instance.GetCamera(0).camCom;
		this.cameraToMirror[1] = Singleton<CoreGameManager>.Instance.GetCamera(0).billboardCam;
		foreach (Camera camera in this.cameraToMirror)
		{
			Matrix4x4 matrix4x = camera.projectionMatrix;
			matrix4x *= Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));
			camera.projectionMatrix = matrix4x;
		}
		RenderPipelineManager.beginCameraRendering += this.ReverseCulling;
		RenderPipelineManager.endCameraRendering += this.ReturnCulling;
		Singleton<SubtitleManager>.Instance.Reverse();
		Singleton<CoreGameManager>.Instance.GetCamera(0).ReverseAudio();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).Reverse();
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003138D File Offset: 0x0002F58D
	private void OnDisable()
	{
		if (this.active)
		{
			this.active = false;
			RenderPipelineManager.beginCameraRendering -= this.ReverseCulling;
			RenderPipelineManager.endCameraRendering -= this.ReturnCulling;
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000313C0 File Offset: 0x0002F5C0
	public void ReverseCulling(ScriptableRenderContext context, Camera camera)
	{
		if (camera == this.cameraToMirror[0] || camera == this.cameraToMirror[1])
		{
			GL.invertCulling = true;
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x000313E8 File Offset: 0x0002F5E8
	public void ReturnCulling(ScriptableRenderContext context, Camera camera)
	{
		if (camera == this.cameraToMirror[0] || camera == this.cameraToMirror[1])
		{
			GL.invertCulling = false;
		}
	}

	// Token: 0x04000A24 RID: 2596
	private Camera[] cameraToMirror = new Camera[2];

	// Token: 0x04000A25 RID: 2597
	private bool active;
}
