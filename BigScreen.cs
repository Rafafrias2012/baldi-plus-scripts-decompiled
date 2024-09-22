using System;
using TMPro;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class BigScreen : MonoBehaviour
{
	// Token: 0x04000451 RID: 1105
	public Animator animator;

	// Token: 0x04000452 RID: 1106
	public GameObject resultsText;

	// Token: 0x04000453 RID: 1107
	public GameObject timeText;

	// Token: 0x04000454 RID: 1108
	public GameObject pointsText;

	// Token: 0x04000455 RID: 1109
	public GameObject totalText;

	// Token: 0x04000456 RID: 1110
	public GameObject multiplierText;

	// Token: 0x04000457 RID: 1111
	public GameObject gradeText;

	// Token: 0x04000458 RID: 1112
	public GameObject timeBonusText;

	// Token: 0x04000459 RID: 1113
	public GameObject gradeBonusText;

	// Token: 0x0400045A RID: 1114
	public TMP_Text time;

	// Token: 0x0400045B RID: 1115
	public TMP_Text points;

	// Token: 0x0400045C RID: 1116
	public TMP_Text total;

	// Token: 0x0400045D RID: 1117
	public TMP_Text multiplier;

	// Token: 0x0400045E RID: 1118
	public TMP_Text grade;

	// Token: 0x0400045F RID: 1119
	public TMP_Text timeBonus;

	// Token: 0x04000460 RID: 1120
	public TMP_Text gradeBonus;

	// Token: 0x04000461 RID: 1121
	public string[] grades = new string[]
	{
		"A+",
		"A",
		"A-",
		"B+",
		"B",
		"B-",
		"C+",
		"C",
		"C-",
		"D+",
		"D",
		"D-",
		"E+",
		"E",
		"E-",
		"F+",
		"F",
		"F-"
	};
}
