using System;
using System.Collections;
class Program
{
    static void DisplayStudents(Hashtable students){
        foreach (DictionaryEntry entry in students){
            Console.WriteLine($"ID: {entry.Key}, Name: {entry.Value}");
        }
    }
    static void Main(string[] args){
        Hashtable students = new Hashtable();
        students.Add("001", "John Doe");
        students.Add("002", "Jane Smith");
        DisplayStudents(students);
        students.Remove("001");
        DisplayStudents(students);  
    }
}
