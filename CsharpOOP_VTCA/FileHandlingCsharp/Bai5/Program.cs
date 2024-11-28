using System;
using System.IO;
class Program {
    static void Main(string[] args) {
        string filePath = @"C:\Users\LOQ\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\CsharpOOPVTCA\FileHandlingCsharp\Bai5\data.bin";
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create))) {
            writer.Write(13579);
            writer.Write(246810);
        }
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) {
            int num1 = reader.ReadInt32();
            int num2 = reader.ReadInt32();
            Console.WriteLine($"Read numbers: {num1}, {num2}");
        }
    }
}