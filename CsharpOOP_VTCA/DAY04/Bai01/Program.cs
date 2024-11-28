using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
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
