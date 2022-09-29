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

        private readonly IUsersRepo _users;
        public FeedController(IPostsRepo posts, IUsersRepo users)
        {
            _posts = posts;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<FeedModel> GetFeed([FromQuery] FeedSearchRequest searchRequest, [FromHeader] string authorization)
        {
            // if(_users.IsValidAccount(authorization))
            // {
                var posts = _posts.SearchFeed(searchRequest);
                var postCount = _posts.Count(searchRequest);
                return FeedModel.Create(searchRequest, posts, postCount);
            // }
            // return BadRequest();
        }
    }
}
