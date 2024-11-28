using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
    public class GradeManager
    {
        private List<Grade> danhSachDiem = new List<Grade>();

        // Thêm điểm cho sinh viên
        public void ThemDiem(Grade diem)
        {
            if (TimDiem(diem.MaSinhVien, diem.MaMonHoc) != null)
            {
                Console.WriteLine("Điểm này đã tồn tại cho sinh viên này trong môn học này.");
                return;
            }
            danhSachDiem.Add(diem);
            Console.WriteLine("Thêm điểm thành công.");
        }

        // Sửa điểm của sinh viên
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
                Console.WriteLine("Không tìm thấy điểm của sinh viên trong môn học này.");
            }
        }

        // Xóa điểm của sinh viên
        public void XoaDiem(string maSinhVien, string maMonHoc)
        {
            Grade diem = TimDiem(maSinhVien, maMonHoc);
            if (diem != null)
            {
                danhSachDiem.Remove(diem);
                Console.WriteLine("Xóa điểm thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy điểm của sinh viên trong môn học này.");
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
            return danhSachDiem.FirstOrDefault(d => d.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase) &&
                                                    d.MaMonHoc.Equals(maMonHoc, StringComparison.OrdinalIgnoreCase));
        }
    }
}