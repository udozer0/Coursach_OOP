using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Coursach_ver2.Model
{
    /// <summary>
    /// Представляет студента в школьной системе.
    /// Содержит информацию об идентификаторе, имени, возрасте, идентификаторе класса, состоянии выбора и связанных оценках.
    /// </summary>
    public class Student : INotifyPropertyChanged
    {
        private string? _id;
        /// <summary>
        /// Идентификатор студента.
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
        /// Имя студента.
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

        private int? _age;
        /// <summary>
        /// Возраст студента.
        /// </summary>
        public int? Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        private string _classId;
        /// <summary>
        /// Идентификатор класса, в котором обучается студент.
        /// </summary>
        public string ClassId
        {
            get => _classId;
            set
            {
                if (_classId != value)
                {
                    _classId = value;
                    OnPropertyChanged(nameof(ClassId));
                }
            }
        }

        private bool _isSelected;
        /// <summary>
        /// Состояние выбора студента.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        /// <summary>
        /// Связанный объект класса.
        /// </summary>
        public Class Class { get; set; }

        /// <summary>
        /// Коллекция оценок студента.
        /// </summary>
        public ObservableCollection<Grade> Grades { get; set; } = new ObservableCollection<Grade>();

        /// <summary>
        /// Конструктор для инициализации студента с заданными параметрами.
        /// </summary>
        /// <param name="name">Имя студента.</param>
        /// <param name="age">Возраст студента.</param>
        /// <param name="_class">Связанный объект класса.</param>
        public Student(string name, int age, Class _class)
        {
            Name = name;
            Age = age;
            Class = _class;
            ClassId = _class.Id;
            Id = Guid.NewGuid().ToString();
            _class.Students.Add(this);
        }

        /// <summary>
        /// Конструктор по умолчанию. Генерирует новый идентификатор для студента.
        /// </summary>
        public Student()
        {
            Id = Guid.NewGuid().ToString();
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