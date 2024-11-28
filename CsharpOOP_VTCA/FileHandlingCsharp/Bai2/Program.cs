using System;
using System.IO;

class Program {
    static void Main() {
        string filePath = @"C:\Users\LOQ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\CsharpOOPVTCA\FileHandlingCsharp\Bai2\input.txt";
        using (StreamReader reader = new StreamReader(filePath)){
            string line;
            while ((line = reader.ReadLine()) != null){
                Console.WriteLine(line);
            }    
        }
    }
}