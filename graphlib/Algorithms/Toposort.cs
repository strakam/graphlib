using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Toposort class contains implementation of TopologicalOrdering method
    /// </summary>
    public static class Toposort
    {
        // Function that finds topologicalOrdering if it exists
        /// <summary>
        /// Method finds topological ordering of a graph.
        /// </summary>
        /// <returns>
        /// It returns list of IDs of vertices sorted in topological order.
        /// </returns>
        public static List<int> TopologicalOrdering(OrientedGraph g)
        {
            List<List<Edge>> graph = g.graph;
            List<int> order = new List<int>();
            /* In visited array there are 3 types of vertices
             * 0 - not visited
             * 1 - visited but not closed
             * 2 - visited and closed */
            byte [] visited = new byte[graph.Count];
            /* If result is true, topological ordering is returned, else - empty
             * list is returned */
            bool result = true;
            for(int i = 0; i < graph.Count; i++)
            {
                // If child vertex is not visied, go search from it
                if(visited[i] == 0)
                {
                    result = tsDFS(i, order, visited, graph);
                    if(!result)
                    {
                        return new List<int>();
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
        static bool tsDFS(int v, List<int> order, byte [] visited, List<List<Edge>> graph)
        {
            // Set as visited but not closed
            visited[v] = 1;
            for(int i = 0; i < graph[v].Count; i++)
            {
                int neighbor = graph[v][i].destination;
                // If child is unvisited, go search from there
                if(visited[neighbor] == 0)
                {
                    bool res = tsDFS(neighbor, order, visited, graph);
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
            visited[v] = 2;
            order.Add(v);
            return true;
        }
    }
}
