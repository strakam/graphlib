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
        public List<long> redPart, bluePart;
    }

    public static class Bipartite
    {
        /// <summary>
        /// CheckBipartity tests, whether graph is bipartite
        /// </summary>
        /// <returns>
        /// It returns instance of class Bipartite with needed info
        /// </returns>
        static Dictionary<long, List<Edge>> graph;
        public static BipartiteInfo CheckBipartity(Graph g)
        {
            BipartiteInfo bp = new BipartiteInfo();
            bp.isBipartite = true;
            graph = g.graph;
            // color[i] is telling to what partition i-th vertex belongs
            Dictionary<long, long> color = new Dictionary<long, long>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                color[kp.Key] = 0;
            }
            // If result is false, is not bipartite and algorithm halts
            bool result = true;
            // Search from unvisited vertices
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                if(color[kp.Key] == 0)
                {
                    result = cbDFS(kp.Key, 1, ref color);
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
                bp.redPart = new List<long>();
                bp.bluePart = new List<long>();
                foreach(KeyValuePair<long, List<Edge>> kp in graph)
                {
                    if(color[kp.Key] == 1)
                    {
                        bp.redPart.Add(kp.Key);
                    }
                    else 
                    {
                        bp.bluePart.Add(kp.Key);
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
        static bool cbDFS(long vertex, short color,
            ref Dictionary<long, long> colors)
        {
            long v = vertex;	
            // Variable result serves the same function as above
            bool result = true;
            // Set color
            colors[v] = color;
            // Check all neighbors and recursively visit them
            for(int i = 0; i < graph[v].Count; i++)
            {
                long neighbor = graph[v][i].destination;
                if(colors[neighbor] == 0)
                {
                    result = cbDFS(neighbor, (short)(color * -1), ref colors);
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
