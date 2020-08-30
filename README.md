# How to use Graphlib

Už z názvu je jasné, že výsledkom tohto projektu je grafová knižnica.
Je napísaná v jazyku C# a je aj určená na používanie v C#.

Jadrom knižnice sú triedy **Graph**, ktorá reprezentuje neorientovaný graf
a **OrientedGraph**. V nich sa nachádzajú všetky metódy knižnice.

Spoločné metódy sú:
- addVertex
- removeVertex
- addEdge
- removeEdge
- printGraph
- floydWarshall
- findShortestPath

Metódy triedy **Graph** sú:
- findArticulations
- findBridges
- getSpanning

Metódy triedy **OrientedGraph** sú:
- findSCCS
- topologicalOrdering
