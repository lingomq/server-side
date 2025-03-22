using LingoMQ.Core.Domain.Common;

namespace LingoMQ.Core.Domain.Words;

public class Language : ValueObject
{
    private string _value;
    private string _code;
    private string _subCode;
    public string Value => _value;
    public string Code => _code;
    public string SubCode => _subCode;
    public static Language UnitedEnglish => new("english", "en", "US");
    public static Language Russian => new("russian", "ru");
    public static Language German => new("german");
    public static Language Japanese => new("japanese", "jr");

    public Language(string value)
    {
        _value = value;
        _code = value;
        _subCode = value;
    }

    public Language(string value, string code)
    {
        _value = value;
        _code = code;
        _subCode = code;
    }

    public Language(string value, string code, string subCode)
    {
        _value = value;
        _code = code;
        _subCode = subCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Code;
        yield return SubCode;
    }
}
