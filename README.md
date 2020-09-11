# Používateľská príručka

Programátorská príručka a ukážkové použitie knižnice sa nachádza vo Wiki tohto projektu

Výsledkom tohto projektu je grafová knižnica.
Je napísaná v jazyku C# a je aj určená na používanie v C#.

Jadrom knižnice sú triedy **Graph** (neorientovaný graf) a **OrientedGraph**,
ktoré obsahujú vlastnosti grafu a metódy na manipuláciu s hranami a vrcholmi.

Každý algoritmus má svoju staticku triedu, v ktorej sa nachádza jeho implementácia.

Spoločné metódy sú:
- AddVertex
- AddEdge
- RemoveEdge
- PrintGraph
- FloydWarshall
- FindShortestPath

Metódy triedy **Graph** sú:
- FindArticulations
- FindBridges
- GetSpanning

Metódy triedy **OrientedGraph** sú:
- FindSCCS
- TopologicalOrdering

## Ukážky a vysvetlenie používania
> Značenie : V = počet vrcholov, E = počet hrán

V prípade oboch tried, na pridanie vrcholu do grafu slúži metóda **AddVertex()**.
Metóda vracia hodnotu typu int, kde daná hodnota reprezentuje index vrcholu v grafe.
Časová zložitosť tejto operácie je **O(1)**.

Na pridávanie hrán je určená metóda **AddEdge**, ktorá má viacero interpretácii.
Pre neorientovaný graf:
- **AddEdge(int v1, int v2, long weight)**
- **AddEdge(int v1, int v2)**

Pre orientovaný graf:
- **AddEdge(int source, int destination, int weight)**
- **AddEdge(int source, int destination)**

V oboch prípadoch ak nieje určený parameter **weight**, váha je nastavená na 1.
V prípade, že všetky hrany majú váhu 1, jedná sa o neohodnotený graf.
Táto metóda pridá v triede **Graph** hranu obojstranne obom vrcholom. V prípade
orientovaného grafu, je priradená iba vrcholu označeným **source**(prvý parameter).

Opačnou metódou pre neorientovaný graf je **RemoveEdge(int v1, int v2)**, ktorá zmaže hranu
spájajúcu vrchol **v1** a **v2** a pre orientovaný graf **RemoveEdge(long source, long destination)**,
ktorá zmaže hranu vedúcu z vrcholu **source** do vrcholu **destination**.
Časová zložitosť tejto funkcie je v najhoršom prípade **O(E)** ale v priemere by mala byť podstatne rýchlejšia.

### Príklad vytvorenia grafu a vloženia vrcholov a hrán
```c#
Graph myGraph = new Graph();
for(int i = 0; i < 5; i++)
{
    myGraph.AddVertex();
}
myGraph.AddEdge(1, 3, 2);
myGraph.AddEdge(4, 1, 5);
myGraph.AddEdge(3, 4);
```
Tento kód vytvorí graf v tvare trojuholníka, kde súčet hodnôt hrán je 8, pretože hrana z vrcholu 3 do 4 má váhu 1.

Ďalšou metódou je **PrintGraph()**, ktorá vypíše pre každý vrchol susedov, ku ktorým od neho vedie hrana.

### Vlastnosti

**Edges** - táto vlastnosť obsahuje list hrán.
```c#
List<Edge> edges = myGraph.Edges;
```

**int NumberOfVertices** - obsahuje počet vrcholov grafu.

**int NumberOfComponents** - obsahuje počet komponent neorientovaného grafu.

**int [] ComponentNumbers** - na i-tom indexe obsahuje číslo komponenty, do ktorej patrí i-ty vrchol

Na reprezentáciu hrán je využitá trieda **Edge**, ktorá má vlastnosti:
```c#
public int source;
public int destination;
public long weight;
```
Pomocou týchto vlastností môže uživateľ zisťovať informácie o hranách pri výstupe metód ako **SpanningTree.GetSpanning()** vysvetlenej nižšie.


