using System;
using System.Collections;
namespace ArrayListDemo{
    class Program{
        static void Main(string[] args){
            ArrayList alist = new ArrayList();
            Console.WriteLine("Add some nums: ");
            alist.Add(45);
            alist.Add(32);
            alist.Add(15);
            alist.Add(99);
            alist.Add(1234);
            Console.WriteLine("Capicity: {0} ", alist.Capacity);
            Console.WriteLine("Count: {0} ", alist.Count);
            Console.Write("Content: ");
            foreach (int i in alist)
            {
                Console.Write(i+" ");
            }
            Console.WriteLine();
            Console.Write("Sorted content: ");
            alist.Sort();
            foreach (int i in alist){
                Console.Write(i+" ");
            }
        }
    }
}