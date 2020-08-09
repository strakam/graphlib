using System;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
		public int[,] floydWarshall()
		{
			int l = size;
			int [,] map = new int[l, l];
			for(int i = 0; i < l; i++)
				for(int j = 0; j < l; j++)
					map[i,j] = (i == j)? 0 : Int32.MaxValue;

			print_graph();
			foreach(KeyValuePair<int, int> kp in indexes)
			{
				int v = v_index(kp.Key);
				for(int i = 0; i < graph[v].Count; i++)
				{
					map[v, v_index(graph[v][i].destination)] = graph[v][i].weight;
				}
			}
			for(int k = 0; k < l; k++)
				for(int i = 0; i < l; i++)
					for(int j = 0; j < l; j++)
						map[i,j] = Math.Min(map[i,j], map[i,k] + map[k,j]);
			return map;
		}
    }
}
