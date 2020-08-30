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

## Ukážky a vysvetlenie používania
> Značenie : V = počet vrcholov, E = počet hrán

V prípade oboch tried, na pridanie vrcholu do grafu slúži metóda **addVertex**.
Má len jeden parameter typu **long**, ktorý je názvom vrcholu:
```
addVertex(5);
```
Využitie pamäte nezáleží od hodnoty identifikátoru najväčšieho vrcholu
a preto je bezškodné využitie veľkých longovych hodnôt.
Časová zložitosť tejto operácie je **O(1)**.

Opačnou operáciou je metóda removeVertex:
```
removeVertex(5);
```
Ktorá zmaže vrchol určený parametrom a vymaže všetky hrany ktoré sú spojené s týmto vrcholom.
Keďže táto metóda musí skontrolovať všetky hrany aby zistila, či sú s daným vrcholom spojené,
časová zložitosť je **O(E)**.



