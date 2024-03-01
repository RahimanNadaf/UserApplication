
namespace UserManagement.Data.Entities;
internal class Logs
{
    public long UserId { get; set; }
    public string Type { get; set; } = default!;
    public string ShowMessage { get; set; } = default!;
    public string Details { get; set; } = default!;
}
