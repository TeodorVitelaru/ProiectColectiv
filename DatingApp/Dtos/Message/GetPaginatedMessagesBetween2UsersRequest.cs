namespace DatingApp.Dtos.Message;

public class GetPaginatedMessagesBetween2UsersRequest
{
    public long SenderId { get; set; }

    public long RecipientId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}