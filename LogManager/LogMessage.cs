using System;

namespace LogManager
{
    public struct LogMessage
    {
        public MessageType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public LogMessage(MessageType type, DateTime timestamp, string message)
        {
            Type = type;
            Timestamp = timestamp;
            Message = message;
        }
    }
} 