using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogManager
{
    public class LogManager
    {
        private List<LogMessage> _messages;

        public LogManager()
        {
            _messages = new List<LogMessage>();
        }

        public LogMessage this[int index]
        {
            get
            {
                if (index < 0 || index >= _messages.Count)
                    throw new IndexOutOfRangeException("Индекс выходит за пределы списка сообщений");
                return _messages[index];
            }
        }

        public int MessageCount => _messages.Count;

        public void AddMessage(LogMessage message)
        {
            _messages.Add(message);
        }

        public IEnumerable<LogMessage> GetMessagesByType(MessageType type)
        {
            return _messages.Where(m => m.Type == type);
        }

        public IEnumerable<LogMessage> GetMessagesByTimeRange(DateTime start, DateTime end)
        {
            return _messages.Where(m => m.Timestamp >= start && m.Timestamp <= end);
        }

        public IEnumerable<LogMessage> GetAllMessages()
        {
            return _messages;
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var message in _messages)
                {
                    writer.WriteLine($"{message.Type}|{message.Timestamp}|{message.Message}");
                }
            }
        }
    }
} 