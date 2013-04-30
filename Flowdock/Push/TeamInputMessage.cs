namespace Flowdock.Push
{
    using System;

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
}