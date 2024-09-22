using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class KickEndManager : BaseGameManager
{
	// Token: 0x06000660 RID: 1632 RVA: 0x0002008E File Offset: 0x0001E28E
	private void Update()
	{
		Singleton<MusicManager>.Instance.StopMidi();
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002009C File Offset: 0x0001E29C
	public override void Initialize()
	{
		if (Singleton<CoreGameManager>.Instance.currentMode == Mode.Free)
		{
			Object.Destroy(Singleton<ElevatorScreen>.Instance.gameObject);
			Singleton<CoreGameManager>.Instance.Quit();
			Object.Destroy(base.gameObject);
			return;
		}
		Singleton<CoreGameManager>.Instance.SpawnPlayers(this.ec);
		Singleton<CoreGameManager>.Instance.readyToStart = true;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x000200F6 File Offset: 0x0001E2F6
	public override void BeginPlay()
	{
		Time.timeScale = 1f;
		AudioListener.pause = false;
	}
}
