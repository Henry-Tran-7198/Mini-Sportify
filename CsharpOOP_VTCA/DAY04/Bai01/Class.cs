using System;
using System.Collections.Generic;

namespace QuanLyTruongHoc
{
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

        public void ThemSinhVien(Student sv)
        {
            DanhSachSinhVien.Add(sv);
        }

        public void XoaSinhVien(string maSinhVien)
        {
            var sv = DanhSachSinhVien.FirstOrDefault(s => s.MaSinhVien == maSinhVien);
            if (sv != null)
            {
                DanhSachSinhVien.Remove(sv);
                Console.WriteLine("Xóa sinh viên thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }
        }
    }
}