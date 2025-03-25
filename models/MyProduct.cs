namespace LottoTryDataJob
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MyProduct")]
    public partial class MyProduct
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
    }
}
