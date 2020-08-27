using System;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
		public long[,] floydWarshall()
		{
			long l = size;
            // Adjacency matrix
			long [,] map = new long[l, l];
            // Set distances to zero or 'infinity'
			for(int i = 0; i < l; i++)
            {
				for(int j = 0; j < l; j++)
                {
					map[i,j] = (i == j)? 0 : long.MaxValue;
                }
            }
            // Set initial distances between vertices 
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				int v = v_index(kp.Key);
				for(int i = 0; i < graph[v].Count; i++)
				{
					map[v, v_index(graph[v][i].destination)] =
                        graph[v][i].weight;
				}
			}
            // Perform basic floyd warshall algorithm
			for(int k = 0; k < l; k++)
            {
				for(int i = 0; i < l; i++)
                {
					for(int j = 0; j < l; j++)
                    {
						map[i,j] = Math.Min(map[i,j], map[i,k] + map[k,j]);
                    }
                }
            }
			return map;
		}
    }
}
