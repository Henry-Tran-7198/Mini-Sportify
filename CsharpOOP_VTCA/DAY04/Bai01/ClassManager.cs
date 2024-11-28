using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
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

        // Xóa lớp học
        public void XoaLop(string maLop)
        {
            Class lop = TimLop(maLop);
            if (lop != null)
            {
                danhSachLop.Remove(lop);
                Console.WriteLine("Xóa lớp học thành công.");
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
}