using Coursach_ver2.Model;
using Coursach_ver2.ViewModel;
using System.Windows;

namespace Coursach_ver2.View
{
    /// <summary>
    /// Логика взаимодействия для ChangeStudentWindow.xaml
    /// </summary>
    public partial class ChangeStudentWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр окна для изменения данных студента.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        /// <param name="student">Студент, данные которого будут изменяться.</param>
        public ChangeStudentWindow(DataBase.AppContext db, Student student)
        {
            InitializeComponent();
            DataContext = new ChangeStudentViewModel(db, student);
        }
    }
}
