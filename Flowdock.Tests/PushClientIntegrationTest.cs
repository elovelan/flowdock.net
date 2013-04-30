namespace Flowdock.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Push;

    using Xunit;

    public class PushClientIntegrationTest
    {
        private readonly string[] _apiToken;

        public PushClientIntegrationTest()
        {
            _apiToken = ConfigurationManager.AppSettings.AllKeys
                            .Select(key => ConfigurationManager.AppSettings[key]).ToArray();
        }

        [Fact]
        public void Push_three_messages_to_a_team_room()
        {
            var pushClient = new PushClient(_apiToken);
            var tasks = new List<Task>();
            
            for (var i = 0; i < 3; i++)
            {
                pushClient.PushToTeamAsync("PushClientTest", "foo@bar.org", "Tester", "Test from integration test. #" + i);
            }
            
            Task.WaitAll(tasks.ToArray());
        }

        [Fact]
        public void Push_three_messages_to_a_chat()
        {
            var pushClient = new PushClient(_apiToken);
            var tasks = new List<Task>();

            for (var i = 0; i < 3; i++)
            {
                pushClient.PushToChatAsync("Hello from integration test #" + i, "int_test");
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}