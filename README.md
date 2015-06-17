# .NET-Connector
.NET Wrapper for Loop54 JSON API

## How to use
Add a reference to the DLLs in the /dist folder. For more usage instructions, see http://docs.loop54.com

## Features:

- Wraps Loop54 JSON API with native .NET functions.
- Handles user identification using random-generated cookies. Note: requires a valid HttpContext.Current.
- Uses X-Forwarded-For as client IP if it's available.
- Has a fallback scheme that rewrites *.54proxy.se to *.108proxy.se if some failure conditions are met
- Handles GZIP
- Relays HTTP data to engine:
  - Referer
  - UserAgent
  - Url

## TODO:

- Remove fallback functionality (rarely necessary since engines are very stable and have redundant failover)
- Handle (and use by default) V2.6-style API calls (with quest name in URL)
