using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyTruongHoc
{
    public class SubjectManager
    {
        private List<Subject> danhSachMonHoc = new List<Subject>();

        // Thêm môn học
        public void ThemMonHoc(Subject monHoc)
        {
            if (TimMonHoc(monHoc.MaMonHoc) != null)
            {
                Console.WriteLine("Mã môn học đã tồn tại.");
                return;
            }
            danhSachMonHoc.Add(monHoc);
            Console.WriteLine("Thêm môn học thành công.");
        }

        // Sửa thông tin môn học
        public void SuaMonHoc(string maMonHoc, string tenMonHoc)
        {
            Subject monHoc = TimMonHoc(maMonHoc);
            if (monHoc != null)
            {
                monHoc.TenMonHoc = tenMonHoc;
                Console.WriteLine("Sửa thông tin môn học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy môn học.");
            }
        }

        // Xóa môn học
        public void XoaMonHoc(string maMonHoc)
        {
            Subject monHoc = TimMonHoc(maMonHoc);
            if (monHoc != null)
            {
                danhSachMonHoc.Remove(monHoc);
                Console.WriteLine("Xóa môn học thành công.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy môn học.");
            }
        }

        // Hiển thị danh sách môn học
        public void HienThiDanhSach()
        {
            foreach (var monHoc in danhSachMonHoc)
            {
                monHoc.HienThiThongTin();
            }
        }

        // Tìm kiếm môn học theo mã
        public Subject TimMonHoc(string maMonHoc)
        {
            return danhSachMonHoc.FirstOrDefault(m => m.MaMonHoc.Equals(maMonHoc, StringComparison.OrdinalIgnoreCase));
        }
    }
}