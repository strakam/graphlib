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
            public int [] disc, low;
            public bool [] articulations, visited;
            public List<List<Edge>> graph;
            public int time = 0;
            public HelperData(Graph g)
            {
                disc = new int [g.graph.Count];
                low = new int [g.graph.Count];
                articulations = new bool [g.graph.Count];
                visited = new bool [g.graph.Count];
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
        public static List<int> FindArticulations(Graph g)
        {
            // variable time is a timestamp indicating when was node visited
            HelperData info = new HelperData(g);
            // answer is a list of articlation points
            List<int> answer = new List<int>();
            for(int i = 0; i < g.graph.Count; i++)
            {
                info.low[i] = Int32.MaxValue;
                info.disc[i] = Int32.MaxValue;
            }
            for(int i = 0; i < g.graph.Count; i++)
            {
                // Start searching for articulations if not visited
                if(!info.visited[i])
                {
                    apDFS(i, i, info);
                }
            }
            for(int i = 0; i < info.articulations.Length; i++)
            {
                if(info.articulations[i])
                {
                    answer.Add(i);
                }
            }
            return answer;
        }

        // Recursive function (modified dfs) that is searching for articulations
        static void apDFS(int v, int parent, HelperData info)
        {
            long children = 0;
            int time = info.time;
            // Uploading informations
            info.visited[v] = true;	
            info.disc[v] = time;
            info.low[v] = time;
            foreach(Edge e in info.graph[v])
            {
                children++;
                int next = e.destination;
                // if a child is not visited, start search from it
                if(!info.visited[next])
                {
                    info.time++;
                    apDFS(e.destination, v, info);
                    /* Compute lowest visitable ancestor and compare it with
                     * current node to determine whether it is an articulation */
                    info.low[v] = Math.Min(info.low[v], info.low[next]);
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
                    info.low[v] = Math.Min(info.low[v], info.low[next]);
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
            for(int i = 0; i < g.graph.Count; i++)
            {
                info.low[i] = Int32.MaxValue;
                info.disc[i] = Int32.MaxValue;
            }
            for(int i = 0; i < g.graph.Count; i++)
            {
                if(!info.visited[i])
                {
                    bDFS(i, i, answer, info);
                }
            }
            return answer;
        }

        /* Function very similiar to ab function, difference is in comparison
         * signs and 1 less if statement */
        static void bDFS(int v, int parent, List<Edge> ans, HelperData info)
        {
            int children = 0;
            int time = info.time;
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
                    bDFS(e.destination, v, ans, info);
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
