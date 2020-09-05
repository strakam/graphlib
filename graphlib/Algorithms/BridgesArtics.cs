using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// This class contains implementations of algorithms
    /// that find artiulation points and bridges in the graph.
    /// </summary>
    public static class BridgesArticulations
    {
        // variable time is a timestamp indicating when was node visited
        static long time = 0;
        static Dictionary<long, List<Edge>> graph;
        struct HelperArrays{
            /* Time for each node is stored in disc, low is the lowest node that
             * is reachable from a given node. */
            public Dictionary<long, long> disc, low;
            public Dictionary<long, bool> articulations, visited;
            public HelperArrays(long s)
            {
                disc = new Dictionary<long, long>();
                low = new Dictionary<long, long>();
                articulations = new Dictionary<long, bool>();
                visited = new Dictionary<long, bool>();
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
        public static List<long> FindArticulations(ref Graph g)
        {
            graph = g.graph;
            HelperArrays info = new HelperArrays(graph.Count);
            // answer is a list of articlation points
            List<long> answer = new List<long>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                info.low[kp.Key] = long.MaxValue;
                info.disc[kp.Key] = long.MaxValue;
                info.visited[kp.Key] = false;
                info.articulations[kp.Key] = false;
            }
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                // Start searching for articulations if not visited
                if(!info.visited[kp.Key])
                {
                    apDFS(kp.Key, kp.Key, ref info);
                }
            }
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                if(info.articulations[kp.Key])
                {
                    answer.Add(kp.Key);
                }
            }
            time = 0;
            return answer;
        }

        // Recursive function (modified dfs) that is searching for articulations
        static void apDFS(long v, long parent, ref HelperArrays info)
        {
            long children = 0;
            // Uploading informations
            info.visited[v] = true;	
            info.disc[v] = time;
            info.low[v] = time;
            foreach(Edge e in graph[v])
            {
                children++;
                long next = e.destination;
                // if a child is not visited, start search from it
                if(!info.visited[next])
                {
                    time++;
                    apDFS(e.destination, v, ref info);
                    /* Compute lowest visitable ancestor and compare it with
                     * current node to determine whether it is an articulation */
                    info.low[v] = 
                        Math.Min(info.low[v], info.low[next]);
                    if(info.disc[v] <= info.low[next] && v != parent)
                    {
                        info.articulations[v] = true;
                    }
                    if(parent == v && children > 1)
                    {
                        info.articulations[v] = true;
                    }
                }
                if(e.destination != parent)
                {
                    info.low[v] = 
                        Math.Min(info.low[v], info.low[next]);
                }
            }
        }

        /// <summary>
        /// Method FindBridges is called to find bridges in graph. Logic is same
        /// as in articulation points entry function.
        /// </summary>
        /// <returns>
        /// It returns List<Edge> that are marked as bridges.
        /// </returns>
        public static List<Edge> FindBridges(ref Graph g)
        {
            HelperArrays info = new HelperArrays(graph.Count);
            List<Edge> answer = new List<Edge>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                info.low[kp.Key] = long.MaxValue;
                info.disc[kp.Key] = long.MaxValue;
                info.visited[kp.Key] = false;
                info.articulations[kp.Key] = false;
            }
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                if(!info.visited[kp.Key])
                {
                    bDFS(kp.Key, kp.Key, ref answer, ref info);
                }
            }
            time = 0;
            return answer;
        }

        /* Function very similiar to ab function, difference is in comparison
         * signs and 1 less if statement */
        static void bDFS(long v, long parent, ref List<Edge> ans, ref HelperArrays info)
        {
            long children = 0;
            info.visited[v] = true;	
            info.disc[v] = time;
            info.low[v] = time;
            foreach(Edge e in graph[v])
            {
                children++;
                long next = e.destination;
                if(!info.visited[next])
                {
                    time++;
                    bDFS(e.destination, v, ref ans, ref info);
                    info.low[v] = 
                        Math.Min(info.low[v], info.low[next]);
                    if(info.disc[v] < info.low[next])
                    {
                        ans.Add(e);
                    }
                }
                if(e.destination != parent)
                {
                    info.low[v] =
                        Math.Min(info.low[v], info.low[next]);
                }
            }
        }
    }
}
