using System;
using NacidRas.Infrastructure.Data;
using NacidRas.Ras.Nomenclatures.Models;

namespace NacidRas.Ras
{
	// One person could have multiple dissertations
	// Only one dissertation could be submitted with one application.
	// Whe application is recieved, bl should check if this persion is already in register and add only parts, not a new Lot.
	public class Dissertation : EntityVersion
	{
		// public bool IsVak { get; set; }
		// Дисертационни трудове от 1962 г.до 27.03.2011 г.
		// Номер и дата на протокол на ВАК за утвърждаване
		// Специализиран научен съвет на ВАК(номенклатура)
		// Място на съхранение(номенклатура)

		// -- Номенклатура от старата база( Място на съхранение )
		public int? DepositoryOldId { get; set; }
		[Skip]
		public DepositoryOld DepositoryOld { get; set; }
		// -- Номенклатура от старата база( Специален научен съвет )
		public int? SnsOldId { get; set; }
		[Skip]
		public SnsOld SnsOld { get; set; }
		public bool DissertationIsCopyrighted { get; set; }
		[CloneUpdate]
		public int? DissertationFileId { get; set; }
		public DissertationFile DissertationFile { get; set; }
		[CloneUpdate]
		public int? SummaryFileId { get; set; }
		public SummaryFile SummaryFile { get; set; }

		// тема на дисертационния труд
		public string Title { get; set; }
		// -- Тема на дисертационния труд на английски от старата база
		public string TitleAlt { get; set; }
		// До 1500 символа.
		public string Annotation { get; set; }
		// -- Анотация на английски от старата база
		public string AnnotationAlt { get; set; }
		// Библиотечна сигнатура
		public string LibraryNumber { get; set; }
		// Дали е задължително да бъде въведена библиотечна сигнатура
		public bool LibraryNumberRequired { get; set; }
		// Дата на защита
		public DateTime? DateOfAcquire { get; set; }

		// Език на основния текст
		public int? LanguageId { get; set; }
		[Skip]
		public Language Language { get; set; }

		// Библиография (брой заглавия)
		public int? NumberOfBibliography { get; set; }

		// Общ обем на дисертационния труд (брой страници)
		public int? NumberOfPages { get; set; }

		// Научен ръководител
		public string Supervisor { get; set; }
		// -- Научен ръководител на английски от старата база
		public string SupervisorAlt { get; set; }
		// Рецензенти
		public string Reviewers { get; set; }
		// -- Рецензенти на английски от старата база
		public string ReviewersAlt { get; set; }
		// Председател на Жури
		public string HeadOfJury { get; set; }
		// -- Председател на Жури на английски от старата база
		public string HeadOfJuryAlt { get; set; }
		// Членове на Жури
		public string Jury { get; set; }
		// Членове на Жури на английски от старата база
		public string JuryAlt { get; set; }
	}
}
