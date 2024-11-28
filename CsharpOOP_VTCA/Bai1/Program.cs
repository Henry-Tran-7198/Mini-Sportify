// 1. Khai báo một mảng số nguyên và khởi tạo các giá trị từ 1 đến 10.
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// 2. Lấy phần tử thứ 3 trong mảng `numbers`.
int thirdElement = numbers[2];

// 3. Cập nhật giá trị phần tử thứ 5 trong mảng thành 20.
numbers[4] = 20;

// 4. In ra tất cả các phần tử trong mảng `numbers`.
for (int i = 0; i < numbers.Length; i++) {
    Console.WriteLine(numbers[i]);
}

// 5. Tính tổng tất cả các phần tử trong mảng `numbers`.
int sum = 0;
for (int i = 0; i < numbers.Length; i++) {
    sum += numbers[i];
}

// 6. Tìm giá trị lớn nhất trong mảng `numbers`.
int max = numbers[0];
for (int i = 1; i < numbers.Length; i++) {
    if (numbers[i] > max) {
        max = numbers[i];
    }
}

// 7. Viết chương trình đảo ngược chuỗi "Hello World".
string str = "Hello World";
char[] charArray = str.ToCharArray();
Array.Reverse(charArray);
string reversed = new string(charArray);

// 8. So sánh hai chuỗi "apple" và "banana".
int result = string.Compare("apple", "banana");

// 9. Kết hợp chuỗi "Hello" và "World".
string combined = "Hello" + " " + "World";

// 10. Kiểm tra chuỗi "test" có tồn tại trong chuỗi "This is a test".
bool contains = "This is a test".Contains("test");

// 11. Lấy chuỗi con từ vị trí thứ 5 trong chuỗi "Hello World".
string sub = "Hello World".Substring(5);

// 12. Tạo enum đại diện cho các ngày trong tuần.
enum Days { Sun, Mon, Tue, Wed, Thu, Fri, Sat }

// 13. In ra giá trị tương ứng của `Days.Mon`.
int monday = (int)Days.Mon;

// 14. Tìm các từ bắt đầu bằng ký tự 'S' trong chuỗi "A Thousand Splendid Suns".
Regex regex = new Regex(@"\bS\S*");
MatchCollection matches = regex.Matches("A Thousand Splendid Suns");

// 15. Tạo cấu trúc `Books` với các thuộc tính `title`, `author`, `book_id`.
struct Books {
    public string title;
    public string author;
    public int book_id;
}

// 16. Khởi tạo và in thông tin của một quyển sách.
Books book1;
book1.title = "C Programming";
book1.author = "Steve Jobs";
book1.book_id = 6495407;
Console.WriteLine(book1.title);

// 17. Thay thế tất cả ký tự 'a' thành 'o' trong chuỗi "banana".
string result = "banana".Replace('a', 'o');

// 18. Tách chuỗi "Hello World from C#" thành các từ riêng lẻ.
string[] words = "Hello World from C#".Split(' ');

// 19. Xóa khoảng trắng ở đầu và cuối chuỗi "   Hello World   ".
string trimmed = "   Hello World   ".Trim();

// 20. So sánh `Days.Mon` với `Days.Fri`.
if (Days.Mon < Days.Fri) {
    Console.WriteLine("Monday comes before Friday");
}

// 21. Khởi tạo mảng 2 chiều có kích thước 3x3.
int[,] matrix = new int[3, 3];

// 22. Truy cập phần tử ở dòng 2, cột 3 trong mảng `matrix`.
int value = matrix[1, 2];

// 23. In toàn bộ các phần tử trong mảng 2 chiều `matrix`.
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        Console.Write(matrix[i, j] + " ");
    }
    Console.WriteLine();
}

// 24. Tìm giá trị lớn nhất trong mảng 2 chiều `matrix`.
int max = matrix[0, 0];
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        if (matrix[i, j] > max) {
            max = matrix[i, j];
        }
    }
}

// 25. Kiểm tra xem chuỗi có phải là định dạng email hay không.
Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
bool isEmail = regex.IsMatch("example@example.com");

// 26. Tìm tất cả các số trong chuỗi "I have 2 apples and 3 oranges".
Regex regex = new Regex(@"\d+");
MatchCollection matches = regex.Matches("I have 2 apples and 3 oranges");
foreach (Match match in matches) {
    Console.WriteLine(match.Value);
}

// 27. Tạo chuỗi từ mảng ký tự `{'H', 'e', 'l', 'l', 'o'}`.
char[] letters = { 'H', 'e', 'l', 'l', 'o' };
string word = new string(letters);

// 28. Sử dụng `String.Format` để hiển thị "Today is 01/10/2024".
DateTime today = new DateTime(2024, 10, 1);
string formatted = String.Format("Today is {0:MM/dd/yyyy}", today);

// 29. In ra tên của ngày dựa trên giá trị của `Days`.
Days day = Days.Mon;
switch (day) {
    case Days.Mon:
        Console.WriteLine("Monday");
        break;
    case Days.Tue:
        Console.WriteLine("Tuesday");
        break;
    // Các trường hợp khác...
}

// 30. So sánh "hello" và "HELLO" không phân biệt hoa thường.
bool isEqual = string.Equals("hello", "HELLO", StringComparison.OrdinalIgnoreCase);

// 31. Kiểm tra xem chuỗi có phải là số điện thoại không (định dạng 10 chữ số).
Regex regex = new Regex(@"^\d{10}$");
bool isPhoneNumber = regex.IsMatch("0123456789");

// 32. Tạo mảng kiểu `Days` và gán giá trị cho từng phần tử.
Days[] weekDays = { Days.Mon, Days.Tue, Days.Wed, Days.Thu, Days.Fri };

// 33. Tạo mảng chuỗi động bằng cách sử dụng từ khóa `List`.
List<string> names = new List<string>();
names.Add("Alice");
names.Add("Bob");

// 34. Tính tổng các phần tử trong mảng 2 chiều `matrix`.
int sum = 0;
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        sum += matrix[i, j];
    }
}

// 35. Tạo struct `User` để lưu thông tin người dùng.
struct User {
    public string name;
    public int age;
}
User user1 = new User();
user1.name = "Alice";
user1.age = 25;