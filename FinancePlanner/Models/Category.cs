public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Type { get; set; } = ""; // Income / Expense

    public int UserId { get; set; }
}