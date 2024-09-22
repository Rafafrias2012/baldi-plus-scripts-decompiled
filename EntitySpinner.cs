using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class EntitySpinner : MonoBehaviour
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x000369BF File Offset: 0x00034BBF
	private void Start()
	{
		this.currentSpeed = this.rotationSpeed;
		if (this.spinner != null)
		{
			this.spinnerAttached = true;
		}
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x000369E4 File Offset: 0x00034BE4
	private void Update()
	{
		if (this.spinnerAttached)
		{
			this.currentSpeed = this.spinner.CurrentSpeed;
		}
		if (Time.deltaTime > 0f)
		{
			this.spin.y = this.currentSpeed * Time.deltaTime * this.environmentObject.Ec.EnvironmentTimeScale;
			for (int i = 0; i < this.entities.Count; i++)
			{
				if (this.entities[i] != null)
				{
					if (Vector3.Magnitude(base.transform.position.ZeroOutY() - this.entities[i].transform.position.ZeroOutY()) <= this.collider.radius)
					{
						Vector3 vector = this.entities[i].transform.position - base.transform.position;
						Vector3 a = Quaternion.Euler(this.spin) * vector;
						this.modifiers[i].movementAddend = (a - vector) / Time.deltaTime;
					}
					else
					{
						this.modifiers[i].movementAddend = Vector3.zero;
					}
				}
				else
				{
					this.entities.RemoveAt(i);
					this.modifiers.RemoveAt(i);
					i--;
				}
			}
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00036B47 File Offset: 0x00034D47
	public void SetSpeed(float speed)
	{
		this.currentSpeed = speed;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00036B50 File Offset: 0x00034D50
	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				this.entities.Add(component);
				this.modifiers.Add(new MovementModifier(Vector3.zero, 1f));
				this.modifiers[this.modifiers.Count - 1].ignoreAirborne = this.groundedOnly;
				component.ExternalActivity.moveMods.Add(this.modifiers[this.modifiers.Count - 1]);
			}
		}
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x00036BEC File Offset: 0x00034DEC
	private void OnTriggerExit(Collider other)
	{
		if (other.isTrigger)
		{
			Entity component = other.GetComponent<Entity>();
			if (component != null)
			{
				for (int i = 0; i < this.entities.Count; i++)
				{
					if (this.entities[i] == component)
					{
						component.ExternalActivity.moveMods.Remove(this.modifiers[i]);
						this.entities.RemoveAt(i);
						this.modifiers.RemoveAt(i);
						i--;
					}
				}
			}
		}
	}

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private EnvironmentObject environmentObject;

	// Token: 0x04000BA2 RID: 2978
	[SerializeField]
	private CapsuleCollider collider;

	// Token: 0x04000BA3 RID: 2979
	[SerializeField]
	private Spinner spinner;

	// Token: 0x04000BA4 RID: 2980
	private List<Entity> entities = new List<Entity>();

	// Token: 0x04000BA5 RID: 2981
	private List<MovementModifier> modifiers = new List<MovementModifier>();

	// Token: 0x04000BA6 RID: 2982
	private Vector3 spin;

	// Token: 0x04000BA7 RID: 2983
	[SerializeField]
	private float rotationSpeed = 90f;

	// Token: 0x04000BA8 RID: 2984
	private float currentSpeed;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private bool groundedOnly = true;

	// Token: 0x04000BAA RID: 2986
	private bool spinnerAttached;
}
