using System;
using UnityEngine;

// Token: 0x020001F7 RID: 503
[CreateAssetMenu(fileName = "SceneObject", menuName = "Custom Assets/Scene Object", order = 9)]
public class SceneObject : ScriptableObject
{
	// Token: 0x04000D7A RID: 3450
	public BaseGameManager manager;

	// Token: 0x04000D7B RID: 3451
	public LevelObject levelObject;

	// Token: 0x04000D7C RID: 3452
	public LevelAsset levelAsset;

	// Token: 0x04000D7D RID: 3453
	public ExtraLevelDataAsset extraAsset;

	// Token: 0x04000D7E RID: 3454
	public LevelDataContainer levelContainer;

	// Token: 0x04000D7F RID: 3455
	public Cubemap skybox;

	// Token: 0x04000D80 RID: 3456
	public Color skyboxColor = Color.white;

	// Token: 0x04000D81 RID: 3457
	public SceneObject nextLevel;

	// Token: 0x04000D82 RID: 3458
	public string levelTitle = "F0";

	// Token: 0x04000D83 RID: 3459
	public int levelNo;

	// Token: 0x04000D84 RID: 3460
	[Header("Store data")]
	public int mapPrice = 250;

	// Token: 0x04000D85 RID: 3461
	public int totalShopItems = 4;

	// Token: 0x04000D86 RID: 3462
	public WeightedItemObject[] shopItems;

	// Token: 0x04000D87 RID: 3463
	public bool storeUsesNextLevelData;

	// Token: 0x04000D88 RID: 3464
	public bool skippable;

	// Token: 0x04000D89 RID: 3465
	public bool usesMap = true;
}
