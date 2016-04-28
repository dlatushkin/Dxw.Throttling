namespace Dxw.Throttling.Core
{
    using System.Web.Http.Filters;

    public class ThrottlingFilter : ActionFilterAttribute, IActionFilter
    {
        public ThrottlingFilter()
        {

        }
    }
}
