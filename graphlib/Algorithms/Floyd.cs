using System;
using System.Collections.Generic;

namespace graphlib
{
    public struct FloydInfo
    {
        public long [,] map;
        Dictionary<long, int> indexes;
        public FloydInfo(ref SharedGraph g)
        {
            long l = g.graph.Count;
            // Adjacency matrix
            map = new long[l, l];
            // Dictionary of vertices and their indices in adjacency matrix
            indexes = new Dictionary<long, int>();
            int i = 0;
            foreach(KeyValuePair<long, List<Edge>> kp in g.graph)
            {
                indexes.Add(kp.Key, i);
                i++;
            }
        }

        public int Vindex(long v)
        {
            return indexes[v];
        }

        public long GetDistance(long source, long destination)
        {
            return map[indexes[source], indexes[destination]];
        }
    }
    public static class FloydWarshall
    {
        /// <summary>
        /// FloydWarshall method finds shortest paths between all pairs of
        /// vertices.
        /// </summary>
        /// <returns>
        /// It returns 2D array where array at [i, j] is a shortest path from
        /// vertex i to j. 
        /// </returns>
        public static FloydInfo AllShortestPaths(ref SharedGraph g)
        {
            FloydInfo fi = new FloydInfo(ref g);
            long l = g.graph.Count;
            // Set distances to zero or 'infinity'
            for(int i = 0; i < l; i++)
            {
                for(int j = 0; j < l; j++)
                {
                    fi.map[i,j] = (i == j)? 0 : long.MaxValue;
                }
            }
            // Set initial distances between vertices 
            foreach(KeyValuePair<long, List<Edge>> kp in g.graph)
            {
                long v = kp.Key;
                for(int i = 0; i < g.graph[v].Count; i++)
                {
                    fi.map[fi.Vindex(v), fi.Vindex(g.graph[v][i].destination)] =
                        g.graph[v][i].weight;
                }
            }	
            // Perform basic floyd warshall algorithm
            for(int k = 0; k < l; k++)
            {
                for(int i = 0; i < l; i++)
                {
                    for(int j = 0; j < l; j++)
                    {
                        fi.map[i,j] = Math.Min(fi.map[i,j],
                            fi.map[i,k] + fi.map[k,j]);
                    }
                }
            }
            return fi;
        }
    }
}
