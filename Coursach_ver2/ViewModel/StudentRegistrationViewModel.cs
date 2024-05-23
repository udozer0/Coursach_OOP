using Coursach_ver2.Hellper;
using Coursach_ver2.Model;
using Coursach_ver2.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// ViewModel для регистрации студентов.
    /// </summary>
    public class StudentRegistrationViewModel : INotifyPropertyChanged
    {
        DataBase.AppContext _db;
        private Class _selectedClass;

        private string _name; // Фамилия студента
        private string _surnameRegex = @"^[А-Яа-яЁё]+$"; // Регулярное выражение для фамилии на русском

        /// <summary>
        /// Фамилия студента.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                // Проверяем, соответствует ли введенное значение регулярному выражению для фамилии на русском
                if (Regex.IsMatch(value, _surnameRegex))
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
                else
                {
                    MessageBox.Show("Фамилия должна содержать только русские буквы.");
                }
            }
        }

        private int _age;
        private string _ageRegex = @"^\d+$"; // Регулярное выражение для возраста (только цифры)

        /// <summary>
        /// Возраст студента.
        /// </summary>
        public int Age
        {
            get { return _age; }
            set
            {
                // Проверяем, соответствует ли введенное значение регулярному выражению для возраста (только цифры)
                if (Regex.IsMatch(value.ToString(), _ageRegex))
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
                else
                {
                    MessageBox.Show("Возраст должен содержать только цифры.");
                }
            }
        }
        public ObservableCollection<Class> Classes { get; set; }

        /// <summary>
        /// Выбранный класс для студента.
        /// </summary>
        public Class SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); }
        }

        private RelayCommand addStudentCommand;

        /// <summary>
        /// Команда для добавления студента.
        /// </summary>
        public RelayCommand AddStudentCommand
        {
            get
            {
                return addStudentCommand ??
                    new RelayCommand(obj =>
                    {
                        if (Name != null && Age != 0 && SelectedClass != null)
                        {
                            Student student = new Student(Name, Age, SelectedClass);

                            SelectedClass.Students.Add(student);
                            _db.Students.Add(student);
                            _db.SaveChanges();
                            MessageBox.Show("Ученик сохранен.");
                            // Закрыть окно после сохранения
                            CloseWindow();
                        }
                        else
                        {
                            MessageBox.Show("Одно из полей не заполнено!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
            }

        }

        /// <summary>
        /// Инициализирует новый экземпляр ViewModel для регистрации студентов.
        /// </summary>
        public StudentRegistrationViewModel(DataBase.AppContext db)
        {
            _db = db;
            LoadClasses();
        }

        /// <summary>
        /// Загружает доступные классы.
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
