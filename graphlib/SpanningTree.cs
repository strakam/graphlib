using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    /* This class contains info about minimum spanning tree of a graph
     * MST finding is implemented via Kruskals algorithm so we use union-find
     * operations here */
    public class SpanningTree
    {
        // list of edges that make the tree
		public List<Edge> edges;
        // sum of weights of edes in the tree
		public long cost;

		public SpanningTree(List<Edge> edges, long cost)
		{
			this.edges = edges;
			this.cost = cost;
		}
    }

	public partial class Graph
	{
        /* ufVertex is short for union-find Vertex. Its a different
         * representation of graph vertex that is useful for union find
         * operations */ 
		public struct ufVertex 
		{
            // Size - number of vertices hanged under given vertex
            // Parent - id of ancestor in union find tree
			public long size, parent;
			public ufVertex(long parent, long size)
			{
				this.size = size;
				this.parent = parent;
			}
		}

        // Comparator for sorting edges in ascending order
		public int comparison(Edge a, Edge b)
		{
			return (int)a.weight - (int)b.weight;
		}

        // Union function - return bool indicating whether merge happened
		bool union(Edge e, ref ufVertex [] p)
		{
			// Union hanging by size
			long root_a = find(e.source, ref p);
			long root_b = find(e.destination, ref p);
            // If we are comparing two different trees, merge them into one
			if(root_a != root_b)
			{
                // Compare size of both trees and hang smaller one under the
                // bigger one to increase performance
				if(p[v_index(root_a)].size > p[v_index(root_b)].size)
				{
                    // Hanging operations using temporary variables
					ufVertex ta = p[v_index(root_a)];
					ta.size += p[v_index(root_b)].size;
					p[v_index(root_a)] = ta;

					ufVertex tb = p[v_index(root_b)];
					tb.parent = root_a;
					p[v_index(root_b)] = tb;
				}
                // Else is the same as above just for opposite case
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

        // Recursively find root of a given vertex in union find tree
		long find(long v, ref ufVertex [] p)
		{
			// Path compression 
			long root = v;
			if(p[v_index(v)].parent != v)
            {
				root = find(p[v_index(v)].parent, ref p);
            }
			ufVertex t = p[v_index(v)];
			t.parent = root;
			p[v_index(v)] = t;
			return root;
		}

        /* Classic implementation of Kruskal -
         * 1. Get all edges
         * 2. Sort them by size
         * 3. Go one by one and add them into MST if needed */
		public SpanningTree get_spanning()
		{
            // List of all graph edges
			List<Edge> edges = new List<Edge>();

            // For every vertex there is union-find representation for it
			ufVertex[] parents = new ufVertex [graph.Count];

            // Edges that are used in MST
			List<Edge> tree_edges = new List<Edge>();
			long total_cost = 0;
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				parents[kp.Value] = new ufVertex(kp.Key, 1);
			}
            // Get all edges
			for(int i = 0; i < graph.Count; i++)
			{
				foreach(Edge e in graph[i])
				{
					edges.Add(e);
				}
			}

            // Sort all edges
			edges.Sort(comparison);

			foreach(Edge e in edges)
			{
                /* If union happened, that means that current edge connected
                 * together two components of a graph so it is added into MST */
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
