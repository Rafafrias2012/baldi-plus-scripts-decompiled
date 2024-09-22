using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000FF RID: 255
public class BaseGameManager : Singleton<BaseGameManager>
{
	// Token: 0x060005F6 RID: 1526 RVA: 0x0001DF8E File Offset: 0x0001C18E
	protected void Start()
	{
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0001DF90 File Offset: 0x0001C190
	private void OnEnable()
	{
		Baldi.OnBaldiSlap += this.BaldiSlapped;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001DFA4 File Offset: 0x0001C1A4
	private void OnDisable()
	{
		Baldi.OnBaldiSlap -= this.BaldiSlapped;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001DFB8 File Offset: 0x0001C1B8
	private void Update()
	{
		if (this.ec.Active)
		{
			this.time += Time.deltaTime * this.ec.EnvironmentTimeScale;
		}
		this.VirtualUpdate();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001DFEB File Offset: 0x0001C1EB
	protected virtual void VirtualUpdate()
	{
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001DFED File Offset: 0x0001C1ED
	// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001DFF5 File Offset: 0x0001C1F5
	public int CurrentLevel
	{
		get
		{
			return this.levelNo;
		}
		set
		{
			this.levelNo = value;
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0001DFFE File Offset: 0x0001C1FE
	public virtual void PrepareLevelData(LevelData data)
	{
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0001E000 File Offset: 0x0001C200
	protected void PrepareToLoad()
	{
		base.StopAllCoroutines();
		this.ec.ResetEvents();
		Time.timeScale = 0f;
		Singleton<CoreGameManager>.Instance.readyToStart = false;
		Singleton<CoreGameManager>.Instance.disablePause = true;
		PropagatedAudioManager.paused = true;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001E03C File Offset: 0x0001C23C
	public virtual void LoadNextLevel()
	{
		this.PrepareToLoad();
		Singleton<CoreGameManager>.Instance.PrepareForReload();
		if (Singleton<CoreGameManager>.Instance.sceneObject.levelNo > Singleton<CoreGameManager>.Instance.lastLevelNumber)
		{
			Singleton<CoreGameManager>.Instance.tripPlayed = false;
		}
		if (this.problems > 0)
		{
			Singleton<CoreGameManager>.Instance.GradeVal += -Mathf.RoundToInt(this.gradeValue * ((float)this.correctProblems / (float)this.problems * 2f - 1f));
		}
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Main)
		{
			foreach (NPC npc in this.ec.npcsToSpawn)
			{
				Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.foundChars, (int)npc.Character);
			}
			foreach (Obstacle value in this.ec.obstacles)
			{
				Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.foundObstcls, (int)value);
			}
			Singleton<PlayerFileManager>.Instance.Find(Singleton<PlayerFileManager>.Instance.clearedLevels, this.levelNo);
		}
		Singleton<SubtitleManager>.Instance.DestroyAll();
		this.LoadSceneObject(Singleton<CoreGameManager>.Instance.sceneObject.nextLevel);
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001E1BC File Offset: 0x0001C3BC
	protected void LoadSceneObject(SceneObject sceneObject)
	{
		this.LoadSceneObject(sceneObject, false);
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001E1C6 File Offset: 0x0001C3C6
	protected virtual void LoadSceneObject(SceneObject sceneObject, bool restarting)
	{
		Singleton<CoreGameManager>.Instance.sceneObject = sceneObject;
		SceneManager.LoadSceneAsync("Game");
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001E1E0 File Offset: 0x0001C3E0
	public virtual void RestartLevel()
	{
		this.PrepareToLoad();
		Singleton<CoreGameManager>.Instance.PrepareForReload();
		Singleton<CoreGameManager>.Instance.BackupMap(this.ec.map);
		Singleton<CoreGameManager>.Instance.RestorePlayers();
		this.LoadSceneObject(Singleton<CoreGameManager>.Instance.sceneObject, true);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001E22D File Offset: 0x0001C42D
	public virtual void EndGame(Transform player, Baldi baldi)
	{
		Singleton<CoreGameManager>.Instance.EndGame(player, baldi);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001E23B File Offset: 0x0001C43B
	public virtual void CollectNotebook(Notebook notebook)
	{
		if (notebook.activity.GetType().Name == "NoActivity")
		{
			this.lastActivity = null;
		}
		this.CollectNotebooks(1);
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001E268 File Offset: 0x0001C468
	public virtual void CollectNotebooks(int count)
	{
		this.foundNotebooks += count;
		if (this.foundNotebooks >= this.ec.notebookTotal)
		{
			this.AllNotebooks();
		}
		if (this.ec.GetBaldi() != null)
		{
			this.AngerBaldi((float)count * this.notebookAngerVal);
		}
		else
		{
			this.missedNotebooks += count;
			if (this.NotebookTotal - this.foundNotebooks > 0)
			{
				this.notebookAngerVal = (float)this.NotebookTotal / ((float)this.NotebookTotal - (float)this.foundNotebooks);
			}
			else
			{
				this.notebookAngerVal = (float)this.NotebookTotal;
				this.ec.angerOnSpawn = true;
			}
		}
		Singleton<CoreGameManager>.Instance.GetHud(0).UpdateNotebookText(0, this.foundNotebooks.ToString() + "/" + Mathf.Max(this.ec.notebookTotal, this.foundNotebooks).ToString(), count > 0);
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0001E360 File Offset: 0x0001C560
	public void PleaseBaldi(float time)
	{
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Character == Character.Baldi)
			{
				npc.GetComponent<Baldi>().Praise(time);
			}
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
	public virtual void AngerBaldi(float val)
	{
		foreach (NPC npc in this.ec.Npcs)
		{
			if (npc.Character == Character.Baldi)
			{
				npc.GetComponent<Baldi>().GetAngry(val);
			}
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001E430 File Offset: 0x0001C630
	public void AddNotebookTotal(int count)
	{
		this.ec.notebookTotal += count;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0001E448 File Offset: 0x0001C648
	protected virtual void AllNotebooks()
	{
		if (!this.allNotebooksFound)
		{
			this.allNotebooksFound = true;
			this.ec.SetElevators(true);
			this.elevatorsToClose = this.ec.elevators.Count - 1;
			foreach (Elevator elevator in this.ec.elevators)
			{
				if (this.ec.elevators.Count > 1)
				{
					elevator.PrepareToClose();
				}
				base.StartCoroutine(this.ReturnSpawnFinal(elevator));
			}
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0001E4F8 File Offset: 0x0001C6F8
	public virtual void CallSpecialManagerFunction(int val, GameObject source)
	{
		if (val < this.specialManagerFunction.Length)
		{
			this.specialManagerFunction[val](source);
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001E513 File Offset: 0x0001C713
	public int NotebookTotal
	{
		get
		{
			return this.ec.notebookTotal;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001E520 File Offset: 0x0001C720
	public int FoundNotebooks
	{
		get
		{
			return this.foundNotebooks;
		}
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001E528 File Offset: 0x0001C728
	public virtual void Initialize()
	{
		Singleton<CoreGameManager>.Instance.SpawnPlayers(this.ec);
		this.ec.AssignPlayers();
		this.ApplyMap(this.ec.map);
		if (this.completeMapOnReady)
		{
			this.ec.map.CompleteMap();
		}
		this.generatorFinished = true;
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			this.ec.map.AddArrow(Singleton<CoreGameManager>.Instance.GetPlayer(i).transform, Color.white);
		}
		Singleton<CoreGameManager>.Instance.RestoreMap(this.ec.map, this.ec);
		this.CollectNotebooks(0);
		GC.Collect();
		if (this.ec.elevators.Count > 0)
		{
			for (int j = 0; j < this.ec.elevators.Count; j++)
			{
				if (this.ec.elevators[j].IsSpawn)
				{
					this.waitToExitSpawn = this.WaitToExitSpawn(this.ec.elevators[j].ColliderGroup);
					base.StartCoroutine(this.waitToExitSpawn);
					break;
				}
			}
		}
		Singleton<CoreGameManager>.Instance.ResetCameras();
		Singleton<CoreGameManager>.Instance.ResetShaders();
		Singleton<CoreGameManager>.Instance.readyToStart = true;
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

	// Token: 0x0600060E RID: 1550 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
	public void ApplyMap(Map map)
	{
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			PlayerManager player = Singleton<CoreGameManager>.Instance.GetPlayer(i);
			map.targets.Add(player.transform);
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0001E6E3 File Offset: 0x0001C8E3
	public virtual void ActivityCompleted(bool correct, Activity activity)
	{
		this.problems++;
		if (correct)
		{
			this.correctProblems++;
		}
		this.lastActivity = activity;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0001E70C File Offset: 0x0001C90C
	public virtual void BeginPlay()
	{
		Time.timeScale = 1f;
		Singleton<CoreGameManager>.Instance.BackupPlayers();
		Singleton<CoreGameManager>.Instance.disablePause = false;
		Singleton<InputManager>.Instance.ActivateActionSet("InGame");
		Singleton<CoreGameManager>.Instance.PlayBegins();
		this.ec.Active = true;
		this.ec.BeginPlay();
		this.playStarted = true;
		AudioListener.pause = false;
		PropagatedAudioManager.paused = false;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0001E77B File Offset: 0x0001C97B
	public virtual void BaldiSlapped(Baldi baldi)
	{
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0001E77D File Offset: 0x0001C97D
	public virtual void BeginSpoopMode()
	{
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0001E77F File Offset: 0x0001C97F
	protected IEnumerator WaitToExitSpawn(ColliderGroup group)
	{
		yield return new WaitForSeconds(1f);
		while (group.HasPlayer)
		{
			yield return null;
		}
		this.ExitedSpawn();
		yield break;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0001E795 File Offset: 0x0001C995
	protected virtual void ExitedSpawn()
	{
		this.ec.SetElevators(false);
		if (this.spawnImmediately)
		{
			this.ec.SpawnNPCs();
		}
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0001E7B6 File Offset: 0x0001C9B6
	protected IEnumerator ReturnSpawnFinal(Elevator elevator)
	{
		while (!elevator.ColliderGroup.HasPlayer && this.elevatorsToClose > 0)
		{
			yield return null;
		}
		if (this.elevatorsToClose > 0)
		{
			elevator.Door.Shut();
			elevator.ColliderGroup.Enable(false);
			elevator.Close();
			this.elevatorsToClose--;
			this.elevatorsClosed++;
			this.ec.MakeNoise(elevator.transform.position + elevator.transform.forward * 10f, 31);
			this.ElevatorClosed(elevator);
		}
		else
		{
			elevator.InsideCollider.Enable(true);
			base.StartCoroutine(this.EnterExit(elevator.InsideCollider));
		}
		if (this.elevatorsToClose <= 0)
		{
			using (List<Elevator>.Enumerator enumerator = this.ec.elevators.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Elevator elevator2 = enumerator.Current;
					if (elevator2.IsOpen)
					{
						elevator2.PrepareForExit();
					}
				}
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0001E7CC File Offset: 0x0001C9CC
	protected virtual void ElevatorClosed(Elevator elevator)
	{
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001E7CE File Offset: 0x0001C9CE
	protected IEnumerator EnterExit(ColliderGroup group)
	{
		while (!group.HasPlayer)
		{
			yield return null;
		}
		this.LoadNextLevel();
		yield break;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
	public void CompleteMapOnReady()
	{
		if (!this.generatorFinished)
		{
			this.completeMapOnReady = true;
			return;
		}
		this.ec.map.CompleteMap();
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001E806 File Offset: 0x0001CA06
	// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001E80E File Offset: 0x0001CA0E
	public EnvironmentController Ec
	{
		get
		{
			return this.ec;
		}
		set
		{
			this.ec = value;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001E817 File Offset: 0x0001CA17
	public float NotebookAngerVal
	{
		get
		{
			return this.notebookAngerVal;
		}
	}

	// Token: 0x04000628 RID: 1576
	public static BaseGameManager currentManager;

	// Token: 0x04000629 RID: 1577
	public bool spawnImmediately = true;

	// Token: 0x0400062A RID: 1578
	public bool beginPlayImmediately;

	// Token: 0x0400062B RID: 1579
	public bool spawnNpcsOnInit;

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	protected ElevatorScreen elevatorScreenPre;

	// Token: 0x0400062D RID: 1581
	protected ElevatorScreen elevatorScreen;

	// Token: 0x0400062E RID: 1582
	protected int foundNotebooks;

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	protected EnvironmentController ec;

	// Token: 0x04000630 RID: 1584
	public LevelObject levelObject;

	// Token: 0x04000631 RID: 1585
	protected Activity lastActivity;

	// Token: 0x04000632 RID: 1586
	protected BaseGameManager.SpecialManagerFunction[] specialManagerFunction = new BaseGameManager.SpecialManagerFunction[0];

	// Token: 0x04000633 RID: 1587
	protected IEnumerator waitToExitSpawn;

	// Token: 0x04000634 RID: 1588
	public IEnumerator waitForTrip;

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	protected float gradeValue = 1f;

	// Token: 0x04000636 RID: 1590
	protected float previousTimeScale;

	// Token: 0x04000637 RID: 1591
	protected float time;

	// Token: 0x04000638 RID: 1592
	protected float notebookAngerVal = 1f;

	// Token: 0x04000639 RID: 1593
	[SerializeField]
	protected int levelNo;

	// Token: 0x0400063A RID: 1594
	[SerializeField]
	protected int defaultLives = 2;

	// Token: 0x0400063B RID: 1595
	protected int elevatorsToClose;

	// Token: 0x0400063C RID: 1596
	protected int elevatorsClosed;

	// Token: 0x0400063D RID: 1597
	protected int problems;

	// Token: 0x0400063E RID: 1598
	protected int correctProblems;

	// Token: 0x0400063F RID: 1599
	protected int missedNotebooks;

	// Token: 0x04000640 RID: 1600
	protected bool allNotebooksFound;

	// Token: 0x04000641 RID: 1601
	protected bool playStarted;

	// Token: 0x04000642 RID: 1602
	protected bool completeMapOnReady;

	// Token: 0x04000643 RID: 1603
	protected bool generatorFinished;

	// Token: 0x0200035D RID: 861
	// (Invoke) Token: 0x06001BAF RID: 7087
	protected delegate void SpecialManagerFunction(GameObject source);
}
