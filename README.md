# .NET-Connector
.NET Wrapper for Loop54 JSON API

## How to use
The Loop54 Connector is available for download as a NuGet package. For installation instructions, see https://www.nuget.org/packages/Loop54.Connector/. For usage instructions, see http://docs.loop54.com.

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
Loop54 engines are (as of 3.1) able to use different API versions depending on the version of the library that connects to it. Contact your technical administrator at Loop54 to make sure that your engine is correctly configured.
