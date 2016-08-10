namespace Dxw.Throttling.Asp
{
    using Core;
    using System.Net.Http;

    public interface IAspArgs
    {
        EventPhase Phase { get; }

        HttpRequestMessage Request { get; set; }

        HttpResponseMessage Response { get; set; }
    }
}

