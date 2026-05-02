namespace FinancePlanner.Models
{
    /// <summary>
    /// Визначає область застосування зміни категорії (видалення або перейменування).
    /// </summary>
    public enum CategoryScope
    {
        All = 0,
        ThisMonth = 1,
        FromNowOn = 2
    }
}
