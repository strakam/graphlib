using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Struct that describes bipartity status of the graph
    /// </summary>
    public struct Bipartite
    {
        /// <value> isBipartite is true if graph is bipartite </value>
        public bool isBipartite {get; set;}
        /// <value> Two lists containing vertices of both partitions </value>
        public List<long> redPart, bluePart;
    }

    public partial class Graph
    {
        /// <summary>
        /// CheckBipartity tests, whether graph is bipartite
        /// </summary>
        /// <returns>
        /// It returns instance of class Bipartite with needed info
        /// </returns>
        public Bipartite CheckBipartity()
        {
            Bipartite bp = new Bipartite();
            bp.isBipartite = true;
            // color[i] is telling to what partition i-th vertex belongs
            short [] color = new short[size];	
            // If result is false, is not bipartite and algorithm halts
            bool result = true;
            // Search from unvisited vertices
            foreach(KeyValuePair<long, int> kp in indexes)
            {
                if(color[Vindex(kp.Key)] == 0)
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
                foreach(KeyValuePair<long, int> kp in indexes)
                {
                    if(color[Vindex(kp.Key)] == 1)
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
        private bool cbDFS(long Vertex, short color, ref short [] colors)
        {
            int v = Vindex(Vertex);	
            // Variable result serves the same function as above
            bool result = true;
            // Set color
            colors[v] = color;
            // Check all Neighbors and recursively visit them
            for(int i = 0; i < graph[v].Count; i++)
            {
                long Neighbor = graph[v][i].destination;
                if(colors[Vindex(Neighbor)] == 0)
                {
                    result = cbDFS(Neighbor, (short)(color * -1), ref colors);
                }
                else if(colors[Vindex(Neighbor)] == color)
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
