using System;
public class SinhVien
{
    private string maSV;
    private string hoTen;
    private double diemTB;
    public SinhVien(string maSV, string hoTen, double diemTB)
    {
        this.maSV = maSV;
        this.hoTen = hoTen;
        this.DiemTB = diemTB;
    }
    public string MaSV{
        get { return maSV; }
        set { maSV = value; }
    }
    public string HoTen{
        get { return hoTen; }
        set { hoTen = value; }
    }
    double DiemTB{
        get { return diemTB; }
        set {
            if (value >= 0 && value <= 10)
            {
                diemTB = value;
            }
            else
            {
                throw new ArgumentException("Điểm TB phải từ 0 đến 10.");
            }
         }
    }
    public void HienThiThongTin(){
        Console.WriteLine($"Mã sv: {MaSV}, Họ tên: {HoTen}, Điểm TB: {DiemTB}");
    }
}
public class Program{
    public static void Main(string[] args){
        try {
            SinhVien sv = new SinhVien("SV001", "Nguyễn Văn A", 8.5);
            sv.HienThiThongTin();
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
        }  
    }  
}