using Coursach_ver2.Hellper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// ViewModel для управления стартовым окном.
    /// </summary>
    public class StartUpWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Команда для начала работы.
        /// </summary>
        public RelayCommand StartWorkCommand { get; private set; }

        /// <summary>
        /// Команда для выхода из приложения.
        /// </summary>
        public RelayCommand ExitCommand { get; private set; }

        private ObservableCollection<string> _userRoles;

        /// <summary>
        /// Список ролей пользователя.
        /// </summary>
        public ObservableCollection<string> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                OnPropertyChanged(nameof(UserRoles));
                // Выбираем первый элемент при установке списка
                SelectedUserRole = _userRoles[0];
            }
        }

        private string _selectedUserRole;

        /// <summary>
        /// Выбранная роль пользователя.
        /// </summary>
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;
                OnPropertyChanged(nameof(SelectedUserRole));
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр ViewModel для стартового окна.
        /// </summary>
        public StartUpWindowViewModel()
        {
            StartWorkCommand = new RelayCommand(OpenMainWindow);
            ExitCommand = new RelayCommand(CloseApplication);
            // Заполняем список ролей пользователя
            UserRoles = new ObservableCollection<string>
            {
                "Ученик",       // Роль ученика
                "Преподаватель",   // Роль преподавателя
                "Директор"      // Роль директора
            };
        }

        /// <summary>
        /// Метод для открытия главного окна.
        /// </summary>
        private void OpenMainWindow(object parameter)
        {
            // Создаем и открываем главное окно с передачей выбранной роли
            var mainWindow = new MainWindow(SelectedUserRole);
            mainWindow.Show();

            // Закрываем текущее окно
            CloseCurrentWindow(parameter);
        }

        /// <summary>
        /// Метод для закрытия приложения.
        /// </summary>
        private void CloseApplication(object parameter)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Метод для закрытия текущего окна.
        /// </summary>
        private void CloseCurrentWindow(object parameter)
        {
            var window = parameter as System.Windows.Window;
            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод вызова события изменения свойства.
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
