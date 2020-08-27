using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    public partial class OrientedGraph:Graph
    {
        /* find_sccs (find strongly connected components is implemented in form
         * of Kosaraju's algorithm */
		public List<List<long>> find_sccs()
		{
            /* List vertices contains graph vertices in order of leaving them in
             * dfs */
			Stack<long> vertices = new Stack<long>();
            /* vertex_components will contain component number of every vertex
             * number -1 means vertex is unvisited */
			int [] vertex_components = new int[graph.Count];
            // Variable that separates vertices into components
			int component = 1;
			for(int i = 0; i < graph.Count; i++)
            {
				vertex_components[i] = -1;
            }

            // Call find_sink on unvisited vertices
			foreach(KeyValuePair<long, int> kp in indexes)
			{
				if(vertex_components[kp.Value] == -1)
                {
					find_sink(kp.Key, ref vertex_components, ref vertices);		
                }
			}
            // Take vertices from stack and perform search on them
			while(vertices.Count > 0)
			{
				if(vertex_components[v_index(vertices.Peek())] == 0)
				{
					assign_components(vertices.Peek(), component, 
                            ref vertex_components, ref vertices);
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
				t[vertex_components[kp.Value]].Add(kp.Key);
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
        /* find_sink is a modified dfs that searches transposed graph and
         * inserts vertices in desired order - that is, source is on top */
		void find_sink(long v, ref int [] vertex_components, ref Stack<long> st)
		{
			vertex_components[v_index(v)] = 0;	
			foreach(Edge e in gT[v_index(v)])
			{
				long next = v_index(e.destination);
				if(vertex_components[next] == -1)
                {
					find_sink(e.destination, ref vertex_components, ref st);
                }
			}
			st.Push(v);
		}
        /* assign_components is a modified dfs that searches graph from given
         * vertices and assigns them to a given component */
        // First argument - current vertex
        // Second argument - component number
        // Third argument - srray of component numbers of all vertices
        // Fourth argument - stack of vertices in correct order from first
        // search
		void assign_components(long v, int c, ref int [] vertex_components, 
                ref Stack<long> st)
		{
			vertex_components[v_index(v)] = c;
			foreach(Edge e in graph[v_index(v)])
			{
				long next = v_index(e.destination);
				if(vertex_components[next] == 0)
                {
					assign_components(e.destination, c, 
                            ref vertex_components, ref st);
                }
			}
		}
    }
}
