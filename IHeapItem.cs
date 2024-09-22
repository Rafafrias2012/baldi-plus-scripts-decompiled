using System;

// Token: 0x0200010C RID: 268
public interface IHeapItem<T> : IComparable<T>
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000699 RID: 1689
	// (set) Token: 0x0600069A RID: 1690
	int HeapIndex { get; set; }
}
