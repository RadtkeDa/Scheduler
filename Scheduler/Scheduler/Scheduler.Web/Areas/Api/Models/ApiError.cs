using System.Collections.Generic;

namespace Scheduler.Web.Areas.Api.Models
{
    public class ApiError
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}