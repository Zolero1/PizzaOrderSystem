using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Payment.Domain;

[Table("payments")]
public class Payment {
    [Key]
    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Required]
    [Column("payment_amount")]
    public decimal PaymentAmount { get; set; }

    [Column("payed_at")]
    public DateTimeOffset? PayedAt { get; set; }
}