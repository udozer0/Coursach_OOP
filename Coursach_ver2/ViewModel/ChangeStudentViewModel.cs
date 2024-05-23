using Coursach_ver2.Hellper;
using Coursach_ver2.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// ViewModel для изменения данных студента.
    /// </summary>
    public class ChangeStudentViewModel : INotifyPropertyChanged
    {
        private DataBase.AppContext _db;
        private string _name;
        private int _age;
        private Class _selectedClass;
        private Student _selectedStudent;

        /// <summary>
        /// Имя студента.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Возраст студента.
        /// </summary>
        public int Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Выбранный студент.
        /// </summary>
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                if (_selectedStudent != null)
                {
                    Name = _selectedStudent.Name;
                    Age = (int)_selectedStudent.Age;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция классов.
        /// </summary>
        public ObservableCollection<Class> Classes { get; set; }

        /// <summary>
        /// Выбранный класс.
        /// </summary>
        public Class SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        private RelayCommand _changeStudentCommand;
        /// <summary>
        /// Команда для изменения данных студента.
        /// </summary>
        public RelayCommand ChangeStudentCommand
        {
            get
            {
                return _changeStudentCommand ??
                    new RelayCommand(obj =>
                    {
                        var oldClass = SelectedStudent.Class;
                        SelectedStudent.Name = Name;
                        SelectedStudent.Age = Age;
                        SelectedStudent.Class = SelectedClass;

                        var studentInDb = _db.Students.FirstOrDefault(s => s.Id == SelectedStudent.Id);
                        if (studentInDb != null)
                        {
                            studentInDb.Name = SelectedStudent.Name;
                            studentInDb.Age = SelectedStudent.Age;
                            studentInDb.ClassId = SelectedClass.Id;
                            studentInDb.Class = SelectedClass;
                            var classInDb = _db.Classes.FirstOrDefault(s => s.Id == SelectedClass.Id);
                            var oldClassInDb = _db.Classes.FirstOrDefault(s => s.Id == oldClass.Id);
                            oldClassInDb.CurrentStudents--;
                            classInDb.CurrentStudents++;
                            try
                            {
                                _db.SaveChanges();
                                MessageBox.Show("Ученик изменен");
                                CloseWindow();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при редактировании ученика: {ex.Message}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ученик не найден в БД");
                        }
                    });
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр ViewModel для изменения данных студента.
        /// </summary>
        /// <param name="db">Контекст базы данных.</param>
        /// <param name="student">Студент, данные которого будут изменяться.</param>
        public ChangeStudentViewModel(DataBase.AppContext db, Student student)
        {
            _db = db;
            SelectedStudent = student;
            LoadClasses();
        }

        /// <summary>
        /// Загружает классы из базы данных.
        /// </summary>
        private void LoadClasses()
        {
            Classes = new ObservableCollection<Class>(_db.Classes.ToList());
            SelectedClass = Classes.FirstOrDefault(c => c.Id == SelectedStudent.ClassId);
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
