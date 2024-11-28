using System;
using System.IO;
class Program{
    static void Main(){
        string filePath = @"C:\Users\LOQ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\CsharpOOPVTCA\FileHandlingCsharp\Bai3\check.txt";
        if (File.Exists(filePath)){
            Console.WriteLine("The file exists at: " + filePath);
        }
        else{
            Console.WriteLine("The file doesn't exist");
        }
    }
}