### Spoločné algoritmy

#### Floyd Warshall (nájdenie najkratších ciest medzi všetkými vrcholmi)
Metóda **AllShortestPaths(ref SharedGraph g)** sa nachádza v statickej triede **FloydWarshall** a platí pre oba typy grafov. Jej výstupom je struct FloydInfo,
ktorý obsahuje vzdialenosti medzi všetkymi dvojicami vrcholov. Na zistenie vzdialenosti medzi ľubovoľným párom slúži metóda **GetDistance(int source, int destination)**, ktorá vráti dĺžku cesty z vrcholu **source** do vrcholu **destination**.
Použitie:
```c#
FloydInfo f = FloydWarshall.AllShortestPaths(Graph g);
long distanceFromAtoB = f.GetDistance(a, b);
```
Tento kód dostane vzdialenosť z vrcholu **a** do vrcholu **b** do premennej **distanceFromAtoB**.

***Pozor, ak medzi dvomi vrcholmi nevedie žiadna cesta, výsledné pole obsahuje na danom indexe hodnotu long.MaxValue***.
Časová zložitosť tohto algoritmu je **O(V^3)**.

#### Dijkstra
Druhou spoločnou metódou je **FindShortestPath(SharedGraph g, long source, long destination)**, ktorá nájde najkratšiu cestu z vrcholu **source** do
vrcholu **destination**. Návratovou hodnotou tejto metódy je inštancia triedy **DijsktraInfo**, ktorá má nasledovné vlasnosti:
```c#
List<int> shortestPath; // zoznam ID vrcholov, ktoré sú zoradené a ležia na najkratšej ceste vedúcej od source k destination
long cost; // obsahuje cenu tejto najkratšej cesty
```
V tejto metóde je použitý Dijsktrov algoritmus, ktorý nefunguje na záporných hranách a preto si je treba
dať pozor. V prípade kedy neexistuje najkratšia cesta, list **shortestPath** bude prádzny.
Časová zložitosť tejto verzie Dijkstrovho algoritmu je **O(ElogV)**.
```c#
DijkstraInfo dInfo = Dijsktra.FindShortestPath(ref g, a, b);
Console.WriteLine("Cena najkratsej cesty z vrcholu {0} do {1} je {2}.", a, b, dInfo.cost);
Console.WriteLine("A vedie cez vrcholy:");
foreach(long i in dInfo.shortestPath)
{
    Console.Write("i ");
}
```
Tento kód nájde najkratšiu cestu medzi dvomi vrcholmi a vypíše potrebné informácie o tejto ceste.
***Táto knižnica neobsahuje algoritmus BFS pre neohodnotené grafy. Avšak použitá halda v Dijkstrovom algoritme je
napísaná tak, aby metóda findShortestPath našla najkratšiu cestu v neohodnotenom grafe v čase O(E+V).***


### Algoritmy pre neorientované grafy

#### Artikulácie
Na hľadanie artikulácii v grafe slúži metóda **FindArticulations(Graph g)**, ktorá sa nachádza v statickej triede BridgesArticulations. 
Jej návratová hodnota je **List\<int\>**, ktorá obsahuje ID všetkých vrcholov, ktoré sú označené ako artikulácie v grafe. 
Artikulácia je vrchol, po ktorého odstránení sa graf rozdelí na viacero komponentov.
```c#
List<int> articulationsVertices = BridgesArticulations.FindArticulations(g);
Console.WriteLine("Artikulacie su vrcholy:");
foreach(long vertexID in articulationVertices)
{
    Console.Write(vertexID + " ");
}
```
V prípade, kedy graf neobsahuje žiadne artikulácie, metóda vráti prázdny list. Časová zložitosť je **O(V+E)**.

