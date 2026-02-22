namespace Application.Common.Pagination;

public class PaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public PaginationRequest()
    {
    }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
    }
}
