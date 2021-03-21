using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Hubs;
using SignalRDemo.Models;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Interface;

namespace SignalRDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        public AdminController(
            IHubContext<NotificationHub> notificationHubContext,
            IHubContext<NotificationUserHub> notificationUserHubContext,
            IUserConnectionManager userConnectionManager)
        {
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Article article)
        {
            await _notificationHubContext.Clients.All
                .SendAsync
                (
                    "sendToUser",
                    article.Heading,
                    article.Content
                );

            return View();
        }

        public IActionResult SendToSpecificUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SendToSpecificUser(Article article)
        {
            var connections = _userConnectionManager.GetUserConnections(article.userId);

            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _notificationUserHubContext.Clients
                        .Client(connectionId)
                        .SendAsync
                        (
                            "sendToUser",
                            article.Heading,
                            article.Content
                        );
                }
            }
            return View();
        }
    }
}