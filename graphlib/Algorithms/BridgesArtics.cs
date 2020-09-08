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
        class HelperData{
            /* Time for each node is stored in disc, low is the lowest node that
             * is reachable from a given node. */
            public Dictionary<long, long> disc, low;
            public Dictionary<long, bool> articulations, visited;
            public Dictionary<long, List<Edge>> graph;
            public long time = 0;
            public HelperData(Graph g)
            {
                disc = new Dictionary<long, long>();
                low = new Dictionary<long, long>();
                articulations = new Dictionary<long, bool>();
                visited = new Dictionary<long, bool>();
                graph = g.graph;
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
        public static List<long> FindArticulations(Graph g)
        {
            // variable time is a timestamp indicating when was node visited
            HelperData info = new HelperData(g);
            // answer is a list of articlation points
            List<long> answer = new List<long>();
            foreach(KeyValuePair<long, List<Edge>> kp in info.graph)
            {
                info.low[kp.Key] = long.MaxValue;
                info.disc[kp.Key] = long.MaxValue;
                info.visited[kp.Key] = false;
                info.articulations[kp.Key] = false;
            }
            foreach(KeyValuePair<long, List<Edge>> kp in info.graph)
            {
                // Start searching for articulations if not visited
                if(!info.visited[kp.Key])
                {
                    apDFS(kp.Key, kp.Key, ref info);
                }
            }
            foreach(KeyValuePair<long, List<Edge>> kp in info.graph)
            {
                if(info.articulations[kp.Key])
                {
                    answer.Add(kp.Key);
                }
            }
            return answer;
        }

        // Recursive function (modified dfs) that is searching for articulations
        static void apDFS(long v, long parent, ref HelperData info)
        {
            long children = 0;
            long time = info.time;
            // Uploading informations
            info.visited[v] = true;	
            info.disc[v] = time;
            info.low[v] = time;
            foreach(Edge e in info.graph[v])
            {
                children++;
                long next = e.destination;
                // if a child is not visited, start search from it
                if(!info.visited[next])
                {
                    info.time++;
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
        /// Method FindBridges is called to find bridges in graph. 
        /// </summary>
        /// <returns>
        /// It returns List<Edge> that are marked as bridges.
        /// </returns>
        public static List<Edge> FindBridges(Graph g)
        {
            HelperData info = new HelperData(g);
            List<Edge> answer = new List<Edge>();
            foreach(KeyValuePair<long, List<Edge>> kp in info.graph)
            {
                info.low[kp.Key] = long.MaxValue;
                info.disc[kp.Key] = long.MaxValue;
                info.visited[kp.Key] = false;
                info.articulations[kp.Key] = false;
            }
            foreach(KeyValuePair<long, List<Edge>> kp in info.graph)
            {
                if(!info.visited[kp.Key])
                {
                    bDFS(kp.Key, kp.Key, answer, ref info);
                }
            }
            return answer;
        }

        /* Function very similiar to ab function, difference is in comparison
         * signs and 1 less if statement */
        static void bDFS(long v, long parent, List<Edge> ans, ref HelperData info)
        {
            long children = 0;
            long time = info.time;
            info.visited[v] = true;	
            info.disc[v] = time;
            info.low[v] = time;
            foreach(Edge e in info.graph[v])
            {
                children++;
                long next = e.destination;
                if(!info.visited[next])
                {
                    info.time++;
                    bDFS(e.destination, v, ans, ref info);
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
