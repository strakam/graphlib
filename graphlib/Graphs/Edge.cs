namespace graphlib
{
    /// <summary>
    /// Class describing edges in graph.
    /// </summary>
    public class Edge
    {
        /// <value> source is starting vertex of edge
        /// <value> destination is ending vertex of edge
        /// <value> weight is value or cost of edge
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
