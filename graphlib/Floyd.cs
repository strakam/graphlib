using System;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
        /// <summary>
        /// FloydWarshall method finds shortest paths between all pairs of
        /// vertices.
        /// </summary>
        /// <returns>
        /// It returns 2D array where array at [i, j] is a shortest path from
        /// vertex i to j. 
        /// </returns>
        public long[,] FloydWarshall()
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
                int v = Vindex(kp.Key);
                for(int i = 0; i < graph[v].Count; i++)
                {
                    map[v, Vindex(graph[v][i].destination)] =
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
