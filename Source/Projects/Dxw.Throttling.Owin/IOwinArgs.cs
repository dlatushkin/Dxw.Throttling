namespace Dxw.Throttling.Owin
{
    using Microsoft.Owin;

    using Core;

    public interface IOwinArgs
    {
        EventPhase Phase { get; }

        IOwinContext OwinContext { get; set; }
    }
}

