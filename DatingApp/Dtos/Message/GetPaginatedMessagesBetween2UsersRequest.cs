namespace DatingApp.Dtos.Message;

public class GetPaginatedMessagesBetween2UsersRequest
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}