using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
    // Lớp trừu tượng Person
    public abstract class Person
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }

        protected Person(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        // Phương thức trừu tượng
        public abstract void HienThiThongTin();
    }

    // Lớp Student kế thừa từ Person
    public class Student : Person
    {
        public string MaSinhVien { get; set; }
        public string Lop { get; set; }
        public double DiemTrungBinh { get; set; }

        public Student(string maSinhVien, string hoTen, DateTime ngaySinh, string lop, double diemTrungBinh)
            : base(hoTen, ngaySinh)
        {
            MaSinhVien = maSinhVien;
            Lop = lop;
            DiemTrungBinh = diemTrungBinh;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine($"Mã SV: {MaSinhVien}, Họ Tên: {HoTen}, Ngày Sinh: {NgaySinh.ToShortDateString()}, Lớp: {Lop}, Điểm TB: {DiemTrungBinh}");
        }
    }

    // Lớp Lecturer kế thừa từ Person
    public class Lecturer : Person
    {
        public string MaGiangVien { get; set; }
        public string Khoa { get; set; }
        public string ChuyenNganh { get; set; }

        public Lecturer(string maGiangVien, string hoTen, DateTime ngaySinh, string khoa, string chuyenNganh)
            : base(hoTen, ngaySinh)
        {
            MaGiangVien = maGiangVien;
            Khoa = khoa;
            ChuyenNganh = chuyenNganh;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine($"Mã GV: {MaGiangVien}, Họ Tên: {HoTen}, Ngày Sinh: {NgaySinh.ToShortDateString()}, Khoa: {Khoa}, Chuyên Ngành: {ChuyenNganh}");
        }
    }

    // Lớp Subject quản lý thông tin môn học
    public class Subject
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }

        public Subject(string maMonHoc, string tenMonHoc)
        {
            MaMonHoc = maMonHoc;
            TenMonHoc = tenMonHoc;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine($"Mã MH: {MaMonHoc}, Tên MH: {TenMonHoc}");
        }
    }

    // Lớp Grade quản lý điểm số của sinh viên
    public class Grade
    {
        public string MaSinhVien { get; set; }
        public string MaMonHoc { get; set; }
        public double Diem { get; set; }

        public Grade(string maSinhVien, string maMonHoc, double diem)
        {
            MaSinhVien = maSinhVien;
            MaMonHoc = maMonHoc;
            Diem = diem;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine($"Mã SV: {MaSinhVien}, Mã MH: {MaMonHoc}, Điểm: {Diem}");
        }
    }

    // Lớp Class quản lý thông tin lớp học
    public class Class
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public Lecturer GiangVienPhuTrach { get; set; }
        public List<Student> DanhSachSinhVien { get; set; }

        public Class(string maLop, string tenLop, Lecturer giangVienPhuTrach)
        {
            MaLop = maLop;
            TenLop = tenLop;
            GiangVienPhuTrach = giangVienPhuTrach;
            DanhSachSinhVien = new List<Student>();
        }

        public void HienThiThongTin()
        {
            Console.WriteLine($"Mã Lớp: {MaLop}, Tên Lớp: {TenLop}, Giảng Viên Phụ Trách: {GiangVienPhuTrach.HoTen}");
            Console.WriteLine("Danh Sách Sinh Viên:");
            foreach (var sv in DanhSachSinhVien)
            {
                Console.WriteLine($"\t{sv.MaSinhVien} - {sv.HoTen}");
            }
        }
    }

    // Lớp quản lý sinh viên
    public class StudentManager
    {
        private List<Student> danhSachSinhVien = new List<Student>();

        // Thêm sinh viên
        public void ThemSinhVien(Student sv)
        {
            if (TimSinhVien(sv.MaSinhVien) != null)
            {
                Console.WriteLine("Mã sinh viên đã tồn tại.");
                return;
            }
            danhSachSinhVien.Add(sv);
            Console.WriteLine("Thêm sinh viên thành công.");
        }

        // Sửa thông tin sinh viên
        public void SuaSinhVien(string maSinhVien, string hoTen, DateTime ngaySinh, string lop, double diemTrungBinh)
        {
            Student sv = TimSinhVien(maSinhVien);
            if (sv != null)
            {
                sv.HoTen = hoTen;
                sv.NgaySinh = ngaySinh;
                sv.Lop = lop;
                sv.DiemTrungBinh = diemTrungBinh;
                Console.WriteLine("Sửa thông tin sinh viên thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }
        }

        // Xoá sinh viên
        public void XoaSinhVien(string maSinhVien)
        {
            Student sv = TimSinhVien(maSinhVien);
            if (sv != null)
            {
                danhSachSinhVien.Remove(sv);
                Console.WriteLine("Xoá sinh viên thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }
        }

        // Hiển thị danh sách sinh viên
        public void HienThiDanhSach()
        {
            foreach (var sv in danhSachSinhVien)
            {
                sv.HienThiThongTin();
            }
        }

        // Tìm kiếm sinh viên theo mã
        public Student TimSinhVien(string maSinhVien)
        {
            return danhSachSinhVien.FirstOrDefault(sv => sv.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase));
        }

        // Tìm kiếm sinh viên theo tên
        public List<Student> TimSinhVienTheoTen(string ten)
        {
            return danhSachSinhVien.Where(sv => sv.HoTen.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Lấy danh sách sinh viên
        public List<Student> LayDanhSachSinhVien()
        {
            return danhSachSinhVien;
        }
    }

    // Lớp quản lý giảng viên
    public class LecturerManager
    {
        private List<Lecturer> danhSachGiangVien = new List<Lecturer>();

        // Thêm giảng viên
        public void ThemGiangVien(Lecturer gv)
        {
            if (TimGiangVien(gv.MaGiangVien) != null)
            {
                Console.WriteLine("Mã giảng viên đã tồn tại.");
                return;
            }
            danhSachGiangVien.Add(gv);
            Console.WriteLine("Thêm giảng viên thành công.");
        }

        // Sửa thông tin giảng viên
        public void SuaGiangVien(string maGiangVien, string hoTen, DateTime ngaySinh, string khoa, string chuyenNganh)
        {
            Lecturer gv = TimGiangVien(maGiangVien);
            if (gv != null)
            {
                gv.HoTen = hoTen;
                gv.NgaySinh = ngaySinh;
                gv.Khoa = khoa;
                gv.ChuyenNganh = chuyenNganh;
                Console.WriteLine("Sửa thông tin giảng viên thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy giảng viên.");
            }
        }

        // Xoá giảng viên
        public void XoaGiangVien(string maGiangVien)
        {
            Lecturer gv = TimGiangVien(maGiangVien);
            if (gv != null)
            {
                danhSachGiangVien.Remove(gv);
                Console.WriteLine("Xoá giảng viên thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy giảng viên.");
            }
        }

        // Hiển thị danh sách giảng viên
        public void HienThiDanhSach()
        {
            foreach (var gv in danhSachGiangVien)
            {
                gv.HienThiThongTin();
            }
        }

        // Tìm kiếm giảng viên theo mã
        public Lecturer TimGiangVien(string maGiangVien)
        {
            return danhSachGiangVien.FirstOrDefault(gv => gv.MaGiangVien.Equals(maGiangVien, StringComparison.OrdinalIgnoreCase));
        }

        // Tìm kiếm giảng viên theo tên
        public List<Lecturer> TimGiangVienTheoTen(string ten)
        {
            return danhSachGiangVien.Where(gv => gv.HoTen.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Lấy danh sách giảng viên
        public List<Lecturer> LayDanhSachGiangVien()
        {
            return danhSachGiangVien;
        }
    }

    // Lớp quản lý môn học
    public class SubjectManager
    {
        private List<Subject> danhSachMonHoc = new List<Subject>();

        // Thêm môn học
        public void ThemMonHoc(Subject mh)
        {
            if (TimMonHoc(mh.MaMonHoc) != null)
            {
                Console.WriteLine("Mã môn học đã tồn tại.");
                return;
            }
            danhSachMonHoc.Add(mh);
            Console.WriteLine("Thêm môn học thành công.");
        }

        // Sửa thông tin môn học
        public void SuaMonHoc(string maMonHoc, string tenMonHoc)
        {
            Subject mh = TimMonHoc(maMonHoc);
            if (mh != null)
            {
                mh.TenMonHoc = tenMonHoc;
                Console.WriteLine("Sửa thông tin môn học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy môn học.");
            }
        }

        // Xoá môn học
        public void XoaMonHoc(string maMonHoc)
        {
            Subject mh = TimMonHoc(maMonHoc);
            if (mh != null)
            {
                danhSachMonHoc.Remove(mh);
                Console.WriteLine("Xoá môn học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy môn học.");
            }
        }

        // Hiển thị danh sách môn học
        public void HienThiDanhSach()
        {
            foreach (var mh in danhSachMonHoc)
            {
                mh.HienThiThongTin();
            }
        }

        // Tìm kiếm môn học theo mã
        public Subject TimMonHoc(string maMonHoc)
        {
            return danhSachMonHoc.FirstOrDefault(mh => mh.MaMonHoc.Equals(maMonHoc, StringComparison.OrdinalIgnoreCase));
        }

        // Tìm kiếm môn học theo tên
        public List<Subject> TimMonHocTheoTen(string ten)
        {
            return danhSachMonHoc.Where(mh => mh.TenMonHoc.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Lấy danh sách môn học
        public List<Subject> LayDanhSachMonHoc()
        {
            return danhSachMonHoc;
        }
    }

    // Lớp quản lý điểm số
    public class GradeManager
    {
        private List<Grade> danhSachDiem = new List<Grade>();

        // Thêm điểm
        public void ThemDiem(Grade diem)
        {
            danhSachDiem.Add(diem);
            Console.WriteLine("Thêm điểm thành công.");
        }

        // Sửa điểm
        public void SuaDiem(string maSinhVien, string maMonHoc, double diemMoi)
        {
            Grade diem = TimDiem(maSinhVien, maMonHoc);
            if (diem != null)
            {
                diem.Diem = diemMoi;
                Console.WriteLine("Sửa điểm thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy điểm.");
            }
        }

        // Xoá điểm
        public void XoaDiem(string maSinhVien, string maMonHoc)
        {
            Grade diem = TimDiem(maSinhVien, maMonHoc);
            if (diem != null)
            {
                danhSachDiem.Remove(diem);
                Console.WriteLine("Xoá điểm thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy điểm.");
            }
        }

        // Hiển thị danh sách điểm
        public void HienThiDanhSach()
        {
            foreach (var diem in danhSachDiem)
            {
                diem.HienThiThongTin();
            }
        }

        // Tìm kiếm điểm theo mã sinh viên và mã môn học
        public Grade TimDiem(string maSinhVien, string maMonHoc)
        {
            return danhSachDiem.FirstOrDefault(d => d.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase) && d.MaMonHoc.Equals(maMonHoc, StringComparison.OrdinalIgnoreCase));
        }

        // Tìm kiếm điểm theo mã sinh viên
        public List<Grade> TimDiemTheoMaSinhVien(string maSinhVien)
        {
            return danhSachDiem.Where(d => d.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Tìm kiếm điểm theo mã môn học
        public List<Grade> TimDiemTheoMaMonHoc(string maMonHoc)
        {
            return danhSachDiem.Where(d => d.MaMonHoc.Equals(maMonHoc, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    // Lớp quản lý lớp học
    public class ClassManager
    {
        private List<Class> danhSachLop = new List<Class>();

        // Thêm lớp học
        public void ThemLop(Class lop)
        {
            if (TimLop(lop.MaLop) != null)
            {
                Console.WriteLine("Mã lớp đã tồn tại.");
                return;
            }
            danhSachLop.Add(lop);
            Console.WriteLine("Thêm lớp học thành công.");
        }

        // Sửa thông tin lớp học
        public void SuaLop(string maLop, string tenLop, Lecturer giangVienPhuTrach)
        {
            Class lop = TimLop(maLop);
            if (lop != null)
            {
                lop.TenLop = tenLop;
                lop.GiangVienPhuTrach = giangVienPhuTrach;
                Console.WriteLine("Sửa thông tin lớp học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy lớp học.");
            }
        }

        // Xoá lớp học
        public void XoaLop(string maLop)
        {
            Class lop = TimLop(maLop);
            if (lop != null)
            {
                danhSachLop.Remove(lop);
                Console.WriteLine("Xoá lớp học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy lớp học.");
            }
        }

        // Thêm sinh viên vào lớp
        public void ThemSinhVienVaoLop(string maLop, Student sv)
        {
            Class lop = TimLop(maLop);
            if (lop != null)
            {
                if (lop.DanhSachSinhVien.Any(s => s.MaSinhVien.Equals(sv.MaSinhVien, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("Sinh viên đã tồn tại trong lớp.");
                    return;
                }
                lop.DanhSachSinhVien.Add(sv);
                Console.WriteLine("Thêm sinh viên vào lớp thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy lớp học.");
            }
        }

        // Xoá sinh viên khỏi lớp
        public void XoaSinhVienKhoiLop(string maLop, string maSinhVien)
        {
            Class lop = TimLop(maLop);
            if (lop != null)
            {
                Student sv = lop.DanhSachSinhVien.FirstOrDefault(s => s.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase));
                if (sv != null)
                {
                    lop.DanhSachSinhVien.Remove(sv);
                    Console.WriteLine("Xoá sinh viên khỏi lớp thành công.");
                }
                else
                {
                    Console.WriteLine("Không tìm thấy sinh viên trong lớp.");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy lớp học.");
            }
        }

        // Hiển thị danh sách lớp học
        public void HienThiDanhSach()
        {
            foreach (var lop in danhSachLop)
            {
                lop.HienThiThongTin();
            }
        }

        // Tìm kiếm lớp học theo mã
        public Class TimLop(string maLop)
        {
            return danhSachLop.FirstOrDefault(lop => lop.MaLop.Equals(maLop, StringComparison.OrdinalIgnoreCase));
        }

        // Tìm kiếm lớp học theo tên
        public List<Class> TimLopTheoTen(string ten)
        {
            return danhSachLop.Where(lop => lop.TenLop.IndexOf(ten, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Lấy danh sách lớp học
        public List<Class> LayDanhSachLop()
        {
            return danhSachLop;
        }
    }

    // Lớp Program chứa hàm Main
    public class Program
    {
        public static void Main(string[] args)
        {
            // Khởi tạo các manager
            StudentManager studentManager = new StudentManager();
            LecturerManager lecturerManager = new LecturerManager();
            SubjectManager subjectManager = new SubjectManager();
            GradeManager gradeManager = new GradeManager();
            ClassManager classManager = new ClassManager();

            bool ketThuc = false;

            while (!ketThuc)
            {
                Console.WriteLine("\n--- Hệ Thống Quản Lý Trường Học ---");
                Console.WriteLine("1. Quản Lý Sinh Viên");
                Console.WriteLine("2. Quản Lý Giảng Viên");
                Console.WriteLine("3. Quản Lý Môn Học");
                Console.WriteLine("4. Quản Lý Lớp Học");
                Console.WriteLine("5. Quản Lý Điểm Số");
                Console.WriteLine("6. Thoát");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        QuanLySinhVien(studentManager);
                        break;
                    case "2":
                        QuanLyGiangVien(lecturerManager);
                        break;
                    case "3":
                        QuanLyMonHoc(subjectManager);
                        break;
                    case "4":
                        QuanLyLopHoc(classManager, lecturerManager, studentManager);
                        break;
                    case "5":
                        QuanLyDiemSo(gradeManager, studentManager, subjectManager);
                        break;
                    case "6":
                        ketThuc = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phần quản lý sinh viên
        public static void QuanLySinhVien(StudentManager studentManager)
        {
            bool quayLai = false;
            while (!quayLai)
            {
                Console.WriteLine("\n--- Quản Lý Sinh Viên ---");
                Console.WriteLine("1. Thêm Sinh Viên");
                Console.WriteLine("2. Sửa Thông Tin Sinh Viên");
                Console.WriteLine("3. Xoá Sinh Viên");
                Console.WriteLine("4. Hiển Thị Danh Sách Sinh Viên");
                Console.WriteLine("5. Tìm Kiếm Sinh Viên");
                Console.WriteLine("6. Quay Lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemSinhVien(studentManager);
                        break;
                    case "2":
                        SuaSinhVien(studentManager);
                        break;
                    case "3":
                        XoaSinhVien(studentManager);
                        break;
                    case "4":
                        studentManager.HienThiDanhSach();
                        break;
                    case "5":
                        TimKiemSinhVien(studentManager);
                        break;
                    case "6":
                        quayLai = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phương thức thêm sinh viên
        public static void ThemSinhVien(StudentManager studentManager)
        {
            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();

            Console.Write("Nhập họ tên: ");
            string hoTen = Console.ReadLine();

            Console.Write("Nhập ngày sinh (dd/mm/yyyy): ");
            DateTime ngaySinh;
            while (!DateTime.TryParse(Console.ReadLine(), out ngaySinh))
            {
                Console.Write("Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại (dd/mm/yyyy): ");
            }

            Console.Write("Nhập lớp: ");
            string lop = Console.ReadLine();

            Console.Write("Nhập điểm trung bình: ");
            double diemTB;
            while (!double.TryParse(Console.ReadLine(), out diemTB))
            {
                Console.Write("Điểm không hợp lệ. Vui lòng nhập lại: ");
            }

            Student sv = new Student(maSV, hoTen, ngaySinh, lop, diemTB);
            studentManager.ThemSinhVien(sv);
        }

        // Phương thức sửa sinh viên
        public static void SuaSinhVien(StudentManager studentManager)
        {
            Console.Write("Nhập mã sinh viên cần sửa: ");
            string maSV = Console.ReadLine();

            Console.Write("Nhập họ tên mới: ");
            string hoTen = Console.ReadLine();

            Console.Write("Nhập ngày sinh mới (dd/mm/yyyy): ");
            DateTime ngaySinh;
            while (!DateTime.TryParse(Console.ReadLine(), out ngaySinh))
            {
                Console.Write("Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại (dd/mm/yyyy): ");
            }

            Console.Write("Nhập lớp mới: ");
            string lop = Console.ReadLine();

            Console.Write("Nhập điểm trung bình mới: ");
            double diemTB;
            while (!double.TryParse(Console.ReadLine(), out diemTB))
            {
                Console.Write("Điểm không hợp lệ. Vui lòng nhập lại: ");
            }

            studentManager.SuaSinhVien(maSV, hoTen, ngaySinh, lop, diemTB);
        }

        // Phương thức xoá sinh viên
        public static void XoaSinhVien(StudentManager studentManager)
        {
            Console.Write("Nhập mã sinh viên cần xoá: ");
            string maSV = Console.ReadLine();
            studentManager.XoaSinhVien(maSV);
        }

        // Phương thức tìm kiếm sinh viên
        public static void TimKiemSinhVien(StudentManager studentManager)
        {
            Console.WriteLine("1. Tìm theo mã sinh viên");
            Console.WriteLine("2. Tìm theo tên");
            Console.Write("Chọn phương thức tìm kiếm: ");
            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    Console.Write("Nhập mã sinh viên: ");
                    string maSV = Console.ReadLine();
                    Student sv = studentManager.TimSinhVien(maSV);
                    if (sv != null)
                        sv.HienThiThongTin();
                    else
                        Console.WriteLine("Không tìm thấy sinh viên.");
                    break;
                case "2":
                    Console.Write("Nhập tên sinh viên: ");
                    string ten = Console.ReadLine();
                    List<Student> danhSach = studentManager.TimSinhVienTheoTen(ten);
                    if (danhSach.Count > 0)
                    {
                        foreach (var s in danhSach)
                        {
                            s.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy sinh viên.");
                    }
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // Phần quản lý giảng viên
        public static void QuanLyGiangVien(LecturerManager lecturerManager)
        {
            bool quayLai = false;
            while (!quayLai)
            {
                Console.WriteLine("\n--- Quản Lý Giảng Viên ---");
                Console.WriteLine("1. Thêm Giảng Viên");
                Console.WriteLine("2. Sửa Thông Tin Giảng Viên");
                Console.WriteLine("3. Xoá Giảng Viên");
                Console.WriteLine("4. Hiển Thị Danh Sách Giảng Viên");
                Console.WriteLine("5. Tìm Kiếm Giảng Viên");
                Console.WriteLine("6. Quay Lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemGiangVien(lecturerManager);
                        break;
                    case "2":
                        SuaGiangVien(lecturerManager);
                        break;
                    case "3":
                        XoaGiangVien(lecturerManager);
                        break;
                    case "4":
                        lecturerManager.HienThiDanhSach();
                        break;
                    case "5":
                        TimKiemGiangVien(lecturerManager);
                        break;
                    case "6":
                        quayLai = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phương thức thêm giảng viên
        public static void ThemGiangVien(LecturerManager lecturerManager)
        {
            Console.Write("Nhập mã giảng viên: ");
            string maGV = Console.ReadLine();

            Console.Write("Nhập họ tên: ");
            string hoTen = Console.ReadLine();

            Console.Write("Nhập ngày sinh (dd/mm/yyyy): ");
            DateTime ngaySinh;
            while (!DateTime.TryParse(Console.ReadLine(), out ngaySinh))
            {
                Console.Write("Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại (dd/mm/yyyy): ");
            }

            Console.Write("Nhập khoa: ");
            string khoa = Console.ReadLine();

            Console.Write("Nhập chuyên ngành: ");
            string chuyenNganh = Console.ReadLine();

            Lecturer gv = new Lecturer(maGV, hoTen, ngaySinh, khoa, chuyenNganh);
            lecturerManager.ThemGiangVien(gv);
        }

        // Phương thức sửa giảng viên
        public static void SuaGiangVien(LecturerManager lecturerManager)
        {
            Console.Write("Nhập mã giảng viên cần sửa: ");
            string maGV = Console.ReadLine();

            Console.Write("Nhập họ tên mới: ");
            string hoTen = Console.ReadLine();

            Console.Write("Nhập ngày sinh mới (dd/mm/yyyy): ");
            DateTime ngaySinh;
            while (!DateTime.TryParse(Console.ReadLine(), out ngaySinh))
            {
                Console.Write("Định dạng ngày sinh không hợp lệ. Vui lòng nhập lại (dd/mm/yyyy): ");
            }

            Console.Write("Nhập khoa mới: ");
            string khoa = Console.ReadLine();

            Console.Write("Nhập chuyên ngành mới: ");
            string chuyenNganh = Console.ReadLine();

            lecturerManager.SuaGiangVien(maGV, hoTen, ngaySinh, khoa, chuyenNganh);
        }

        // Phương thức xoá giảng viên
        public static void XoaGiangVien(LecturerManager lecturerManager)
        {
            Console.Write("Nhập mã giảng viên cần xoá: ");
            string maGV = Console.ReadLine();
            lecturerManager.XoaGiangVien(maGV);
        }

        // Phương thức tìm kiếm giảng viên
        public static void TimKiemGiangVien(LecturerManager lecturerManager)
        {
            Console.WriteLine("1. Tìm theo mã giảng viên");
            Console.WriteLine("2. Tìm theo tên");
            Console.Write("Chọn phương thức tìm kiếm: ");
            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    Console.Write("Nhập mã giảng viên: ");
                    string maGV = Console.ReadLine();
                    Lecturer gv = lecturerManager.TimGiangVien(maGV);
                    if (gv != null)
                        gv.HienThiThongTin();
                    else
                        Console.WriteLine("Không tìm thấy giảng viên.");
                    break;
                case "2":
                    Console.Write("Nhập tên giảng viên: ");
                    string ten = Console.ReadLine();
                    List<Lecturer> danhSach = lecturerManager.TimGiangVienTheoTen(ten);
                    if (danhSach.Count > 0)
                    {
                        foreach (var g in danhSach)
                        {
                            g.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy giảng viên.");
                    }
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // Phần quản lý môn học
        public static void QuanLyMonHoc(SubjectManager subjectManager)
        {
            bool quayLai = false;
            while (!quayLai)
            {
                Console.WriteLine("\n--- Quản Lý Môn Học ---");
                Console.WriteLine("1. Thêm Môn Học");
                Console.WriteLine("2. Sửa Thông Tin Môn Học");
                Console.WriteLine("3. Xoá Môn Học");
                Console.WriteLine("4. Hiển Thị Danh Sách Môn Học");
                Console.WriteLine("5. Tìm Kiếm Môn Học");
                Console.WriteLine("6. Quay Lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemMonHoc(subjectManager);
                        break;
                    case "2":
                        SuaMonHoc(subjectManager);
                        break;
                    case "3":
                        XoaMonHoc(subjectManager);
                        break;
                    case "4":
                        subjectManager.HienThiDanhSach();
                        break;
                    case "5":
                        TimKiemMonHoc(subjectManager);
                        break;
                    case "6":
                        quayLai = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phương thức thêm môn học
        public static void ThemMonHoc(SubjectManager subjectManager)
        {
            Console.Write("Nhập mã môn học: ");
            string maMH = Console.ReadLine();

            Console.Write("Nhập tên môn học: ");
            string tenMH = Console.ReadLine();

            Subject mh = new Subject(maMH, tenMH);
            subjectManager.ThemMonHoc(mh);
        }

        // Phương thức sửa môn học
        public static void SuaMonHoc(SubjectManager subjectManager)
        {
            Console.Write("Nhập mã môn học cần sửa: ");
            string maMH = Console.ReadLine();

            Console.Write("Nhập tên môn học mới: ");
            string tenMH = Console.ReadLine();

            subjectManager.SuaMonHoc(maMH, tenMH);
        }

        // Phương thức xoá môn học
        public static void XoaMonHoc(SubjectManager subjectManager)
        {
            Console.Write("Nhập mã môn học cần xoá: ");
            string maMH = Console.ReadLine();
            subjectManager.XoaMonHoc(maMH);
        }

        // Phương thức tìm kiếm môn học
        public static void TimKiemMonHoc(SubjectManager subjectManager)
        {
            Console.WriteLine("1. Tìm theo mã môn học");
            Console.WriteLine("2. Tìm theo tên môn học");
            Console.Write("Chọn phương thức tìm kiếm: ");
            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    Console.Write("Nhập mã môn học: ");
                    string maMH = Console.ReadLine();
                    Subject mh = subjectManager.TimMonHoc(maMH);
                    if (mh != null)
                        mh.HienThiThongTin();
                    else
                        Console.WriteLine("Không tìm thấy môn học.");
                    break;
                case "2":
                    Console.Write("Nhập tên môn học: ");
                    string ten = Console.ReadLine();
                    List<Subject> danhSach = subjectManager.TimMonHocTheoTen(ten);
                    if (danhSach.Count > 0)
                    {
                        foreach (var m in danhSach)
                        {
                            m.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy môn học.");
                    }
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // Phần quản lý lớp học
        public static void QuanLyLopHoc(ClassManager classManager, LecturerManager lecturerManager, StudentManager studentManager)
        {
            bool quayLai = false;
            while (!quayLai)
            {
                Console.WriteLine("\n--- Quản Lý Lớp Học ---");
                Console.WriteLine("1. Thêm Lớp Học");
                Console.WriteLine("2. Sửa Thông Tin Lớp Học");
                Console.WriteLine("3. Xoá Lớp Học");
                Console.WriteLine("4. Thêm Sinh Viên Vào Lớp");
                Console.WriteLine("5. Xoá Sinh Viên Khỏi Lớp");
                Console.WriteLine("6. Hiển Thị Danh Sách Lớp Học");
                Console.WriteLine("7. Tìm Kiếm Lớp Học");
                Console.WriteLine("8. Quay Lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemLopHoc(classManager, lecturerManager);
                        break;
                    case "2":
                        SuaLopHoc(classManager, lecturerManager);
                        break;
                    case "3":
                        XoaLopHoc(classManager);
                        break;
                    case "4":
                        ThemSinhVienVaoLop(classManager, studentManager);
                        break;
                    case "5":
                        XoaSinhVienKhoiLop(classManager);
                        break;
                    case "6":
                        classManager.HienThiDanhSach();
                        break;
                    case "7":
                        TimKiemLopHoc(classManager);
                        break;
                    case "8":
                        quayLai = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phương thức thêm lớp học
        public static void ThemLopHoc(ClassManager classManager, LecturerManager lecturerManager)
        {
            Console.Write("Nhập mã lớp: ");
            string maLop = Console.ReadLine();

            Console.Write("Nhập tên lớp: ");
            string tenLop = Console.ReadLine();

            Console.Write("Nhập mã giảng viên phụ trách: ");
            string maGV = Console.ReadLine();
            Lecturer gv = lecturerManager.TimGiangVien(maGV);
            if (gv == null)
            {
                Console.WriteLine("Không tìm thấy giảng viên. Vui lòng thêm giảng viên trước.");
                return;
            }

            Class lop = new Class(maLop, tenLop, gv);
            classManager.ThemLop(lop);
        }

        // Phương thức sửa lớp học
        public static void SuaLopHoc(ClassManager classManager, LecturerManager lecturerManager)
        {
            Console.Write("Nhập mã lớp cần sửa: ");
            string maLop = Console.ReadLine();

            Console.Write("Nhập tên lớp mới: ");
            string tenLop = Console.ReadLine();

            Console.Write("Nhập mã giảng viên phụ trách mới: ");
            string maGV = Console.ReadLine();
            Lecturer gv = lecturerManager.TimGiangVien(maGV);
            if (gv == null)
            {
                Console.WriteLine("Không tìm thấy giảng viên. Vui lòng thêm giảng viên trước.");
                return;
            }

            classManager.SuaLop(maLop, tenLop, gv);
        }

        // Phương thức xoá lớp học
        public static void XoaLopHoc(ClassManager classManager)
        {
            Console.Write("Nhập mã lớp cần xoá: ");
            string maLop = Console.ReadLine();
            classManager.XoaLop(maLop);
        }

        // Phương thức thêm sinh viên vào lớp
        public static void ThemSinhVienVaoLop(ClassManager classManager, StudentManager studentManager)
        {
            Console.Write("Nhập mã lớp: ");
            string maLop = Console.ReadLine();

            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();
            Student sv = studentManager.TimSinhVien(maSV);
            if (sv == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên. Vui lòng thêm sinh viên trước.");
                return;
            }

            classManager.ThemSinhVienVaoLop(maLop, sv);
        }

        // Phương thức xoá sinh viên khỏi lớp
        public static void XoaSinhVienKhoiLop(ClassManager classManager)
        {
            Console.Write("Nhập mã lớp: ");
            string maLop = Console.ReadLine();

            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();

            classManager.XoaSinhVienKhoiLop(maLop, maSV);
        }

        // Phương thức tìm kiếm lớp học
        public static void TimKiemLopHoc(ClassManager classManager)
        {
            Console.WriteLine("1. Tìm theo mã lớp");
            Console.WriteLine("2. Tìm theo tên lớp");
            Console.Write("Chọn phương thức tìm kiếm: ");
            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    Console.Write("Nhập mã lớp: ");
                    string maLop = Console.ReadLine();
                    Class lop = classManager.TimLop(maLop);
                    if (lop != null)
                        lop.HienThiThongTin();
                    else
                        Console.WriteLine("Không tìm thấy lớp học.");
                    break;
                case "2":
                    Console.Write("Nhập tên lớp: ");
                    string ten = Console.ReadLine();
                    List<Class> danhSach = classManager.TimLopTheoTen(ten);
                    if (danhSach.Count > 0)
                    {
                        foreach (var l in danhSach)
                        {
                            l.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy lớp học.");
                    }
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }

        // Phần quản lý điểm số
        public static void QuanLyDiemSo(GradeManager gradeManager, StudentManager studentManager, SubjectManager subjectManager)
        {
            bool quayLai = false;
            while (!quayLai)
            {
                Console.WriteLine("\n--- Quản Lý Điểm Số ---");
                Console.WriteLine("1. Thêm Điểm");
                Console.WriteLine("2. Sửa Điểm");
                Console.WriteLine("3. Xoá Điểm");
                Console.WriteLine("4. Hiển Thị Danh Sách Điểm");
                Console.WriteLine("5. Tìm Kiếm Điểm");
                Console.WriteLine("6. Quay Lại");
                Console.Write("Chọn chức năng: ");
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemDiem(gradeManager, studentManager, subjectManager);
                        break;
                    case "2":
                        SuaDiem(gradeManager);
                        break;
                    case "3":
                        XoaDiem(gradeManager);
                        break;
                    case "4":
                        gradeManager.HienThiDanhSach();
                        break;
                    case "5":
                        TimKiemDiem(gradeManager);
                        break;
                    case "6":
                        quayLai = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ.");
                        break;
                }
            }
        }

        // Phương thức thêm điểm
        public static void ThemDiem(GradeManager gradeManager, StudentManager studentManager, SubjectManager subjectManager)
        {
            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();
            Student sv = studentManager.TimSinhVien(maSV);
            if (sv == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên. Vui lòng thêm sinh viên trước.");
                return;
            }

            Console.Write("Nhập mã môn học: ");
            string maMH = Console.ReadLine();
            Subject mh = subjectManager.TimMonHoc(maMH);
            if (mh == null)
            {
                Console.WriteLine("Không tìm thấy môn học. Vui lòng thêm môn học trước.");
                return;
            }

            Console.Write("Nhập điểm: ");
            double diem;
            while (!double.TryParse(Console.ReadLine(), out diem))
            {
                Console.Write("Điểm không hợp lệ. Vui lòng nhập lại: ");
            }

            Grade diemMoi = new Grade(maSV, maMH, diem);
            gradeManager.ThemDiem(diemMoi);
        }

        // Phương thức sửa điểm
        public static void SuaDiem(GradeManager gradeManager)
        {
            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();

            Console.Write("Nhập mã môn học: ");
            string maMH = Console.ReadLine();

            Console.Write("Nhập điểm mới: ");
            double diemMoi;
            while (!double.TryParse(Console.ReadLine(), out diemMoi))
            {
                Console.Write("Điểm không hợp lệ. Vui lòng nhập lại: ");
            }

            gradeManager.SuaDiem(maSV, maMH, diemMoi);
        }

        // Phương thức xoá điểm
        public static void XoaDiem(GradeManager gradeManager)
        {
            Console.Write("Nhập mã sinh viên: ");
            string maSV = Console.ReadLine();

            Console.Write("Nhập mã môn học: ");
            string maMH = Console.ReadLine();

            gradeManager.XoaDiem(maSV, maMH);
        }

        // Phương thức tìm kiếm điểm
        public static void TimKiemDiem(GradeManager gradeManager)
        {
            Console.WriteLine("1. Tìm theo mã sinh viên và mã môn học");
            Console.WriteLine("2. Tìm theo mã sinh viên");
            Console.WriteLine("3. Tìm theo mã môn học");
            Console.Write("Chọn phương thức tìm kiếm: ");
            string luaChon = Console.ReadLine();

            switch (luaChon)
            {
                case "1":
                    Console.Write("Nhập mã sinh viên: ");
                    string maSV = Console.ReadLine();
                    Console.Write("Nhập mã môn học: ");
                    string maMH = Console.ReadLine();
                    Grade diem = gradeManager.TimDiem(maSV, maMH);
                    if (diem != null)
                        diem.HienThiThongTin();
                    else
                        Console.WriteLine("Không tìm thấy điểm.");
                    break;
                case "2":
                    Console.Write("Nhập mã sinh viên: ");
                    maSV = Console.ReadLine();
                    List<Grade> danhSachDiemSV = gradeManager.TimDiemTheoMaSinhVien(maSV);
                    if (danhSachDiemSV.Count > 0)
                    {
                        foreach (var d in danhSachDiemSV)
                        {
                            d.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy điểm cho sinh viên này.");
                    }
                    break;
                case "3":
                    Console.Write("Nhập mã môn học: ");
                    maMH = Console.ReadLine();
                    List<Grade> danhSachDiemMH = gradeManager.TimDiemTheoMaMonHoc(maMH);
                    if (danhSachDiemMH.Count > 0)
                    {
                        foreach (var d in danhSachDiemMH)
                        {
                            d.HienThiThongTin();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy điểm cho môn học này.");
                    }
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }
    }
}