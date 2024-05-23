using Coursach_ver2.ViewModel;
using System.Windows;

namespace Coursach_ver2.View
{
    /// <summary>
    /// Логика взаимодействия для ClassRegistrationWindow.xaml
    /// </summary>
    public partial class ClassRegistrationWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр окна для регистрации класса.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public ClassRegistrationWindow(DataBase.AppContext db)
        {
            InitializeComponent();
            DataContext = new ClassRegistrationViewModel(db);
        }
    }
}
