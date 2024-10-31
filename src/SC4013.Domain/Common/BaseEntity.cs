namespace SC4013.Domain.Common;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}