using System.Collections.Generic;

namespace graphlib
{
    // Struct that describes bipartity status of the graph
    public struct Bipartite
    {
        public bool isBipartite {get; set;}
        // Two lists containing vertices of both partitions
        public List<long> redPart, bluePart;
    }

    public partial class Graph
    {
        public Bipartite checkBipartity()
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
                if(color[vIndex(kp.Key)] == 0)
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
                    if(color[vIndex(kp.Key)] == 1)
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
        private bool cbDFS(long vertex, short color, ref short [] colors)
        {
            int v = vIndex(vertex);	
            // Variable result serves the same function as above
            bool result = true;
            // Set color
            colors[v] = color;
            // Check all neighbors and recursively visit them
            for(int i = 0; i < graph[v].Count; i++)
            {
                long neighbor = graph[v][i].destination;
                if(colors[vIndex(neighbor)] == 0)
                {
                    result = cbDFS(neighbor, (short)(color * -1), ref colors);
                }
                else if(colors[vIndex(neighbor)] == color)
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
