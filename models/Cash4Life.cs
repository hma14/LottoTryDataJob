namespace LottoTryDataJob
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class Cash4Life
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DrawNumber { get; set; }

        public string DrawDate { get; set; }

        public int? Number1 { get; set; }

        public int? Number2 { get; set; }

        public int? Number3 { get; set; }

        public int? Number4 { get; set; }

        public int? Number5 { get; set; }
    }
}
