namespace Flowdock.Push
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

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

            var requestMessages = CreateRequestMessagesForAllApiTokens(message);

            await Dispatch(requestMessages, TeamInboxUrl);
        }

        public Task PushToChatAsync(string content, string externalUserName, string tags = null)
        {
            return PushToChatAsync(new ChatInputMessage(content, externalUserName) {Tags = tags});
        }

        public async Task PushToChatAsync(ChatInputMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var requestMessages = CreateRequestMessagesForAllApiTokens(message);

            await Dispatch(requestMessages, ChatUrl);
        }

        private async Task Dispatch(IEnumerable<HttpRequestMessage> requestMessages, Uri baseAddress)
        {
            var httpClient = new HttpClient() { BaseAddress = baseAddress };
            foreach (var message in requestMessages)
            {
                // TODO aggregate response messages - return/log?
                await httpClient.SendAsync(message);
            }
        }

        private IEnumerable<HttpRequestMessage> CreateRequestMessagesForAllApiTokens<T>(T content)
        {
            return _apiTokens.Select(token => new HttpRequestMessage(HttpMethod.Post, new Uri(token, UriKind.Relative))
                                                  {
                                                      Content = new ObjectContent<T>(content, CreateJsonMediaTypeFormatter())
                                                  });
        }

        private static JsonMediaTypeFormatter CreateJsonMediaTypeFormatter()
        {
            return new JsonMediaTypeFormatter
                       {
                           SerializerSettings =  new JsonSerializerSettings
                                                     {
                                                         NullValueHandling = NullValueHandling.Ignore,
                                                         ContractResolver = new CamelCaseToLowerCasePlusUnderscoresResolver()
                                                     }
                       };
        }
    }
}