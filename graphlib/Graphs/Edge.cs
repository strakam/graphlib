using System;
using System.IO;
using System.Collections.Generic;

namespace graphlib
{
    /// <summary>
    /// Class describing edges in graph.
    /// </summary>
    public class Edge
    {
        // In oriented graph names are self explanatory
        public long source {get; set;}
        public long destination {get;set;}
        public long weight {get;set;}
        public Edge(long source, long destination, long weight)
        {
            this.source = source;
            this.destination = destination;
            this.weight = weight;
        }
    }
}
