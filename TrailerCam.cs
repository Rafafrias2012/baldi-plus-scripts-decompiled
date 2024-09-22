using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E7 RID: 487
public class TrailerCam : MonoBehaviour
{
	// Token: 0x06000B01 RID: 2817 RVA: 0x00039D18 File Offset: 0x00037F18
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			this.direction = Direction.North;
			this.StartSlide();
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			this.direction = Direction.East;
			this.StartSlide();
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			this.direction = Direction.South;
			this.StartSlide();
		}
		else if (Input.GetKeyDown(KeyCode.J))
		{
			this.direction = Direction.West;
			this.StartSlide();
		}
		else if (Input.GetKeyDown(KeyCode.O))
		{
			this.EndSlide(false);
		}
		else if (Input.GetKeyDown(KeyCode.P))
		{
			this.EndSlide(true);
		}
		else if (Input.GetKeyDown(KeyCode.U))
		{
			this.speed += 1f;
			this.currentSpeed = this.speed;
		}
		else if (Input.GetKeyDown(KeyCode.H))
		{
			this.speed -= 1f;
			this.currentSpeed = this.speed;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.snapToGrid = !this.snapToGrid;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			this.snapRotation = !this.snapRotation;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.noClip = !this.noClip;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.noHud = !this.noHud;
			Singleton<CoreGameManager>.Instance.GetCamera(0).canvasCam.enabled = !this.noHud;
		}
		if (this.sliding)
		{
			this.player.transform.position += this.direction.ToVector3() * Time.deltaTime * this.currentSpeed;
		}
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x00039EDC File Offset: 0x000380DC
	private void StartSlide()
	{
		if (this.noClip)
		{
			this.player.gameObject.layer = 12;
		}
		if (this.snapRotation)
		{
			this.player.transform.eulerAngles = new Vector3(0f, Mathf.Round(this.player.transform.localEulerAngles.y / 90f) * 90f, 0f);
		}
		if (this.snapToGrid)
		{
			Vector3 position = default(Vector3);
			position.x = Mathf.Floor(this.player.transform.position.x / 10f) * 10f + 5.00001f;
			position.z = Mathf.Floor(this.player.transform.position.z / 10f) * 10f + 5.00001f;
			position.y = this.player.transform.position.y;
			this.player.transform.position = position;
		}
		this.currentSpeed = this.speed;
		this.sliding = true;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0003A009 File Offset: 0x00038209
	private void EndSlide(bool gradual)
	{
		if (!gradual)
		{
			this.player.gameObject.layer = 16;
			this.sliding = false;
			return;
		}
		base.StartCoroutine(this.GradualStop());
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0003A035 File Offset: 0x00038235
	private IEnumerator GradualStop()
	{
		while (this.currentSpeed > 0f)
		{
			this.currentSpeed -= Time.deltaTime * this.speed;
			yield return null;
		}
		this.EndSlide(false);
		yield break;
	}

	// Token: 0x04000C91 RID: 3217
	[SerializeField]
	private PlayerManager player;

	// Token: 0x04000C92 RID: 3218
	private Direction direction;

	// Token: 0x04000C93 RID: 3219
	public float speed;

	// Token: 0x04000C94 RID: 3220
	private float currentSpeed;

	// Token: 0x04000C95 RID: 3221
	private bool sliding;

	// Token: 0x04000C96 RID: 3222
	public bool snapToGrid;

	// Token: 0x04000C97 RID: 3223
	public bool snapRotation;

	// Token: 0x04000C98 RID: 3224
	public bool noClip;

	// Token: 0x04000C99 RID: 3225
	public bool noHud;
}
