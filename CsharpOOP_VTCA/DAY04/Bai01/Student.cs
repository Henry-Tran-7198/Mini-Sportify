using System;

namespace QuanLyTruongHoc
{
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
}