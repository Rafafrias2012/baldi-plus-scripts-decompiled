using System;

// Token: 0x0200010B RID: 267
public class Heap<T> where T : IHeapItem<T>
{
	// Token: 0x0600068F RID: 1679 RVA: 0x00021086 File Offset: 0x0001F286
	public Heap(int maxHeapSize)
	{
		this.items = new T[maxHeapSize];
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002109A File Offset: 0x0001F29A
	public void Clear()
	{
		this.currentItemCount = 0;
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x000210A3 File Offset: 0x0001F2A3
	public void Add(T item)
	{
		item.HeapIndex = this.currentItemCount;
		this.items[this.currentItemCount] = item;
		this.SortUp(item);
		this.currentItemCount++;
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x000210E0 File Offset: 0x0001F2E0
	public T RemoveFirst()
	{
		T result = this.items[0];
		this.currentItemCount--;
		this.items[0] = this.items[this.currentItemCount];
		this.items[0].HeapIndex = 0;
		this.SortDown(this.items[0]);
		return result;
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00021150 File Offset: 0x0001F350
	public void UpdateItem(T item)
	{
		this.SortUp(item);
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00021159 File Offset: 0x0001F359
	public int Count
	{
		get
		{
			return this.currentItemCount;
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00021164 File Offset: 0x0001F364
	public bool Contains(T item)
	{
		return item.HeapIndex < this.currentItemCount && object.Equals(this.items[item.HeapIndex], item);
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000211B0 File Offset: 0x0001F3B0
	private void SortDown(T item)
	{
		for (;;)
		{
			int num = item.HeapIndex * 2 + 1;
			int num2 = item.HeapIndex * 2 + 2;
			if (num >= this.currentItemCount)
			{
				return;
			}
			int num3 = num;
			if (num2 < this.currentItemCount && this.items[num].CompareTo(this.items[num2]) < 0)
			{
				num3 = num2;
			}
			if (item.CompareTo(this.items[num3]) >= 0)
			{
				break;
			}
			this.Swap(item, this.items[num3]);
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00021258 File Offset: 0x0001F458
	private void SortUp(T item)
	{
		int num = (item.HeapIndex - 1) / 2;
		for (;;)
		{
			T t = this.items[num];
			if (item.CompareTo(t) <= 0)
			{
				break;
			}
			this.Swap(item, t);
			num = (item.HeapIndex - 1) / 2;
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x000212B4 File Offset: 0x0001F4B4
	private void Swap(T itemA, T itemB)
	{
		this.items[itemA.HeapIndex] = itemB;
		this.items[itemB.HeapIndex] = itemA;
		int heapIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = heapIndex;
	}

	// Token: 0x040006B5 RID: 1717
	private T[] items;

	// Token: 0x040006B6 RID: 1718
	private int currentItemCount;
}
