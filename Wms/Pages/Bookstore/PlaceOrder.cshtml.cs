using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wms.Models.Shared;

namespace Wms.Pages.Bookstore;

public class PlaceOrderModel : PageModel
{
    [BindProperty]
    [Display(Name ="Order message", Description = "Order detail message")]
    public string OrderMessage { get; set; }

    private readonly ConnectionFactory conncectionFactory;

    public PlaceOrderModel(IOptions<RabbitMqOptions> options)
    {
        conncectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(options.Value.Url)
        };
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        //if (Category != null)
        //{
        //    await categoryService.CreateAsync(Category);
        //    ModelState.Clear();
        //    Category = new Category();
        //}
        //Console.WriteLine(OrderMessage);

        using (var connection = conncectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(
                queue: "hello"
                , durable: false
                , exclusive: false
                , autoDelete: false
                , arguments: null);

            var body = Encoding.UTF8.GetBytes(OrderMessage);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

            Console.WriteLine(" [x] Sent {0}", OrderMessage);
        }

        return Page();
    }
}
