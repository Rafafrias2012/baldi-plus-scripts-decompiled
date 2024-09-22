using System;
using UnityEngine;

// Token: 0x020001F2 RID: 498
[CreateAssetMenu(fileName = "Level List", menuName = "Custom Assets/Level List Object", order = 11)]
public class LevelListObject : ScriptableObject
{
	// Token: 0x04000CFC RID: 3324
	public SceneObject[] scenes = new SceneObject[256];
}
