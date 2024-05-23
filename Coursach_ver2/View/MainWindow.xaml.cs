using System.Windows;
using Coursach_ver2.Model;
using Coursach_ver2.ViewModel;

namespace Coursach_ver2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр главного окна приложения.
        /// </summary>
        /// <param name="selectedUserRole">Роль выбранного пользователя.</param>
        public MainWindow(string selectedUserRole)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(selectedUserRole);
        }

        /// <summary>
        /// Обработчик события изменения выбранного элемента в дереве.
        /// </summary>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SelectedStudent = e.NewValue as Student;
            }
        }
    }
}
