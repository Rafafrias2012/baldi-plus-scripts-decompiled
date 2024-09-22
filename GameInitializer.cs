using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000FE RID: 254
public class GameInitializer : MonoBehaviour
{
	// Token: 0x060005F2 RID: 1522 RVA: 0x0001DDB9 File Offset: 0x0001BFB9
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
	public virtual void Initialize()
	{
		this.sceneObject = Singleton<CoreGameManager>.Instance.sceneObject;
		Time.timeScale = 0f;
		Shader.SetGlobalTexture("_Skybox", this.sceneObject.skybox);
		Shader.SetGlobalColor("_SkyboxColor", this.sceneObject.skyboxColor);
		EnvironmentController ec = Object.Instantiate<EnvironmentController>(this.ecPre);
		LevelBuilder levelBuilder = null;
		if (this.sceneObject.levelObject != null)
		{
			levelBuilder = Object.Instantiate<LevelBuilder>(this.generatorPre);
			levelBuilder.ld = this.sceneObject.levelObject;
		}
		else if (this.sceneObject.levelAsset != null)
		{
			levelBuilder = Object.Instantiate<LevelBuilder>(this.loaderPre);
			levelBuilder.levelAsset = this.sceneObject.levelAsset;
			levelBuilder.extraAsset = this.sceneObject.extraAsset;
		}
		else if (this.sceneObject.levelContainer != null)
		{
			levelBuilder = Object.Instantiate<LevelBuilder>(this.loaderPre);
			levelBuilder.levelContainer = this.sceneObject.levelContainer;
			levelBuilder.extraAsset = this.sceneObject.extraAsset;
		}
		BaseGameManager baseGameManager = Object.Instantiate<BaseGameManager>(this.sceneObject.manager);
		baseGameManager.levelObject = this.sceneObject.levelObject;
		baseGameManager.Ec = ec;
		baseGameManager.CurrentLevel = this.sceneObject.levelNo;
		if (Singleton<CoreGameManager>.Instance.sceneObject == Singleton<CoreGameManager>.Instance.levelMapHasBeenPurchasedFor)
		{
			baseGameManager.CompleteMapOnReady();
		}
		levelBuilder.Ec = ec;
		levelBuilder.seedOffset = this.sceneObject.levelNo;
		levelBuilder.StartGenerate();
		this.waitForGenerator = this.WaitForGenerator(levelBuilder, baseGameManager);
		base.StartCoroutine(this.waitForGenerator);
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001DF70 File Offset: 0x0001C170
	protected IEnumerator WaitForGenerator(LevelBuilder lb, BaseGameManager gm)
	{
		while (lb.levelInProgress && !lb.levelCreated)
		{
			yield return null;
		}
		RenderSettings.skybox.SetColor("_Tint", Color.gray);
		gm.Initialize();
		yield break;
	}

	// Token: 0x04000623 RID: 1571
	private SceneObject sceneObject;

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	private EnvironmentController ecPre;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	private LevelBuilder generatorPre;

	// Token: 0x04000626 RID: 1574
	[SerializeField]
	private LevelBuilder loaderPre;

	// Token: 0x04000627 RID: 1575
	private IEnumerator waitForGenerator;
}
