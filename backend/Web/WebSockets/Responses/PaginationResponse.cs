using Web.WebSockets.Requests;

namespace Web.WebSockets.Responses;
public class PaginationResponse
{

    public int CountPage { get; set; }
    public int CountTotal { get; set; }

    public object Result { get; set; }

    public PaginationResponse(List<object> objects, PaginationRequest pagination)
    {
        Result = objects;

        CountTotal = objects.Count();
        
        CountPage = (int)Math.Ceiling(
            objects.Count() / (double)pagination.Size);
    }
}
