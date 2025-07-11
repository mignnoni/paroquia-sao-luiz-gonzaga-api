namespace BuildingBlocks.Domain;

public static class Months
{
    private static readonly Dictionary<int, string> M = new()
    {
        { 1, "Janeiro" }, { 2, "Fevereiro" }, { 3, "Março" },
        { 4, "Abril" }, { 5, "Maio" }, { 6, "Junho" },
        { 7, "Julho" }, { 8, "Agosto" }, { 9, "Setembro" },
        { 10, "Outubro" }, { 11, "Novembro" }, { 12, "Dezembro" }
    };

    public static string GetMonthStr(int month) => M[month];
}
