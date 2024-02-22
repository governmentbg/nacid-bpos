using System;
using System.Globalization;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public sealed class DateConverter : IDateConverter
	{
		public bool TryDecode(string date, out DateTime dateTime)
		{
			if (DateTimeOffset.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset offset))
			{
				dateTime = offset.UtcDateTime;
				return true;
			}

			dateTime = DateTime.MinValue.ToUniversalTime();
			return false;
		}

		public DateTime? Decode(string date)
		{
			if (TryDecode(date, out var decodedDate))
			{
				return decodedDate;
			}

			return null;
		}

		public string Encode(string granularity, DateTime? date)
		{
			if (date.HasValue)
				return date.Value.ToUniversalTime().ToString(granularity);
			else
				return string.Empty;
		}
	}
}
