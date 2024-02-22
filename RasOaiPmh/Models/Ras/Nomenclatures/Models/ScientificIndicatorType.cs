using System.ComponentModel;
using NacidRas.Infrastructure.Data;

namespace NacidRas.Ras
{
	public class ScientificIndicatorType : Nomenclature
	{
		public int Area { get; set; }
		// Considering change to Enum
		public string IndicatorGroup { get; set; }

		public IndicatorType IndicatorType { get; set; }
		//public int Id { get; set; }
		//public string Name { get; set; }
		//public bool IsCustom { get; set; }
		public CustomLogicType? CustomLogicType { get; set; }
	}
	public enum IndicatorType
	{
		[Description("Избор с текст")]
		Text = 1,
		[Description("Множествен избор на публикация")]
		Publication = 2,
		[Description("Множествен избор на публикация и цитати към нея")]
		TextAndPublicationWithQuotes = 3,
		[Description("Множествен избор на текс и точки")]
		TextAndScore = 4
	}

	public enum CustomLogicType
	{
		[Description("Копиране на Тема и дата на защита на доктор")]
		CopyTypeDoctor = 1,
		[Description("Копиране на Тема и дата на защита на доктор на науките")]
		CopyTypeDoctorOfScience = 2
	}
}
