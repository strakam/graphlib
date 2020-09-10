using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Struct that describes bipartity status of the graph
    /// </summary>
    public class BipartiteInfo
    {
        /// <value> isBipartite is true if graph is bipartite </value>
        public bool isBipartite {get; set;}
        /// <value> Two lists containing vertices of both partitions </value>
        public List<int> redPart, bluePart;
    }

    public static class Bipartite
    {
        /// <summary>
        /// CheckBipartity tests, whether graph is bipartite
        /// </summary>
        /// <returns>
        /// It returns instance of class Bipartite with needed info
        /// </returns>
        public static BipartiteInfo CheckBipartity(Graph g)
        {
            List<List<Edge>> graph = g.graph;
            BipartiteInfo bp = new BipartiteInfo();
            bp.isBipartite = true;
            // color[i] is telling to what partition i-th vertex belongs
            int [] color = new int[graph.Count];

            // If result is false, is not bipartite and algorithm halts
            bool result = true;
            if(g.GetNumberOfComponents() > 1)
            {
                bp.isBipartite = false;
                return bp;
            }
            // Search from unvisited vertices
            for(int i = 0; i < graph.Count; i++)
            {
                if(color[i] == 0)
                {
                    result = cbDFS(i, 1, color, graph);
                    if(!result)
                    {
                        bp.isBipartite = false;
                        break;
                    }
                }
            }
            /* If the graph is bipartite, divide it into two parts
             * according to vertex colors */
            if(result)
            {
                bp.redPart = new List<int>();
                bp.bluePart = new List<int>();
                for(int i = 0; i < graph.Count; i++)
                {
                    if(color[i] == 1)
                    {
                        bp.redPart.Add(i);
                    }
                    else 
                    {
                        bp.bluePart.Add(i);
                    }
                }
            }
            return bp;
        }
        /* cbDFS (checkBipartity dfs) is a modified dfs function
         * that tries to divide vertices into two groups */
        // First argument - current vertex number
        // Second argument - its color
        // Third argument - array of colors of all vertices
        static bool cbDFS(int v, short color, int [] colors, List<List<Edge>> g)
        {
            // Variable result serves the same function as above
            bool result = true;
            // Set color
            colors[v] = color;
            // Check all neighbors and recursively visit them
            for(int i = 0; i < g[v].Count; i++)
            {
                int neighbor = g[v][i].destination;
                if(colors[neighbor] == 0)
                {
                    result = cbDFS(neighbor, (short)(color * -1), colors, g);
                }
                else if(colors[(neighbor)] == color)
                {
                    result = false;
                }
                if(!result)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
