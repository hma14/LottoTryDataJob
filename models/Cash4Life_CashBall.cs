namespace LottoTryDataJob
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class Cash4Life_CashBall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DrawNumber { get; set; }

        public string DrawDate { get; set; }

        
        public int? CashBall { get; set; }
    }
}
