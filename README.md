# .NET-Connector
.NET Wrapper for Loop54 JSON API

## How to use
Add a reference to the DLLs in the /dist folder. For more usage instructions, see http://docs.loop54.com

## Features:

- Wraps Loop54 JSON API with native .NET functions.
- Handles user identification using random-generated cookies. Note: requires a valid HttpContext.Current.
- Uses X-Forwarded-For as client IP if it's available.
- Has options for compatibility with different engine versions
- Handles GZIP
- Relays HTTP data to engine:
  - Referer
  - UserAgent
  - Url

##TODO
- Better documentation
