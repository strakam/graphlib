# Používateľská príručka

Výsledkom tohto projektu je grafová knižnica.
Je napísaná v jazyku C# a je aj určená na používanie v C#.

Ukážka:
Máme csv súbor, ktorý pre každý program obsahuje zoznam ďalších programov, ktoré musia byť nainštalované pred ním (dependencies).
Chceme prísť na to, v akom poradí máme tieto programy inštalovať tak, aby vo chvíli inštalovania daného programu boli všetky
jeho dependencies už nainštalované. Pomocou triedy **OrientedGraph** a jej metód vytvoríme graf, reprezentujúci vzťahy medzi programami.
Vizuálne vyzerá takto:

![dependencypicture](https://user-images.githubusercontent.com/19777512/92326386-fb753080-f051-11ea-9d75-273ad4c33051.png)

Využijeme metódu TopologicalOrdering, ktorá nájde takéto usporiadanie a vypíšeme ho.
Výstup je:
> Install in following order:
> xorg-xinit --> zsh --> pandoc --> wget --> snapd --> which --> systemd --> python3 --> tar --> mono --> sxhkd --> unzip --> graphlib


Jadrom knižnice sú triedy **Graph** (neorientovaný graf) a **OrientedGraph**,
ktoré obsahujú vlastnosti grafu a metódy na manipuláciu s hranami a vrcholmi.

Každý algoritmus má svoju staticku triedu, v ktorej sa nachádza jeho implementácia.

Spoločné metódy sú:
- AddVertex
- RemoveVertex
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

V prípade oboch tried, na pridanie vrcholu do grafu slúži metóda **AddVertex(long vertexID)**.
Má len jeden parameter typu **long**, ktorý je názvom vrcholu.
Využitie pamäte nezáleží od hodnoty identifikátoru najväčšieho vrcholu
a preto je bezškodné využitie veľkých longovych hodnôt.
Časová zložitosť tejto operácie je **O(1)**.

Opačnou operáciou je metóda **RemoveVertex(long vertexID)**,
Ktorá zmaže vrchol určený parametrom a vymaže všetky hrany ktoré sú spojené s týmto vrcholom.
Keďže táto metóda musí skontrolovať všetky hrany aby zistila, či sú s daným vrcholom spojené,
časová zložitosť je **O(E)**.

Na pridávanie hrán je určená metóda **AddEdge**, ktorá má viacero interpretácii.
Pre neorientovaný graf:
- **AddEdge(long v1, long v2, long weight)**
- **AddEdge(long v1, long v2)**

Pre orientovaný graf:
- **AddEdge(long source, long destination, long weight)**
- **AddEdge(long source, long destination)**

V oboch prípadoch ak nieje určený parameter **weight**, váha je nastavená na 1.
V prípade, že všetky hrany majú váhu 1, jedná sa o neohodnotený graf.
Táto metóda pridá v triede **Graph** hranu obojstranne obom vrcholom. V prípade
orientovaného grafu, je priradená iba vrcholu označeným **source**(prvý parameter).

Opačnou metódou pre neorientovaný graf je **RemoveEdge(long v1, long v2)**, ktorá zmaže hranu
spájajúcu vrchol **v1** a **v2** a pre orientovaný graf **RemoveEdge(long source, long destination)**,
ktorá zmaže hranu vedúcu z vrcholu **source** do vrcholu **destination**.
Časová zložitosť tejto funkcie je v najhoršom prípade **O(E)** ale v priemere by mala byť podstatne rýchlejšia.

### Príklad vytvorenia grafu a vloženia vrcholov a hrán
```c#
Graph myGraph = new Graph();
for(int i = 1; i < 5; i++)
{
    myGraph.AddVertex(i);
}
myGraph.AddEdge(1, 3, 2);
myGraph.AddEdge(4, 1, 5);
myGraph.AddEdge(3, 4);
```
Tento kód vytvorí graf v tvare trojuholníka, kde súčet hodnôt hrán je 8, pretože hrana z vrcholu 3 do 4 má váhu 1.

Ďalšou metódou je **PrintGraph()**, ktorá vypíše pre každý vrchol susedov, ku ktorým od neho vedie hrana.

Na reprezentáciu hrán je využitá trieda **Edge**, ktorá má vlastnosti:
```c#
public long source;
public long destination;
public long weight;
```
Pomocou týchto vlastností môže uživateľ zisťovať informácie o hranách pri výstupe metód ako **SpanningTree.GetSpanning()** vysvetlenej nižšie.


### Spoločné algoritmy

#### Floyd Warshall (nájdenie najkratších ciest medzi všetkými vrcholmi)
Metóda **AllShortestPaths(ref SharedGraph g)** sa nachádza v statickej triede **FloydWarshall** a platí pre oba typy grafov. Jej výstupom je struct FloydInfo,
ktorý obsahuje vzdialenosti medzi všetkymi dvojicami vrcholov. Na zistenie vzdialenosti medzi ľubovoľným párom slúži metóda **GetDistance(long source, long destination)**, ktorá vráti dĺžku cesty z vrcholu **source** do vrcholu **destination**.
Použitie:
```c#
FloydInfo f = FloydWarshall.AllShortestPaths(ref Graph g);
long distanceFromAtoB = f.GetDistance(a, b);
```
Tento kód dostane vzdialenosť z vrcholu **a** do vrcholu **b** do premennej **distanceFromAtoB**.

***Pozor, ak medzi dvomi vrcholmi nevedie žiadna cesta, výsledné pole obsahuje na danom indexe hodnotu long.MaxValue***.
Časová zložitosť tohto algoritmu je **O(V^3)**.

#### Dijkstra
Druhou spoločnou metódou je **FindShortestPath(ref SharedGraph g, long source, long destination)**, ktorá nájde najkratšiu cestu z vrcholu **source** do
vrcholu **destination**. Návratovou hodnotou tejto metódy je inštancia triedy **DijsktraInfo**, ktorá má nasledovné vlasnosti:
```c#
List<long> shortestPath; // zoznam ID vrcholov, ktoré sú zoradené a ležia na najkratšej ceste vedúcej od source k destination
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
Na hľadanie artikulácii v grafe slúži metóda **FindArticulations(ref Graph g)**, ktorá sa nachádza v statickej triede BridgesArticulations. 
Jej návratová hodnota je **List\<long\>**, ktorá obsahuje ID všetkých vrcholov, ktoré sú označené ako artikulácie v grafe. 
Artikulácia je vrchol, po ktorého odstránení sa graf rozdelí na viacero komponentov.
```c#
List<long> articulationsVertices = BridgesArticulations.FindArticulations(ref g);
Console.WriteLine("Artikulacie su vrcholy:");
foreach(long vertexID in articulationVertices)
{
    Console.Write(vertexID + " ");
}
```
V prípade, kedy graf neobsahuje žiadne artikulácie, metóda vráti prázdny list. Časová zložitosť je **O(V+E)**.

#### Mosty
Most je hranový ekvivalent artikulácie. Ak zmažeme most, v grafe nám pribudne ďalšia komponenta. Metóda
na hľadanie mostov je **FindBridges(ref Graph g)**, ktorá sa taktiež nachádza v statickej triede BridgesArticulations
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
SpanningTreeInfo MSTInfo = SpanningTree.GetSpanning(ref g);
Console.WriteLine("Cena najlacnejsej kostry je " + MSTInfo.cost);
Console.WriteLine("Hrany obsiahnuté v minimálnej kostre grafu g sú:");
Graph MSTofG = MSTInfo.MST;
MSTofG.PrintGraph();
```
Na hľadanie najacnejšej kostry je použitý Kruskalov algoritmus, ktorý využíva Union-Find štruktúru.
Časová zložitosť je **O(ElogE)**.


### Algoritmy pre orientované grafy

#### Hľadanie silno súvislých komponent
Pre tento problém je určená metóda **FindSCCS(ref OrientedGraph og)** nachádzajúca sa v statickej triede Scc. 
**FindSCCS(ref OrientedGraph og)** vracia list grafov, kde každý graf je jedna silno súvisla komponenta vstupného grafu.
Komponenta v orientovanom grafe je taký podgraf, v ktorom medzi každou dvojicou vrcholov existuje orientovaná cesta. 
Na hľadanie týchto komponent je použitý Kosaraju algoritmus.
```c#
List<OrientedGraph> components = Scc.FindSCCS(ref og);
for(int i = 1; i <= components.Count; i++)
{
    Console.WriteLine("Komponenta cislo {0} je:", i);
    components[i].PrintGraph();
    Console.WriteLine();
}
```
Časová zložitosť tohto algoritmu je **O(E+V)**.

#### Hľadanie topologického usporiadania
Topologické usporiadanie vrcholov v orientovanom grafe je také usporiadanie, v ktorom ak
je vrchol **v** v danom usporiadaní pred vrcholom **u**, tak potom nemôže existovať orientovaná
hrana z vrcholu **u** do **v**. Každý acyklický orientovaný graf má takéto usporiadanie.
Grafy ktoré majú kružnicu, nemajú topologické usporiadanie. Na hľadanie tohto usporiadania
je určená metóda **TopologicalOrdering()**, ktorá sa nachádza v statickej triede Toposort
a vracia zoznam vrcholov **List\<long\>** 
zoradený podľa vyššie spomenutej vlastnosti.
```c#
List<long> ordering = Toposort.TopologicalOrdering(ref og);
Console.WriteLine("Topologicke usporiadanie je nasledovne:");
foreach(long vertexID in ordering)
{
    Console.Write(vertexID + " ");
}
```
***Pozor, ak topologické usporiadanie neexistuje, výsledný zoznam bude prázdny.*** Časová
zložitosť tejto metódy je **O(V+E)**.
