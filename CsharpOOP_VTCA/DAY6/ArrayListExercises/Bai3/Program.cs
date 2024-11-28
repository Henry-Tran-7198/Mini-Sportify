using System;
using System.Collections;

class Exercise3 {
    static void Main() {
        ArrayList subjects = new ArrayList();
        subjects.AddRange(new string[] { "Toán", "Lý", "Hóa", "Sinh", "Văn" });
        
        Console.Write("Nhập chỉ số môn học (0 - {0}): ", subjects.Count - 1);
        string input = Console.ReadLine();
        
        try {
            int index = Convert.ToInt32(input);
            string subject = (string)subjects[index];
            Console.WriteLine("Môn học tại vị trí {0} là: {1}", index, subject);
        }
        catch (ArgumentOutOfRangeException) {
            Console.WriteLine("Chỉ số vượt quá giới hạn của danh sách.");
        }
        catch (FormatException) {
            Console.WriteLine("Vui lòng nhập 1 số nguyên hợp lệ.");
        }
    }
}
