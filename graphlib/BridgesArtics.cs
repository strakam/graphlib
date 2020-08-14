using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
		int time = 0;
		struct ab{
			int [] disc;
			int [] low; 
			bool [] aps;
			public ab(int s)
			{
				disc = new int[s];
				low = new int[s];
				aps = new bool[s];
			}
		}
		public List<int> find_aps()
		{
			bool [] visited = new bool[graph.Count];
			int [] disc = new int[graph.Count];
			int [] low = new int[graph.Count];
			bool [] aps = new bool[graph.Count];
			List<int> ans = new List<int>();
			for(int i = 0; i < graph.Count; i++)
			{
				low[i] = 30000;
				disc[i] = 300000;
			}
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(!visited[kp.Value])
					ap(kp.Key, kp.Key, ref visited, ref disc, ref aps, ref low);
			}
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(aps[kp.Value]) ans.Add(kp.Key);
			}
			ans.Sort();
			return ans;
		}

		void ap(int v, int parent, ref bool [] visited, ref int [] disc, 
				ref bool [] aps, ref int [] low)
		{
			int children = 0;
			visited[v_index(v)] = true;	
			disc[v_index(v)] = time;
			low[v_index(v)] = time;
			foreach(Edge e in graph[v_index(v)])
			{
				children++;
				int next = v_index(e.destination);
				if(!visited[next])
				{
					time++;
					ap(e.destination, v, ref visited, ref disc, ref aps, ref low);
					low[v_index(v)] = Math.Min(low[v_index(v)], low[next]);
					if(disc[v_index(v)] <= low[next] && v != parent)
						aps[v_index(v)] = true;
					if(parent == v && children > 1)
						aps[v_index(v)] = true;
				}
				if(e.destination != parent)
					low[v_index(v)] = Math.Min(low[v_index(v)], low[next]);
			}
		}
    }
}
