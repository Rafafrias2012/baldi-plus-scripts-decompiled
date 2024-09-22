using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000130 RID: 304
public class PropagatedAudioManager : AudioManager
{
	// Token: 0x06000737 RID: 1847 RVA: 0x00024F80 File Offset: 0x00023180
	protected override void VirtualAwake()
	{
		base.VirtualAwake();
		if (this.audioDevice != null)
		{
			Object.Destroy(this.audioDevice);
		}
		GameObject gameObject = new GameObject(base.transform.name + "_SoundPropagator");
		this.audioDevice = gameObject.AddComponent<AudioSource>();
		this.audioDevice.spatialBlend = 1f;
		this.audioDevice.rolloffMode = AudioRolloffMode.Linear;
		this.audioDevice.minDistance = 10000f;
		this.audioDevice.maxDistance = 10000f;
		this.audioDevice.dopplerLevel = 0f;
		this.propagationPosition = base.transform.position;
		this.audioDevice.transform.position = this.propagationPosition;
		this.audioDevice.volume = 0f;
		this.VirtualUpdate();
		this.VirtualLateUpdate();
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x00025064 File Offset: 0x00023264
	protected override void VirtualUpdate()
	{
		base.VirtualUpdate();
		if (!this.environmentSet && GameCamera.dijkstraMap != null && GameCamera.dijkstraMap.EnvironmentController != null)
		{
			this.environment = GameCamera.dijkstraMap.EnvironmentController;
			this.audioDevice.transform.SetParent(this.environment.soundPropagationTransform);
			this.environmentSet = true;
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x000250CC File Offset: 0x000232CC
	protected override void VirtualLateUpdate()
	{
		if (base.AnyAudioIsPlaying && !this.audioHasBeenPlaying)
		{
			this.startedPlayingThisFrame = true;
			this.audioHasBeenPlaying = true;
		}
		else
		{
			this.audioHasBeenPlaying = base.AnyAudioIsPlaying;
			this.startedPlayingThisFrame = false;
		}
		if (!PropagatedAudioManager.paused && base.AnyAudioIsPlaying && this.environmentSet && GameCamera.dijkstraMap != null && !GameCamera.dijkstraMap.PendingUpdate)
		{
			Vector3 position = Singleton<CoreGameManager>.Instance.GetCamera(0).transform.position;
			IntVector2 intVector = IntVector2.GetGridPosition(base.transform.position);
			if (this.blocksSelf)
			{
				IntVector2 intVector2 = intVector;
				for (int i = 0; i < 4; i++)
				{
					if (GameCamera.dijkstraMap.DirectionToSource(intVector) != (Direction)i && this.environment.CellFromPosition(intVector).ConstNavigable((Direction)i) && this.environment.CellFromPosition(intVector).Mutes((Direction)i) <= 1 && GameCamera.dijkstraMap.Value(intVector + ((Direction)i).ToIntVector2()) < GameCamera.dijkstraMap.Value(intVector2) - 1)
					{
						intVector2 = intVector + ((Direction)i).ToIntVector2();
					}
				}
				intVector = intVector2;
			}
			if (this.environment.CellFromPosition(intVector).Silent || (float)GameCamera.dijkstraMap.Value(intVector) * 10f > this.maxDistance + (float)GameCamera.dijkstraMap.Value(intVector))
			{
				this.audioDevice.volume = 0f;
				this.propagatedDistance = this.maxDistance;
				return;
			}
			IntVector2 gridPosition = IntVector2.GetGridPosition(position);
			IntVector2 b = intVector;
			IntVector2 intVector3 = intVector;
			IntVector2 intVector4 = intVector;
			Direction direction = GameCamera.dijkstraMap.DirectionToSource(intVector);
			GameCamera.dijkstraMap.DirectionToSource(intVector);
			OpenTileGroup openTileGroup = this.environment.CellFromPosition(intVector).openTileGroup;
			float num = 0f;
			GameCamera.dijkstraMap.Value(intVector);
			this.propagationTurningPoints.Clear();
			this.propagationTurningPoints.Add(intVector);
			bool flag = false;
			if (this.environment.CellFromPosition(gridPosition).openTileGroup != null && this.environment.CellFromPosition(gridPosition).openTileGroup == this.environment.CellFromPosition(intVector).openTileGroup)
			{
				this.UpdatePropagationPosition(base.transform.position, true);
				num = Vector3.Magnitude(base.transform.position - position);
				this.propagatedDistance = num;
				Debug.DrawLine(this.audioDevice.transform.position, position, Color.blue);
			}
			else
			{
				while (!flag)
				{
					if (this.environment.CellFromPosition(intVector).openTileGroup != openTileGroup)
					{
						intVector3 = intVector4;
						this.AddTurningPoint(intVector3);
						direction = GameCamera.dijkstraMap.DirectionToSource(intVector);
						GameCamera.dijkstraMap.Value(intVector3);
						openTileGroup = this.environment.CellFromPosition(intVector).openTileGroup;
					}
					else if (GameCamera.dijkstraMap.DirectionToSource(intVector) != direction && GameCamera.dijkstraMap.DirectionToSource(intVector) != Direction.Null && this.environment.CellFromPosition(intVector).openTileGroup == null)
					{
						intVector3 = intVector4;
						this.AddTurningPoint(intVector3);
						direction = GameCamera.dijkstraMap.DirectionToSource(intVector);
						GameCamera.dijkstraMap.Value(intVector3);
					}
					intVector4 = intVector;
					if (GameCamera.dijkstraMap.DirectionToSource(intVector) != Direction.Null)
					{
						intVector += GameCamera.dijkstraMap.DirectionToSource(intVector).ToIntVector2();
					}
					else
					{
						flag = true;
					}
				}
				if (intVector3 == b)
				{
					this.UpdatePropagationPosition(base.transform.position, true);
					num = Vector3.Magnitude(base.transform.position - position);
					Debug.DrawLine(this.audioDevice.transform.position, position, Color.blue);
				}
				else
				{
					this.UpdatePropagationPosition(this.environment.CellFromPosition(intVector3).CenterWorldPosition, this.startedPlayingThisFrame);
					Debug.DrawLine(base.transform.position, this.environment.CellFromPosition(this.propagationTurningPoints[1]).CenterWorldPosition, Color.yellow);
					num += Vector3.Magnitude(base.transform.position - this.environment.CellFromPosition(this.propagationTurningPoints[1]).CenterWorldPosition);
					for (int j = 2; j < this.propagationTurningPoints.Count; j++)
					{
						Debug.DrawLine(this.environment.CellFromPosition(this.propagationTurningPoints[j]).CenterWorldPosition, this.environment.CellFromPosition(this.propagationTurningPoints[j - 1]).CenterWorldPosition, Color.cyan);
						num += Vector3.Magnitude(this.environment.CellFromPosition(this.propagationTurningPoints[j]).CenterWorldPosition - this.environment.CellFromPosition(this.propagationTurningPoints[j - 1]).CenterWorldPosition);
					}
					Debug.DrawLine(this.propagationPosition, position, Color.blue);
					num += Vector3.Magnitude(this.propagationPosition - position);
				}
			}
			this.audioDevice.volume = Mathf.Pow(Mathf.Max((this.maxDistance - num) / this.maxDistance, 0f), 2.7182817f) * this.volumeMultiplier;
			this.propagatedDistance = num;
		}
		else
		{
			this.audioDevice.volume = 0f;
			this.propagatedDistance = this.maxDistance;
		}
		if (this.lerpTime < 1f)
		{
			this.lerpTime = Mathf.Min(this.lerpTime + Time.unscaledDeltaTime * 3f, 1f);
			this.audioDevice.transform.position = new Vector3(Mathf.SmoothStep(this.previousAudioDevicePosition.x, this.propagationPosition.x, this.lerpTime), Mathf.SmoothStep(this.previousAudioDevicePosition.y, this.propagationPosition.y, this.lerpTime), Mathf.SmoothStep(this.previousAudioDevicePosition.z, this.propagationPosition.z, this.lerpTime));
		}
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x000256DC File Offset: 0x000238DC
	private void AddTurningPoint(IntVector2 point)
	{
		if (this.propagationTurningPoints[this.propagationTurningPoints.Count - 1] != point)
		{
			this.propagationTurningPoints.Add(point);
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002570C File Offset: 0x0002390C
	private void UpdatePropagationPosition(Vector3 position, bool instant)
	{
		if (!instant)
		{
			if (position != this.propagationPosition)
			{
				this.previousAudioDevicePosition = this.audioDevice.transform.position;
				this.propagationPosition = position;
				this.lerpTime = 1f - this.lerpTime;
				return;
			}
		}
		else
		{
			this.lerpTime = 1f;
			this.propagationPosition = position;
			this.audioDevice.transform.position = position;
		}
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0002577D File Offset: 0x0002397D
	protected override void UpdateAudioDeviceVolume()
	{
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0002577F File Offset: 0x0002397F
	public override float GetSubtitleScale(Transform cameraTransform)
	{
		if (this.propagatedDistance >= this.maxDistance)
		{
			return 0f;
		}
		return 1f - this.propagatedDistance / this.maxDistance;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000257A8 File Offset: 0x000239A8
	private void OnDestroy()
	{
		if (this.audioDevice != null)
		{
			Object.Destroy(this.audioDevice.gameObject);
		}
	}

	// Token: 0x040007EF RID: 2031
	public static bool paused = true;

	// Token: 0x040007F0 RID: 2032
	private EnvironmentController environment;

	// Token: 0x040007F1 RID: 2033
	private AudioSource propagationSource;

	// Token: 0x040007F2 RID: 2034
	private Vector3 propagationPosition;

	// Token: 0x040007F3 RID: 2035
	private Vector3 previousAudioDevicePosition;

	// Token: 0x040007F4 RID: 2036
	private List<IntVector2> propagationTurningPoints = new List<IntVector2>();

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private float minDistance = 10f;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	private float maxDistance = 100f;

	// Token: 0x040007F7 RID: 2039
	private float lerpTime;

	// Token: 0x040007F8 RID: 2040
	private float propagatedDistance;

	// Token: 0x040007F9 RID: 2041
	[SerializeField]
	private bool blocksSelf;

	// Token: 0x040007FA RID: 2042
	private bool environmentSet;

	// Token: 0x040007FB RID: 2043
	private bool startedPlayingThisFrame;

	// Token: 0x040007FC RID: 2044
	private bool audioHasBeenPlaying;
}
