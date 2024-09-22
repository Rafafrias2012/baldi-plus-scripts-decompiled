using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C2 RID: 450
public class GameButton : GameButtonBase
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x00036CA4 File Offset: 0x00034EA4
	protected override void Pressed(int playerNumber)
	{
		base.Pressed(playerNumber);
		foreach (IButtonReceiver buttonReceiver in this.buttonReceivers)
		{
			buttonReceiver.ButtonPressed(true);
		}
		this.meshRenderer.sharedMaterial = this.pressed;
		this.audMan.PlaySingle(this.audPress);
		base.StopAllCoroutines();
		base.StartCoroutine(this.Reset());
		UnityEvent onPress = this.OnPress;
		if (onPress == null)
		{
			return;
		}
		onPress.Invoke();
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00036D44 File Offset: 0x00034F44
	public static GameButton BuildInArea(EnvironmentController ec, IntVector2 posA, IntVector2 posB, int buttonRange, GameObject receiver, GameButton buttonPre, Random cRng)
	{
		GameButton gameButton = null;
		List<Cell> list = ec.FindCellsInNavigableRange(buttonRange, new IntVector2[]
		{
			posA
		});
		Cell cell = null;
		while (cell == null && list.Count > 0)
		{
			int index = cRng.Next(0, list.Count);
			if (list[index].HasFreeWall)
			{
				cell = list[index];
			}
			else
			{
				list.RemoveAt(index);
			}
		}
		if (cell != null)
		{
			gameButton = Object.Instantiate<GameButton>(buttonPre, cell.TileTransform);
			gameButton.SetUp(receiver.GetComponent<IButtonReceiver>());
			Direction direction = cell.RandomUncoveredDirection(cRng);
			gameButton.transform.rotation = direction.ToRotation();
			cell.HardCoverWall(direction, true);
		}
		return gameButton;
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00036DEB File Offset: 0x00034FEB
	private IEnumerator Reset()
	{
		float time = this.resetTime;
		while (time >= 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		this.meshRenderer.sharedMaterial = this.unPressed;
		this.audMan.PlaySingle(this.audUnpress);
		yield break;
	}

	// Token: 0x04000BAB RID: 2987
	[SerializeField]
	private UnityEvent OnPress;

	// Token: 0x04000BAC RID: 2988
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private Material unPressed;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	private Material pressed;

	// Token: 0x04000BAF RID: 2991
	[SerializeField]
	private AudioManager audMan;

	// Token: 0x04000BB0 RID: 2992
	[SerializeField]
	private SoundObject audPress;

	// Token: 0x04000BB1 RID: 2993
	[SerializeField]
	private SoundObject audUnpress;

	// Token: 0x04000BB2 RID: 2994
	[SerializeField]
	private float resetTime = 0.12f;
}
