public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Type { get; set; } = ""; // Income / Expense

    public int UserId { get; set; }

    public decimal ProjectedAmount { get; set; }

    public int StartMonth { get; set; }
    public int StartYear { get; set; }
    
    public int EndMonth { get; set; }
    public int EndYear { get; set; }
}