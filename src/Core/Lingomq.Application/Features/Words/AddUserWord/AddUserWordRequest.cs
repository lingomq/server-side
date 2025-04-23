using System.Text.Json.Serialization;

namespace LingoMQ.Core.Application.Features.Words.AddUserWord;

public class AddUserWordRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid WordId { get; set; }

    public AddUserWordRequest(Guid userId, Guid wordId)
    {
        UserId = userId;
        WordId = wordId;
    }
}
