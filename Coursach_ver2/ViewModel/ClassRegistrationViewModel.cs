using Coursach_ver2.Hellper;
using Coursach_ver2.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// ViewModel для регистрации нового класса.
    /// </summary>
    public class ClassRegistrationViewModel : INotifyPropertyChanged
    {
        private readonly Regex _classNameRegex = new Regex(@"^(1[0-1]|[1-9])[а-яА-Я]$", RegexOptions.Compiled);

        private readonly string _surnameRegex = @"^[А-Яа-яЁё]+$"; // Регулярное выражение для фамилии на русском
        private readonly DataBase.AppContext _db;
        private string _name;
        private int _maxStudents;
        private string _teacher;

        /// <summary>
        /// Название класса.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_classNameRegex.IsMatch(value))
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
                else
                {
                    MessageBox.Show("Название класса должно содержать число от 1 до 11 и одну букву русского алфавита от а до я.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Фамилия учителя.
        /// </summary>
        public string Teacher
        {
            get { return _teacher; }
            set
            {
                // Проверяем, соответствует ли введенное значение регулярному выражению для фамилии на русском
                if (Regex.IsMatch(value, _surnameRegex))
                {
                    _teacher = value;
                    OnPropertyChanged(nameof(Teacher));
                }
                else
                {
                    MessageBox.Show("Фамилия должна содержать только русские буквы.");
                }
            }
        }

        /// <summary>
        /// Максимальное количество студентов в классе.
        /// </summary>
        public int MaxStudents
        {
            get { return _maxStudents; }
            set { _maxStudents = value; OnPropertyChanged(nameof(MaxStudents)); }
        }

        /// <summary>
        /// Коллекция классов.
        /// </summary>
        public ObservableCollection<Class> Classes { get; set; }

        private RelayCommand _addClassCommand;
        /// <summary>
        /// Команда для добавления нового класса.
        /// </summary>
        public RelayCommand AddClassCommand
        {
            get
            {
                return _addClassCommand ??
                    (_addClassCommand = new RelayCommand(obj =>
                    {
                        if (Name != null && Teacher != null && MaxStudents != 0)
                        {
                            Class _class = new Class(Name, Teacher, 0, MaxStudents);
                            _db.Classes.Add(_class);
                            _db.SaveChanges();
                            MessageBox.Show("Класс создан и сохранен в БД");
                            CloseWindow();
                        }
                        else
                        {
                            MessageBox.Show("Одно из полей не заполнено!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }));
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр ViewModel для регистрации нового класса.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        public ClassRegistrationViewModel(DataBase.AppContext db)
        {
            _db = db;
            LoadClasses();
        }

        /// <summary>
        /// Загружает классы из базы данных.
        /// </summary>
        private void LoadClasses()
        {
            Classes = new ObservableCollection<Class>(_db.Classes.ToList());
        }

        /// <summary>
        /// Закрывает окно.
        /// </summary>
        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
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
