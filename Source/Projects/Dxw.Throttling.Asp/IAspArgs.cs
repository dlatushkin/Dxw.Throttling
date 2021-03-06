﻿namespace Dxw.Throttling.Asp
{
    using Core;
    using System.Net.Http;

    public interface IAspArgs: IPhased
    {
        HttpRequestMessage Request { get; set; }

        HttpResponseMessage Response { get; set; }
    }
}

