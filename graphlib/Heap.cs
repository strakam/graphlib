using System.Collections.Generic;
using System;
namespace graphlib
{
    public class Heap
    {
		List<Vertex> heap = new List<Vertex>();
		// Key is vertex, value is position in heap
		public Dictionary<long, int> pos = new Dictionary<long, int>();

		public struct Vertex
		{
			public long v {get;set;}
			public long cost {get;set;}
			public long parent {get;set;}
			public Vertex(long v, long cost, long parent)
			{
				this.v = v;
				this.cost = cost;
				this.parent = parent;
			}
		}

		void swap(int i, int j)
		{
			Vertex t = heap[i];
			int pos_i = pos[heap[i].v];
			int pos_j = pos[heap[j].v];
			heap[i] = heap[j];
			heap[j] = t;
			pos[heap[i].v] = pos_i;
			pos[heap[j].v] = pos_j;
		}

		void heapify(int i)
		{
			while(i > 0)
			{
				if(heap[(i-1)/2].cost > heap[i].cost)
					swap((i-1)/2, i);					
				i = (i - 1) / 2;
			}
		}

		public void add(long vertex, long cost, long parent)
		{
			heap.Add(new Vertex(vertex, cost, parent));	
			pos.Add(vertex, heap.Count-1);
			heapify(heap.Count-1);
		}

		public Vertex pop()
		{
			Vertex top = heap[0];
			swap(0, heap.Count-1);
			pos.Remove(top.v);
			heap.RemoveAt(heap.Count-1);
			int i = 0;
			while(i < heap.Count)
			{
				int l = i*2+1, r = i*2+2, smallest = i;
				if(l < heap.Count && heap[l].cost < heap[i].cost)
					smallest = l;
				if(r < heap.Count && heap[r].cost < heap[smallest].cost)
					smallest = r;
				if(smallest != i)
				{
					swap(smallest, i);
					i = smallest;
				}
				else break;
			}
			return top;
		}

		public long size()
		{
			return heap.Count;
		}

		public void decrease_key(int target, long val, long new_parent)
		{
			if(heap[pos[target]].cost > val)
			{
				Vertex t = heap[pos[target]];
				t.cost = val;
				t.parent = new_parent;
				heap[pos[target]] = t;
				heapify(pos[target]);
			}
		}
    }
}
