using System;
public class HinhChuNhat{
    private double chieuDai;
    private double chieuRong;
    public HinhChuNhat(double chieuDai, double chieuRong) {
        this.chieuDai=chieuDai;
        this.chieuRong=chieuRong;
    }    
    public double ChieuDai{
        get { return chieuDai; }
        set {
            if (value > 0){
                chieuDai = value;
            }
            else{
                throw new ArgumentException("Chiều dài phải > 0.");
            }
        }
    }
    public double ChieuRong{
        get { return chieuRong; }
        set { 
            if (value > 0){
                chieuRong = value;
            }
            else{
                throw new ArgumentException("Chiều rộng phải > 0.");
            }
        }
    }
    public double TinhDienTich(){
        return chieuDai*chieuRong;
    }
    public double TinhChuVi(){
        return (chieuDai+chieuRong)/2;
    }
    public void HienThiThongTin(){
        Console.WriteLine($"Dài: {ChieuDai}, Rộng: {ChieuRong}");
        Console.WriteLine($"Diện tích: {TinhDienTich()}, Chu vi: {TinhChuVi()}");
    }
    public class Program{
        public static void Main(string[] args) {
            try{
                HinhChuNhat hcn = new HinhChuNhat(5.0, 3.5);
                hcn.HienThiThongTin();
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
        }
    }
}