#### Mosty
Most je hranový ekvivalent artikulácie. Ak zmažeme most, v grafe nám pribudne ďalšia komponenta. Metóda
na hľadanie mostov je **FindBridges(Graph g)**, ktorá sa taktiež nachádza v statickej triede BridgesArticulations
a jej výstupom je **List\<Edge\>**, čiže zoznam hrán, ktoré sú mostami.
```c#
List<Edge> listOfEdges = BridgesArticulations.FindBridges(ref g);
Console.WriteLine("Mosty su:");
foreach(Edge e in listOfEdges)
{
    Console.WriteLine("Hrana veduca z {0} do {1}", e.source, e.destination);
}
```
Ak graf neobsahuje žiadne mosty, vráti prázdny list.
Časová zložitosť tejto metódy je taktiež **O(V+E)**.

#### Hľadanie najlacnejšej kostry
Pre nájdenie najlacnejšej kostry grafu slúži metóda **GetSpanning()**, ktorá sa nachádza v statickej triede
SpanningTree. Jej návratovou hodnotou je inštancia triedy **SpanningTreeInfo**, ktorá má tieto vlasnosti:
```c#
Graph MST; // graf, ktorý je najlacnejšou kostrou vstupného grafu
long cost; // cena najlacnejšej kostry
```
Kostra grafu je množina hrán, ktorá spája všetky vrcholy do jednej komponenty a neobsahuje kružnicu.
Najlacnejšia kostra je taká kostra, ktorej súčet všetkých cien hrán je najmenšia.
```c#
SpanningTreeInfo MSTInfo = SpanningTree.GetSpanning(g);
Console.WriteLine("Cena najlacnejsej kostry je " + MSTInfo.cost);
Console.WriteLine("Hrany obsiahnuté v minimálnej kostre grafu g sú:");
Graph MSTofG = MSTInfo.MST;
MSTofG.PrintGraph();
```
Na hľadanie najacnejšej kostry je použitý Kruskalov algoritmus, ktorý využíva Union-Find štruktúru.
Časová zložitosť je **O(ElogE)**.


### Algoritmy pre orientované grafy

#### Hľadanie silno súvislých komponent
Pre tento problém je určená metóda **FindSCCS(OrientedGraph og)** nachádzajúca sa v statickej triede Scc. 
**FindSCCS(OrientedGraph og)** vracia pole, kde hodnota na i-tom indexe reprezentuje číslo komponenty, v ktorej sa daný vrhcol nachádza.
Komponenta v orientovanom grafe je taký podgraf, v ktorom medzi každou dvojicou vrcholov existuje orientovaná cesta. 
Na hľadanie týchto komponent je použitý Kosaraju algoritmus.
```c#
int [] components = Scc.FindSCCS(og);
for(int i = 0; i < components.Length; i++)
{
    Console.WriteLine("Vrchol " + i " lezi v komponente + components[i]);
}
```
Časová zložitosť tohto algoritmu je **O(E+V)**.

#### Hľadanie topologického usporiadania
Topologické usporiadanie vrcholov v orientovanom grafe je také usporiadanie, v ktorom ak
je vrchol **v** v danom usporiadaní pred vrcholom **u**, tak potom nemôže existovať orientovaná
hrana z vrcholu **u** do **v**. Každý acyklický orientovaný graf má takéto usporiadanie.
Grafy ktoré majú kružnicu, nemajú topologické usporiadanie. Na hľadanie tohto usporiadania
je určená metóda **TopologicalOrdering()**, ktorá sa nachádza v statickej triede Toposort
a vracia zoznam vrcholov **List\<int\>** 
zoradený podľa vyššie spomenutej vlastnosti.
```c#
List<int> ordering = Toposort.TopologicalOrdering(og);
Console.WriteLine("Topologicke usporiadanie je nasledovne:");
foreach(long vertexID in ordering)
{
    Console.Write(vertexID + " ");
}
```
***Pozor, ak topologické usporiadanie neexistuje, výsledný zoznam bude prázdny.*** Časová
zložitosť tejto metódy je **O(V+E)**.
