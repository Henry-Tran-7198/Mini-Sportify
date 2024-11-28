using System;
public class Xe{
    private string hang;
    private string mauSac;
    private double gia;
    public Xe(string hang, string mauSac, double gia){
        this.Hang = hang;
        this.MauSac = mauSac;
        this.Gia = gia;
    }
    public string Hang{
        get{ return hang; }
        set{
            if (!string.IsNullOrEmpty(value)){
                hang = value;
            }
            else{
                throw new ArgumentException("Invalid hang!");
            }
        }   
    }
    public string MauSac{
        get { return mauSac; }
        set{
            if (!string.IsNullOrWhiteSpace(value)){
                mauSac = value;
            }    
            else{
                throw new ArgumentException("Invalid color!");
            }
        }  
    }
    public double Gia{
        get { return gia; }
        set{
            if (value >= 0){
                gia = value;
            }
            else{
                throw new ArgumentException("Invalid price! ");
            }
        } 
    }
    public void HienThiThongTin(){
        Console.WriteLine($"Hãng: {Hang}, Màu sắc: {MauSac}, Giá: {Gia:C}");
    }
    public class Program
    {
        public static void Main(string[] args){
            try{
                Xe xe1 = new Xe("Toyota", "Đỏ", 500000000);
                xe1.HienThiThongTin();
                Xe xe2 = new Xe("Mercedes", "Đen", 10000000000);
                xe2.HienThiThongTin();
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
        }
    }
}