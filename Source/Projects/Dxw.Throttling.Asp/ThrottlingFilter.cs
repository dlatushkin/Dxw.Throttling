namespace Dxw.Throttling.Asp
{
    using System.Web.Http.Filters;

    public class ThrottlingFilter : ActionFilterAttribute, IActionFilter
    {
        public ThrottlingFilter()
        {

        }
    }
}
