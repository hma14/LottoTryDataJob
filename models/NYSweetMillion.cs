namespace LottoTryDataJob
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("NYSweetMillion")]
    public partial class NYSweetMillion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DrawNumber { get; set; }

        [StringLength(25)]
        public string DrawDate { get; set; }

        public int? Number1 { get; set; }

        public int? Number2 { get; set; }

        public int? Number3 { get; set; }

        public int? Number4 { get; set; }

        public int? Number5 { get; set; }

        public int? Number6 { get; set; }
    }
}
