# How to use Graphlib

Už z názvu je jasné, že výsledkom tohto projektu je grafová knižnica.
Je napísaná v jazyku C# a je aj určená na používanie v C#.

Jadrom knižnice sú triedy **Graph** (neorientovaný graf) a **OrientedGraph**. 
V nich sa nachádzajú všetky metódy knižnice.

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
```c#
Graph g = new Graph();
g.addVertex(4);
g.addVertex(1);
g.addVertex(3);
g.addEdge(1, 3, 2);
g.addEdge(4, 1, 5);
g.addEdge(3, 4);
```
Tento kód vytvorí graf v tvare trojuholníka, kde súčet hodnôt hrán je 8, pretože hrana z vrcholu 3 do 4 má váhu 1.

Ďalšou metódou je **printGraph()**, ktorá vypíše pre každý vrchol susedov, ku ktorým od neho vedie hrana.

Na reprezentáciu hrán je využitá trieda **Edge**, ktorá má vlastnosti:
```c#
public long source;
public long destination;
public long weight;
```
Pomocou týchto vlastností môže uživateľ zisťovať informácie o hranách pri výstupe metód ako **getSpanning()** vysvetlenej nižšie.


### Spoločné algoritmy

#### Floyd Warshall (nájdenie najkratších ciest medzi všetkými vrcholmi)
Metóda **floydWarshall()** platí pre oba typy grafov. jej výstupom je dvojrozmerné pole s rozmermi **VxV**, kde na pozícii **[i,j]**
je vzdialenosť vrcholu s indexom **j** od vrcholu s indexom **i**. Pozor, pri orientovaných grafoch sa **[i,j]** a **[j,i]** môžu líšiť.
Keďže index vrcholu **v** je odlišný od samotnej hodnoty **v**, na zistenie vzdialenosti v tomto 2D poli použijeme metódu **vIndex(v)**.
Použitie:
```c#
long answer[,] = g.floydWarshall();
long distance = answer[g.vIndex(a), g.vIndex(b)];
```
Tento kód dostane vzdialenosť z vrcholu **a** do vrcholu **b** do premennej **distance**.

***Pozor, ak medzi dvomi vrcholmi nevedie žiadna cesta, výsledné pole obsahuje na danom indexe hodnotu long.MaxValue***.
Časová zložitosť tohto algoritmu je **O(V^3)**.

#### Dijkstra
Druhou spoločnou metódou je **findShortestPath(long source, long destination)**, ktorá nájde najkratšiu cestu z vrcholu **source** do
vrcholu **destination**. Návratovou hodnotou tejto metódy je inštancia triedy **Dijsktra**, ktorá má nasledovné vlasnosti:
```c#
List<long> shortestPath; // zoznam ID vrcholov, ktoré sú zoradené a ležia na najkratšej ceste vedúcej od source k destination
long cost; // obsahuje cenu tejto najkratšej cesty
```
Ako už z názvu triedy vyplýva, v tejto metóde je použitý Dijsktrov algoritmus, ktorý nefunguje na záporných hranách a preto si je treba
dať pozor. Časová zložitosť tejto verzie Dijkstrovho algoritmu je **O(ElogV)**.
```c#
Dijkstra d = g.findShortestPath(a, b);
Console.WriteLine("Cena najkratsej cesty z vrcholu {0} do {1} je {2}.", a, b, d.cost);
Console.WriteLine("A vedie cez vrcholy:");
foreach(long i in d.shortestPath)
{
    Console.Write("i ");
}
```
Tento kód nájde najkratšiu cestu medzi dvomi vrcholmi a vypíše potrebné informácie o tejto ceste.
***Táto knižnica neobsahuje algoritmus BFS pre neohodnotené grafy. Avšak použitá halda v Dijkstrovom algoritme je
napísaná tak, aby metóda findShortestPath našla najkratšiu cestu v neohodnotenom grafe v čase O(E+V).***


### Algoritmy pre neorientované grafy

