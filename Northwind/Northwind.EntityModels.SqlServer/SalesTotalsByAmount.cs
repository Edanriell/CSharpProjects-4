using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EntityModels;

[Keyless]
public partial class SalesTotalsByAmount
{
    [Column(TypeName = "money")]
    public decimal? SaleAmount { get; set; }
    public int OrderId { get; set; }

    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ShippedDate { get; set; }
}
