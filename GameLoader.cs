using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001D4 RID: 468
public class GameLoader : MonoBehaviour
{
	// Token: 0x06000A8F RID: 2703 RVA: 0x00037E18 File Offset: 0x00036018
	private void Start()
	{
		if (this.initOnStart)
		{
			this.Initialize(2);
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00037E2C File Offset: 0x0003602C
	public void Initialize(int lives)
	{
		Object.Instantiate<CoreGameManager>(this.cgmPre);
		if (this.useSeed)
		{
			Singleton<CoreGameManager>.Instance.SetSeed(this.seed);
		}
		else
		{
			Singleton<CoreGameManager>.Instance.SetRandomSeed();
		}
		Singleton<CoreGameManager>.Instance.SetLives(lives);
		Singleton<CursorManager>.Instance.LockCursor();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00037E7E File Offset: 0x0003607E
	public void AssignElevatorScreen(ElevatorScreen screen)
	{
		screen.OnLoadReady += this.LoadReady;
		this.elevatorAssigned = true;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x00037E9C File Offset: 0x0003609C
	public void LoadSavedGame()
	{
		Object.Instantiate<CoreGameManager>(this.cgmPre);
		SavedGameData savedGameData = Singleton<PlayerFileManager>.Instance.savedGameData;
		Singleton<CoreGameManager>.Instance.SetSeed(savedGameData.seed);
		Singleton<CoreGameManager>.Instance.SetLives(savedGameData.lives);
		Singleton<CoreGameManager>.Instance.AddPoints(savedGameData.ytps, 0, false, false);
		Singleton<CoreGameManager>.Instance.tripPlayed = savedGameData.fieldTripPlayed;
		Singleton<CoreGameManager>.Instance.johnnyHelped = savedGameData.johnnyHelped;
		if (savedGameData.mapAvailable)
		{
			Singleton<CoreGameManager>.Instance.LoadSavedMap(savedGameData.foundMapTiles.ConvertTo2d(savedGameData.mapSizeX, savedGameData.mapSizeZ));
		}
		if (savedGameData.mapPurchased)
		{
			Singleton<CoreGameManager>.Instance.levelMapHasBeenPurchasedFor = this.list.scenes[savedGameData.levelId];
		}
		Singleton<CoreGameManager>.Instance.RestoreSavedItems(savedGameData.items);
		Singleton<CoreGameManager>.Instance.RestoreSavedLockerItems(savedGameData.lockerItems);
		Singleton<PlayerFileManager>.Instance.savedGameData.saveAvailable = false;
		Singleton<PlayerFileManager>.Instance.Save();
		Singleton<CursorManager>.Instance.LockCursor();
		this.SetMode(0);
		this.LoadLevel(this.list.scenes[savedGameData.levelId]);
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00037FC3 File Offset: 0x000361C3
	public void LoadLevel(SceneObject sceneObject)
	{
		Singleton<MusicManager>.Instance.StopMidi();
		Singleton<CoreGameManager>.Instance.sceneObject = sceneObject;
		Singleton<CoreGameManager>.Instance.lastLevelNumber = sceneObject.levelNo;
		if (!this.elevatorAssigned)
		{
			this.LoadReady();
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00037FF8 File Offset: 0x000361F8
	public void LoadReady()
	{
		SceneManager.LoadSceneAsync("Game");
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00038005 File Offset: 0x00036205
	public void SetMode(int gameMode)
	{
		Singleton<CoreGameManager>.Instance.currentMode = (Mode)gameMode;
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00038012 File Offset: 0x00036212
	public void CheckSeed()
	{
		if (this.seedInput.UseSeed)
		{
			this.useSeed = true;
			this.seed = this.seedInput.Seed;
			return;
		}
		this.useSeed = false;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00038041 File Offset: 0x00036241
	public void SetSave(bool val)
	{
		Singleton<CoreGameManager>.Instance.SaveEnabled = val;
	}

	// Token: 0x04000C1E RID: 3102
	public CoreGameManager cgmPre;

	// Token: 0x04000C1F RID: 3103
	public LevelListObject list;

	// Token: 0x04000C20 RID: 3104
	[SerializeField]
	private SeedInput seedInput;

	// Token: 0x04000C21 RID: 3105
	public int seed;

	// Token: 0x04000C22 RID: 3106
	public bool initOnStart;

	// Token: 0x04000C23 RID: 3107
	public bool useSeed;

	// Token: 0x04000C24 RID: 3108
	private bool elevatorAssigned;
}
