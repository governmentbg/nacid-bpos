using System;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
    public interface IDateConverter
    {
        bool TryDecode(string date, out DateTime dateTime);

        DateTime? Decode(string date);

        string Encode(string granularity, DateTime? date);
    }
}
