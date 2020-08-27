using System.Collections.Generic;

namespace graphlib
{
    // Struct that describes bipartity status of the graph
	public struct Bipartite
	{
		public bool is_bipartite {get; set;}
        // Two lists containing vertices of both partitions
		public List<long> red_part, blue_part;
	}
    public partial class Graph
    {
		public Bipartite check_bipartity()
		{
			Bipartite bp = new Bipartite();
			bp.is_bipartite = true;
            // color[i] is telling to what partition i-th vertex belongs
			short [] color = new short[size];	
            // If result is false, is not bipartite and algorithm halts
			bool result = true;
            // Search from unvisited vertices
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				if(color[v_index(kp.Key)] == 0)
				{
					result = cb_dfs(kp.Key, 1, ref color);
					if(!result)
					{
						bp.is_bipartite = false;
						break;
					}
				}
			}
            /* If the graph is bipartite, divide it into two parts
             * according to vertex colors */
			if(result)
			{
				bp.red_part = new List<long>();
				bp.blue_part = new List<long>();
				foreach(KeyValuePair<long, int> kp in indexes)
				{
					if(color[v_index(kp.Key)] == 1)
                    {
						bp.red_part.Add(kp.Key);
                    }
					else 
                    {
						bp.blue_part.Add(kp.Key);
                    }
				}
			}
			return bp;
		}
        /* cb_dfs (check_bipartity dfs) is a modified dfs function
         * that tries to divide vertices into two groups */
        // First argument - current vertex number
        // Second argument - its color
        // Third argument - array of colors of all vertices
		private bool cb_dfs(long vertex, short color, ref short [] colors)
		{
			int v = v_index(vertex);	
            // Variable result serves the same function as above
			bool result = true;
            // Set color
			colors[v] = color;
            // Check all neighbors and recursively visit them
			for(int i = 0; i < graph[v].Count; i++)
			{
				long neighbor = graph[v][i].destination;
				if(colors[v_index(neighbor)] == 0)
				{
					result = cb_dfs(neighbor, (short)(color * -1), ref colors);
				}
				else if(colors[v_index(neighbor)] == color)
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
