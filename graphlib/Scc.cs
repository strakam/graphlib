using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class OrientedGraph:IGraph
    {
        /* findSCCS (find strongly connected components is implemented in form
         * of Kosaraju's algorithm */
        public List<List<long>> FindSCCS()
        {
            /* List vertices contains graph vertices in order of leaving them in
             * dfs */
            Stack<long> vertices = new Stack<long>();
            /* vertexComponents will contain component number of every vertex
             * number -1 means vertex is unvisited */
            int [] vertexComponents = new int[graph.Count];
            // Variable that separates vertices into components
            int component = 1;
            for(int i = 0; i < graph.Count; i++)
            {
                vertexComponents[i] = -1;
            }

            // Call findSink on unvisited vertices
            foreach(KeyValuePair<long, int> kp in indexes)
            {
                if(vertexComponents[kp.Value] == -1)
                {
                    findSink(kp.Key, ref vertexComponents, ref vertices);		
                }
            }
            // Take vertices from stack and perform search on them
            while(vertices.Count > 0)
            {
                if(vertexComponents[Vindex(vertices.Peek())] == 0)
                {
                    assignComponents(vertices.Peek(), component, 
                            ref vertexComponents, ref vertices);
                    component++;
                }
                vertices.Pop();
            }

            /* Helper list that contains numbers of vertices if i-th component
             * in i-th list, its just temporary */
            List<List<long>> t = new List<List<long>>();
            for(int i = 0; i < graph.Count+1; i++)
            {
                t.Add(new List<long>());
            }

            // Comps will contain 2D list of components. 1 list = 1 component
            List<List<long>> comps = new List<List<long>>();

            foreach(KeyValuePair<long, int> kp in indexes)
            {
                t[vertexComponents[kp.Value]].Add(kp.Key);
            }
            // Squish components into smaller list
            for(int i = 1; i < graph.Count+1; i++)
            {
                if(t[i].Count > 0)
                {
                    comps.Add(t[i]);
                }
            }
            return comps;
        }
        /* findSink is a modified dfs that searches transposed graph and
         * inserts vertices in desired order - that is, source is on top */
        void findSink(long v, ref int [] vertexComponents, ref Stack<long> st)
        {
            vertexComponents[Vindex(v)] = 0;	
            foreach(Edge e in gT[Vindex(v)])
            {
                long next = Vindex(e.destination);
                if(vertexComponents[next] == -1)
                {
                    findSink(e.destination, ref vertexComponents, ref st);
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
        void assignComponents(long v, int c, ref int [] vertexComponents, 
                ref Stack<long> st)
        {
            vertexComponents[Vindex(v)] = c;
            foreach(Edge e in graph[Vindex(v)])
            {
                long next = Vindex(e.destination);
                if(vertexComponents[next] == 0)
                {
                    assignComponents(e.destination, c, 
                            ref vertexComponents, ref st);
                }
            }
        }
    }
}
