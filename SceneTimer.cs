using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000135 RID: 309
public class SceneTimer : MonoBehaviour
{
	// Token: 0x0600074A RID: 1866 RVA: 0x00025953 File Offset: 0x00023B53
	private void Start()
	{
		this.time = this.initialTime;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00025964 File Offset: 0x00023B64
	private void Update()
	{
		this.time -= Time.unscaledDeltaTime;
		if (this.time <= 0f)
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				Singleton<PlayerFileManager>.Instance.SetDefaultControls();
				Debug.Log("Control held on start. Resetting controls.");
			}
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				Singleton<PlayerFileManager>.Instance.SetDefaults();
				Singleton<PlayerFileManager>.Instance.UpdateResolution();
				if (Singleton<PlayerFileManager>.Instance.vsync)
				{
					QualitySettings.vSyncCount = 1;
				}
				else
				{
					QualitySettings.vSyncCount = 0;
				}
				Singleton<PlayerFileManager>.Instance.Save();
				Debug.Log("Alt held on start. Resetting settings.");
			}
			SceneManager.LoadScene(this.scene);
		}
	}

	// Token: 0x04000800 RID: 2048
	public string scene;

	// Token: 0x04000801 RID: 2049
	public float initialTime = 5f;

	// Token: 0x04000802 RID: 2050
	private float time;
}
