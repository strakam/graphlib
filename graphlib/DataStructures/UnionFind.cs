using System.Collections.Generic;
namespace graphlib
{
    /// <summary>
    /// UFvertex is an union-find representation of graph vertices
    /// </summary>
    public struct UFvertex
    {
        /// <value> Size - number of vertices hanged under given vertex </value>
        /// <value> Parent - id of ancestor in union find tree </value>
        public long size, parent;
        public UFvertex(long parent)
        {
            this.size = 1;
            this.parent = parent;
        }
    }

    /// <summary>
    /// UnionFind is a static class that contains union-find operations working
    /// on UFvertex structs
    /// </summary>
    public static class UnionFind
    {
        /// <summary>
        /// Union function - this merges two components of a graph in union-find
        /// representation.
        /// </summary>
        /// <returns>
        /// It returns boolean indicating whether merge happened or not.
        /// If true is returned, it means that two components weren't previously
        /// connected and were connected by union method.
        /// If false is returned, these components were already connected.
        /// </returns>
        /// <param name="e"> Is an edge, that tries to connect two
        /// components.</param>
        /// <param name="p"> Is a dictionary, that contains pairs long
        /// (vertexID) and it's union find representation. </param>
        public static bool Union(Edge e, ref Dictionary<long, UFvertex> p)
        {
            // Union hanging by size
            long rootA = Find(e.source, ref p);
            long rootB = Find(e.destination, ref p);
            // If we are comparing two different trees, merge them into one
            if(rootA != rootB)
            {
                // Compare size of both trees and hang smaller one under the
                // bigger one to increase performance
                if(p[rootA].size > p[rootB].size)
                {
                    // Hanging operations using temporary variables
                    UFvertex ta = p[rootA];
                    ta.size += p[rootB].size;
                    p[rootA] = ta;

                    UFvertex tb = p[rootB];
                    tb.parent = rootA;
                    p[rootB] = tb;
                }
                // Else is the same as above just for opposite case
                else
                {
                    UFvertex tb = p[rootB];
                    tb.size += p[rootA].size;
                    p[rootB] = tb;

                    UFvertex ta = p[rootA];
                    ta.parent = rootB;
                    p[rootA] = ta;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Recursively find root of a given vertex in union find tree
        /// </summary>
        /// <returns>
        /// It return long that is ID of root vertex of given component
        /// </returns>
        public static long Find(long v, ref Dictionary<long, UFvertex> p)
        {
            // Path compression 
            long root = v;
            if(p[v].parent != v)
            {
                root = Find(p[v].parent, ref p);
            }
            UFvertex t = p[v];
            t.parent = root;
            p[v] = t;
            return root;
        }
    }
}
