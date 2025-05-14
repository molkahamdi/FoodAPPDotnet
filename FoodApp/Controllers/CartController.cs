using FoodApp.ContextDBConfig;
using FoodApp.Models;
using FoodApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly FoodAppDBContext context;
        private readonly IData data;
        public CartController(IData data,FoodAppDBContext context)
        {
            this.data = data;
            this.context=context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await data.GetUser(HttpContext.User);
            cart.UserId = user?.Id;
            if (ModelState.IsValid)
            {
                context.Carts.Add(cart);    
                context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetAddedCarts()
        {
            var user =await data.GetUser(HttpContext.User); 
            var carts=context.Carts.Where(c=>c.UserId==user.Id).ToList();
            return Ok(carts);
        }
    }
}
