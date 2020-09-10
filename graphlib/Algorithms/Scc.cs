using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Class Scc contains information about implementation of FindSCCS method
    /// that finds strongly connected components of a given graph
    /// </summary>
    public static class Scc
    {
        /// <summary>
        /// findSCCS (find strongly connected components is implemented in form
        /// of Kosaraju's algorithm.
        /// </summary>
        /// <returns>
        /// It returns int[] where value at i-th index returns number of
        /// component where i-th index belongs.
        /// <returns>
        /// <param name="g"> is OrientedGraph that is operated on. </param>
        public static int[] FindSCCS(OrientedGraph g)
        {
            List<List<Edge>> graph = g.graph;
            List<List<Edge>> gT = new List<List<Edge>>();
            /* List vertices contains graph vertices in order of leaving them in
             * dfs */
            Stack<int> vertices = new Stack<int>();
            /* vertexComponents will contain component number of every vertex
             * number -1 means vertex is unvisited */
            int [] vertexComponents = new int[graph.Count];

            // Set initial vertex status and create transpose graph
            for(int i = 0; i < graph.Count; i++)
            {
                vertexComponents[i] = -1;
                gT.Add(new List<Edge>());
            }

            foreach(List<Edge> l in graph)
            {
                foreach(Edge e in l)
                {
                    Edge reversed = new Edge(e.destination, e.source, e.weight);
                    gT[e.destination].Add(reversed);
                }
            }

            // Variable that separates vertices into components
            int component = 1;

            // Call findSink on unvisited vertices
            for(int i = 0; i < graph.Count; i++)
            {
                if(vertexComponents[i] == -1)
                {
                    FindSink(i, vertexComponents, vertices, gT);		
                }
            }
            // Take vertices from stack and perform search on them
            while(vertices.Count > 0)
            {
                if(vertexComponents[vertices.Peek()] == 0)
                {
                    assignComponents(vertices.Peek(), component, 
                            vertexComponents, graph);
                    component++;
                }
                vertices.Pop();
            }
            return vertexComponents;
        }
        /* findSink is a modified dfs that searches transposed graph and
         * inserts vertices in desired order - that is, source is on top */
        static void FindSink(int v, int [] vertexComponents, Stack<int> st, 
            List<List<Edge>> gT)
        {
            vertexComponents[v] = 0;	
            foreach(Edge e in gT[v])
            {
                long next = e.destination;
                if(vertexComponents[next] == -1)
                {
                    FindSink(e.destination, vertexComponents, st, gT);
                }
            }
            st.Push(v);
        }
        /* assignComponents is a modified dfs that searches graph from given
         * vertices and assigns them to a given component */
        // First argument - current vertex
        // Second argument - component number
        // Third argument - srray of component numbers of all vertices
        // Fourth argument - stack of vertices in correct order from first
        // search
        static void assignComponents(int v, int c, int [] vertexComponents, List<List<Edge>> graph)
        {
            vertexComponents[v] = c;
            foreach(Edge e in graph[v])
            {
                long next = e.destination;
                if(vertexComponents[next] == 0)
                {
                    assignComponents(e.destination, c, vertexComponents, graph);
                }
            }
        }
    }
}
