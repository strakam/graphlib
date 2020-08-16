using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
		int time = 0;
		struct ab{
			public int [] disc, low;
			public bool [] aps, visited;
			public ab(int s)
			{
				disc = new int[s];
				low = new int[s];
				aps = new bool[s];
				visited = new bool[s];
			}
		}

		public List<int> find_aps()
		{
			ab info = new ab(graph.Count);
			List<int> ans = new List<int>();
			for(int i = 0; i < graph.Count; i++)
			{
				info.low[i] = Int32.MaxValue;
				info.disc[i] = Int32.MaxValue;
			}
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(!info.visited[kp.Value])
					ap(kp.Key, kp.Key, ref info);
			}
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(info.aps[kp.Value]) ans.Add(kp.Key);
			}
			time = 0;
			return ans;
		}

		void ap(int v, int parent, ref ab info)
		{
			int children = 0;
			info.visited[v_index(v)] = true;	
			info.disc[v_index(v)] = time;
			info.low[v_index(v)] = time;
			foreach(Edge e in graph[v_index(v)])
			{
				children++;
				int next = v_index(e.destination);
				if(!info.visited[next])
				{
					time++;
					ap(e.destination, v, ref info);
					info.low[v_index(v)] = Math.Min(info.low[v_index(v)], info.low[next]);
					if(info.disc[v_index(v)] <= info.low[next] && v != parent)
						info.aps[v_index(v)] = true;
					if(parent == v && children > 1)
						info.aps[v_index(v)] = true;
				}
				if(e.destination != parent)
					info.low[v_index(v)] = Math.Min(info.low[v_index(v)], info.low[next]);
			}
		}

		public List<Edge> find_bridges()
		{
			ab info = new ab(graph.Count);
			List<Edge> ans = new List<Edge>();
			for(int i = 0; i < graph.Count; i++)
			{
				info.low[i] = Int32.MaxValue;
				info.disc[i] = Int32.MaxValue;
			}
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(!info.visited[kp.Value])
					fb(kp.Key, kp.Key, ref ans, ref info);
			}
			return ans;
		}

		void fb(int v, int parent, ref List<Edge> ans, ref ab info)
		{
			int children = 0;
			info.visited[v_index(v)] = true;	
			info.disc[v_index(v)] = time;
			info.low[v_index(v)] = time;
			foreach(Edge e in graph[v_index(v)])
			{
				children++;
				int next = v_index(e.destination);
				if(!info.visited[next])
				{
					time++;
					fb(e.destination, v, ref ans, ref info);
					info.low[v_index(v)] = Math.Min(info.low[v_index(v)], info.low[next]);
					if(info.disc[v_index(v)] < info.low[next])
						ans.Add(e);
				}
				if(e.destination != parent)
					info.low[v_index(v)] = Math.Min(info.low[v_index(v)], info.low[next]);
			}
		}
    }
}
