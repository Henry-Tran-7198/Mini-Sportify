using System;
     using System.IO;

     class Program
     {
         static void Main()
         {
             string filePath = @"C:\Users\LOQ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\CsharpOOPVTCA\DAYIDK\Bai1\hello.txt";
             string content = "Hello, World!";
             
             File.WriteAllText(filePath, content);
             Console.WriteLine("File has been created and written.");
         }
     }