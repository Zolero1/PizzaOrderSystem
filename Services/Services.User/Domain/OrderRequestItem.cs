using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.User.Domain;

[Table("order_request_items")]
public class OrderRequestItem
{
    [Column("id"),Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column("article"),Required]
    public string Article { get; set; }
    [Column("price"),Required]
    public decimal Price { get; set; }

    public OrderRequest OrderRequest { get; set; }
}