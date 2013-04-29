namespace Flowdock.Push
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class PushClient
    {
        private static readonly Uri ApiBaseUri = new Uri("https://api.flowdock.com/v1/messages/");
        private static readonly Uri TeamInboxUrl = new Uri(ApiBaseUri, "team_inbox/");
        private static readonly Uri ChatUrl = new Uri(ApiBaseUri, "chat/");

        private readonly string[] _apiTokens;

        public PushClient(params string[] apiTokens)
        {
            if (apiTokens == null) throw new ArgumentNullException("apiTokens");

            _apiTokens = apiTokens;
        }

        public Task PushToTeamAsync(string source, string fromAddress, string subject, string content,
                                    string fromName = null, string replyTo = null, string project = null,
                                    string tags = null, string links = null)
        {
            return PushToTeamAsync(new TeamInputMessage(source, fromAddress, subject, content)
                           {
                               FromName = fromName,
                               ReplyTo = replyTo,
                               Project = project,
                               Tags = tags,
                               Links = links
                           });
        }

        public async Task PushToTeamAsync(TeamInputMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var httpClientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = TeamInboxUrl };

            foreach (var request in _apiTokens.Select(token => new HttpRequestMessage(HttpMethod.Post,
                                                                                      new Uri(token, UriKind.Relative))))
            {
                request.Content = new ObjectContent<TeamInputMessage>(message, new CustomJsonMediaTypeFormatter());
                await httpClient.SendAsync(request);
            }
        }

        public Task PushToChatAsync(string content, string externalUserName, string tags = null)
        {
            return PushToChatAsync(new ChatInputMessage(content, externalUserName) {Tags = tags});
        }

        public async Task PushToChatAsync(ChatInputMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var httpClientHandler = new HttpClientHandler();

            var httpClient = new HttpClient(httpClientHandler) { BaseAddress = ChatUrl };

            foreach (var request in _apiTokens.Select(token => new HttpRequestMessage(HttpMethod.Post,
                                                                                      new Uri(token, UriKind.Relative))))
            {
                request.Content = new ObjectContent<ChatInputMessage>(message, new CustomJsonMediaTypeFormatter());
                await httpClient.SendAsync(request);
            }
        }
    }

    public class ChatInputMessage
    {
        // Required properties
        public string Content { get; set; }
        public string ExternalUserName { get; set; }

        // Optional properties
        public string Tags { get; set; }

        public ChatInputMessage(string content, string externalUserName)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (externalUserName == null) throw new ArgumentNullException("externalUserName");

            Content = content;
            ExternalUserName = externalUserName;
        }
    }

    internal class CustomJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonMediaTypeFormatter()
        {
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.ContractResolver = new CamelCaseToLowerCasePlusUnderscoresResolver();
        }
    }

    public class TeamInputMessage
    {
        // Required properties
        public string Source { get; private set; }
        public string FromAddress { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }

        // Optional properties
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Project { get; set; }
        public string Tags { get; set; }
        public string Links { get; set; }

        public TeamInputMessage(string source, string fromAddress, string subject, string content)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (fromAddress == null) throw new ArgumentNullException("fromAddress");
            if (subject == null) throw new ArgumentNullException("subject");
            if (content == null) throw new ArgumentNullException("content");

            Source = source;
            FromAddress = fromAddress;
            Subject = subject;
            Content = content;
        }
    }

    public class CamelCaseToLowerCasePlusUnderscoresResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, @"(\p{Ll})(\p{Lu})", "$1_$2", RegexOptions.Compiled).ToLower();
        }
    }
}