using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000223 RID: 547
[ExecuteInEditMode]
[AddComponentMenu("Volumetric Audio/VA Path")]
public class VA_Path : VA_Shape
{
	// Token: 0x06000C62 RID: 3170 RVA: 0x00041654 File Offset: 0x0003F854
	protected override void LateUpdate()
	{
		base.LateUpdate();
		Vector3 vector = default(Vector3);
		if (VA_Helper.GetListenerPosition(ref vector) && this.Points.Count > 1)
		{
			Vector3 vector2 = vector;
			Vector3 vector3 = base.transform.InverseTransformPoint(vector2);
			float num = float.PositiveInfinity;
			Vector3 position = Vector3.zero;
			for (int i = 1; i < this.Points.Count; i++)
			{
				Vector3 vector4 = VA_Helper.ClosestPointToLineSegment(this.Points[i - 1], this.Points[i], vector3);
				float sqrMagnitude = (vector4 - vector3).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					position = vector4;
				}
			}
			vector2 = base.transform.TransformPoint(position);
			base.SetOuterPoint(vector2);
		}
	}

	// Token: 0x04000ECB RID: 3787
	[Tooltip("The local space points for the path")]
	public List<Vector3> Points = new List<Vector3>();
}
