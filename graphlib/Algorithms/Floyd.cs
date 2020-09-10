using System;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// This struct contains lengths of shortest paths between all pairs of
    /// vertices. Also it contains GetDistance(long source, long destination)
    /// method that returns distance of shortest path between these two
    /// vertices.
    /// </summary>
    public class FloydInfo
    {
        public long [,] map;
        public FloydInfo(SharedGraph g)
        {
            int l = g.graph.Count;
            // Adjacency matrix
            map = new long[l, l];
        }

        public long GetDistance(int source, int destination)
        {
            return map[source, destination];
        }
    }

    /// <summary>
    /// FloydWarshall method finds shortest paths between all pairs of
    /// vertices.
    /// </summary>
    /// <returns>
    /// It returns instance of FloydInfo containing needed information.
    /// </returns>
    public static class FloydWarshall
    {
        public static FloydInfo AllShortestPaths(SharedGraph g)
        {
            FloydInfo fi = new FloydInfo(g);
            int l = g.graph.Count;
            // Set distances to zero or 'infinity'
            for(int i = 0; i < l; i++)
            {
                for(int j = 0; j < l; j++)
                {
                    fi.map[i,j] = (i == j)? 0 : long.MaxValue;
                }
            }
            // Set initial distances between vertices 
            for(int i = 0; i < g.graph.Count; i++)
            {
                foreach(Edge e in g.graph[i])
                {
                    fi.map[i, e.destination] = e.weight;
                }
            }	
            // Perform basic floyd warshall algorithm
            for(int k = 0; k < l; k++)
            {
                for(int i = 0; i < l; i++)
                {
                    for(int j = 0; j < l; j++)
                    {
                        long far = long.MaxValue;
                        if(fi.map[i, k] == far || fi.map[k, j] == far)
                        {
                            continue;
                        }
                        fi.map[i,j] = Math.Min(fi.map[i,j],
                            fi.map[i,k] + fi.map[k,j]);
                    }
                }
            }
            return fi;
        }
    }
}
