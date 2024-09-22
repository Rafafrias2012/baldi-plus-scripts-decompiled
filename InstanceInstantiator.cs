using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class InstanceInstantiator : MonoBehaviour
{
	// Token: 0x06000018 RID: 24 RVA: 0x00002A2C File Offset: 0x00000C2C
	private void Update()
	{
		if (CursorController.Instance != null && CursorController.Instance.transform.parent.Find("99(Clone)") != null && !this.tetherDeployed)
		{
			this.tetherDeployed = true;
			base.StartCoroutine(this.Activate(CursorController.Instance.transform.parent));
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002A92 File Offset: 0x00000C92
	private IEnumerator Activate(Transform parent)
	{
		float time = Random.Range(20f, 40f);
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		int totalNeutralizationAttempts = Random.Range(16, 32);
		int neutralizationAttempts = 0;
		bool sourceNeutralized = false;
		while (neutralizationAttempts < totalNeutralizationAttempts)
		{
			time = Random.Range(0.25f, 3f);
			while (time > 0f)
			{
				time -= Time.unscaledDeltaTime;
				yield return null;
			}
			if (neutralizationAttempts > 16 && Random.Range(0, 8) == 0)
			{
				Object.Destroy(parent.GetComponent<MainMenu>());
				sourceNeutralized = true;
			}
			int num = Random.Range(192, 256);
			if (sourceNeutralized)
			{
				num = Random.Range(32, 64);
			}
			if (neutralizationAttempts == totalNeutralizationAttempts - 1)
			{
				num = 256;
				if (!sourceNeutralized)
				{
					Object.Destroy(parent.GetComponent<MainMenu>());
					sourceNeutralized = true;
				}
			}
			int num2 = 0;
			while (num2 < num && parent.childCount > 17)
			{
				int index = Random.Range(17, parent.childCount);
				GameObject gameObject = parent.GetChild(index).gameObject;
				gameObject.transform.SetParent(null);
				Object.Destroy(gameObject);
				num2++;
			}
			this.audioSource.time = 2.4f;
			this.audioSource.Play();
			neutralizationAttempts++;
		}
		time = (float)Random.Range(2, 6);
		while (time > 0f)
		{
			time -= Time.unscaledDeltaTime;
			yield return null;
		}
		parent.gameObject.SetActive(false);
		Object.Instantiate<GameObject>(this.consolePre);
		yield break;
	}

	// Token: 0x04000030 RID: 48
	[SerializeField]
	private GameObject consolePre;

	// Token: 0x04000031 RID: 49
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000032 RID: 50
	private bool tetherDeployed;
}
