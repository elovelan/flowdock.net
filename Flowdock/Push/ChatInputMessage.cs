namespace Flowdock.Push
{
    using System;

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
}