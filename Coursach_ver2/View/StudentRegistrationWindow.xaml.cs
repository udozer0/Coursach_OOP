using Coursach_ver2.ViewModel;
using System.Windows;

namespace Coursach_ver2.View
{
    /// <summary>
    /// Окно регистрации студента
    /// </summary>
    public partial class StudentRegistrationWindow : Window
    {
        /// <summary>
        /// Инициализирует новое окно регистрации студента.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public StudentRegistrationWindow(DataBase.AppContext db)
        {
            InitializeComponent();
            DataContext = new StudentRegistrationViewModel(db);
        }

    }
}
