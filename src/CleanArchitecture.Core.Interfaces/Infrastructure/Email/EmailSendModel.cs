using System.Collections.Generic;

namespace CleanArchitecture.Core.Interfaces.Infrastructure.Email
{
    public class EmailSendModel
    {
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string> Substitutions { get; set; }
    }
}
