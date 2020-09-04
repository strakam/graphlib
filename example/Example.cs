using System;
using System.Collections.Generic;
using graphlib;

namespace example
{
    public class Example
    {
        ///
        /// <summary>
        /// This is example program that reads CSV file containing dependencies
        /// for installing different programs and outputs order in which 
        /// programs have to be installed so that no dependencies are missing 
        /// when programs are installed.
        /// </summary>
        public static void Main()
        {
            OrientedGraph og = new OrientedGraph();
            // Read input data
            string[] lines = System.IO.File.ReadAllLines("dependencies.csv");

            // Assign index to every program (create pairing - name, index)
            Dictionary<string, long> programs = new Dictionary<string, long>();
            string [] names = new string[lines.Length];
            int i = 0;
            // Parse input, add vertices to graph and remember their indices
            foreach(string line in lines)
            {
                string programName = line.Split(',')[0];
                programs.Add(programName, i);
                names[i] = programName;
                og.AddVertex(i);
                i++;
            }
            
            // Add edges to a graph
            foreach(string line in lines)
            {
                string[] dependencies = line.Split(',');
                for(int j = 1; j < dependencies.Length; j++)
                {
                    long destination = programs[dependencies[j]];
                    long source = programs[dependencies[0]];
                    og.AddEdge(source, destination);
                }
            }
            // Find topological ordering of dependencies
            List<long> topoOrder = Toposort.TopologicalOrdering(ref og);
            
            // Print order in which programs have to be installed
            Console.WriteLine("Install in following order:");
            for(int j = 0; j < topoOrder.Count; j++)
            {
                Console.Write(names[topoOrder[j]] + " ");
                if(j+1 < topoOrder.Count)
                {
                    Console.Write("--> ");
                }
            }
            Console.WriteLine();
        }
    }
}
