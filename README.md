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

V prípade oboch tried, na pridanie vrcholu do grafu slúži metóda **addVertex(long vertexID)**.
Má len jeden parameter typu **long**, ktorý je názvom vrcholu.
Využitie pamäte nezáleží od hodnoty identifikátoru najväčšieho vrcholu
a preto je bezškodné využitie veľkých longovych hodnôt.
Časová zložitosť tejto operácie je **O(1)**.

Opačnou operáciou je metóda **removeVertex(long vertexID)**,
Ktorá zmaže vrchol určený parametrom a vymaže všetky hrany ktoré sú spojené s týmto vrcholom.
Keďže táto metóda musí skontrolovať všetky hrany aby zistila, či sú s daným vrcholom spojené,
časová zložitosť je **O(E)**.

Na pridávanie hrán je určená metóda **addEdge**, ktorá má viacero interpretácii.
Pre neorientovaný graf:
- **addEdge(long v1, long v2, long weight)**
- **addEdge(long v1, long v2)**

Pre orientovaný graf:
- **addEdge(long source, long destination, long weight)**
- **addEdge(long source, long destination)**

V oboch prípadoch ak nieje určený parameter **weight**, váha je nastavená na 1.
V prípade, že všetky hrany majú váhu 1, jedná sa o neohodnotený graf.
Táto metóda pridá v triede **Graph** hranu obojstranne obom vrcholom. V prípade
orientovaného grafu, je priradená iba vrcholu označeným **source**(prvý parameter).

Opačnou metódou pre neorientovaný graf je **removeEdge(long v1, long v2)**, ktorá zmaže hranu
spájajúcu vrchol **v1** a **v2** a pre orientovaný graf **removeEdge(long source, long destination)**,
ktorá zmaže hranu vedúcu z vrcholu **source** do vrcholu **destination**.
Časová zložitosť tejto funkcie je v najhoršom prípade **O(E)** ale v priemere by mala byť podstatne rýchlejšia.

### Príklad vytvorenia grafu a vloženia vrcholov a hrán
```
Graph g = new Graph();
g.addVertex(4);
g.addVertex(1);
g.addVertex(3);
g.addEdge(1, 3, 2);
g.addEdge(4, 1, 5);
g.addEdge(3, 4);
```
Tento kód vytvorí graf v tvare trojuholníka, kde súčet hodnôt hrán je 8, pretože hrana z vrcholu 3 do 4 má váhu 1.
