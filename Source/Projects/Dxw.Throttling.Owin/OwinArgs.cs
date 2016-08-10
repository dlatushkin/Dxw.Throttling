namespace Dxw.Throttling.Owin
{
    using System;
    using System.Net.Http;
    using Core;
    using Microsoft.Owin;

    public class OwinArgs : IOwinArgs
    {
        public EventPhase Phase { get; set; }

        public IOwinContext OwinContext { get; set; }
    }
}
