using System;
using System.Collections;
class Exercise2{
    static void Main(){
        ArrayList numbers = new ArrayList();
        for (int i = 1; i <= 10; i++){
            numbers.Add(i);
        }
        numbers.Add(11);
        numbers.Remove(5);
        Console.WriteLine("Danh sách sau khi thêm 11 và xóa 5: ");
        foreach (int number in numbers){
            Console.WriteLine(number);  
        }
    }
}