using System;
using System.IO;
class Program{
    static void Main(string[] args){
        string filePath = @"C:\Users\LOQ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\CsharpOOPVTCA\FileHandlingCsharp\Bai4\log.txt";
        string newContent = "This is a new log entry.";
        using (StreamWriter writer = new StreamWriter(filePath, false)){
            writer.WriteLine(newContent);
        }

        Console.WriteLine("New content has been appended to the file. ");
    }
}