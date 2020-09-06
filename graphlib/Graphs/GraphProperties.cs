using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// In this file are all properties that are contained in graphs.
    /// Every graph property should be declared here. It increases readability.
    /// </summary>
    public partial class SharedGraph
    {
        /// <value> Internal representation of the graph </value>
        public Dictionary<long, List<Edge>> graph = 
            new Dictionary<long, List<Edge>>();

        /// <value> Number of vertices of a given graph </value>
        public long NumberOfVertices = 0;
    }
}
