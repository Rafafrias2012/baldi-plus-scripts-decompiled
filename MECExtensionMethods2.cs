using System;
using System.Collections.Generic;
using System.Threading;
using MEC;
using UnityEngine;

// Token: 0x0200009B RID: 155
public static class MECExtensionMethods2
{
	// Token: 0x0600036C RID: 876 RVA: 0x000124F4 File Offset: 0x000106F4
	public static IEnumerator<float> Delay(this IEnumerator<float> coroutine, float timeToDelay)
	{
		yield return Routine.WaitForSeconds(timeToDelay);
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		yield break;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0001250A File Offset: 0x0001070A
	public static IEnumerator<float> Delay(this IEnumerator<float> coroutine, Func<bool> condition)
	{
		while (!condition())
		{
			yield return 0f;
		}
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		yield break;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00012520 File Offset: 0x00010720
	public static IEnumerator<float> Delay<T>(this IEnumerator<float> coroutine, T data, Func<T, bool> condition)
	{
		while (!condition(data))
		{
			yield return 0f;
		}
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		yield break;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001253D File Offset: 0x0001073D
	public static IEnumerator<float> DelayFrames(this IEnumerator<float> coroutine, int framesToDelay)
	{
		for (;;)
		{
			int num = framesToDelay;
			framesToDelay = num - 1;
			if (num <= 0)
			{
				break;
			}
			yield return 0f;
		}
		while (coroutine.MoveNext())
		{
			float num2 = coroutine.Current;
			yield return num2;
		}
		yield break;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00012553 File Offset: 0x00010753
	public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, GameObject gameObject)
	{
		while (Routine.MainThread != Thread.CurrentThread || (gameObject && gameObject.activeInHierarchy && coroutine.MoveNext()))
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00012569 File Offset: 0x00010769
	public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, GameObject gameObject1, GameObject gameObject2)
	{
		while (Routine.MainThread != Thread.CurrentThread || (gameObject1 && gameObject1.activeInHierarchy && gameObject2 && gameObject2.activeInHierarchy && coroutine.MoveNext()))
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00012586 File Offset: 0x00010786
	public static IEnumerator<float> CancelWith<T>(this IEnumerator<float> coroutine, T script) where T : MonoBehaviour
	{
		GameObject myGO = script.gameObject;
		while (Routine.MainThread != Thread.CurrentThread || (myGO && myGO.activeInHierarchy && script != null && coroutine.MoveNext()))
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001259C File Offset: 0x0001079C
	public static IEnumerator<float> CancelWith(this IEnumerator<float> coroutine, Func<bool> condition)
	{
		if (condition == null)
		{
			yield break;
		}
		while (Routine.MainThread != Thread.CurrentThread || (condition() && coroutine.MoveNext()))
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000374 RID: 884 RVA: 0x000125B2 File Offset: 0x000107B2
	public static IEnumerator<float> PauseWith(this IEnumerator<float> coroutine, GameObject gameObject)
	{
		while (Routine.MainThread == Thread.CurrentThread && gameObject)
		{
			if (gameObject.activeInHierarchy)
			{
				if (!coroutine.MoveNext())
				{
					yield break;
				}
				yield return coroutine.Current;
			}
			else
			{
				yield return float.NegativeInfinity;
			}
		}
		yield break;
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000125C8 File Offset: 0x000107C8
	public static IEnumerator<float> PauseWith(this IEnumerator<float> coroutine, GameObject gameObject1, GameObject gameObject2)
	{
		while (Routine.MainThread == Thread.CurrentThread && gameObject1 && gameObject2)
		{
			if (gameObject1.activeInHierarchy && gameObject2.activeInHierarchy)
			{
				if (!coroutine.MoveNext())
				{
					yield break;
				}
				yield return coroutine.Current;
			}
			else
			{
				yield return float.NegativeInfinity;
			}
		}
		yield break;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x000125E5 File Offset: 0x000107E5
	public static IEnumerator<float> PauseWith<T>(this IEnumerator<float> coroutine, T script) where T : MonoBehaviour
	{
		GameObject myGO = script.gameObject;
		while (Routine.MainThread == Thread.CurrentThread && myGO && myGO.GetComponent<T>() != null)
		{
			if (myGO.activeInHierarchy && script.enabled)
			{
				if (!coroutine.MoveNext())
				{
					yield break;
				}
				yield return coroutine.Current;
			}
			else
			{
				yield return float.NegativeInfinity;
			}
		}
		yield break;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x000125FB File Offset: 0x000107FB
	public static IEnumerator<float> PauseWith(this IEnumerator<float> coroutine, Func<bool> condition)
	{
		if (condition == null)
		{
			yield break;
		}
		while (Routine.MainThread != Thread.CurrentThread || (condition() && coroutine.MoveNext()))
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00012611 File Offset: 0x00010811
	public static IEnumerator<float> KillWith(this IEnumerator<float> coroutine, CoroutineHandle otherCoroutine)
	{
		while (otherCoroutine.IsRunning && coroutine.MoveNext())
		{
			yield return coroutine.Current;
		}
		yield break;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00012627 File Offset: 0x00010827
	public static IEnumerator<float> Append(this IEnumerator<float> coroutine, IEnumerator<float> nextCoroutine)
	{
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		if (nextCoroutine == null)
		{
			yield break;
		}
		while (nextCoroutine.MoveNext())
		{
			float num2 = nextCoroutine.Current;
			yield return num2;
		}
		yield break;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0001263D File Offset: 0x0001083D
	public static IEnumerator<float> Append(this IEnumerator<float> coroutine, Action onDone)
	{
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		if (onDone != null)
		{
			onDone();
		}
		yield break;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00012653 File Offset: 0x00010853
	public static IEnumerator<float> Prepend(this IEnumerator<float> coroutine, IEnumerator<float> lastCoroutine)
	{
		if (lastCoroutine != null)
		{
			while (lastCoroutine.MoveNext())
			{
				float num = lastCoroutine.Current;
				yield return num;
			}
		}
		while (coroutine.MoveNext())
		{
			float num2 = coroutine.Current;
			yield return num2;
		}
		yield break;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00012669 File Offset: 0x00010869
	public static IEnumerator<float> Prepend(this IEnumerator<float> coroutine, Action onStart)
	{
		if (onStart != null)
		{
			onStart();
		}
		while (coroutine.MoveNext())
		{
			float num = coroutine.Current;
			yield return num;
		}
		yield break;
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001267F File Offset: 0x0001087F
	public static IEnumerator<float> Superimpose(this IEnumerator<float> coroutineA, IEnumerator<float> coroutineB)
	{
		return coroutineA.Superimpose(coroutineB, Routine.Instance);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0001268D File Offset: 0x0001088D
	public static IEnumerator<float> Superimpose(this IEnumerator<float> coroutineA, IEnumerator<float> coroutineB, Routine instance)
	{
		while (coroutineA != null || coroutineB != null)
		{
			if (coroutineA != null && instance.localTime >= coroutineA.Current && !coroutineA.MoveNext())
			{
				coroutineA = null;
			}
			if (coroutineB != null && instance.localTime >= coroutineB.Current && !coroutineB.MoveNext())
			{
				coroutineB = null;
			}
			if ((coroutineA != null && float.IsNaN(coroutineA.Current)) || (coroutineB != null && float.IsNaN(coroutineB.Current)))
			{
				yield return float.NaN;
			}
			else if (coroutineA != null && coroutineB != null)
			{
				yield return (coroutineA.Current < coroutineB.Current) ? coroutineA.Current : coroutineB.Current;
			}
			else if (coroutineA == null && coroutineB != null)
			{
				yield return coroutineB.Current;
			}
			else if (coroutineA != null)
			{
				yield return coroutineA.Current;
			}
		}
		yield break;
	}

	// Token: 0x0600037F RID: 895 RVA: 0x000126AA File Offset: 0x000108AA
	public static IEnumerator<float> Hijack(this IEnumerator<float> coroutine, Func<float, float> newReturn)
	{
		if (newReturn == null)
		{
			yield break;
		}
		while (coroutine.MoveNext())
		{
			float arg = coroutine.Current;
			yield return newReturn(arg);
		}
		yield break;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x000126C0 File Offset: 0x000108C0
	public static IEnumerator<float> RerouteExceptions(this IEnumerator<float> coroutine, Action<Exception> exceptionHandler)
	{
		for (;;)
		{
			try
			{
				if (!coroutine.MoveNext())
				{
					yield break;
				}
			}
			catch (Exception obj)
			{
				if (exceptionHandler != null)
				{
					exceptionHandler(obj);
				}
				yield break;
			}
			yield return coroutine.Current;
		}
		yield break;
	}
}
