using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("feed")]
    public class FeedController : ControllerBase
    {
        private readonly IPostsRepo _posts;

        public FeedController(IPostsRepo posts)
        {
            _posts = posts;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest, [FromHeader] string authorization)
        {
            
            string encodedData = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Substring("Base ".Length)));
            Console.WriteLine(encodedData);
            string[] userNamePassword = encodedData.Split(":");
            string userName = userNamePassword[0];
            string password = userNamePassword[1];
            
            Console.WriteLine(userName);
            Console.WriteLine(password);

            var posts = _posts.SearchFeed(searchRequest);
            var postCount = _posts.Count(searchRequest);
            return FeedModel.Create(searchRequest, posts, postCount);
        }
    }
}
