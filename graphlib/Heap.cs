using System.Collections.Generic;
using System;
namespace graphlib
{
    public class Heap
    {
		List<Vertex> heap = new List<Vertex>();
		// Key is vertex, value is position in heap
		public Dictionary<int, int> pos = new Dictionary<int, int>();

		public struct Vertex
		{
			public int v {get;set;}
			public int cost {get;set;}
			public Vertex(int v, int cost)
			{
				this.v = v;
				this.cost = cost;
			}
		}

		void swap(int i, int j)
		{
			Vertex t = heap[i];
			int pos_i = pos[heap[i].v]; // Tu bolo i-cko
			int pos_j = pos[heap[j].v]; // TU bolo j-cko
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

		public void add(int vertex, int cost)
		{
			heap.Add(new Vertex(vertex, cost));	
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

		public int size()
		{
			return heap.Count;
		}

		public void decrease_key(int target, int val)
		{
			if(heap[pos[target]].cost > val)
			{
				Vertex t = heap[pos[target]];
				t.cost = val;
				heap[pos[target]] = t;
				heapify(pos[target]);
			}
		}
    }
}
