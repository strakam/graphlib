using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class OrientedGraph:Graph
    {
		public List<List<int>> find_sccs()
		{
			Stack<int> vertices = new Stack<int>();
			int [] helper = new int[graph.Count];
			int comp = 1;
			for(int i = 0; i < graph.Count; i++)
				helper[i] = -1;
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				if(helper[kp.Value] == -1)
					dfs1(kp.Key, ref helper, ref vertices);		
			}
			while(vertices.Count > 0)
			{
				if(helper[v_index(vertices.Peek())] == 0)
				{
					dfs2(vertices.Peek(), comp, ref helper, ref vertices);
					comp++;
				}
				vertices.Pop();
			}
			List<List<int>> t = new List<List<int>>();
			for(int i = 0; i < graph.Count+1; i++)
				t.Add(new List<int>());
			List<List<int>> comps = new List<List<int>>();
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				t[helper[kp.Value]].Add(kp.Key);
			}
			for(int i = 1; i < graph.Count+1; i++)
			{
				if(t[i].Count > 0)
					comps.Add(t[i]);
			}
			return comps;
		}

		void dfs1(int v, ref int [] helper, ref Stack<int> st)
		{
			helper[v_index(v)] = 0;	
			foreach(Edge e in gT[v_index(v)])
			{
				int next = v_index(e.destination);
				if(helper[next] == -1)
					dfs1(e.destination, ref helper, ref st);
			}
			st.Push(v);
		}

		void dfs2(int v, int c, ref int [] helper, ref Stack<int> st)
		{
			helper[v_index(v)] = c;
			foreach(Edge e in graph[v_index(v)])
			{
				int next = v_index(e.destination);
				if(helper[next] == 0)
					dfs2(e.destination, c, ref helper, ref st);
			}
		}
    }
}
