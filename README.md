# .NET-Connector
.NET Wrapper for Loop54 JSON API

## How to use
Add a reference to the DLLs in the /dist folder. For more usage instructions, see http://docs.loop54.com

## Features

- Wraps Loop54 JSON API with native .NET functions.
- Handles user identification using random-generated cookies. Note: requires a valid HttpContext.Current.
- Uses X-Forwarded-For as client IP if it's available.
- Has options for compatibility with different engine versions.
- Configurable HTTP timeout.
- Built-in time measuring of different steps of the request/response cycle (for debugging/performance analysis).
- Handles GZIP.
- Uses HTTP Keep-Alive if the engine endpoint supports it.
- Relays HTTP data to engine:
  - Referer
  - UserAgent
  - Url
  - Library version
  
## Backward compatibility

When implementing against older versions of Loop54 engines (or new engines which have been configured with compatibility bridges to be compatible with older API:s), custom options need to be set. To set custom options, create a Loop54.RequestOptions object and pass it to the Loop54.Request constructor.

To enable backward compatibility:

- Pre 2.6 engines: Set V25Url to true in the options object.
- Pre 2.3 engines: Set V22Collections to true in the options object.
  
## TODO
- NuGet package
