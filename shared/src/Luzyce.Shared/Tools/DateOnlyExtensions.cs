namespace Luzyce.Shared.Tools;

public static class DateOnlyExtensions
{
    public static DateOnly? AddMonths(this DateOnly? date, int months) =>
        date.HasValue 
            ? DateOnly.FromDateTime(date.Value.ToDateTime(TimeOnly.MinValue).AddMonths(months)) 
            : null;

}
