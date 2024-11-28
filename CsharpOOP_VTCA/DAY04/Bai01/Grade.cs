using System;

namespace QuanLyTruongHoc
{
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
}