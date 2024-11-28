using System;
public class SanPham{
    private string tenSP;
    private double gia;
    private int soLuong;
    public SanPham(string tenSP, double gia, int soLuong){
        this.TenSP= tenSP;
        this.Gia= gia;  
        this.SoLuong= soLuong;
    }
    public string TenSP{
        get{ return tenSP; }
        set{
            if (!string.IsNullOrEmpty(value)){
                tenSP = value;
            }
            else{
                throw new ArgumentException("Invalid tenSP!");
            }
        } 
    }
    public double Gia{
        get{ return gia; }
        set{
            if (value >= 0){
                gia = value;
            }
            else{ throw new ArgumentException("Invalid gia!"); }
        }  
    }
    public int SoLuong{
        get{ return soLuong;}
        if (value >=0 ){
            soLuong = value;
        }
        else{throw new ArgumentException("Invalid soluong!"); }
    }
    public double TinhTongGiaTri(){
        return Gia*SoLuong;
    }
    public void HienThiThongTin(){
        Console.WriteLine($"Tên SP: {TenSP}, Số lượng: {SoLuong}, Tổng giá trị: {TinhTongGiaTri():C}");
    }
    public class Program
    {
        public static void Main(string[] args){
            SanPham sp1 = new SanPham("Laptop", 200000, 10);
            sp1.HienThiThongTin();
            SanPham sp2 = new SanPham("Đt", 500000, 20);
        }
        catch (Exception ex){
            Console.WriteLine(ex.Message);
        } 
    } 
}