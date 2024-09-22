using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

// Token: 0x02000101 RID: 257
public class CoreGameManager : Singleton<CoreGameManager>
{
	// Token: 0x0600061D RID: 1565 RVA: 0x0001E858 File Offset: 0x0001CA58
	private void Start()
	{
		if (this.spawnOnStart)
		{
			this.SpawnPlayers(this.environmentToSpawn);
		}
		this.disablePause = true;
		Canvas[] array = this.pauseScreens;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].worldCamera = Singleton<GlobalCam>.Instance.Cam;
		}
		for (int j = 0; j < 4; j++)
		{
			if (this.backupItems.Count <= j)
			{
				this.backupItems.Add(new ItemObject[9]);
			}
		}
		if (this.randomizeSeedOnStart)
		{
			this.SetRandomSeed();
		}
		else if (this.setSeedOnStart)
		{
			this.SetSeed(this.seedToSet);
		}
		Shader.SetGlobalColor("_FogColor", Color.white);
		Shader.SetGlobalFloat("_FogStartDistance", 5f);
		Shader.SetGlobalFloat("_FogMaxDistance", 100f);
		Shader.SetGlobalFloat("_FogStrength", 0f);
		int num = 0;
		while (num < AudioManager.audioMemory.Length && num < this.initLoadedSounds.Length)
		{
			AudioManager.audioMemory[num] = this.initLoadedSounds[num];
			num++;
		}
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0001E960 File Offset: 0x0001CB60
	private void Update()
	{
		if (Singleton<InputManager>.Instance.GetDigitalInput("Pause", true))
		{
			this.Pause(true);
		}
		if (!Singleton<GlobalCam>.Instance.TransitionActive && Singleton<InputManager>.Instance.GetDigitalInput("MapPlus", true) && this.sceneObject.usesMap)
		{
			if (!this.MapOpen && !this.Paused)
			{
				if (this.GetPlayer(0).ec.map != null)
				{
					this.GetCamera(0).mapCam.enabled = false;
					this.Pause(false);
					this.OpenMap();
					return;
				}
			}
			else if (this.GetPlayer(0).ec.map != null)
			{
				this.Pause(false);
				this.CloseMap();
			}
		}
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0001EA23 File Offset: 0x0001CC23
	public void UpdateLightMap()
	{
		if (this.updateLightMap && !CoreGameManager.lightMapPaused)
		{
			this.lightMapTexture.Apply(false, false);
			this.updateLightMap = false;
		}
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0001EA48 File Offset: 0x0001CC48
	public void SpawnPlayers(EnvironmentController ec)
	{
		bool flag = false;
		for (int i = 0; i < this.setPlayers; i++)
		{
			if (this.players[i] == null)
			{
				if (!Singleton<PlayerFileManager>.Instance.authenticMode)
				{
					this.huds[i] = Object.Instantiate<HudManager>(this.hudPref);
				}
				else
				{
					this.huds[i] = this.authenticHud;
				}
				this.huds[i].hudNum = i;
				this.players[i] = Object.Instantiate<PlayerManager>(this.playerPref);
				this.players[i].playerNumber = i;
				this.cameras[i] = Object.Instantiate<GameCamera>(this.cameraPref);
				this.cameras[i].canvasCam.transform.parent = null;
				Object.DontDestroyOnLoad(this.cameras[i].canvasCam);
				this.cameras[i].UpdateTargets(this.players[i].cameraBase, 30);
				this.cameras[i].offestPos = Vector3.zero;
				this.cameras[i].camNum = i;
				GameCamera.dijkstraMap = new DijkstraMap(ec, PathType.Sound, new Transform[]
				{
					this.cameras[i].transform
				});
				if (!Singleton<PlayerFileManager>.Instance.authenticMode)
				{
					this.huds[i].Canvas().worldCamera = this.cameras[i].canvasCam;
				}
				if (i == 0 && !Singleton<PlayerFileManager>.Instance.authenticMode)
				{
					Singleton<GlobalCam>.Instance.ChangeType(CameraRenderType.Overlay);
					this.cameras[i].camCom.GetUniversalAdditionalCameraData().cameraStack.Add(Singleton<GlobalCam>.Instance.RenderTexCam);
				}
			}
			else
			{
				PlayerManager playerManager = this.players[i];
				if (ec.Players[i] == null)
				{
					this.players[i] = Object.Instantiate<PlayerManager>(this.playerPref);
					this.players[i].playerNumber = i;
					this.players[i].itm.items = playerManager.itm.items;
					this.players[i].itm.UpdateSelect();
					this.cameras[i].UpdateTargets(this.players[i].cameraBase, 30);
					this.cameras[i].offestPos = Vector3.zero;
					this.cameras[i].ResetControllable();
					this.cameras[i].matchTargetRotation = true;
					this.cameras[i].cameraModifiers = new List<CameraModifier>(this.players[i].camMods);
					GameCamera.dijkstraMap = new DijkstraMap(ec, PathType.Sound, new Transform[]
					{
						this.cameras[i].transform
					});
					playerManager.gameObject.SetActive(false);
					playerManager.gameObject.SetActive(false);
				}
				else
				{
					this.players[i] = ec.Players[i];
					this.players[i].itm.items = playerManager.itm.items;
					this.cameras[i].UpdateTargets(this.players[i].cameraBase, 30);
					this.cameras[i].offestPos = Vector3.zero;
					this.cameras[i].ResetControllable();
					this.cameras[i].matchTargetRotation = true;
					this.cameras[i].cameraModifiers = new List<CameraModifier>(this.players[i].camMods);
					GameCamera.dijkstraMap = new DijkstraMap(ec, PathType.Sound, new Transform[]
					{
						this.cameras[i].transform
					});
					playerManager.gameObject.SetActive(false);
					this.players[i].gameObject.SetActive(true);
					flag = true;
				}
				if (playerManager.ec == null)
				{
					Object.Destroy(playerManager.gameObject);
				}
			}
			if (!flag)
			{
				this.players[i].Teleport(ec.spawnPoint);
				this.players[i].transform.rotation = ec.spawnRotation;
				this.players[i].plm.height = ec.spawnPoint.y;
				ec.AssignPlayers();
			}
			if (ec.map != null)
			{
				if (ec.map.cams.Count <= i)
				{
					ec.map.targets.Add(this.players[i].transform);
					this.cameras[i].mapCam = Object.Instantiate<Camera>(this.cameras[i].mapCamPre, ec.map.transform);
					ec.map.cams.Add(this.cameras[i].mapCam);
					this.cameras[i].mapCam.transform.rotation = Quaternion.identity;
					this.cameras[i].camCom.GetUniversalAdditionalCameraData().cameraStack.Insert(1, this.cameras[i].mapCam);
				}
				else
				{
					this.cameras[i].mapCam = ec.map.cams[i];
				}
			}
		}
		if (this.restoreItemsOnSpawn)
		{
			this.RestorePlayers();
			this.restoreItemsOnSpawn = false;
		}
		Singleton<GlobalCam>.Instance.SetListener(false);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0001EF56 File Offset: 0x0001D156
	public void PlayBegins()
	{
		if (Singleton<PlayerFileManager>.Instance.authenticMode)
		{
			this.authenticScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0001EF75 File Offset: 0x0001D175
	public PlayerManager GetPlayer(int num)
	{
		return this.players[num];
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0001EF80 File Offset: 0x0001D180
	public void RestoreSavedItems(int[] itemVals)
	{
		if (this.backupItems.Count < 1)
		{
			this.backupItems.Add(new ItemObject[9]);
		}
		for (int i = 0; i < itemVals.Length; i++)
		{
			this.backupItems[0][i] = Singleton<PlayerFileManager>.Instance.itemObjects[itemVals[i]];
		}
		this.restoreItemsOnSpawn = true;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0001EFE4 File Offset: 0x0001D1E4
	public void RestoreSavedLockerItems(int[] itemVals)
	{
		for (int i = 0; i < this.backupLockerItems.Length; i++)
		{
			this.backupLockerItems[i] = Singleton<PlayerFileManager>.Instance.itemObjects[itemVals[i]];
			this.currentLockerItems[i] = Singleton<PlayerFileManager>.Instance.itemObjects[itemVals[i]];
		}
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0001F038 File Offset: 0x0001D238
	public void BackupPlayers()
	{
		for (int i = 0; i < this.setPlayers; i++)
		{
			for (int j = 0; j < this.players[i].itm.items.Length; j++)
			{
				this.backupItems[i][j] = this.players[i].itm.items[j];
			}
			this.thoughtPointsThisLevel[i] = 0;
		}
		for (int k = 0; k < this.backupLockerItems.Length; k++)
		{
			this.backupLockerItems[k] = this.currentLockerItems[k];
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0001F0C4 File Offset: 0x0001D2C4
	public void RestorePlayers()
	{
		for (int i = 0; i < this.setPlayers; i++)
		{
			for (int j = 0; j < this.players[i].itm.items.Length; j++)
			{
				this.players[i].itm.items[j] = this.backupItems[i][j];
			}
			this.players[i].itm.UpdateItems();
			this.players[i].itm.UpdateSelect();
			this.huds[i].ReInit();
		}
		for (int k = 0; k < this.backupLockerItems.Length; k++)
		{
			this.currentLockerItems[k] = this.backupLockerItems[k];
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0001F177 File Offset: 0x0001D377
	public GameCamera GetCamera(int num)
	{
		return this.cameras[num];
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0001F184 File Offset: 0x0001D384
	public void ResetCameras()
	{
		for (int i = 0; i < this.setPlayers; i++)
		{
			this.cameras[i].UpdateTargets(this.players[i].cameraBase, 30);
			this.cameras[i].offestPos = Vector3.zero;
			this.cameras[i].camCom.farClipPlane = 1000f;
			this.cameras[i].billboardCam.farClipPlane = 1000f;
			this.cameras[i].StopRendering(false);
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0001F20C File Offset: 0x0001D40C
	public HudManager GetHud(int num)
	{
		return this.huds[num];
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0001F218 File Offset: 0x0001D418
	public void Pause(bool openScreen)
	{
		if (!this.disablePause && !Singleton<GlobalCam>.Instance.TransitionActive)
		{
			if (!this.paused)
			{
				this.previousScale = Time.timeScale;
				Time.timeScale = 0f;
				this.paused = true;
				AudioListener.pause = true;
				Singleton<InputManager>.Instance.ActivateActionSet("Interface");
				base.StartCoroutine(this.CameraShutoffDelay());
				if (openScreen)
				{
					if (Singleton<PlayerFileManager>.Instance.authenticMode)
					{
						Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
						this.pauseScreen.SetActive(true);
						if (Singleton<PlayerFileManager>.Instance.authenticMode)
						{
							this.authenticScreen.gameObject.SetActive(false);
						}
					}
					else
					{
						this.pauseScreen.SetActive(true);
						Singleton<GlobalCam>.Instance.FadeIn(UiTransition.Dither, 0.01666667f);
					}
				}
			}
			else
			{
				Time.timeScale = this.previousScale;
				this.paused = false;
				this.GetCamera(0).StopRendering(false);
				AudioListener.pause = false;
				Singleton<InputManager>.Instance.ActivateActionSet("InGame");
				Singleton<InputManager>.Instance.StopFrame();
				if (this.mapOpen)
				{
					this.GetPlayer(0).ec.map.TurnOff();
				}
				if (Singleton<PlayerFileManager>.Instance.authenticMode)
				{
					Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
					this.authenticScreen.gameObject.SetActive(true);
				}
				else
				{
					Singleton<GlobalCam>.Instance.Transition(UiTransition.Dither, 0.01666667f);
				}
				this.pauseScreen.SetActive(false);
				if (this.mapOpen)
				{
					this.CloseMap();
				}
				Singleton<CursorManager>.Instance.LockCursor();
			}
			Singleton<MusicManager>.Instance.PauseMidi(this.paused);
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0001F3CB File Offset: 0x0001D5CB
	private IEnumerator CameraShutoffDelay()
	{
		yield return null;
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		if (this.paused)
		{
			this.GetCamera(0).StopRendering(true);
		}
		yield break;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0001F3DC File Offset: 0x0001D5DC
	public void OpenMap()
	{
		if (!this.disablePause)
		{
			this.mapOpen = true;
			this.GetPlayer(0).ec.map.OpenMap();
			this.GetCamera(0).overlayCam.enabled = false;
			this.GetHud(0).Hide(true);
			base.StartCoroutine(this.MapTransitionDelay(this.GetPlayer(0).ec.map));
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001F44C File Offset: 0x0001D64C
	public void CloseMap()
	{
		if (!this.disablePause)
		{
			this.mapOpen = false;
			this.GetCamera(0).mapCam.clearFlags = CameraClearFlags.Nothing;
			this.GetPlayer(0).ec.map.CloseMap();
			this.GetCamera(0).overlayCam.enabled = true;
			this.GetHud(0).Hide(false);
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0001F4AF File Offset: 0x0001D6AF
	private IEnumerator MapTransitionDelay(Map map)
	{
		yield return null;
		while (Singleton<GlobalCam>.Instance.TransitionActive)
		{
			yield return null;
		}
		if (this.paused)
		{
			this.GetCamera(0).mapCam.clearFlags = CameraClearFlags.Color;
			map.TurnOn();
		}
		yield break;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001F4C8 File Offset: 0x0001D6C8
	public void PrepareForReload()
	{
		Singleton<InputManager>.Instance.ActivateActionSet("Interface");
		this.authenticScreen.gameObject.SetActive(false);
		this.audMan.FlushQueue(true);
		this.huds[0].ReInit();
		for (int i = 0; i < this.setPlayers; i++)
		{
			if (this.cameras[i] != null && this.cameras[i].mapCam != null)
			{
				this.cameras[i].camCom.GetUniversalAdditionalCameraData().cameraStack.Remove(this.cameras[i].mapCam);
			}
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0001F56E File Offset: 0x0001D76E
	public void Quit()
	{
		Singleton<GlobalCam>.Instance.SetListener(true);
		Singleton<SubtitleManager>.Instance.DestroyAll();
		this.ReturnToMenu();
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0001F58C File Offset: 0x0001D78C
	public void ReturnToMenu()
	{
		Singleton<BaseGameManager>.Instance.Ec.ResetEvents();
		Object.Destroy(Singleton<BaseGameManager>.Instance);
		Singleton<InputManager>.Instance.ActivateActionSet("Interface");
		Singleton<GlobalCam>.Instance.ChangeType(CameraRenderType.Base);
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
		AudioListener.pause = false;
		PropagatedAudioManager.paused = true;
		Singleton<MusicManager>.Instance.StopFile();
		Singleton<MusicManager>.Instance.SetCorruption(false);
		this.DestroyPlayers();
		Object.Destroy(base.gameObject);
		if (Singleton<PlayerFileManager>.Instance.vsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		this.ResetShaders();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0001F632 File Offset: 0x0001D832
	public void ResetShaders()
	{
		Shader.SetGlobalInt("_ColorGlitching", 0);
		Shader.SetGlobalInt("_SpriteColorGlitching", 0);
		Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
		Shader.SetGlobalFloat("_TileVertexGlitchIntensity", 0f);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0001F668 File Offset: 0x0001D868
	private void DestroyPlayers()
	{
		for (int i = 0; i < this.setPlayers; i++)
		{
			Object.Destroy(this.players[i].gameObject);
			Object.Destroy(this.cameras[i].camCom.gameObject);
			Object.Destroy(this.cameras[i].canvasCam.gameObject);
			Object.Destroy(this.cameras[i].overlayCam.gameObject);
			Object.Destroy(this.cameras[i].gameObject);
			Object.Destroy(this.huds[i].gameObject);
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0001F704 File Offset: 0x0001D904
	public void EndGame(Transform player, Baldi baldi)
	{
		Time.timeScale = 0f;
		Singleton<MusicManager>.Instance.StopMidi();
		this.disablePause = true;
		this.GetCamera(0).UpdateTargets(baldi.transform, 0);
		this.GetCamera(0).offestPos = (player.position - baldi.transform.position).normalized * 2f + Vector3.up;
		this.GetCamera(0).SetControllable(false);
		this.GetCamera(0).matchTargetRotation = false;
		this.audMan.volumeModifier = 0.6f;
		AudioManager audioManager = this.audMan;
		WeightedSelection<SoundObject>[] loseSounds = baldi.loseSounds;
		audioManager.PlaySingle(WeightedSelection<SoundObject>.RandomSelection(loseSounds));
		base.StartCoroutine(this.EndSequence());
		Singleton<InputManager>.Instance.Rumble(1f, 2f);
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
	public void SaveAndQuit()
	{
		SavedGameData savedGameData = new SavedGameData();
		for (int i = 0; i < savedGameData.items.Length; i++)
		{
			savedGameData.items[i] = 0;
			for (int j = 0; j < Singleton<PlayerFileManager>.Instance.itemObjects.Count; j++)
			{
				if (this.players[0].itm.items[i] == Singleton<PlayerFileManager>.Instance.itemObjects[j])
				{
					savedGameData.items[i] = j;
					break;
				}
			}
		}
		for (int k = 0; k < savedGameData.lockerItems.Length; k++)
		{
			savedGameData.lockerItems[k] = 0;
			for (int l = 0; l < Singleton<PlayerFileManager>.Instance.itemObjects.Count; l++)
			{
				if (Singleton<CoreGameManager>.Instance.currentLockerItems[k] == Singleton<PlayerFileManager>.Instance.itemObjects[l])
				{
					savedGameData.lockerItems[k] = l;
					break;
				}
			}
		}
		savedGameData.levelId = this.sceneObject.levelNo;
		savedGameData.levelId = this.nextLevel.levelNo;
		savedGameData.ytps = this.GetPoints(0);
		savedGameData.lives = this.lives;
		savedGameData.seed = this.seed;
		savedGameData.saveAvailable = true;
		savedGameData.fieldTripPlayed = this.tripPlayed;
		savedGameData.johnnyHelped = this.johnnyHelped;
		this.BackupMap(Singleton<BaseGameManager>.Instance.Ec.map);
		savedGameData.mapPurchased = this.saveMapPurchased;
		savedGameData.mapAvailable = this.saveMapAvailable;
		savedGameData.foundMapTiles = this.foundTilesToRestore.ConvertTo1d(this.savedMapSize.x, this.savedMapSize.z);
		savedGameData.mapSizeX = this.savedMapSize.x;
		savedGameData.mapSizeZ = this.savedMapSize.z;
		Singleton<PlayerFileManager>.Instance.savedGameData = savedGameData;
		Singleton<PlayerFileManager>.Instance.Save();
		this.Quit();
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
	private IEnumerator EndSequence()
	{
		float time = 1f;
		Shader.SetGlobalColor("_SkyboxColor", Color.black);
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			this.GetCamera(0).camCom.farClipPlane = 500f * time;
			this.GetCamera(0).billboardCam.farClipPlane = 500f * time;
			yield return null;
		}
		this.GetCamera(0).camCom.farClipPlane = 1000f;
		this.GetCamera(0).billboardCam.farClipPlane = 1000f;
		this.GetCamera(0).StopRendering(true);
		this.audMan.FlushQueue(true);
		AudioListener.pause = true;
		time = 2f;
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		if (this.lives < 1 && this.extraLives < 1)
		{
			Singleton<GlobalCam>.Instance.SetListener(true);
			this.ReturnToMenu();
		}
		else
		{
			if (this.lives > 0)
			{
				this.lives--;
			}
			else
			{
				this.extraLives--;
			}
			Singleton<BaseGameManager>.Instance.RestartLevel();
		}
		yield break;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0001F9CF File Offset: 0x0001DBCF
	public void Boom()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0001F9DC File Offset: 0x0001DBDC
	public void SetSeed(int value)
	{
		this.seed = value;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0001F9E5 File Offset: 0x0001DBE5
	public void SetRandomSeed()
	{
		this.seed = Random.Range(-2147483647, 2147483391);
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0001F9FC File Offset: 0x0001DBFC
	public void SetLives(int val)
	{
		this.lives = val;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0001FA05 File Offset: 0x0001DC05
	public void BackupMap(Map map)
	{
		if (this.sceneObject.usesMap)
		{
			this.restoreMap = true;
			this.foundTilesToRestore = map.foundTiles;
			this.savedMapSize = map.size;
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0001FA33 File Offset: 0x0001DC33
	public void LoadSavedMap(bool[,] map)
	{
		this.restoreMap = true;
		this.foundTilesToRestore = map;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001FA44 File Offset: 0x0001DC44
	public void RestoreMap(Map map, EnvironmentController ec)
	{
		if (this.restoreMap && this.sceneObject.usesMap)
		{
			for (int i = 0; i < map.size.x; i++)
			{
				for (int j = 0; j < map.size.z; j++)
				{
					if (this.foundTilesToRestore[i, j])
					{
						map.Find(i, j, ec.cells[i, j].ConstBin, ec.cells[i, j].room);
					}
				}
			}
		}
		this.restoreMap = false;
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
	public void AddLives(int val)
	{
		this.extraLives += val;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0001FAE4 File Offset: 0x0001DCE4
	public void AddPoints(int points, int player, bool playAnimation)
	{
		this.AddPoints(points, player, playAnimation, true);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0001FAF0 File Offset: 0x0001DCF0
	public void AddPoints(int points, int player, bool playAnimation, bool includeInLevelTotal)
	{
		if (points > 0)
		{
			points = Mathf.RoundToInt((float)points * this.YtpMultiplier);
		}
		if (playAnimation)
		{
			this.GetHud(player).PointsAnimator.AddScore(points, this.GetPoints(player), player);
		}
		this.thoughtPoints[player] += points;
		if (includeInLevelTotal)
		{
			this.thoughtPointsThisLevel[player] += points;
		}
		if (this.GetHud(player) != null && !playAnimation)
		{
			this.GetHud(player).PointsAnimator.UpdateScore(this.thoughtPoints[player]);
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001FB7F File Offset: 0x0001DD7F
	public void AddMultiplier(float value)
	{
		this.ytpMultipliers.Add(value);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0001FB8D File Offset: 0x0001DD8D
	public void RemoveMultiplier(float value)
	{
		this.ytpMultipliers.Remove(value);
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0001FB9C File Offset: 0x0001DD9C
	public void AwardGradeBonus()
	{
		this.AddPoints(CoreGameManager.gradeBonusVal[this.grade], 0, false, false);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0001FBB3 File Offset: 0x0001DDB3
	public void UpdateLighting(Color color, IntVector2 pos)
	{
		this.lightMapTexture.SetPixel(pos.x, pos.z, color);
		this.updateLightMap = true;
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001FBD4 File Offset: 0x0001DDD4
	public ItemObject NoneItem
	{
		get
		{
			return this.noneItem;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001FBDC File Offset: 0x0001DDDC
	private float YtpMultiplier
	{
		get
		{
			float num = 1f;
			foreach (float num2 in this.ytpMultipliers)
			{
				num += num2;
			}
			return num;
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0001FC34 File Offset: 0x0001DE34
	public int GetPoints(int player)
	{
		return this.thoughtPoints[player];
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0001FC3E File Offset: 0x0001DE3E
	public int GetPointsThisLevel(int player)
	{
		return this.thoughtPointsThisLevel[player];
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001FC48 File Offset: 0x0001DE48
	public int Seed()
	{
		return this.seed;
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001FC50 File Offset: 0x0001DE50
	public bool Paused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001FC58 File Offset: 0x0001DE58
	public bool MapOpen
	{
		get
		{
			return this.mapOpen;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600064C RID: 1612 RVA: 0x0001FC60 File Offset: 0x0001DE60
	public int Lives
	{
		get
		{
			return this.lives;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001FCA0 File Offset: 0x0001DEA0
	// (set) Token: 0x0600064D RID: 1613 RVA: 0x0001FC68 File Offset: 0x0001DE68
	public int GradeVal
	{
		get
		{
			return this.grade;
		}
		set
		{
			this.grade = value;
			if (this.grade < 0)
			{
				this.grade = 0;
				return;
			}
			if (this.grade >= CoreGameManager.grades.Length)
			{
				this.grade = CoreGameManager.grades.Length - 1;
			}
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
	public string Grade
	{
		get
		{
			return CoreGameManager.grades[this.grade];
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001FCB6 File Offset: 0x0001DEB6
	// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001FCBE File Offset: 0x0001DEBE
	public bool SaveEnabled
	{
		get
		{
			return this.saveEnabled;
		}
		set
		{
			this.saveEnabled = value;
		}
	}

	// Token: 0x04000647 RID: 1607
	public bool spawnOnStart;

	// Token: 0x04000648 RID: 1608
	public bool randomizeSeedOnStart;

	// Token: 0x04000649 RID: 1609
	public bool setSeedOnStart;

	// Token: 0x0400064A RID: 1610
	public int seedToSet;

	// Token: 0x0400064B RID: 1611
	public EnvironmentController environmentToSpawn;

	// Token: 0x0400064C RID: 1612
	public SceneObject sceneObject;

	// Token: 0x0400064D RID: 1613
	public SceneObject nextLevel;

	// Token: 0x0400064E RID: 1614
	public SceneObject levelMapHasBeenPurchasedFor;

	// Token: 0x0400064F RID: 1615
	private PlayerManager[] players = new PlayerManager[4];

	// Token: 0x04000650 RID: 1616
	public PlayerManager playerPref;

	// Token: 0x04000651 RID: 1617
	public AuthenticScreenManager authenticScreen;

	// Token: 0x04000652 RID: 1618
	private GameCamera[] cameras = new GameCamera[4];

	// Token: 0x04000653 RID: 1619
	public GameCamera cameraPref;

	// Token: 0x04000654 RID: 1620
	private HudManager[] huds = new HudManager[4];

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	private HudManager hudPref;

	// Token: 0x04000656 RID: 1622
	[SerializeField]
	private HudManager authenticHud;

	// Token: 0x04000657 RID: 1623
	public Canvas[] pauseScreens;

	// Token: 0x04000658 RID: 1624
	public GameObject pauseScreen;

	// Token: 0x04000659 RID: 1625
	public AudioManager audMan;

	// Token: 0x0400065A RID: 1626
	public AudioManager musicMan;

	// Token: 0x0400065B RID: 1627
	public SoundObject[] initLoadedSounds = new SoundObject[32];

	// Token: 0x0400065C RID: 1628
	[HideInInspector]
	public FieldTripObject currentFieldTrip;

	// Token: 0x0400065D RID: 1629
	[SerializeField]
	private ItemObject noneItem;

	// Token: 0x0400065E RID: 1630
	public Texture2D lightMapTexture;

	// Token: 0x0400065F RID: 1631
	private static string[] grades = new string[]
	{
		"A+",
		"A",
		"A-",
		"B+",
		"B",
		"B-",
		"C+",
		"C",
		"C-",
		"D+",
		"D",
		"D-",
		"E+",
		"E",
		"E-",
		"F+",
		"F",
		"F-"
	};

	// Token: 0x04000660 RID: 1632
	public static int[] gradeBonuses = new int[]
	{
		100,
		95,
		90,
		85,
		80,
		75,
		70,
		65,
		60,
		55,
		50,
		40,
		30,
		20,
		10,
		5,
		1,
		0
	};

	// Token: 0x04000661 RID: 1633
	public static int[] gradeBonusVal = new int[]
	{
		100,
		95,
		90,
		85,
		80,
		75,
		70,
		65,
		60,
		55,
		50,
		40,
		30,
		20,
		10,
		5,
		1,
		0
	};

	// Token: 0x04000662 RID: 1634
	private int grade = 7;

	// Token: 0x04000663 RID: 1635
	public Mode currentMode;

	// Token: 0x04000664 RID: 1636
	private float previousScale;

	// Token: 0x04000665 RID: 1637
	private int[] thoughtPoints = new int[4];

	// Token: 0x04000666 RID: 1638
	private int[] thoughtPointsThisLevel = new int[4];

	// Token: 0x04000667 RID: 1639
	public bool disablePause;

	// Token: 0x04000668 RID: 1640
	public bool readyToStart;

	// Token: 0x04000669 RID: 1641
	public bool tripAvailable;

	// Token: 0x0400066A RID: 1642
	public bool tripPlayed;

	// Token: 0x0400066B RID: 1643
	public bool levelGenError;

	// Token: 0x0400066C RID: 1644
	public bool johnnyHelped;

	// Token: 0x0400066D RID: 1645
	public bool saveMapPurchased;

	// Token: 0x0400066E RID: 1646
	public bool saveMapAvailable;

	// Token: 0x0400066F RID: 1647
	public static bool lightMapPaused;

	// Token: 0x04000670 RID: 1648
	private bool paused;

	// Token: 0x04000671 RID: 1649
	private bool restoreMap;

	// Token: 0x04000672 RID: 1650
	private bool mapOpen;

	// Token: 0x04000673 RID: 1651
	private bool updateLightMap;

	// Token: 0x04000674 RID: 1652
	private bool restoreItemsOnSpawn;

	// Token: 0x04000675 RID: 1653
	private bool saveEnabled;

	// Token: 0x04000676 RID: 1654
	private IntVector2 savedMapSize;

	// Token: 0x04000677 RID: 1655
	private bool[,] foundTilesToRestore = new bool[0, 0];

	// Token: 0x04000678 RID: 1656
	private List<float> ytpMultipliers = new List<float>();

	// Token: 0x04000679 RID: 1657
	public int setPlayers = 1;

	// Token: 0x0400067A RID: 1658
	public int lastLevelNumber;

	// Token: 0x0400067B RID: 1659
	private int seed;

	// Token: 0x0400067C RID: 1660
	private int lives;

	// Token: 0x0400067D RID: 1661
	private int extraLives;

	// Token: 0x0400067E RID: 1662
	private List<ItemObject[]> backupItems = new List<ItemObject[]>();

	// Token: 0x0400067F RID: 1663
	public ItemObject[] currentLockerItems = new ItemObject[3];

	// Token: 0x04000680 RID: 1664
	private ItemObject[] backupLockerItems = new ItemObject[3];

	// Token: 0x04000681 RID: 1665
	private List<NPC> backupNpcs = new List<NPC>();
}
