using System;
public class TaiKhoan{
    private double soDu;
    public TaiKhoan(double soDuBanDau){
        SoDu= soDuBanDau;
    }
    public double SoDu{
        get{ return soDu; }
        private set{
            if (value >= 0){
                soDu=value;
            }
            else{
                throw new ArgumentException("Số dư k hợp lệ. ");
            }
        }
    }
    public void NapTien(double soTien){
        if (soTien > 0){
            SoDu += soTien;
            Console.WriteLine($"Đã nạp {soTien}. Số dư hiện tại: {SoDu}");
        }
        else {
            throw new ArgumentException("Số tiền k hợp lệ. ");
        }
    }
    private void RutTien(double soTien){
        if (soTien > 0){
            if(SoDu >= soTien){
                SoDu -= soTien;
                Console.WriteLine($"Đã rút {soTien}. Số dư hiện tại: {SoDu}");
            }
            else{
                throw new ArgumentException("Số dư k đủ. ");
            }
        }
        else{
            throw new ArgumentException("Số tiền phải rút > 0");
        }
    }
    public void HienThiSoDu(){
        Console.WriteLine($"Số dư hiện tại: {SoDu}");
    }
    public class Program
    {
        public static void Main(string[] args){
            try {
                TaiKhoan tk = new TaiKhoan(1000);
                tk.HienThiSoDu();
                tk.NapTien(500);
                tk.RutTien(300);
                tk.RutTien(10000);
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);  
            }
        }
    }
}