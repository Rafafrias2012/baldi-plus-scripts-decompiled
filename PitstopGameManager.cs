using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class PitstopGameManager : BaseGameManager
{
	// Token: 0x0600066F RID: 1647 RVA: 0x00020440 File Offset: 0x0001E640
	protected override void AwakeFunction()
	{
		if (this.fieldTripMode)
		{
			this.tripScreen = Object.Instantiate<Canvas>(this.tripScreenPre);
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0002045B File Offset: 0x0001E65B
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		CoreGameManager instance = Singleton<CoreGameManager>.Instance;
		if (instance == null)
		{
			return;
		}
		PlayerManager player = instance.GetPlayer(0);
		if (player == null)
		{
			return;
		}
		player.plm.AddStamina(100f, true);
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00020488 File Offset: 0x0001E688
	public override void PrepareLevelData(LevelData data)
	{
		base.PrepareLevelData(data);
		if (this.fieldTripMode)
		{
			this.currentFieldTrip = this.forcedTrip;
			data.roomAssetsPlacements.Add(this.currentFieldTrip.tripHub);
			Singleton<CoreGameManager>.Instance.tripAvailable = true;
		}
		else if (Singleton<CoreGameManager>.Instance.nextLevel.levelNo == this.tierOneTripLevel)
		{
			WeightedSelection<FieldTripObject>[] items = this.tierOneTrips;
			this.currentFieldTrip = WeightedSelection<FieldTripObject>.RandomSelection(items);
			data.roomAssetsPlacements.Add(this.currentFieldTrip.tripHub);
			Singleton<CoreGameManager>.Instance.tripAvailable = true;
		}
		Singleton<CoreGameManager>.Instance.currentFieldTrip = this.currentFieldTrip;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00020530 File Offset: 0x0001E730
	public override void Initialize()
	{
		Singleton<CoreGameManager>.Instance.SpawnPlayers(this.ec);
		this.ec.AssignPlayers();
		bool completeMapOnReady = this.completeMapOnReady;
		this.generatorFinished = true;
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
		}
		this.CollectNotebooks(0);
		GC.Collect();
		if (this.ec.elevators.Count > 0)
		{
			for (int j = 0; j < this.ec.elevators.Count; j++)
			{
				if (this.ec.elevators[j].IsSpawn)
				{
					this.waitToExitSpawn = base.WaitToExitSpawn(this.ec.elevators[j].ColliderGroup);
					base.StartCoroutine(this.waitToExitSpawn);
					break;
				}
			}
		}
		Singleton<CoreGameManager>.Instance.ResetCameras();
		Singleton<CoreGameManager>.Instance.ResetShaders();
		Singleton<CoreGameManager>.Instance.readyToStart = true;
		Singleton<CoreGameManager>.Instance.GetHud(0).SetNotebookDisplay(false);
		if (this.beginPlayImmediately)
		{
			this.BeginPlay();
		}
		if (this.spawnNpcsOnInit)
		{
			this.ec.SpawnNPCs();
			this.ec.StartEventTimers();
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0002065A File Offset: 0x0001E85A
	public override void BeginPlay()
	{
		base.BeginPlay();
		Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.Disable(true);
		if (this.fieldTripMode)
		{
			this.CallSpecialManagerFunction(1, base.gameObject);
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002068D File Offset: 0x0001E88D
	public override void CollectNotebooks(int count)
	{
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00020690 File Offset: 0x0001E890
	protected override void ExitedSpawn()
	{
		this.ec.elevators[0].InsideCollider.Enable(true);
		base.StartCoroutine(base.EnterExit(this.ec.elevators[0].InsideCollider));
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x000206DC File Offset: 0x0001E8DC
	public override void LoadNextLevel()
	{
		base.PrepareToLoad();
		if (Singleton<ElevatorScreen>.Instance != null)
		{
			this.elevatorScreen = Singleton<ElevatorScreen>.Instance;
			this.elevatorScreen.Reinit();
			this.elevatorScreen.OnLoadReady += base.LoadNextLevel;
			return;
		}
		this.elevatorScreen = Object.Instantiate<ElevatorScreen>(this.elevatorScreenPre);
		this.elevatorScreen.OnLoadReady += base.LoadNextLevel;
		this.elevatorScreen.Initialize();
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0002075D File Offset: 0x0001E95D
	protected override void LoadSceneObject(SceneObject sceneObject, bool restarting)
	{
		base.LoadSceneObject(Singleton<CoreGameManager>.Instance.nextLevel, restarting);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00020770 File Offset: 0x0001E970
	public override void CallSpecialManagerFunction(int val, GameObject source)
	{
		switch (val)
		{
		case 0:
			if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
			{
				if (Singleton<CoreGameManager>.Instance.nextLevel.levelNo > Singleton<CoreGameManager>.Instance.lastLevelNumber)
				{
					Singleton<CoreGameManager>.Instance.SetLives(this.defaultLives);
				}
				Singleton<CoreGameManager>.Instance.SaveAndQuit();
				return;
			}
			Singleton<CoreGameManager>.Instance.Quit();
			return;
		case 1:
			base.StartCoroutine(this.FieldTripTransition(true));
			return;
		case 2:
			if (!this.fieldTripMode)
			{
				base.StartCoroutine(this.FieldTripTransition(false));
				return;
			}
			Singleton<CoreGameManager>.Instance.Quit();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0002080D File Offset: 0x0001EA0D
	private IEnumerator FieldTripTransition(bool entering)
	{
		Time.timeScale = 0f;
		AudioListener.pause = true;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.SetInteractionState(false);
		Singleton<CoreGameManager>.Instance.disablePause = true;
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
		{
			Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.foundTrips, (int)this.currentFieldTrip.trip);
		}
		if (this.tripScreen == null)
		{
			this.tripScreen = Object.Instantiate<Canvas>(this.tripScreenPre);
		}
		if (!this.fieldTripMode)
		{
			Singleton<GlobalCam>.Instance.FadeIn(UiTransition.Dither, 0.01666667f);
		}
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		yield return null;
		if (entering)
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(0).Teleport(this.currentFieldTrip.spawnPoint);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.rotation = this.currentFieldTrip.spawnDirection.ToRotation();
			Shader.SetGlobalTexture("_Skybox", this.currentFieldTrip.skybox);
		}
		else
		{
			Singleton<CoreGameManager>.Instance.GetPlayer(0).Teleport(this.fieldTripExitSpawnPoint);
			Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.rotation = Direction.East.ToRotation();
			Shader.SetGlobalTexture("_Skybox", Singleton<CoreGameManager>.Instance.sceneObject.skybox);
		}
		float time = 1f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
		Object.Destroy(this.tripScreen.gameObject);
		Time.timeScale = 1f;
		AudioListener.pause = false;
		Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.SetInteractionState(true);
		Singleton<CoreGameManager>.Instance.disablePause = false;
		yield break;
	}

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	protected Canvas tripScreenPre;

	// Token: 0x04000690 RID: 1680
	protected Canvas tripScreen;

	// Token: 0x04000691 RID: 1681
	[SerializeField]
	private WeightedFieldTrip[] tierOneTrips = new WeightedFieldTrip[0];

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private WeightedFieldTrip[] tierTwoTrips = new WeightedFieldTrip[0];

	// Token: 0x04000693 RID: 1683
	[SerializeField]
	private WeightedFieldTrip[] tierThreeTrips = new WeightedFieldTrip[0];

	// Token: 0x04000694 RID: 1684
	[SerializeField]
	private FieldTripObject forcedTrip;

	// Token: 0x04000695 RID: 1685
	private FieldTripObject currentFieldTrip;

	// Token: 0x04000696 RID: 1686
	[SerializeField]
	private Vector3 fieldTripExitSpawnPoint;

	// Token: 0x04000697 RID: 1687
	[SerializeField]
	private int tierOneTripLevel = 3;

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	private bool fieldTripMode;
}
