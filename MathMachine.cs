using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class MathMachine : Activity, IClickable<int>
{
	// Token: 0x0600038E RID: 910 RVA: 0x000127CE File Offset: 0x000109CE
	private void Start()
	{
		this.ReInit();
		this.room.ec.AddActivity(this);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x000127E8 File Offset: 0x000109E8
	private void Update()
	{
		if (this.corrupted)
		{
			this.val1Text.text = this.corruptedNums[Random.Range(0, this.corruptedNums.Length)];
			this.val2Text.text = this.corruptedNums[Random.Range(0, this.corruptedNums.Length)];
			this.answerText.text = this.corruptedQs[Random.Range(0, this.corruptedQs.Length)];
			this.signText.text = this.corruptedOps[Random.Range(0, this.corruptedOps.Length)];
		}
		if (!this.completed)
		{
			for (int i = 0; i < this.currentNumbers.Count; i++)
			{
				if (this.currentNumbers[i] == null)
				{
					this.currentNumbers[i] = Object.Instantiate<MathMachineNumber>(this.numberPres[i], this.room.transform);
					this.currentNumbers[i].Floater.Initialize(this.room);
					this.currentNumbers[i].transform.position = this.currentNumbers[i].transform.position + Vector3.up * 5f;
					this.currentNumbers[i].mathMachine = this;
				}
			}
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00012950 File Offset: 0x00010B50
	private void LateUpdate()
	{
		if (!this.completed)
		{
			for (int i = 0; i < this.currentNumbers.Count; i++)
			{
				if (this.currentNumbers[i] != null && !this.currentNumbers[i].Popping && this.room.ec.CellFromPosition(this.currentNumbers[i].transform.position).room != this.room)
				{
					this.currentNumbers[i].Pop();
				}
			}
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x000129F4 File Offset: 0x00010BF4
	public override void ReInit()
	{
		base.ReInit();
		while (this.currentNumbers.Count > 0)
		{
			Object.Destroy(this.currentNumbers[0].gameObject);
			this.currentNumbers.RemoveAt(0);
		}
		for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
		{
			this.playerIsHolding[i] = false;
		}
		this.baldiAngered = false;
		for (int j = 0; j < 10; j++)
		{
			MathMachineNumber mathMachineNumber = Object.Instantiate<MathMachineNumber>(this.numberPres[j], this.room.transform);
			mathMachineNumber.Floater.Initialize(this.room);
			mathMachineNumber.mathMachine = this;
			this.currentNumbers.Add(mathMachineNumber);
		}
		this.NewProblem();
		this.answeredProblems = 0;
		if (this.totalProblems > 1)
		{
			this.totalTmp.gameObject.SetActive(true);
			this.totalTmp.text = string.Format("<sprite={0}><sprite=10><sprite={1}>", this.answeredProblems, this.totalProblems);
		}
		this.notebook.transform.position = this.meshRenderer.transform.position + this.meshRenderer.transform.forward * this.notebookDistance + Vector3.up * 5f;
		this.notebook.gameObject.SetActive(false);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00012B60 File Offset: 0x00010D60
	private void NewProblem()
	{
		this._availableAnswers.Clear();
		foreach (MathMachineNumber mathMachineNumber in this.currentNumbers)
		{
			if (mathMachineNumber.Available)
			{
				this._availableAnswers.Add(mathMachineNumber.Value);
			}
		}
		this.answer = -1;
		while (this.answer < 0 && this._availableAnswers.Count > 0)
		{
			this.answerText.text = "?";
			if (Random.Range(0, 2) > 0)
			{
				this.addition = false;
			}
			if (!this.addition)
			{
				this.signText.text = "-";
			}
			else
			{
				this.signText.text = "+";
			}
			int num = Random.Range(0, 10);
			int num2;
			if (this.addition)
			{
				num2 = Random.Range(0, 10 - num);
			}
			else
			{
				num2 = Random.Range(0, num + 1);
			}
			this.val1Text.text = num.ToString();
			this.num1 = num;
			this.val2Text.text = num2.ToString();
			this.num2 = num2;
			if (this.addition)
			{
				this.answer = num + num2;
			}
			else
			{
				this.answer = num - num2;
			}
			if (!this._availableAnswers.Contains(this.answer))
			{
				this.answer = -1;
			}
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00012CD4 File Offset: 0x00010ED4
	public override void Completed(int player)
	{
		base.Completed(player);
		if (this.givePoints)
		{
			Singleton<CoreGameManager>.Instance.AddPoints(this.normalPoints, player, true);
		}
		base.StartCoroutine(this.BalloonPopper());
		this.answerText.text = this.answer.ToString();
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00012D28 File Offset: 0x00010F28
	public override void Corrupt(bool val)
	{
		base.Corrupt(val);
		if (!val)
		{
			this.answerText.text = "?";
			if (!this.addition)
			{
				this.signText.text = "-";
			}
			else
			{
				this.signText.text = "+";
			}
			this.val1Text.text = this.num1.ToString();
			this.val2Text.text = this.num2.ToString();
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00012DA5 File Offset: 0x00010FA5
	public override void SetBonusMode(bool val)
	{
		base.SetBonusMode(val);
		if (val)
		{
			this.bonusQSign.gameObject.SetActive(true);
			this.ReInit();
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00012DC8 File Offset: 0x00010FC8
	public void Clicked(int player)
	{
		if (this.playerIsHolding[player])
		{
			this.currentNumbers[this.playerHolding[player]].trackPlayer = false;
			this.currentNumbers[this.playerHolding[player]].gameObject.SetActive(false);
			this.currentNumbers[this.playerHolding[player]].Use();
			if (this.playerHolding[player] == this.answer && !this.corrupted)
			{
				this.answeredProblems++;
				this.totalTmp.text = string.Format("<sprite={0}><sprite=10><sprite={1}>", this.answeredProblems, this.totalProblems);
				if (this.answeredProblems >= this.totalProblems)
				{
					this.Completed(player, true, this);
					Singleton<BaseGameManager>.Instance.PleaseBaldi(this.baldiPause);
					if (this.meshRenderer != null)
					{
						this.meshRenderer.sharedMaterial = this.correctMat;
					}
				}
				else
				{
					this.NewProblem();
				}
				this.audMan.PlaySingle(this.audWin);
			}
			else
			{
				this.audMan.PlaySingle(this.audLose);
				this.room.ec.MakeNoise(base.transform.position, this.wrongNoiseVal);
				this.givePoints = false;
				if (this.meshRenderer != null)
				{
					this.meshRenderer.sharedMaterial = this.incorrectMat;
				}
				if (!this.baldiAngered)
				{
					Singleton<BaseGameManager>.Instance.AngerBaldi(1f);
					this.baldiAngered = true;
				}
				this.Completed(player, false, this);
			}
			this.NumberDropped(player);
		}
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00012F6F File Offset: 0x0001116F
	public void ClickableSighted(int player)
	{
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00012F71 File Offset: 0x00011171
	public void ClickableUnsighted(int player)
	{
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00012F73 File Offset: 0x00011173
	public bool ClickableHidden()
	{
		return false;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00012F76 File Offset: 0x00011176
	public bool ClickableRequiresNormalHeight()
	{
		return false;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00012F7C File Offset: 0x0001117C
	public void NumberClicked(int val, int player)
	{
		if (this.playerIsHolding[player])
		{
			this.currentNumbers[this.playerHolding[player]].ReInit();
			this.currentNumbers[this.playerHolding[player]].Floater.Entity.Teleport(this.currentNumbers[val].transform.position);
		}
		this.playerIsHolding[player] = true;
		this.playerHolding[player] = val;
		this.currentNumbers[val].TrackPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(player), player);
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00013012 File Offset: 0x00011212
	public void NumberDropped(int player)
	{
		this.playerIsHolding[player] = false;
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001301D File Offset: 0x0001121D
	private IEnumerator BalloonPopper()
	{
		for (int i = 0; i < this.currentNumbers.Count; i++)
		{
			if (!this.currentNumbers[i].Floater.isActiveAndEnabled)
			{
				Object.Destroy(this.currentNumbers[i].gameObject);
				this.currentNumbers.RemoveAt(i);
				i--;
			}
			else
			{
				this.currentNumbers[i].Disable();
			}
		}
		float minDelay = 0.1f;
		float maxDelay = 0.3f;
		float delay = 0f;
		while (this.currentNumbers.Count > 0)
		{
			int index = Random.Range(0, this.currentNumbers.Count);
			this.currentNumbers[index].Pop();
			this.currentNumbers.RemoveAt(index);
			delay = Random.Range(minDelay, maxDelay);
			while (delay > 0f)
			{
				delay -= Time.deltaTime * this.room.ec.EnvironmentTimeScale;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040003B7 RID: 951
	[SerializeField]
	private TMP_Text val1Text;

	// Token: 0x040003B8 RID: 952
	[SerializeField]
	private TMP_Text val2Text;

	// Token: 0x040003B9 RID: 953
	[SerializeField]
	private TMP_Text signText;

	// Token: 0x040003BA RID: 954
	[SerializeField]
	private TMP_Text answerText;

	// Token: 0x040003BB RID: 955
	[SerializeField]
	private TMP_Text totalTmp;

	// Token: 0x040003BC RID: 956
	[SerializeField]
	private MathMachineNumber[] numberPres = new MathMachineNumber[10];

	// Token: 0x040003BD RID: 957
	[SerializeField]
	private SoundObject audWin;

	// Token: 0x040003BE RID: 958
	[SerializeField]
	private SoundObject audLose;

	// Token: 0x040003BF RID: 959
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x040003C0 RID: 960
	[SerializeField]
	private SpriteRenderer bonusQSign;

	// Token: 0x040003C1 RID: 961
	[SerializeField]
	private Material correctMat;

	// Token: 0x040003C2 RID: 962
	[SerializeField]
	private Material incorrectMat;

	// Token: 0x040003C3 RID: 963
	public List<MathMachineNumber> currentNumbers = new List<MathMachineNumber>();

	// Token: 0x040003C4 RID: 964
	private string[] corruptedNums = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};

	// Token: 0x040003C5 RID: 965
	private string[] corruptedOps = new string[]
	{
		"+",
		"-",
		"X",
		"/"
	};

	// Token: 0x040003C6 RID: 966
	private string[] corruptedQs = new string[]
	{
		"?",
		"!"
	};

	// Token: 0x040003C7 RID: 967
	private bool addition = true;

	// Token: 0x040003C8 RID: 968
	[SerializeField]
	private float notebookDistance = 2.25f;

	// Token: 0x040003C9 RID: 969
	private List<int> _availableAnswers = new List<int>();

	// Token: 0x040003CA RID: 970
	[SerializeField]
	private int normalPoints = 25;

	// Token: 0x040003CB RID: 971
	[SerializeField]
	private int bonusPoints = 50;

	// Token: 0x040003CC RID: 972
	[SerializeField]
	private int totalProblems = 1;

	// Token: 0x040003CD RID: 973
	[SerializeField]
	[Range(0f, 127f)]
	private int wrongNoiseVal = 126;

	// Token: 0x040003CE RID: 974
	private int[] playerHolding = new int[4];

	// Token: 0x040003CF RID: 975
	private int answer;

	// Token: 0x040003D0 RID: 976
	private int num1;

	// Token: 0x040003D1 RID: 977
	private int num2;

	// Token: 0x040003D2 RID: 978
	private int answeredProblems;

	// Token: 0x040003D3 RID: 979
	private bool[] playerIsHolding = new bool[4];

	// Token: 0x040003D4 RID: 980
	private bool baldiAngered;

	// Token: 0x040003D5 RID: 981
	private bool givePoints = true;
}