#### Artikulácie
Na hľadanie artikulácii v grafe slúži metóda **findArticulations()**. Jej návratová hodnota je **List\<long\>**,
ktorá obsahuje ID všetkých vrcholov, ktoré sú označené ako artikulácie v grafe. Artikulácia je vrchol, po ktorého
odstránení sa graf rozdelí na viacero komponentov.
```c#
List<long> ans = g.findArticulations();
Console.WriteLine("Artikulacie su vrcholy:");
foreach(long i in ans)
{
    Console.Write(i);
}
```
Časová zložitosť je **O(V+E)**.

#### Mosty
Most je hranový ekvivalent artikulácie. Ak zmažeme most, v grafe nám pribudne ďalšia komponenta. Metóda
na hľadanie mostov je **findBridges()** a jej výstupom je **List\<Edge\>**, čiže zoznam hrán, ktoré sú mostami.
```c#
List<Edge> le = g.findBridges();
Console.WriteLine("Mosty su:");
foreach(Edge e in le)
{
    Console.WriteLine("Hrana veduca z {0} do {1}", e.source, e.destination);
}
```
Časová zložitosť tejto metódy je taktiež **O(V+E)**.

#### Hľadanie najlacnejšej kostry
Pre nájdenie najlacnejšej kostry grafu slúži metóda **getSpanning()**. Jej návratovou hodnotou je
inštancia triedy **SpanningTree**, ktorá má tieto vlasnosti:
```c#
List<edge> edges; // zoznam hrán, ktoré tvoria najlacnejšiu kostru
long cost; // cena najlacnejšej kostry
```
Kostra grafu je množina hrán, ktorá spája všetky vrcholy do jednej komponenty a neobsahuje kružnicu.
Najlacnejšia kostra je taká kostra, ktorej súčet všetkých cien hrán je najmenšia.
```c#
SpanningTree st = g.getSpanning();
Console.WriteLine("Cena najlacnejsej kostry je " + st.cost);
Console.WriteLine("A jej hrany su:");
foreach(Edge e in st.edges)
{
    Console.WriteLine("Hrana veduca z {0} do {1}", e.source, e.destination);
}
```
Na hľadanie najacnejšej kostry je použitý Kruskalov algoritmus, ktorý využíva Union-Find štruktúru.
Časová zložitosť je **O(ElogE)**.


### Algoritmy pre orientované grafy

#### Hľadanie silno súvislých komponent
Pre tento problém je určená metóda **findSCCS()**, ktorá vracia 2D list, kde každý list obsahuje
ID vrcholov, ktoré ležia v jednej komponente. Komponenta v orientovanom grafe je množina vrcholov,
v ktorej medzi každou dvojicou vrcholov existuje orientovaná cesta. Na hľadanie týchto komponent je
použitý Kosaraju algoritmus.
```c#
List<List<long>> components = g.findSCCS();
for(int i = 1; i <= components.Count; i++)
{
    Console.WriteLine("Vrcholy v komponente {0} su:", i);
    foreach(long v in l)
    {
        Console.Write(v + " ");
    }
    Console.WriteLine();
}
```
Časová zložitosť tohto algoritmu je **O(E+V)**.

#### Hľadanie topologického usporiadania
Topologické usporiadanie vrcholov v orientovanom grafe je také usporiadanie, v ktorom ak
je vrchol **v** v danom usporiadaní pred vrcholom **u**, tak potom nemôže existovať orientovaná
hrana z vrcholu **u** do **v**. Každý acyklický orientovaný graf má takéto usporiadanie.
Grafy ktoré majú kružnicu, nemajú topologické usporiadanie. Na hľadanie tohto usporiadania
je určená metóda **topologicalOrdering()**, ktorá vracia zoznam vrcholov **List\<long\>** 
zoradený podľa vyššie spomenutej vlastnosti.
```c#
List<long> ordering = g.topologicalOrdering();
Console.WriteLine("Topologicke usporiadanie je nasledovne:");
foreach(long v in ordering)
{
    Console.Write(v + " ");
}
```
***Pozor, ak topologické usporiadanie neexistuje, výsledný zoznam bude prázdny.*** Časová
zložitosť tejto metódy je **O(V+E)**.
