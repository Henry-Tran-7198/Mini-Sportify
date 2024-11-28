using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
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

        // Xóa sinh viên
        public void XoaSinhVien(string maSinhVien)
        {
            Student sv = TimSinhVien(maSinhVien);
            if (sv != null)
            {
                danhSachSinhVien.Remove(sv);
                Console.WriteLine("Xóa sinh viên thành công.");
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
            return danhSachSinhVien.FirstOrDefault(s => s.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase));
        }
    }
}