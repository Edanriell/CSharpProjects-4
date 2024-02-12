using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EntityModels;

namespace Web.Pages;

public class CustomerOrdersModel : PageModel
{
    public Customer? Customer;

    private NorthwindContext _db;

    public CustomerOrdersModel(NorthwindContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        string? id = HttpContext.Request.Query["id"];

        Customer = _db.Customers.Include(c => c.Orders).SingleOrDefault(c => c.CustomerId == id);
    }
}
