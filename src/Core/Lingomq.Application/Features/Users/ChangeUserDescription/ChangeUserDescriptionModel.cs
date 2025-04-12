using System.Text.Json.Serialization;

namespace LingoMQ.Core.Application.Features.Users.ChangeUserDescription;

public class ChangeUserDescriptionModel
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public required string Description { get; set; }
}
