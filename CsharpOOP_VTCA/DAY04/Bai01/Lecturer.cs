using System;

namespace QuanLyTruongHoc
{
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
}