using Data_Structure_Examples.DataStructures;
using System;
using System.Linq;

LinkedList<int> myList = new LinkedList<int>();

myList.Add(0);
myList.Add(1);
myList.Add(2);
myList.Add(3);

foreach (var element in myList) {
    Console.WriteLine(element);
}

