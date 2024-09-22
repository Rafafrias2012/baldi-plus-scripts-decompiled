using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000F6 RID: 246
public class Credits : MonoBehaviour
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x0001C674 File Offset: 0x0001A874
	private void Start()
	{
		Singleton<MusicManager>.Instance.PlayMidi("Elevator", true);
		string path = Path.Combine(Application.streamingAssetsPath, "credits.txt");
		if (File.Exists(path))
		{
			this.backerNames = new List<string>(File.ReadAllLines(path));
		}
		else
		{
			Debug.LogError("Cannot find credits file!");
			this.backerNames.Add("Error reading credits file.");
		}
		Debug.Log("Credits revision: " + this.backerNames[0]);
		this.backerNames.RemoveAt(0);
		for (int i = 0; i < this.backerNames.Count; i++)
		{
			if (this.backerNames[i].Contains("#SYMBOLS"))
			{
				this.backerNames[i] = this.backerNames[i].Replace("#SYMBOLS", "");
			}
		}
		string[] array = this.backerNames.ToArray();
		Array.Sort<string>(array, delegate(string a, string b)
		{
			a = new string(a.Where(new Func<char, bool>(char.IsLetter)).ToArray<char>());
			b = new string(b.Where(new Func<char, bool>(char.IsLetter)).ToArray<char>());
			return a.CompareTo(b);
		});
		this.backerNames = new List<string>(array);
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.backerNames.Count; j++)
		{
			if (this.backerColumns.Count == num2)
			{
				this.backerColumns.Add(Object.Instantiate<TMP_Text>(this.creditsColumnPrefab, this.scrollingCredits));
				this.backerColumns[num2].text = "";
			}
			this.backerColumns[num2].text = this.backerColumns[num2].text + this.backerNames[j] + "\n";
			num++;
			if (num >= this.linesPerColumn || j == this.backerNames.Count - 1)
			{
				this.backerColumns[num2].ForceMeshUpdate(false, false);
				Vector2 vector = new Vector2(Mathf.Max(this.minColumnDistance, this.backerColumns[num2].textBounds.size.x), 360f);
				this.backerColumns[num2].rectTransform.sizeDelta = vector;
				if (num2 > 0)
				{
					this.backerColumns[num2].transform.localPosition = new Vector3(this.backerColumns[num2 - 1].rectTransform.localPosition.x + (this.backerColumns[num2 - 1].rectTransform.sizeDelta.x / 2f + vector.x / 2f) + 20f, 0f, 0f);
				}
				num = 0;
				num2++;
			}
		}
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0001C93C File Offset: 0x0001AB3C
	private void Update()
	{
		this.time += Time.deltaTime;
		if (this.time >= this.timeBetweenFrames)
		{
			this.i++;
			this.time = 0f;
			if (this.i < this.screens.Count)
			{
				Singleton<GlobalCam>.Instance.Transition(UiTransition.SwipeRight, 0.2f);
				this.screens[this.i - 1].gameObject.SetActive(false);
				this.screens[this.i].gameObject.SetActive(true);
				this.screens[this.i].gameObject.SetActive(false);
				this.screens[this.i].gameObject.SetActive(true);
				if (this.i == this.screens.Count - 1)
				{
					base.StartCoroutine(this.CreditsScroll());
				}
			}
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.boost = this.boostSpeed - this.scrollSpeed;
			return;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.boost = (this.boostSpeed + this.scrollSpeed) * -1f;
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Singleton<InputManager>.Instance.GetDigitalInput("MouseSubmit", true) || Singleton<InputManager>.Instance.GetDigitalInput("Pause", true))
		{
			SceneManager.LoadScene("MainMenu");
			return;
		}
		this.boost = 0f;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0001CAC8 File Offset: 0x0001ACC8
	public IEnumerator CreditsScroll()
	{
		float realPos = 0f;
		Vector2 _position = default(Vector2);
		while (this.scrollingCredits.anchoredPosition.x > -this.endPoint)
		{
			if (!Singleton<GlobalCam>.Instance.TransitionActive)
			{
				realPos = Mathf.Min(realPos - Time.deltaTime * (this.scrollSpeed + this.boost), 0f);
				_position.x = Mathf.Round(realPos);
				this.scrollingCredits.anchoredPosition = _position;
				yield return null;
			}
			else
			{
				yield return null;
			}
		}
		SceneManager.LoadScene("MainMenu");
		yield break;
	}

	// Token: 0x040005D3 RID: 1491
	public List<Canvas> screens;

	// Token: 0x040005D4 RID: 1492
	public RectTransform scrollingCredits;

	// Token: 0x040005D5 RID: 1493
	private List<string> backerNames = new List<string>();

	// Token: 0x040005D6 RID: 1494
	[SerializeField]
	private TMP_Text creditsColumnPrefab;

	// Token: 0x040005D7 RID: 1495
	private List<TMP_Text> backerColumns = new List<TMP_Text>();

	// Token: 0x040005D8 RID: 1496
	public float timeBetweenFrames = 5f;

	// Token: 0x040005D9 RID: 1497
	public float scrollSpeed;

	// Token: 0x040005DA RID: 1498
	public float boostSpeed = 600f;

	// Token: 0x040005DB RID: 1499
	public float endPoint;

	// Token: 0x040005DC RID: 1500
	public float minColumnDistance = 240f;

	// Token: 0x040005DD RID: 1501
	private float time;

	// Token: 0x040005DE RID: 1502
	private float boost;

	// Token: 0x040005DF RID: 1503
	private int i;

	// Token: 0x040005E0 RID: 1504
	public int linesPerColumn = 13;
}
