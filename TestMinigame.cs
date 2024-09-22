using System;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class TestMinigame : Minigame
{
	// Token: 0x0600030A RID: 778 RVA: 0x0000FDF6 File Offset: 0x0000DFF6
	public override void Initialize(MinigameBase minigameBase, bool endlessMode)
	{
		base.Initialize(minigameBase, endlessMode);
		minigameBase.AudioManager.PlaySingle(this.audTest);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000FE14 File Offset: 0x0000E014
	private void Update()
	{
		this.testSprite.SetPosition(new Vector2(Mathf.Sin(base.minigameBase.Time) * 100f + 240f, Mathf.Sin(base.minigameBase.Time * 0.6f) * 50f + 180f));
		this.testSprite.RectTransform.localScale = new Vector3(Mathf.Sin(base.minigameBase.Time * 0.2f), Mathf.Sin(base.minigameBase.Time * 0.2f), 1f);
	}

	// Token: 0x04000334 RID: 820
	[SerializeField]
	private SoundObject audTest;

	// Token: 0x04000335 RID: 821
	[SerializeField]
	private SpriteController testSprite;
}
