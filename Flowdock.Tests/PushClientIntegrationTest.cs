namespace Flowdock.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Push;

    using Xunit;

    public class PushClientIntegrationTest
    {
        private const string ApiToken = "API-TOKEN-GOES-HERE";

        [Fact]
        public void Push_ten_messages_to_a_team_room()
        {
            var pushClient = new PushClient(ApiToken);
            var tasks = new List<Task>();
            
            for (var i = 0; i < 10; i++)
            {
                pushClient.PushToTeamAsync("PushClientTest", "Foo@Bar.org", "Tester", "Test from integration test. #" + i);
            }
            
            Task.WaitAll(tasks.ToArray());
        }
    }
}