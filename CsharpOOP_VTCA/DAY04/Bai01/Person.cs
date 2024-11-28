using System;

namespace QuanLyTruongHoc
{
    public abstract class Person
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }

        protected Person(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        public abstract void HienThiThongTin();
    }
}