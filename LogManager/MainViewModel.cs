using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.ComponentModel;
using System.Linq;

namespace LogManager
{
    public class MainViewModel : ViewModelBase
    {
        private readonly LogManager _logManager;
        private ObservableCollection<LogMessage> _messages;
        private string _newMessage = string.Empty;
        private MessageType _selectedMessageType;
        private DateTime _startDate;
        private DateTime _endDate;
        private ListSortDirection _currentSortDirection = ListSortDirection.Ascending;
        private string _currentSortProperty = "Timestamp";

        public MainViewModel()
        {
            _logManager = new LogManager();
            _messages = new ObservableCollection<LogMessage>();
            _selectedMessageType = MessageType.Information;
            _startDate = DateTime.Now.AddDays(-1);
            _endDate = DateTime.Now;

            AddMessageCommand = new RelayCommand(AddMessage);
            FilterByTypeCommand = new RelayCommand(FilterByType);
            FilterByDateRangeCommand = new RelayCommand(FilterByDateRange);
            SaveToFileCommand = new RelayCommand(SaveToFile);
            ResetFiltersCommand = new RelayCommand(ResetFilters);
        }

        public ObservableCollection<LogMessage> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public MessageType SelectedMessageType
        {
            get => _selectedMessageType;
            set
            {
                _selectedMessageType = value;
                OnPropertyChanged(nameof(SelectedMessageType));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand AddMessageCommand { get; }
        public ICommand FilterByTypeCommand { get; }
        public ICommand FilterByDateRangeCommand { get; }
        public ICommand SaveToFileCommand { get; }
        public ICommand ResetFiltersCommand { get; }

        public void Sort(string propertyName)
        {
            if (_currentSortProperty == propertyName)
            {
                _currentSortDirection = _currentSortDirection == ListSortDirection.Ascending 
                    ? ListSortDirection.Descending 
                    : ListSortDirection.Ascending;
            }
            else
            {
                _currentSortProperty = propertyName;
                _currentSortDirection = ListSortDirection.Ascending;
            }

            var sortedMessages = _currentSortDirection == ListSortDirection.Ascending
                ? _messages.OrderBy(m => GetPropertyValue(m, propertyName))
                : _messages.OrderByDescending(m => GetPropertyValue(m, propertyName));

            Messages = new ObservableCollection<LogMessage>(sortedMessages);
        }

        private object GetPropertyValue(LogMessage message, string propertyName)
        {
            return propertyName switch
            {
                "Type" => message.Type,
                "Timestamp" => message.Timestamp,
                "Message" => message.Message,
                _ => throw new ArgumentException($"Неизвестное свойство: {propertyName}")
            };
        }

        private void AddMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
                return;

            var message = new LogMessage(SelectedMessageType, DateTime.Now, NewMessage);
            _logManager.AddMessage(message);
            Messages.Add(message);
            NewMessage = string.Empty;
        }

        private void FilterByType()
        {
            Messages.Clear();
            foreach (var message in _logManager.GetMessagesByType(SelectedMessageType))
            {
                Messages.Add(message);
            }
        }

        private void FilterByDateRange()
        {
            Messages.Clear();
            foreach (var message in _logManager.GetMessagesByTimeRange(StartDate, EndDate))
            {
                Messages.Add(message);
            }
        }

        private void SaveToFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _logManager.SaveToFile(saveFileDialog.FileName);
            }
        }

        private void ResetFilters()
        {
            Messages.Clear();
            foreach (var message in _logManager.GetAllMessages())
            {
                Messages.Add(message);
            }
        }
    }
} 