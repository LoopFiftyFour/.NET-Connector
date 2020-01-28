using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Loop54.Properties;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Loop54")]
[assembly: AssemblyCompany("The Loop54 Group AB")]
[assembly: AssemblyProduct("Loop54")]
[assembly: AssemblyCopyright("Copyright ©  2018")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d3db88c7-9433-4e8d-9e4c-b4e1248fdc49")]

// This is the NuGet Package version. It's displayed as "Product version" in Windows Explorer.
[assembly: AssemblyInformationalVersion(PackageSemanticVersion.Full)]

// This is the "File version" displayed in Windows explorer.
// [BUILD_COUNTER] is replaced with the TeamCity build counter when running in TeamCity.
[assembly: AssemblyFileVersion(PackageSemanticVersion.Full + ".[BUILD_COUNTER]")]

// This is the assembly version used for resolving references at compile and run time. It includes only the major version, so that it breaks only when that changes.
[assembly: AssemblyVersion(PackageSemanticVersion.Major + ".0.0.0")]

[assembly: InternalsVisibleTo("Loop54.Tests.NetCore")]
[assembly: InternalsVisibleTo("Loop54.Tests.NetFramework")]

namespace Loop54.Properties
{
    // Use semantic versioning here:
    // *   Major increments for a breaking change;
    // *   Minor increments for a non-breaking feature change;
    // *   Patch increments for non-breaking fixes or performance improvements.
    // Assembly version will only change when Major changes, so references to it should break then and only then.
    internal static class PackageSemanticVersion
    {
        public const string Major = "5";
        public const string Minor = "1";
        public const string Patch = "3";

        public const string Full = Major + "." + Minor + "." + Patch;
    }

    internal class VersionHeaders
    {
        // The names of these headers need to be consistent with the other connectors (Java, PHP, etc.)

        public const string ApiVersionHeader = "Api-Version";
        public const string ApiVersion = "V3";

        public const string LibVersionHeader = "Lib-Version";
        public const string LibVersion = "NET:" + PackageSemanticVersion.Full;
    }
}

// Version history

// 2.0.0
// 2.0.1 stöd för nytt event-system
// 2.0.2 lagt till IP
// 2.0.3 lagt till Referer och Url, samt gjort UserId så att den inte har : i för IPV6[assembly: AssemblyVersion("2.0.3 lagt till Referer och Url, samt gjort UserId så att den inte har : i för IPV6
// 2.0.4 Request.Serialized är nu public
// 2.0.5 late-deserialization av alla objekt i response
// 2.0.6 GZIP
// 2.0.7 metoder för att mäta tid
// 2.0.8 potentiella prestandaförbättringar + mer tidsmätning
// 2.0.9 mera potentiella prestandaförbättringar
// 2.0.10 lagt till UserAgent i alla anrop
// 2.0.11 modifierad logik för fallbacks
// 2.0.12 använder using-statements istället för att explicit stänga och disposa objekt
// 2.0.13 använder X-Forwarded-For som IP om den finns
// 2.0.14 ytterligare prestandajusteringar
// 2.0.15 stöd för att Entities kan ha Hashtable eller ArrayList-attribut
// 2.0.16 escape på Url, Referer och UserAgent
// 2.0.17 Case 113 + 153 + 165
// 2.0.18 Ändrat till 5000 ConnectionLimit
// 2.0.20 ReadDataTime på response
// 2.0.21 Moved to GitHub
// 2.0.22 Moved to GitHub
// 2.1.0 Changed versioning to semantic for NuGet package (instaed of 1.0.0) and assembly version to MAJOR.0.0.0 so it doesn't break for minor upgrades
// 3.0.0 Added .NET Standard 2.0 version. Allowed HttpContext to be passed in explicitly (and it may be null).
// 3.1.0 Removed IP address from UserId.
// 3.2.0 Removed "LibraryVersion" JSON property and updated "Lib-Version" header to be consistent with other connectors.
// 4.0.0 Rewritten library to work with V3 of the api
// 4.0.1 Synchronous calls are no longer deadlocked because of task system
// 4.1.0 Added support for multiple instances of the client
// 4.2.0 Better support for custom request data 
// 5.0.0 Added code samples, removed SearchResponse.Query and AutoCompleteResponse.Query and added support for ScopeAttributeName in scoped AutoComplete
// 5.0.1 Run the scoped query autocomplete test in the code examples project. Changed some code example names and printouts. Added a license document.
// 5.0.2 Update ASP.NET Core version
// 5.0.3 Run more tests in the code examples project (no items returned in search/faceting, related results returned in search).
// 5.0.4 Api-Version value "v3" -> "V3"
// 5.1.0 Added support for Sync request and some related things
// 5.1.1 Better error if the endpoint response is not a valid engine response
// 5.1.2 Allow overriding request headers sent to the engine
