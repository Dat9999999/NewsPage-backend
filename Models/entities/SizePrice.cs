using NewsPage.Enums;

public class SizePrice{
    public Guid Id {get; set;}
    public AdvertiseSize SizeType {get; set;}

    public decimal Cost {get; set;}
    public DateTime UpdateAt {get; set;}
    public DateTime CreateAt {get; set;}

}