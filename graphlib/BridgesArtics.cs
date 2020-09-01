using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class Graph
    {
        // variable time is a timestamp indicating when was node visited
        long time = 0;
        struct HelperArrays{
            /* Time for each node is stored in disc, low is the lowest node that
             * is reachable from a given node. */
            public long [] disc, low;
            public bool [] articulations, visited;
            public HelperArrays(long s)
            {
                disc = new long[s];
                low = new long[s];
                articulations = new bool[s];
                visited = new bool[s];
            }
        }
        /// <summary>
        /// Method FindArticulations is made for finding articulation 
        /// vertices in a graph.
        /// </summary>
        /// <returns>
        /// It returns List<long> where elements of a list are IDs of
        /// articulation vertices.
        /// </returns>
        public List<long> FindArticulations()
        {
            HelperArrays info = new HelperArrays(graph.Count);
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
        void apDFS(long v, long parent, ref HelperArrays info)
        {
            long children = 0;
            // Uploading informations
            info.visited[Vindex(v)] = true;	
            info.disc[Vindex(v)] = time;
            info.low[Vindex(v)] = time;
            foreach(Edge e in graph[Vindex(v)])
            {
                children++;
                long next = Vindex(e.destination);
                // if a child is not visited, start search from it
                if(!info.visited[next])
                {
                    time++;
                    apDFS(e.destination, v, ref info);
                    /* Compute lowest visitable ancestor and compare it with
                     * current node to determine whether it is an articulation */
                    info.low[Vindex(v)] = 
                        Math.Min(info.low[Vindex(v)], info.low[next]);
                    if(info.disc[Vindex(v)] <= info.low[next] && v != parent)
                    {
                        info.articulations[Vindex(v)] = true;
                    }
                    if(parent == v && children > 1)
                    {
                        info.articulations[Vindex(v)] = true;
                    }
                }
                if(e.destination != parent)
                {
                    info.low[Vindex(v)] = 
                        Math.Min(info.low[Vindex(v)], info.low[next]);
                }
            }
        }

        /// <summary>
        /// Method FindBridges is called to find bridges in graph. Logic is same
        /// as in articulation points entry function.
        /// </summary>
        /// <returns>
        /// It returns list of edges that are marked as bridges.
        /// </returns>
        public List<Edge> FindBridges()
        {
            HelperArrays info = new HelperArrays(graph.Count);
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
        void bDFS(long v, long parent, ref List<Edge> ans, ref HelperArrays info)
        {
            long children = 0;
            info.visited[Vindex(v)] = true;	
            info.disc[Vindex(v)] = time;
            info.low[Vindex(v)] = time;
            foreach(Edge e in graph[Vindex(v)])
            {
                children++;
                long next = Vindex(e.destination);
                if(!info.visited[next])
                {
                    time++;
                    bDFS(e.destination, v, ref ans, ref info);
                    info.low[Vindex(v)] = 
                        Math.Min(info.low[Vindex(v)], info.low[next]);
                    if(info.disc[Vindex(v)] < info.low[next])
                    {
                        ans.Add(e);
                    }
                }
                if(e.destination != parent)
                {
                    info.low[Vindex(v)] =
                        Math.Min(info.low[Vindex(v)], info.low[next]);
                }
            }
        }
    }
}
