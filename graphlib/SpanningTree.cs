using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public class SpanningTree
    {
		public List<Edge> edges;
		public int cost;

		public SpanningTree(List<Edge> edges, int cost)
		{
			this.edges = edges;
			this.cost = cost;
		}
    }

	public partial class Graph
	{
		public struct ufVertex 
		{
			public int size, parent;
			public ufVertex(int parent, int size)
			{
				this.size = size;
				this.parent = parent;
			}
		}

		public int comparison(Edge a, Edge b)
		{
			return a.weight - b.weight;
		}

		bool union(Edge e, ref ufVertex [] p)
		{
			// Union hanging by size
			int root_a = find(e.source, ref p);
			int root_b = find(e.destination, ref p);
			if(root_a != root_b)
			{
				if(p[v_index(root_a)].size > p[v_index(root_b)].size)
				{
					ufVertex ta = p[v_index(root_a)];
					ta.size += p[v_index(root_b)].size;
					p[v_index(root_a)] = ta;

					ufVertex tb = p[v_index(root_b)];
					tb.parent = root_a;
					p[v_index(root_b)] = tb;
				}
				else
				{
					ufVertex tb = p[v_index(root_b)];
					tb.size += p[v_index(root_a)].size;
					p[v_index(root_b)] = tb;

					ufVertex ta = p[v_index(root_a)];
					ta.parent = root_b;
					p[v_index(root_a)] = ta;
				}
				return true;
			}
			return false;
		}

		int find(int v, ref ufVertex [] p)
		{
			// Path compression
			int root = v;
			if(p[v_index(v)].parent != v)
				root = find(p[v_index(v)].parent, ref p);
			ufVertex t = p[v_index(v)];
			t.parent = root;
			p[v_index(v)] = t;
			return root;
		}

		public SpanningTree get_spanning()
		{
			List<Edge> edges = new List<Edge>();
			ufVertex[] parents = new ufVertex [graph.Count];
			List<Edge> tree_edges = new List<Edge>();
			int total_cost = 0;

			foreach(KeyValuePair<int, int> kp in indexes)
			{
				parents[kp.Value] = new ufVertex(kp.Key, 1);
			}
			for(int i = 0; i < graph.Count; i++)
			{
				foreach(Edge e in graph[i])
				{
					edges.Add(e);
				}
			}
			edges.Sort(comparison);
			foreach(Edge e in edges)
			{
				if(union(e, ref parents))
				{
					tree_edges.Add(e);
					total_cost += e.weight;
				}
			}
			return new SpanningTree(tree_edges, total_cost);
		}
	}
}
