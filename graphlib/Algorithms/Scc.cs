using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public static class Scc
    {
        static Dictionary<long, List<Edge>> graph;
        static Dictionary<long, List<Edge>> gT;
        /* findSCCS (find strongly connected components is implemented in form
         * of Kosaraju's algorithm */
        public static List<OrientedGraph> FindSCCS(ref SharedGraph g)
        {
            graph = g.graph;
            /* List vertices contains graph vertices in order of leaving them in
             * dfs */
            Stack<long> vertices = new Stack<long>();
            /* vertexComponents will contain component number of every vertex
             * number -1 means vertex is unvisited */
            Dictionary<long, long> vertexComponents = 
                new Dictionary<long, long>();

            // Set initial vertex status and create transpose graph
            gT = new Dictionary<long, List<Edge>>();
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                vertexComponents.Add(kp.Key, -1);
                gT.Add(kp.Key, new List<Edge>());
            }

            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                foreach(Edge e in kp.Value)
                {
                    Edge reversed = new Edge(e.destination, e.source, e.weight);
                    gT[e.destination].Add(reversed);
                }
            }

            // Variable that separates vertices into components
            int component = 1;

            // Call findSink on unvisited vertices
            foreach(KeyValuePair<long, List<Edge>> kp in graph)
            {
                if(vertexComponents[kp.Key] == -1)
                {
                    FindSink(kp.Key, ref vertexComponents, ref vertices);		
                }
            }
            List<OrientedGraph> components = new List<OrientedGraph>();
            // Take vertices from stack and perform search on them
            while(vertices.Count > 0)
            {
                if(vertexComponents[vertices.Peek()] == 0)
                {
                    OrientedGraph componentGraph = new OrientedGraph();
                    assignComponents(vertices.Peek(), component, 
                            ref vertexComponents, ref componentGraph);
                    component++;
                    components.Add(componentGraph);
                }
                vertices.Pop();
            }
            return components;
        }
        /* findSink is a modified dfs that searches transposed graph and
         * inserts vertices in desired order - that is, source is on top */
        static void FindSink(long v, ref Dictionary<long, long> vertexComponents, ref Stack<long> st)
        {
            vertexComponents[v] = 0;	
            foreach(Edge e in gT[v])
            {
                long next = e.destination;
                if(vertexComponents[next] == -1)
                {
                    FindSink(e.destination, ref vertexComponents, ref st);
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
        static void assignComponents(long v, int c, 
        ref Dictionary<long, long> vertexComponents, ref OrientedGraph g)
        {
            vertexComponents[v] = c;
            foreach(Edge e in graph[v])
            {
                long next = e.destination;
                if(vertexComponents[next] == 0)
                {
                    g.AddVertex(next);
                    g.AddEdge(e.source, e.destination, e.weight);
                    assignComponents(e.destination, c, 
                            ref vertexComponents, ref g);
                }
            }
        }
    }
}
