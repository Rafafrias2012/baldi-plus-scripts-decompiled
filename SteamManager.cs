using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x020001FD RID: 509
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0003BCE6 File Offset: 0x00039EE6
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0003BD0A File Offset: 0x00039F0A
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0003BD16 File Offset: 0x00039F16
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0003BD1E File Offset: 0x00039F1E
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0003BD2C File Offset: 0x00039F2C
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			return;
		}
		this.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0003BDCC File Offset: 0x00039FCC
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0003BE1A File Offset: 0x0003A01A
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0003BE3E File Offset: 0x0003A03E
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0003BE4E File Offset: 0x0003A04E
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		if (pCallback.m_bActive != 0 && Singleton<CoreGameManager>.Instance != null && !Singleton<CoreGameManager>.Instance.Paused)
		{
			Singleton<CoreGameManager>.Instance.Pause(true);
		}
	}

	// Token: 0x04000DA4 RID: 3492
	protected static bool s_EverInitialized;

	// Token: 0x04000DA5 RID: 3493
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	// Token: 0x04000DA6 RID: 3494
	protected static SteamManager s_instance;

	// Token: 0x04000DA7 RID: 3495
	protected bool m_bInitialized;

	// Token: 0x04000DA8 RID: 3496
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
