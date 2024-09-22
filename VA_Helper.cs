using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public static class VA_Helper
{
	// Token: 0x06000C27 RID: 3111 RVA: 0x0004025C File Offset: 0x0003E45C
	public static bool GetListenerPosition(ref Vector3 position)
	{
		if (VA_AudioListener.Instances.Count > 0)
		{
			position = VA_AudioListener.Instances[0].transform.position;
			return true;
		}
		if (!VA_Helper.Enabled(VA_Helper.cachedAudioListener))
		{
			AudioListener[] array = Object.FindObjectsOfType<AudioListener>();
			for (int i = array.Length - 1; i >= 0; i--)
			{
				AudioListener audioListener = array[i];
				if (audioListener.isActiveAndEnabled)
				{
					VA_Helper.cachedAudioListener = audioListener;
					break;
				}
			}
		}
		if (VA_Helper.Enabled(VA_Helper.cachedAudioListener))
		{
			position = VA_Helper.cachedAudioListener.transform.position;
			return true;
		}
		return false;
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000402ED File Offset: 0x0003E4ED
	public static Camera GetCamera(Camera camera = null)
	{
		if (camera == null || !camera.isActiveAndEnabled)
		{
			camera = Camera.main;
		}
		return camera;
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00040308 File Offset: 0x0003E508
	public static Vector2 SinCos(float a)
	{
		return new Vector2(Mathf.Sin(a), Mathf.Cos(a));
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0004031B File Offset: 0x0003E51B
	public static void Destroy(Object o)
	{
		Object.Destroy(o);
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00040323 File Offset: 0x0003E523
	public static bool Enabled(Behaviour b)
	{
		return b != null && b.isActiveAndEnabled;
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00040336 File Offset: 0x0003E536
	public static float Divide(float a, float b)
	{
		if (VA_Helper.Zero(b))
		{
			return 0f;
		}
		return a / b;
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00040349 File Offset: 0x0003E549
	public static float Reciprocal(float v)
	{
		if (VA_Helper.Zero(v))
		{
			return 0f;
		}
		return 1f / v;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00040360 File Offset: 0x0003E560
	public static bool Zero(float v)
	{
		return Mathf.Approximately(v, 0f);
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x00040370 File Offset: 0x0003E570
	public static void MatrixTrs(Vector3 t, Quaternion q, Vector3 s, ref Matrix4x4 m)
	{
		float num = q.x * 2f;
		float num2 = q.y * 2f;
		float num3 = q.z * 2f;
		float num4 = q.x * num;
		float num5 = q.y * num2;
		float num6 = q.z * num3;
		float num7 = q.x * num2;
		float num8 = q.x * num3;
		float num9 = q.y * num3;
		float num10 = q.w * num;
		float num11 = q.w * num2;
		float num12 = q.w * num3;
		m.m00 = 1f - (num5 + num6);
		m.m10 = num7 + num12;
		m.m20 = num8 - num11;
		m.m30 = 0f;
		m.m01 = num7 - num12;
		m.m11 = 1f - (num4 + num6);
		m.m21 = num9 + num10;
		m.m31 = 0f;
		m.m02 = num8 + num11;
		m.m12 = num9 - num10;
		m.m22 = 1f - (num4 + num5);
		m.m32 = 0f;
		m.m03 = t.x;
		m.m13 = t.y;
		m.m23 = t.z;
		m.m33 = 1f;
		m.m00 *= s.x;
		m.m10 *= s.x;
		m.m20 *= s.x;
		m.m01 *= s.y;
		m.m11 *= s.y;
		m.m21 *= s.y;
		m.m02 *= s.z;
		m.m12 *= s.z;
		m.m22 *= s.z;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00040550 File Offset: 0x0003E750
	public static Matrix4x4 RotationMatrix(Quaternion q)
	{
		return Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00040562 File Offset: 0x0003E762
	public static Matrix4x4 TranslationMatrix(Vector3 xyz)
	{
		return VA_Helper.TranslationMatrix(xyz.x, xyz.y, xyz.z);
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0004057C File Offset: 0x0003E77C
	public static Matrix4x4 TranslationMatrix(float x, float y, float z)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m03 = x;
		identity.m13 = y;
		identity.m23 = z;
		return identity;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x000405A8 File Offset: 0x0003E7A8
	public static Matrix4x4 ScalingMatrix(float xyz)
	{
		return VA_Helper.ScalingMatrix(xyz, xyz, xyz);
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x000405B2 File Offset: 0x0003E7B2
	public static Matrix4x4 ScalingMatrix(Vector3 xyz)
	{
		return VA_Helper.ScalingMatrix(xyz.x, xyz.y, xyz.z);
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x000405CC File Offset: 0x0003E7CC
	public static Matrix4x4 ScalingMatrix(float x, float y, float z)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = x;
		identity.m11 = y;
		identity.m22 = z;
		return identity;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x000405F8 File Offset: 0x0003E7F8
	public static float DampenFactor(float dampening, float elapsed)
	{
		return 1f - Mathf.Pow(2.7182817f, -dampening * elapsed);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00040610 File Offset: 0x0003E810
	public static Quaternion Dampen(Quaternion current, Quaternion target, float dampening, float elapsed, float minStep = 0f)
	{
		float num = VA_Helper.DampenFactor(dampening, elapsed);
		float maxDelta = Quaternion.Angle(current, target) * num + minStep * elapsed;
		return VA_Helper.MoveTowards(current, target, maxDelta);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0004063C File Offset: 0x0003E83C
	public static float Dampen(float current, float target, float dampening, float elapsed, float minStep = 0f)
	{
		float num = VA_Helper.DampenFactor(dampening, elapsed);
		float maxDelta = Mathf.Abs(target - current) * num + minStep * elapsed;
		return VA_Helper.MoveTowards(current, target, maxDelta);
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0004066C File Offset: 0x0003E86C
	public static Vector3 Dampen3(Vector3 current, Vector3 target, float dampening, float elapsed, float minStep = 0f)
	{
		float num = VA_Helper.DampenFactor(dampening, elapsed);
		float maxDistanceDelta = Mathf.Abs((target - current).magnitude) * num + minStep * elapsed;
		return Vector3.MoveTowards(current, target, maxDistanceDelta);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x000406A8 File Offset: 0x0003E8A8
	public static Quaternion MoveTowards(Quaternion current, Quaternion target, float maxDelta)
	{
		float b = Quaternion.Angle(current, target);
		return Quaternion.Slerp(current, target, VA_Helper.Divide(maxDelta, b));
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000406CB File Offset: 0x0003E8CB
	public static float MoveTowards(float current, float target, float maxDelta)
	{
		if (target > current)
		{
			current = Math.Min(target, current + maxDelta);
		}
		else
		{
			current = Math.Max(target, current - maxDelta);
		}
		return current;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x000406EC File Offset: 0x0003E8EC
	public static Vector3 ClosestPointToLineSegment(Vector3 a, Vector3 b, Vector3 point)
	{
		float magnitude = (b - a).magnitude;
		Vector3 normalized = (b - a).normalized;
		return a + Mathf.Clamp(Vector3.Dot(point - a, normalized), 0f, magnitude) * normalized;
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00040740 File Offset: 0x0003E940
	public static bool PointLeftOfLine(Vector2 a, Vector2 b, Vector2 p)
	{
		return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) >= 0f;
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x0004078E File Offset: 0x0003E98E
	public static bool PointLeftOfLine(float ax, float ay, float bx, float by, float px, float py)
	{
		return (bx - ax) * (py - ay) - (px - ax) * (by - ay) >= 0f;
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x000407AC File Offset: 0x0003E9AC
	public static bool PointRightOfLine(Vector2 a, Vector2 b, Vector2 p)
	{
		return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x000407FA File Offset: 0x0003E9FA
	public static Vector2 VectorXY(Vector3 xyz)
	{
		return new Vector2(xyz.x, xyz.y);
	}

	// Token: 0x04000EB9 RID: 3769
	public static int MeshVertexLimit = 65000;

	// Token: 0x04000EBA RID: 3770
	private static AudioListener cachedAudioListener;
}
