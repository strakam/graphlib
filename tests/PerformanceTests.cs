using System;
using NUnit.Framework;
using System.Collections.Generic;
using graphlib;

namespace tests
{
    public class PerformanceTests
    {
        [Test]
        public void KruskalTest()
        {
            Graph g = GraphGenerator(1000, 10000);
            SpanningTreeInfo spi = SpanningTree.GetSpanning(g);
            Assert.AreEqual(4, spi.cost);
        }



        public Graph GraphGenerator(int noVertices, int noEdges)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            var rand = new Random();
            Graph g = new Graph();
            for(int i = 0; i < noVertices; i++)
            {
                g.AddVertex(i);
            }
            for(int i = 0; i < noEdges; i++)
            {
                /* long weight = (long)rand.Next(1000000); */
                /* Edge e = new Edge((long)rand.Next(1000), (long)rand.Next(1000), weight); */
                /* if(!edges.Contains(e)) */
                /* { */
                    g.AddEdge((i+3)%noVertices, (i+4)%noVertices, 1);
                /* } */
            }
            return g;
        }
    }
}
