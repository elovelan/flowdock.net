flowdock.net
============

Flowdock (www.flowdock.com) API client for .NET


To run the integration tests, use an xUnit.NET runner and add a ApiTokens.config file
to the Flowdock.Tests project/directory, containing an appSettings section. For each
flow you would like to send the integration test output to, add a key/value pair (
the key name can be anything).

Currently are only the Push operations supported, but I've planned to implement the
Streaming and REST APIs as well.
