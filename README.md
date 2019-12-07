# ASPNet Core Api Template

[![Build status](https://ci.appveyor.com/api/projects/status/aratl2f9pd3fyykw/branch/master?svg=true)](https://ci.appveyor.com/project/MirzaMerdovic/dotnetcore-webapistarter/branch/master) [![CodeFactor](https://www.codefactor.io/repository/github/mirzamerdovic/dotnetcore-webapistarter/badge)](https://www.codefactor.io/repository/github/mirzamerdovic/dotnetcore-webapistarter) [![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FDotNetCore-WebApiStarter.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FDotNetCore-WebApiStarter?ref=badge_shield)

# Introduction

A thin template that should give you benefit of having the common stuff setup and couple of extra things that might be useful to people who are learning.

# Where can I get it?

The template is available on [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=lazybyte.LazyByte-AspNetCore-WebApiStarterTemplate)  

# Why do I need it?
In all honesty you don't it's just a template after all, but in case you are frequently building Web APIs you may found it useful, or may found parts that you can re-use to build one for yourself.  

In case you are still reading this is what template brings on the table:
* Configuration setup which includes appsettings.{env}.json transformation and reading the data using the [Options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1)
* [CORS](https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1) that is configure to allow everyhting, but it should be very easy to configure it for a real world scenario.
* [Response caching](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-3.1)
* Health check implementation using [readiness and liveness probes](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1#separate-readiness-and-liveness-probes)
* Swagger
* FooController - a simple API controller:
    * Routing
    * HTTP response messages using IActionResult
    * File upload example that support complex models which means POCO class with properties of type IFormFile
* Customer middleware example

# Feedback
If you have any issues or suggestion that you believe might make the template better please don't hestitate to let me know and if it's not a big bother please use one of the templates: [bug](https://github.com/MirzaMerdovic/DotNetCore-WebApiStarter/issues/new?template=bug_report.md)/[new feature](https://github.com/MirzaMerdovic/DotNetCore-WebApiStarter/issues/new?template=feature_request.md)
