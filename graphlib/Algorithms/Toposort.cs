using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public static class Toposort
    {
        private static Dictionary<long, List<Edge>> graph;
        // Function that finds topologicalOrdering if it exists
        /// <summary>
        /// Method finds topological ordering of a graph.
        /// </summary>
        /// <returns>
        /// It returns list of IDs of vertices sorted in topological order.
        /// </returns>
        public static List<long> TopologicalOrdering(ref OrientedGraph g)
        {
            graph = g.graph;
            List<long> order = new List<long>();
            /* In visited array there are 3 types of vertices
             * 0 - not visited
             * 1 - visited but not closed
             * 2 - visited and closed */
            Dictionary<long, long> visited = new Dictionary<long, long>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                visited.Add(kp.Key, 0);
            }
            /* If result is true, topological ordering is returned, else - empty
             * list is returned */
            bool result = true;
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                // If child vertex is not visied, go search from it
                if(visited[kp.Key] == 0)
                {
                    result = tsDFS(kp.Key, ref order, ref visited);
                    if(!result)
                    {
                        return new List<long>();
                    }
                }
            }
            return order;
        }

        /* tsDFS (topological sort dfs) is a modified dfs that searches the
         * graph and adds vertices to list in topological order */
        // First argument - current vertex
        // Second argument - list of all vertices in order
        // Third argument - arrays that tells status of vertices
        static bool tsDFS(long vertex, ref List<long> order, ref Dictionary<long, long> visited)
        {
            long v = vertex;
            // Set as visited but not closed
            visited[v] = 1;
            for(int i = 0; i < graph[v].Count; i++)
            {
                long neighbor = graph[v][i].destination;
                // If child is unvisited, go search from there
                if(visited[neighbor] == 0)
                {
                    bool res = tsDFS(neighbor, ref order, ref visited);
                    if(!res)
                    {
                        return false;
                    }
                }
                // If a child is visited but not closed, we found a cycle
                // so topological ordering does not exist
                else if(visited[neighbor] == 1)
                {
                    return false;
                }
            }
            // Set as closed and add to ordering
            visited[vertex] = 2;
            order.Add(vertex);
            return true;
        }
    }
}
