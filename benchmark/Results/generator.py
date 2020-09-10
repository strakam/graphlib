#!/bin/env python3
# pylint: disable=C0103 # UPPER case
import matplotlib.pyplot as plt
import sys

files = [sys.argv[i] for i in range(1, len(sys.argv))]
title = input("Enter graph title: ")
outputFile = input("Enter name of output file: ")
label1 = input("Enter label for first file graph: ")
label2 = input("Enter label for second file graph: ")
labels = [label1, label2]
fig, ax = plt.subplots()
it = 0
t = []
for f in files:
    inputFile = open(f, "r")
    data = {}
    for line in inputFile:
        d = line.split(',')
        data[d[0]] = int(d[1])
    names = list(data.keys())
    values = list(data.values())
    ax.plot(names, values, label=labels[it])
    t.append(data[max(data, key=data.get)])
    t.append(data[min(data, key=data.get)])
    it += 1

ax.set_xlabel('Edges')
ax.set_ylabel('Milliseconds')
ax.set_title(title)
plt.yticks(t, t)
plt.xticks([names[0], names[-1]])
plt.legend()
plt.savefig('Graphs/' + outputFile + '.png')
plt.show()
