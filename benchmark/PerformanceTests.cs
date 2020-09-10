using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using graphlib;
namespace benchmark
{
    class PerformanceTests
    {
        static void Main()
        {
            Benchmark(200000, 500000, 5000, 2, "Kruskal-200k-500k");
        }

        static void Benchmark(int vertices, int maxEdges, int increment, int repetitions, string filename)
        {
            Stopwatch timer = new Stopwatch();
            using(StreamWriter file = new StreamWriter("Results/Data/"+filename+".csv"))
            {
                for(int edges = 10000; edges <= maxEdges; edges += increment)
                {
                    long averageTime = 0;
                    var rand = new Random();
                    for(int j = 0; j < repetitions; j++)
                    {
                        Graph g = GraphGenerator(vertices, edges);
                        timer.Start();
                        SpanningTreeInfo sti = SpanningTree.GetSpanning(g);
                        timer.Stop();
                        averageTime += timer.ElapsedMilliseconds;
                        timer.Reset();
                    }
                    averageTime /= repetitions;
                    file.WriteLine("{0},{1}", edges, averageTime);
                    Console.WriteLine(edges + " " + averageTime);
                }
            }
        }

        static Graph GraphGenerator(int noVertices, int noEdges)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            var rand = new Random();
            Graph g = new Graph();
            for(int i = 0; i < noVertices; i++)
            {
                g.AddVertex();
            }
            for(int i = 0; i < noEdges; i++)
            {
                long weight = (long)rand.Next(1000000);
                int a = rand.Next(100000);
                int b = rand.Next(100000);
                Edge e = new Edge(a, b, weight);
                if(!edges.Contains(e))
                {
                    g.AddEdge(a, b, weight);
                    edges.Add(e);
                }
                else
                {
                    i--;
                }
            }
            return g;
        }
        static OrientedGraph OrientedGraphGenerator(int noVertices, int noEdges)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            var rand = new Random();
            OrientedGraph g = new OrientedGraph();
            for(int i = 0; i < noVertices; i++)
            {
                g.AddVertex();
            }
            for(int i = 0; i < noEdges; i++)
            {
                long weight = (long)rand.Next(1000000);
                int a = rand.Next(100000);
                int b = rand.Next(100000);
                Edge e = new Edge(a, b, weight);
                if(!edges.Contains(e))
                {
                    g.AddEdge(a, b, weight);
                    edges.Add(e);
                }
                else
                {
                    i--;
                }
            }
            return g;
        }
    }
}
