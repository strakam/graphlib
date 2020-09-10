using System;
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
        public int size, parent;
        public UFvertex(int parent, int size)
        {
            this.size = size;
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
        public static bool Union(Edge e, UFvertex [] p)
        {
            // Union hanging by size
            int rootA = Find(e.source, p);
            int rootB = Find(e.destination, p);
            // If we are comparing two different trees, merge them into one
            if(rootA != rootB)
            {
                // Compare size of both trees and hang smaller one under the
                // bigger one to increase performance
                if(p[rootA].size > p[rootB].size)
                {
                    AssignChild(p, rootA, rootB);
                }
                // Else is the same as above just for opposite case
                else
                {
                    AssignChild(p, rootB, rootA);
                }
                return true;
            }
            return false;
        }
        static void AssignChild(UFvertex [] p, int pIdx, int cIdx)
        {
            UFvertex t = p[pIdx];
            t.size += p[cIdx].size;
            p[pIdx] = t;

            t = p[cIdx];
            t.parent = pIdx;
            p[cIdx] = t;
        }

        /// <summary>
        /// Recursively find root of a given vertex in union find tree
        /// </summary>
        /// <returns>
        /// It returns a long that is ID of root vertex of given component
        /// </returns>
        public static int Find(int v, UFvertex [] p)
        {
            // Path compression 
            int root = v;
            while(p[root].parent != root)
            {
                root = p[root].parent;
            }
            
            p[v].parent = root;

            return root;
        }
    }
}
