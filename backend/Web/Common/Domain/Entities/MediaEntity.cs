using Web.Common.Domain.Enums;

namespace Web.Common.Domain.Entities;
public class MediaEntity : BaseEntity
{
    public long Size { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    
    public MediaType Type { get; set; }

}
