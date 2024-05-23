using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Coursach_ver2.Model
{
    /// <summary>
    /// Представляет класс в школьной системе.
    /// Содержит информацию о классе, включая его идентификатор, название, текущих студентов, преподавателя и максимальное количество студентов.
    /// </summary>
    public class Class : INotifyPropertyChanged
    {
        private string? _id;
        /// <summary>
        /// Идентификатор класса.
        /// </summary>
        public string? Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string? _name;
        /// <summary>
        /// Название класса.
        /// </summary>
        public string? Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private int _currentStudents;
        /// <summary>
        /// Текущее количество студентов в классе.
        /// </summary>
        public int CurrentStudents
        {
            get => _currentStudents;
            set
            {
                if (_currentStudents != value)
                {
                    _currentStudents = value;
                    OnPropertyChanged(nameof(CurrentStudents));
                }
            }
        }

        private string? _teacher;
        /// <summary>
        /// Имя преподавателя класса.
        /// </summary>
        public string? Teacher
        {
            get => _teacher;
            set
            {
                if (_teacher != value)
                {
                    _teacher = value;
                    OnPropertyChanged(nameof(Teacher));
                }
            }
        }

        private int _maxStudents;
        /// <summary>
        /// Максимальное количество студентов в классе.
        /// </summary>
        public int MaxStudents
        {
            get => _maxStudents;
            set
            {
                if (_maxStudents != value)
                {
                    _maxStudents = value;
                    OnPropertyChanged(nameof(MaxStudents));
                }
            }
        }

        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        /// <summary>
        /// Список студентов в классе.
        /// </summary>
        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                if (_students != value)
                {
                    _students = value;
                    OnPropertyChanged(nameof(Students));
                }
            }
        }

        /// <summary>
        /// Фильтрует студентов по заданному тексту поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска студентов.</param>
        /// <returns>Отфильтрованный список студентов.</returns>
        public IEnumerable<Student> FilterStudents(string searchText)
        {
            var filteredStudents = Students.Where(student => student.Name.Contains(searchText)).ToList();
            Students = new ObservableCollection<Student>(filteredStudents);
            OnPropertyChanged(nameof(Students));
            return filteredStudents;
        }

        /// <summary>
        /// Генерирует числовой идентификатор заданной длины.
        /// </summary>
        /// <param name="length">Длина идентификатора.</param>
        /// <returns>Сгенерированный идентификатор.</returns>
        private string GenerateNumericId(int length)
        {
            return string.Concat(Enumerable.Range(0, length).Select(_ => new Random().Next(10).ToString()));
        }

        /// <summary>
        /// Конструктор по умолчанию. Генерирует новый идентификатор для класса.
        /// </summary>
        public Class()
        {
            Id = GenerateNumericId(8);
        }

        /// <summary>
        /// Конструктор для инициализации класса с заданными параметрами.
        /// </summary>
        /// <param name="name">Название класса.</param>
        /// <param name="teacher">Имя преподавателя.</param>
        /// <param name="currentStudents">Текущее количество студентов.</param>
        /// <param name="maxStudents">Максимальное количество студентов.</param>
        public Class(string name, string teacher, int currentStudents, int maxStudents)
        {
            Name = name;
            Teacher = teacher;
            CurrentStudents = currentStudents;
            MaxStudents = maxStudents;
            Id = GenerateNumericId(8);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Уведомляет об изменении свойства.
        /// </summary>
        /// <param name="prop">Имя изменившегося свойства.</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}