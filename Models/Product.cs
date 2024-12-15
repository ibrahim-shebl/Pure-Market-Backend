using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [MaxLength(250)]
    public string ProductName { get; set; }

    public double Price { get; set; }

    public double OldPrice { get; set; }

    public string Category { get; set; }

    public string Brand { get; set; }

    public double Rate { get; set; }

    public double DiscountPercentage { get; set; }

    [MaxLength(2500)]
    public string Description { get; set; }

    public string CatImgPath { get; set; }  
}
