using System;

namespace QuanLyTruongHoc
{
    public class Subject
    {
        public string MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }

        public Subject(string maMonHoc, string tenMonHoc)
        {
            MaMonHoc = maMonHoc;
            TenMonHoc = tenMonHoc;
        }

        public void HienThiThongTin()
        {
            Console.WriteLine($"Mã MH: {MaMonHoc}, Tên MH: {TenMonHoc}");
        }
    }
}