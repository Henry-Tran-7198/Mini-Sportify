using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
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

        // Xóa giảng viên
        public void XoaGiangVien(string maGiangVien)
        {
            Lecturer gv = TimGiangVien(maGiangVien);
            if (gv != null)
            {
                danhSachGiangVien.Remove(gv);
                Console.WriteLine("Xóa giảng viên thành công.");
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
    }
}