using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
        // variable time is a timestamp indicating when was node visited
		long time = 0;
		struct helperArrays{
            // time for each node is stored in disc, low is the lowest node that
            // is reachable from a given node
			public long [] disc, low;
			public bool [] articulations, visited;
			public helperArrays(long s)
			{
				disc = new long[s];
				low = new long[s];
				articulations = new bool[s];
				visited = new bool[s];
			}
		}
        // Find articulation points
		public List<long> findArticulations()
		{
			helperArrays info = new helperArrays(graph.Count);
            // answer is a list of articlation points
			List<long> answer = new List<long>();
			for(int i = 0; i < graph.Count; i++)
			{
				info.low[i] = long.MaxValue;
				info.disc[i] = long.MaxValue;
			}
			foreach(KeyValuePair<long, int> kp in indexes)
			{
                // Start searching for articulations if not visited
				if(!info.visited[kp.Value])
                {
					apDFS(kp.Key, kp.Key, ref info);
                }
			}
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				if(info.articulations[kp.Value])
                {
                    answer.Add(kp.Key);
                }
			}
            time = 0;
			return answer;
		}

        // Recursive function (modified dfs) that is searching for articulations
		void apDFS(long v, long parent, ref helperArrays info)
		{
			long children = 0;
            // Uploading informations
			info.visited[vIndex(v)] = true;	
			info.disc[vIndex(v)] = time;
			info.low[vIndex(v)] = time;
			foreach(Edge e in graph[vIndex(v)])
			{
				children++;
				long next = vIndex(e.destination);
                // if a child is not visited, start search from it
				if(!info.visited[next])
				{
					time++;
					apDFS(e.destination, v, ref info);
                    /* Compute lowest visitable ancestor and compare it with
                     * current node to determine whether it is an articulation */
					info.low[vIndex(v)] = 
                        Math.Min(info.low[vIndex(v)], info.low[next]);
					if(info.disc[vIndex(v)] <= info.low[next] && v != parent)
                    {
						info.articulations[vIndex(v)] = true;
                    }
					if(parent == v && children > 1)
                    {
						info.articulations[vIndex(v)] = true;
                    }
				}
				if(e.destination != parent)
                {
					info.low[vIndex(v)] = 
                        Math.Min(info.low[vIndex(v)], info.low[next]);
                }
			}
		}

        /* This function is called to find bridges in graph
         * Logic is same as in artiulation points entry function */
		public List<Edge> findBridges()
		{
			helperArrays info = new helperArrays(graph.Count);
			List<Edge> answer = new List<Edge>();
			for(long i = 0; i < graph.Count; i++)
			{
				info.low[i] = long.MaxValue;
				info.disc[i] = long.MaxValue;
			}
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				if(!info.visited[kp.Value])
                {
					bDFS(kp.Key, kp.Key, ref answer, ref info);
                }
			}
            time = 0;
			return answer;
		}

        /* Function very similiar to ab function, difference is in comparison
         * signs and 1 less if statement */
		void bDFS(long v, long parent, ref List<Edge> ans, ref helperArrays info)
		{
			long children = 0;
			info.visited[vIndex(v)] = true;	
			info.disc[vIndex(v)] = time;
			info.low[vIndex(v)] = time;
			foreach(Edge e in graph[vIndex(v)])
			{
				children++;
				long next = vIndex(e.destination);
				if(!info.visited[next])
				{
					time++;
					bDFS(e.destination, v, ref ans, ref info);
					info.low[vIndex(v)] = 
                        Math.Min(info.low[vIndex(v)], info.low[next]);
					if(info.disc[vIndex(v)] < info.low[next])
                    {
						ans.Add(e);
                    }
				}
				if(e.destination != parent)
                {
					info.low[vIndex(v)] =
                        Math.Min(info.low[vIndex(v)], info.low[next]);
                }
			}
		}
    }
}
