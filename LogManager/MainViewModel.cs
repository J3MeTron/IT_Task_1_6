using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.ComponentModel;
using System.Linq;
using System.Globalization;

namespace LogManager
{
    public class MainViewModel : ViewModelBase
    {
        private readonly LogManager _logManager;
        private ObservableCollection<LogMessage> _messages;
        private string _newMessage = string.Empty;
        private MessageType _selectedMessageType;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private ListSortDirection _currentSortDirection = ListSortDirection.Ascending;
        private string _currentSortProperty = "Timestamp";

        public MainViewModel()
        {
            _logManager = new LogManager();
            _messages = new ObservableCollection<LogMessage>();
            _selectedMessageType = MessageType.Information;
            _startDateTime = DateTime.Now.Date;
            _endDateTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

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
                OnPropertyChanged(nameof(MessageCount));
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
            get => _startDateTime.Date;
            set
            {
                _startDateTime = value.Date.Add(_startDateTime.TimeOfDay);
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public DateTime EndDate
        {
            get => _endDateTime.Date;
            set
            {
                _endDateTime = value.Date.Add(_endDateTime.TimeOfDay);
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public string StartTime
        {
            get => _startDateTime.ToString("HH:mm:ss");
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                {
                    _startDateTime = _startDateTime.Date.Add(time.TimeOfDay);
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public string EndTime
        {
            get => _endDateTime.ToString("HH:mm:ss");
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                {
                    _endDateTime = _endDateTime.Date.Add(time.TimeOfDay);
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }

        public ICommand AddMessageCommand { get; }
        public ICommand FilterByTypeCommand { get; }
        public ICommand FilterByDateRangeCommand { get; }
        public ICommand SaveToFileCommand { get; }
        public ICommand ResetFiltersCommand { get; }

        public int MessageCount => Messages.Count;

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

            // Используем текущее время с точностью до секунд
            var currentTime = DateTime.Now;
            currentTime = new DateTime(
            currentTime.Year, currentTime.Month, currentTime.Day,
            currentTime.Hour, currentTime.Minute, currentTime.Second
            );
            var message = new LogMessage(SelectedMessageType, currentTime, NewMessage);
            _logManager.AddMessage(message);
            Messages.Add(message);
            NewMessage = string.Empty;
        }

        private void FilterByType()
        {
            var filteredMessages = _logManager.GetMessagesByType(SelectedMessageType).ToList();
            Messages = new ObservableCollection<LogMessage>(filteredMessages);
        }

        private void FilterByDateRange()
        {
            // Используем полное значение DateTime с учетом времени
            var startDate = _startDateTime;
            var endDate = _endDateTime;

            // Округляем до секунд, чтобы избежать проблем с миллисекундами
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second);

            // Если время конца меньше времени начала, считаем что это следующий день
            if (endDate < startDate)
            {
                endDate = endDate.AddDays(1);
            }

            var filteredMessages = _logManager.GetMessagesByTimeRange(startDate, endDate).ToList();
            Messages = new ObservableCollection<LogMessage>(filteredMessages);
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
            var allMessages = _logManager.GetAllMessages().ToList();
            Messages = new ObservableCollection<LogMessage>(allMessages);
            
            // Сброс времени на текущее
            _startDateTime = DateTime.Now.Date;
            _endDateTime = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(EndDate));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
        }
    }
} 