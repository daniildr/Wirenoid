using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wirenoid.WebApi.DbItems;
using Wirenoid.WebApi.DbItems.Models;
using System.Net;

namespace Wirenoid.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaunchesController : ControllerBase
    {
        private readonly CacheDbContext _context;

        public LaunchesController(CacheDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaunchedContainer>>> GetLaunches() => await _context.launchedContainers.ToListAsync();

        [HttpGet("dockerId")]
        public ActionResult<LaunchedContainer> GetLaunchByDockerId(string dockerId) => _context.launchedContainers.First(x => x.DockerId == dockerId);

        [HttpGet("url")]
        public ActionResult<LaunchedContainer> GetLaunchByUrl(string url) => _context.launchedContainers.First(x => x.Url == url);

        [HttpGet("guid")]
        public ActionResult<LaunchedContainer> GetLaunchById(Guid guid) => _context.launchedContainers.First(x => x.Id == guid);

        [HttpDelete]
        public ActionResult<HttpStatusCode> DeleteLaunch(Guid guid)
        {
            try
            {
                var container = _context.launchedContainers.First(x => x.Id == guid);
                _context.launchedContainers.Remove(container);
                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost]
        public ActionResult<HttpStatusCode> AddLaunch(string dockerId, string url)
        {
            try
            {
                _context.launchedContainers.Add(
                    new LaunchedContainer
                    {
                        Id = Guid.NewGuid(),
                        DockerId = dockerId,
                        Url = url
                    });
                _context.SaveChanges();
                return HttpStatusCode.OK;
            }
            catch (Exception) 
            { 
                return HttpStatusCode.InternalServerError; 
            }
        }
    }
}
