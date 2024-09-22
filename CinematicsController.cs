using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class CinematicsController : MonoBehaviour
{
	// Token: 0x04000536 RID: 1334
	public PlayerManager player;

	// Token: 0x04000537 RID: 1335
	private Vector3 _position;

	// Token: 0x04000538 RID: 1336
	private Vector3 _rotation;

	// Token: 0x04000539 RID: 1337
	public float moveSpeed = 1f;

	// Token: 0x0400053A RID: 1338
	public static bool hideHud;

	// Token: 0x0400053B RID: 1339
	private bool gridSnap;

	// Token: 0x0400053C RID: 1340
	private bool moveBack;

	// Token: 0x0400053D RID: 1341
	private bool attractBaldi;
}
