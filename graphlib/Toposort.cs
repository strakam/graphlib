using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class OrientedGraph:Graph
    {

		public List<int> toposort()
		{
			List<int> order = new List<int>();
			int[] visited = new int[this.size];		
			bool res = true;
			foreach(KeyValuePair<int, int> kp in this.indexes)
			{
				if(visited[v_index(kp.Key)] == 0)
				{
					res = ts(kp.Key, ref order, ref visited);
					if(!res)
						return new List<int>();
				}
			}
			return order;
		}

		private bool ts(int vertex, ref List<int> order, ref int[] visited)
		{
			int v = v_index(vertex);
			visited[v] = 1;
			for(int i = 0; i < graph[v].Count; i++)
			{
				int neighbor = graph[v][i].destination;
				if(visited[v_index(neighbor)] == 0)
				{
					bool res = ts(neighbor, ref order, ref visited);
					if(!res) return false;
				}
				else if(visited[v_index(neighbor)] == 1)
				{
					Console.WriteLine("Cycle!!!");
					return false;
				}
			}
			visited[v_index(vertex)] = 2;
			order.Add(vertex);
			return true;
		}
    }
}
