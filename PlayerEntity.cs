using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class PlayerEntity : Entity
{
	// Token: 0x0600055C RID: 1372 RVA: 0x0001B684 File Offset: 0x00019884
	public override void MoveWithCollision(Vector3 movement)
	{
		this.characterController.Move(movement);
		if (this.transform.position.y != Entity.physicalHeight)
		{
			Vector3 position = this.transform.position;
			position.y = Entity.physicalHeight;
			this.transform.position = position;
		}
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001B6D9 File Offset: 0x000198D9
	public override void Teleport(Vector3 position)
	{
		base.Teleport(position);
		Physics.SyncTransforms();
	}

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	private CharacterController characterController;
}
