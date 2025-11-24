namespace ProyectoPokemon.Models;
public class Pagination<T>
{
    public List<T> Items { get; set; }
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 0;
    public int Offset { get; set; } = 0;
    public int TotalItems { get; set; } = 0;
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public Pagination(List<T> items, int count, int pageIndex, int pageSize)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalItems = count;
        PageSize = pageSize;
    }
    public bool HasPreviousPage => (PageIndex>1);
    public bool HasNextPage => (PageIndex < TotalPages);
}
