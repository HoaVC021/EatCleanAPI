using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EatCleanAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EatCleanAPI.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using EatCleanAPI.Services;
using Microsoft.Extensions.Configuration;

namespace EatCleanAPI.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly VegafoodBotContext _context;
        private readonly ILogger<OrdersController> _logger;
        // service of mail gun
        private readonly IEmailSender _emailSender;
        private readonly IViewRenderService _viewRenderService;
        //service of sendgird
        private IMailService _mailService;
        private IConfiguration _configuration;

        public OrdersController(VegafoodBotContext context, IEmailSender emailSender,
            IViewRenderService viewRenderService, IMailService mailService, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
            _mailService = mailService;
            _configuration = configuration;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {

            return await _context.Orders.ToListAsync();
        }



        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // GET: api/Orders/Mine
        [HttpGet("Mine/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersMine(int id)
        {
            var orders = await _context.Orders
                                      .Where(od => od.UserId == id)
                                      .ToListAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Order>>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var user = _context.Users.Where(user => user.UserId == order.UserId).FirstOrDefault();

            //var xx = _context.Users.Where(user => user.UserId == order.UserId).OrderByDescending(p => p.UserId)
            //.ToListAsync();

            var newOrderId = _context.Orders.Where(or => or.UserId == order.UserId).OrderByDescending(p => p.UserId).ToList();

            int x = newOrderId.Last().OrderId;
            string Id = x.ToString();

            var emailModel = new RepliedOrderVm()
            {
                UserName = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Time= DateTime.Now,
                Price= order.TotalPrice,
                Paypal= order.PaypalMethod,
                Address= order.Address,
                PaymentId = Id,
            };

            var htmlContent = await _viewRenderService.RenderToStringAsync("_RepliedCommentEmail", emailModel);
            await _mailService.SendEmailAsync(user.Email, "Your order at Vegafood succeessed", htmlContent);

            // await _mailService.SendEmailAsync("hongthaiit1998@gmail.com", "Bạn đã mua hàng thành công", "<h1>Hey!, You success order on Vegafood</h1><p>Your order on Vegafood at " + DateTime.Now + "</p>");

            //return Ok("Order success,please check email for order on Vegafood !!!");
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        [HttpPost("Email")]
        public async Task<ActionResult<IEnumerable<Order>>> PostEmail(ReplyOrderRequest request) 
        {

            var order = _context.Orders.Where(order => order.OrderId == request.orderId).FirstOrDefault();
            var user = _context.Users.Where(user => user.UserId == order.UserId).FirstOrDefault();

            if (order != null)
            {

                // send Mail

                var emailModel = new RepliedOrderVm()
                {
                    UserName = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Email = order.EmailAddress,

                };
                //https://github.com/leemunroe/responsive-html-email-template
                var htmlContent = await _viewRenderService.RenderToStringAsync("_RepliedCommentEmail", emailModel);

                await _emailSender.SendEmailAsync(order.EmailAddress, "Bạn đã mua hàng thành công", htmlContent);

                return Ok("Đã gửi email thành công");
               // return CreatedAtAction(nameof(GetOrder), new { id = user.UserId }, request);

                //await _mailService.SendEmailAsync(order.EmailAddress, "New Order", "<h1>Hey!, You success order on Vegafood</h1><p>Your order on Vegafood at " + DateTime.Now + "</p>");
                //return Ok("Mua hang thanh cong");
            }
            else
            {

                return BadRequest("Gửi không thành công");
            }

            return Ok();


            //Send mail
            //if (comment.ReplyId.HasValue)
            //{
            //    var repliedComment = await _context.Comments.FindAsync(comment.ReplyId.Value);
            //    var repledUser = await _context.Users.FindAsync(repliedComment.OwnerUserId);
            //    var emailModel = new RepliedOrderVm()
            //    {
            //        LastName = request.LastName,
            //        FirstName = request.FirstName,
            //        PhoneNumber = request.PhoneNumber,
            //        UserName = request.UserName,
            //        Email = request.Email,
            //    };
            //    https://github.com/leemunroe/responsive-html-email-template
            //    var htmlContent = await _viewRenderService.RenderToStringAsync("_RepliedCommentEmail", emailModel);
            //    await _emailSender.SendEmailAsync(repledUser.Email, "Có người đang trả lời bạn", htmlContent);
            //}
            //return CreatedAtAction(nameof(GetCommentDetail), new { id = knowledgeBaseId, commentId = comment.Id }, new CommentVm()
            //{
            //    Id = comment.Id
            //});


        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }


        //public int? ValidateToken(string token)
        //{
        //    if (token == null)
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_jwtsettings.Secret);
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        //        // return user id from JWT token if validation successful
        //        return userId;
        //    }
        //    catch
        //    {
        //        // return null if validation fails
        //        return null;
        //    }
        //}

    }
}
