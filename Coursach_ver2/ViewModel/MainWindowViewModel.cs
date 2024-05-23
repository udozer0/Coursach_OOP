using Coursach_ver2.Hellper;
using Coursach_ver2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Coursach_ver2.DataBase;
using System.Windows;
using Coursach_ver2.View;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.Windows.Threading;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// Класс, представляющий модель представления для главного окна приложения. 
    /// Он управляет данными, отображаемыми на главном окне, включая список студентов, классов и предметов,
    /// а также логику сортировки и отображения информации о количестве студентов.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Текст, отображающий информацию о количестве отфильтрованных студентов.
        /// </summary>
        private string _filteredStudentCountText;

        /// <summary>
        /// Свойство для доступа к тексту с информацией о количестве отфильтрованных студентов.
        /// </summary>
        public string FilteredStudentCountText
        {
            get => _filteredStudentCountText;
            set
            {
                _filteredStudentCountText = value;
                OnPropertyChanged(nameof(FilteredStudentCountText));
            }
        }

        /// <summary>
        /// Обновляет текст, отображающий количество отфильтрованных студентов.
        /// </summary>
        /// <param name="allSubStudents">Общее количество отфильтрованных студентов.</param>
        private void UpdateFilteredStudentCountText(int allSubStudents)
        {
            FilteredStudentCountText = $"Показано {allSubStudents} из {AllStudents.Count} студентов";
        }
        /// <summary>
        /// Указывает, нужно ли сортировать учеников.
        /// </summary>
        private bool _sortStudentsIndex;

        /// <summary>
        /// Свойство для доступа к управлению сортировкой учеников.
        /// </summary>
        public bool SortStudentsIndex
        {
            get { return _sortStudentsIndex; }
            set
            {
                _sortStudentsIndex = value;
                OnPropertyChanged(nameof(SortStudentsIndex));
                SortStudents();
            }
        }
        /// <summary>
        /// Указывает, нужно ли сортировать классы.
        /// </summary>
        private bool _sortClassesIndex;

        /// <summary>
        /// Свойство для доступа к управлению сортировкой классов.
        /// </summary>
        public bool SortClassesIndex
        {
            get { return _sortClassesIndex; }
            set
            {
                _sortClassesIndex = value;
                OnPropertyChanged(nameof(SortClassesIndex));
                // Вызов метода для сортировки классов
                SortClasses();
            }
        }


        /// <summary>
        /// Указывает, нужно ли сортировать отфильтрованные классы.
        /// </summary>
        private bool _sortFilterClassesIndex;

        /// <summary>
        /// Свойство для доступа к управлению сортировкой отфильтрованных классов.
        /// </summary>
        public bool SortFilterClassesIndex
        {
            get { return _sortFilterClassesIndex; }
            set
            {
                _sortFilterClassesIndex = value;
                OnPropertyChanged(nameof(SortFilterClassesIndex));
                // Вызов метода для сортировки классов
                SortClasses();
            }
        }
        /// <summary>
        /// Сортирует классы и отфильтрованные классы в соответствии с выбранными параметрами сортировки.
        /// </summary>
        private void SortClasses()
        {
            if (SortClassesIndex)
            {
                Classes = new ObservableCollection<Class>(_classes.OrderBy(c => int.Parse(c.Name.Substring(0, c.Name.Length - 1))).ThenBy(c => c.Name));
            }
            else
            {
                Classes = new ObservableCollection<Class>(_classes.OrderByDescending(s => s.Name));
            }
            if (SortFilterClassesIndex)
            {
                FilteredClasses = new ObservableCollection<Class>(_filteredClasses.OrderBy(c => int.Parse(c.Name.Substring(0, c.Name.Length - 1))).ThenBy(c => c.Name));
            }
            else
            {
                FilteredClasses = new ObservableCollection<Class>(_filteredClasses.OrderByDescending(s => s.Name));
            }
        }
        /// <summary>
        /// Сортирует студентов в каждом классе в соответствии с выбранными параметрами сортировки.
        /// </summary>
        private void SortStudents()
        {
            foreach (var _class in Classes)
            {
                if (SortStudentsIndex)
                {
                    var sortedStudents = new ObservableCollection<Student>(_class.Students.OrderBy(s => s.Name));
                    _class.Students.Clear();
                    foreach (var student in sortedStudents)
                    {
                        _class.Students.Add(student);
                    }
                }
                else
                {
                    var sortedStudents = new ObservableCollection<Student>(_class.Students.OrderByDescending(s => s.Name));
                    _class.Students.Clear();
                    foreach (var student in sortedStudents)
                    {
                        _class.Students.Add(student);
                    }
                }
            }
        }

        /// <summary>
        /// Коллекция предметов для отображения в пользовательском интерфейсе.
        /// </summary>
        private ObservableCollection<Subject> _perfomanceSubjects;

        /// <summary>
        /// Свойство доступа к коллекции предметов для отображения в пользовательском интерфейсе.
        /// </summary>
        public ObservableCollection<Subject> PerfomanceSubjects
        {
            get { return _perfomanceSubjects; }
            set
            {
                _perfomanceSubjects = value;
                OnPropertyChanged(nameof(PerfomanceSubjects));
            }
        }

        /// <summary>
        /// Коллекция студентов для отображения в пользовательском интерфейсе.
        /// </summary>
        private ObservableCollection<Student> _students;

        /// <summary>
        /// Свойство доступа к коллекции студентов для отображения в пользовательском интерфейсе.
        /// </summary>
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }


        /// <summary>
        /// Коллекция предметов для отображения в пользовательском интерфейсе.
        /// </summary>
        private ObservableCollection<Subject> _subjects;

        /// <summary>
        /// Свойство доступа к коллекции предметов для отображения в пользовательском интерфейсе.
        /// </summary>
        public ObservableCollection<Subject> Subjects
        {
            get { return _subjects; }
            set
            {
                _subjects = value;
                OnPropertyChanged(nameof(Subjects));
            }
        }

        /// <summary>
        /// Путь к файлу базы данных.
        /// </summary>
        private string FilePath = "School.db";

        /// <summary>
        /// Контекст базы данных приложения.
        /// </summary>
        DataBase.AppContext _db = new DataBase.AppContext();

        /// <summary>
        /// Статус базы данных.
        /// </summary>
        private string _databaseStatus;

        /// <summary>
        /// Свойство доступа к статусу базы данных.
        /// </summary>
        public string DatabaseStatus
        {
            get { return _databaseStatus; }
            set
            {
                if (_databaseStatus != value)
                {
                    _databaseStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Выбранный класс.
        /// </summary>
        private Class _selectedClass;

        /// <summary>
        /// Свойство доступа к выбранному классу.
        /// </summary>
        public Class SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                UpdateStudentsGrades();
            }
        }

        /// <summary>
        /// Выбранный предмет.
        /// </summary>
        private Subject _selectedSubject;

        /// <summary>
        /// Свойство доступа к выбранному предмету.
        /// </summary>
        public Subject SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                UpdateStudentsGrades();
            }
        }

        /// <summary>
        /// Выбранный студент.
        /// </summary>
        private Student _selectedStudent;

        /// <summary>
        /// Свойство доступа к выбранному студенту.
        /// </summary>
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                if (_selectedStudent != value)
                {
                    if (_selectedStudent != null)
                    {
                        _selectedStudent.IsSelected = false;
                    }

                    _selectedStudent = value;

                    if (_selectedStudent != null)
                    {
                        _selectedStudent.IsSelected = true;
                    }

                    OnPropertyChanged(nameof(SelectedStudent));
                }
            }
        }

        /// <summary>
        /// Метод, вызываемый при изменении выбранного элемента.
        /// </summary>
        /// <param name="selectedItem">Выбранный элемент.</param>
        private void OnSelectedItemChanged(object selectedItem)
        {
            SelectedStudent = selectedItem as Student;
        }

        /// <summary>
        /// Команда для создания базы данных.
        /// </summary>
        public RelayCommand CreateDatabaseCommand { get; private set; }

        /// <summary>
        /// Команда для удаления базы данных.
        /// </summary>
        public RelayCommand DeleteDatabaseCommand { get; private set; }

        /// <summary>
        /// Асинхронный метод для создания базы данных.
        /// </summary>
        private async Task CreateDatabaseAsync()
        {
            if (!_db.DatabaseExists())
            {
                await _db.CreateDatabaseAsync();
                LoadSubjectsFromDatabase();
                LoadClassesFromDatabase();
                DatabaseStatus = "База данных создана";
                MessageBox.Show(DatabaseStatus);
            }
            else
            {
                MessageBox.Show("БД создана! Пшел вон");
            }
        }
        /// <summary>
        /// Асинхронный метод для удаления базы данных.
        /// </summary>
        private async Task DeleteDatabaseAsync()
        {
            if (_db.DatabaseExists())
            {
                try
                {
                    await _db.DeleteDatabaseAsync();
                    LoadSubjectsFromDatabase();
                    LoadClassesFromDatabase();
                    DatabaseStatus = $"База данных {FilePath} удалена";
                    MessageBox.Show(DatabaseStatus);
                }
                catch (Exception ex)
                {
                    DatabaseStatus = $"Ошибка удаления БД: {ex.Message}";
                    MessageBox.Show(DatabaseStatus);
                }
            }
            else
            {
                MessageBox.Show("БД не создана! Пшел вон");
            }
        }

        /// <summary>
        /// Команда для генерации отчета о студенте.
        /// </summary>
        private RelayCommand _generateReportCommand;
        public RelayCommand GenerateReportCommand
        {
            get
            {
                return _generateReportCommand ??
                  (_generateReportCommand = new RelayCommand(
                      obj =>
                      {
                          // Предполагаем, что `SelectedStudent` уже выбран
                          if (SelectedStudent != null)
                          {
                              try
                              {
                                  SelectedStudent.Class = SelectedClass;

                                  // Открываем окно с отчетом для выбранного ученика
                                  StudentReportViewModel viewModel = new StudentReportViewModel(SelectedStudent, Subjects);

                                  // Установка DataContext окна на viewModel
                                  StudentReportWindow reportWindow = new StudentReportWindow();
                                  reportWindow.DataContext = viewModel;

                                  // Открытие окна
                                  reportWindow.Show();

                              }
                              catch (Exception ex)
                              {
                                  // Показываем сообщение об ошибке
                              }
                          }
                      },
                      obj => SelectedStudent != null // Команда доступна, если выбран студент
                  ));
            }
        }

        /// <summary>
        /// Команда для сохранения базы данных.
        /// </summary>
        private RelayCommand _saveDatabaseCommand;
        public RelayCommand SaveDatabaseCommand
        {
            get
            {
                return _saveDatabaseCommand ?? (_saveDatabaseCommand = new RelayCommand(
                    (obj) =>
                    {
                        var saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Database Files|*.db";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            var filePath = saveFileDialog.FileName;
                            _db.SaveDatabase(filePath);
                            DatabaseStatus = $"База данных {filePath} сохранена";
                            MessageBox.Show(DatabaseStatus);
                        }
                    },
                    (obj) => true));
            }
        }



        /// <summary>
        /// Команда для открытия базы данных.
        /// </summary>
        private RelayCommand _openDatabaseCommand;
        public RelayCommand OpenDatabaseCommand
        {
            get
            {
                return _openDatabaseCommand ?? (_openDatabaseCommand = new RelayCommand(
                    (obj) =>
                    {
                        var openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Database Files|*.db";
                        if (openFileDialog.ShowDialog() == true)
                        {
                            var filePath = openFileDialog.FileName;
                            FilePath = filePath;
                            _db = new DataBase.AppContext(filePath);
                            LoadSubjectsFromDatabase();
                            LoadClassesFromDatabase();
                            DatabaseStatus = $"База данных {FilePath} открыта и доступна для работы";
                            MessageBox.Show(DatabaseStatus);
                        }
                    },
                    (obj) => true));
            }
        }

        /// <summary>
        /// Команда для регистрации студента.
        /// </summary>
        private RelayCommand _registerStudentCommand;
        public RelayCommand RegisterStudentCommand
        {
            get
            {
                return _registerStudentCommand ??
                    (_registerStudentCommand = new RelayCommand(obj =>
                    {
                        var userWindow = new StudentRegistrationWindow(_db);
                        userWindow.ShowDialog();
                        LoadClassesFromDatabase();
                    }));
            }
        }

        /// <summary>
        /// Команда для регистрации класса.
        /// </summary>
        private RelayCommand _registerClassCommand;
        public RelayCommand RegisterClassCommand
        {
            get
            {
                return _registerClassCommand ??
                    (_registerClassCommand = new RelayCommand(obj =>
                    {
                        var userWindow = new ClassRegistrationWindow(_db);
                        userWindow.ShowDialog();
                        LoadClassesFromDatabase();
                    }));
            }
        }

        /// <summary>
        /// Команда для добавления оценки.
        /// </summary>
        private RelayCommand _addGradeCommand;
        public RelayCommand AddGradeCommand
        {
            get
            {
                return _addGradeCommand ??
                    (_addGradeCommand = new RelayCommand(obj =>
                    {
                        if (SelectedAllSubject != null)
                        {
                            Random rnd = new Random();
                            var subject = _db.Subjects.FirstOrDefault(s => s.Id == SelectedAllSubject.Id);
                            foreach (var student in _db.Students)
                            {
                                int gradeValue = rnd.Next(1, 11); // Генерация случайной оценки от 1 до 10
                                Grade grade = new Grade(GetRandomDate(), gradeValue, student, subject);

                                // Добавляем оценку в контекст базы данных
                                _db.Grades.Add(grade);
                            }
                            MessageBox.Show("Оценки добавлены");

                            _db.SaveChanges();
                            LoadClassesFromDatabase();
                            UpdateAllStudentsGrades();
                            UpdateStudentsGrades();
                            UpdateSelectedStudentPerformance();
                        }
                        else
                        {
                            MessageBox.Show("Не выбран предмет для генерации оценки");
                        }
                    }));
            }
        }


        /// <summary>
        /// Команда для изменения данных студента.
        /// </summary>
        private RelayCommand _changeStudentCommand;
        public RelayCommand ChangeStudentCommand
        {
            get
            {
                return _changeStudentCommand ??
                    (_changeStudentCommand = new RelayCommand(obj =>
                    {
                        if (SelectedStudent != null)
                        {
                            var userWindow = new ChangeStudentWindow(_db, SelectedStudent);
                            userWindow.ShowDialog();
                            LoadClassesFromDatabase();
                        }
                    }));
            }
        }

        /// <summary>
        /// Команда для удаления выбранного студента.
        /// </summary>
        private RelayCommand _deleteStudentCommand;
        public RelayCommand DeleteStudentCommand
        {
            get
            {
                return _deleteStudentCommand ??
                    (_deleteStudentCommand = new RelayCommand(obj =>
                    {
                        var student = _db.Students.Include(s => s.Grades).FirstOrDefault(s => s.Id == SelectedStudent.Id);
                        if (student != null)
                        {
                            _db.Grades.RemoveRange(student.Grades); // Удалить все оценки ученика
                            _db.Students.Remove(student); // Удалить ученика
                            _db.SaveChanges();
                            LoadClassesFromDatabase();
                        }

                        MessageBox.Show("Ученик удален");
                    }));
            }
        }

        /// <summary>
        /// Текст для поиска студентов.
        /// </summary>
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterStudents();
                    SortClasses();
                }
            }
        }

        /// <summary>
        /// Фильтрация студентов по имени.
        /// </summary>
        private void FilterStudents()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Если строка поиска пуста, отображаем все классы без фильтрации
                FilteredClasses = new ObservableCollection<Class>(Classes);

                UpdateFilteredStudentCountText(AllStudents.Count);
            }
            else
            {
                int i = 0;
                // Иначе фильтруем студентов в каждом классе и обновляем FilteredClasses
                FilteredClasses = new ObservableCollection<Class>(
                    Classes.Select(classItem =>
                    {
                        var filteredStudents = classItem.Students.Where(student => student.Name.Contains(SearchText)).ToList();
                        i += filteredStudents.Count;
                        return new Class(classItem.Name, classItem.Teacher, classItem.CurrentStudents, classItem.MaxStudents)
                        {
                            Students = new ObservableCollection<Student>(filteredStudents)
                        };
                    })
                );

                UpdateFilteredStudentCountText(i);
            }
        }

        /// <summary>
        /// Отфильтрованные классы с учетом поиска по студентам.
        /// </summary>
        private ObservableCollection<Class> _filteredClasses;
        public ObservableCollection<Class> FilteredClasses
        {
            get { return _filteredClasses; }
            set
            {
                _filteredClasses = value;
                OnPropertyChanged(nameof(FilteredClasses));
            }
        }

        /// <summary>
        /// Коллекция классов.
        /// </summary>
        private ObservableCollection<Class> _classes;
        public ObservableCollection<Class> Classes
        {
            get { return _classes; }
            set
            {
                _classes = value;
                OnPropertyChanged("Classes");
            }
        }

        /// <summary>
        /// Команда для генерации тестовых данных.
        /// </summary>
        private RelayCommand generateDataCommand;
        public RelayCommand GenerateDataCommand
        {
            get
            {
                return generateDataCommand ??
                    (generateDataCommand = new RelayCommand(obj =>
                    {
                        //                        if(!_db.DatabaseExists())
                        if (_db.DatabaseExists())
                        {
                            if (_db.Subjects.Count<Subject>() == 0)
                            {
                                // Логика генерации данных
                                var subjects = new List<string>
                                {
                                    "Математика",
                                    "Русский",
                                    "Физика",
                                    "Химия",
                                    "Литература"
                                };
                                // Генерируем 
                                foreach (var subject in subjects)
                                {
                                    Subject newObj = new Subject(subject);
                                    newObj.Name = subject;
                                    _db.Subjects.Add(newObj);
                                }
                                _db.SaveChanges();
                            }
                            if (_db.Classes.Count<Class>() == 0)
                            {
                                int _id = 1;
                                var classes = new List<(string Name, string Teacher, int CurrentStudents, int MaxStudents, int id)>
                                {
                                    ("1а", "Иванова Н.П.", 0, 30, _id++),
                                    ("2а", "Петрова О.И.", 0, 30, _id++),
                                    ("3а", "Сидоров П.М.", 0, 25, _id++),
                                    ("4а", "Козлова Г.А.", 0, 25, _id++),
                                    ("5а", "Смирнов Л.Г.", 0, 30, _id++),
                                    ("6а", "Никитина А.В.", 0, 25, _id++),
                                    ("7а", "Беляева Е.С.", 0, 20, _id++),
                                    ("8а", "Григорьев Д.В.", 0, 30, _id++),
                                    ("9а", "Михайлова Т.К.", 0, 30, _id++),
                                    ("10а", "Захаров В.И.", 0, 30, _id ++),
                                    ("11а", "Васильева К.С.", 0, 20, _id ++),
                                    ("5б", "Василькова К.С.", 0, 25, _id ++)
                                };

                                var students = new List<(string Name, int Age)>
                                {("Иванов", 15),("Петров", 16),("Сидоров", 17),("Козлов", 15),
                                    ("Смирнов", 16),("Никитин", 17),("Беляев", 15),("Григорьев", 16),
                                    ("Михайлов", 17),("Захаров", 15),("Васильев", 16),("Павлов", 17),
                                    ("Федоров", 15),("Алексеев", 16),("Дмитриев", 17),("Сергеев", 15),
                                    ("Андреев", 16),("Юрьев", 17),("Тимофеев", 15),("Степанов", 16),
                                    ("Николаев", 17),("Александров", 15),("Егоров", 16),("Максимов", 17),
                                    ("Ильин", 15),("Павлов", 16),("Ефимов", 17),("Леонтьев", 15),
                                    ("Матвеев", 16),("Артемьев", 17),("Давыдов", 15),("Борисов", 16),
                                    ("Кириллов", 17),("Тихонов", 15),("Филиппов", 16),("Марков", 17),
                                    ("Антонов", 15),("Артемов", 16),("Миронов", 17),("Петухов", 18),
                                    ("Кузнецов", 15),("Исаев", 16),("Савельев", 17),("Трофимов", 15),
                                    ("Герасимов", 16),("Моисеев", 17),("Еремеев", 15),("Калашников", 16),
                                    ("Кабанов", 17),("Носов", 15),("Шубин", 16),("Игнатов", 17),
                                    ("Лобанов", 15),("Поляков", 16),("Цветков", 17),("Данилов", 15),
                                    ("Лобнов", 15),("Полков", 16),("Цветкова", 14),("Данилова", 14),
                                    ("Белов", 16),("Суворов", 17),("Волков", 15)};
                                int i = 1;
                                foreach (var _class in classes)
                                {
                                    var (Name, Teacher, CurrentStudents, MaxStudents, Id) = _class;
                                    Class newObj = new Class(Name, Teacher, CurrentStudents, MaxStudents);
                                    while(i%5!=0)
                                    {
                                        newObj.Students.Add(new Student(students[i].Name, students[i].Age, newObj));
                                        newObj.CurrentStudents++;
                                        i++;
                                    }
                                    newObj.Students.Add(new Student(students[i].Name, students[i].Age, newObj)); 
                                    newObj.CurrentStudents++;
                                    i++;
                                    _db.Classes.Add(newObj);
                                }
                                _db.SaveChanges();

                                Random rnd = new Random();
                                foreach (var student in _db.Students)
                                {
                                    foreach (var subject in _db.Subjects)
                                    {
                                        int gradeValue = rnd.Next(1, 11); // Генерация случайной оценки от 1 до 10
                                        //Grade grade = new Grade(DateTime.Now.ToString("yyyy-MM-dd"), gradeValue);
                                        Grade grade = new Grade(GetRandomDate(), gradeValue, student, subject);
                                        student.Grades.Add(grade);
                                        subject.Grades.Add(grade);

                                        // Добавляем оценку в контекст базы данных
                                        _db.Grades.Add(grade);
                                    }
                                }
                                rnd = new Random();
                                foreach (var student in _db.Students)
                                {
                                    foreach (var subject in _db.Subjects)
                                    {
                                        int gradeValue = rnd.Next(1, 11); // Генерация случайной оценки от 1 до 10
                                        Grade grade = new Grade(GetRandomDate(), gradeValue, student, subject);
                                        student.Grades.Add(grade);
                                        subject.Grades.Add(grade);

                                        // Добавляем оценку в контекст базы данных
                                        _db.Grades.Add(grade);
                                    }
                                }

                                _db.SaveChanges();





                                LoadSubjectsFromDatabase();
                                LoadClassesFromDatabase();
                            }
                            else
                            {
                                MessageBox.Show("Внимание! Тестовую сборку можно использовать только при пустой бд!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Внимание! Тестовую сборку можно использовать только при созданной бд!");
                        }
                    }));
            }
        }

        /// <summary>
        /// Создание нового экземпляра <see cref="MainWindowViewModel"/>.
        /// </summary>
        /// <param name="selectedUserRole">Выбранная роль пользователя.</param>
        public MainWindowViewModel(string selectedUserRole)
        {
            SelectedUserRole = selectedUserRole;
            UpdateDirectorTabVisibility();
            UpdateTeacherTabVisibility();
            CreateDatabaseCommand = new RelayCommand(async (parameter) => await CreateDatabaseAsync());
            DeleteDatabaseCommand = new RelayCommand(async (parameter) => await DeleteDatabaseAsync());
            LoadSubjectsFromDatabase();
            LoadClassesFromDatabase();

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedClass))
                {
                    LoadStudents(); // Загрузка учеников при изменении выбранного класса
                }
            };
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedClass) || args.PropertyName == nameof(SelectedSubject))
                {
                    UpdateStudentsGrades(); // Обновление оценок при изменении выбранного класса или предмета
                }
            };
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedClass) || args.PropertyName == nameof(SelectedStudent))
                {
                    UpdateSelectedStudentPerformance(); // Обновление предметов и оценок при изменении выбранного класса или ученика
                }
            };

            UpdateFilteredStudentCountText(AllStudents.Count);
        }


        /// <summary>
        /// Загрузка предметов из базы данных.
        /// </summary>
        private void LoadSubjectsFromDatabase()
        {
            Subjects = new ObservableCollection<Subject>();
            if (_db.DatabaseExists())
            {
                string connectionString = "Data Source="+FilePath;
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT Id, Name FROM Subjects";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subject = new Subject
                            {
                                Id = reader.GetString(0),
                                Name = reader.GetString(1)
                            };
                            Subjects.Add(subject);
                        }
                    }

                    connection.Close();
                }
            }
        }
        /// <summary>
        /// Загрузка классов из базы данных.
        /// </summary>
        private void LoadClassesFromDatabase()
        {
            Classes = new ObservableCollection<Class>();
            AllStudents=new ObservableCollection<Student>();
            if (_db.DatabaseExists())
            {
                string connectionString = "Data Source="+FilePath;
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT Id, Name, CurrentStudents, Teacher, MaxStudents FROM Classes";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _class = new Class
                            {
                                Id = reader.GetString(0), // Изменено чтение идентификатора класса на строку
                                Name = reader.GetString(1),
                                CurrentStudents = reader.GetInt32(2),
                                Teacher = reader.GetString(3),
                                MaxStudents = reader.GetInt32(4),
                                Students = new ObservableCollection<Student>()
                            };
                            Classes.Add(_class);

                            // Преобразование строки идентификатора класса в integer для использования в запросе
                            //int classId = int.Parse(_class.Id);
                            string classId = _class.Id;
                            // Загрузка студентов для текущего класса
                            using (var studentCommand = connection.CreateCommand())
                            {
                                studentCommand.CommandText = "SELECT Id, Name, Age, ClassId FROM Students WHERE ClassId = @classId";
                                studentCommand.Parameters.AddWithValue("@classId", classId);
                                using (var studentReader = studentCommand.ExecuteReader())
                                {
                                    while (studentReader.Read())
                                    {
                                        var student = new Student
                                        {
                                            Id = studentReader.GetString(0),
                                            Name = studentReader.GetString(1),
                                            Age = studentReader.GetInt32(2),
                                            ClassId = studentReader.GetString(3),
                                            Class = Classes.FirstOrDefault(c => c.Id == classId.ToString()),
                                            Grades = new ObservableCollection<Grade>()
                                        };
                                        AllStudents.Add(student);
                                        _class.Students.Add(student);

                                        // Загрузка оценок для текущего студента
                                        using (var gradeCommand = connection.CreateCommand())
                                        {
                                            gradeCommand.CommandText = "SELECT Data, Value, SubjectId, StudentId FROM Grades WHERE StudentId = @studentId";
                                            gradeCommand.Parameters.AddWithValue("@studentId", student.Id);
                                            using (var gradeReader = gradeCommand.ExecuteReader())
                                            {
                                                while (gradeReader.Read())
                                                {
                                                    var grade = new Grade
                                                    {
                                                        Data = gradeReader.GetString(0),
                                                        Value = gradeReader.GetInt32(1),
                                                        SubjectId=gradeReader.GetString(2),
                                                        StudentId=gradeReader.GetString(3)
                                                    };
                                                    var subjectGrades = Subjects.FirstOrDefault(sub => sub.Id == grade.SubjectId)?.Grades;
                                                    student.Grades.Add(grade);
                                                    subjectGrades.Add(grade);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    connection.Close();
                }
                FilteredClasses = Classes;
            }
        }

        /// <summary>
        /// Загрузка учеников в зависимости от выбранного класса.
        /// </summary>
        private void LoadStudents()
        {
            if (SelectedClass != null)
            {
                Students = new ObservableCollection<Student>(SelectedClass.Students);
            }
            else
            {
                Students = new ObservableCollection<Student>();
            }
        }

        /// <summary>
        /// Коллекция всех учеников.
        /// </summary>
        private ObservableCollection<Student> _allStudents;
        public ObservableCollection<Student> AllStudents
        {
            get { return _allStudents; }
            set
            {
                _allStudents = value;
                OnPropertyChanged(nameof(AllStudents));
            }
        }

        /// <summary>
        /// Коллекция всех учеников для выбранного предмета.
        /// </summary>
        private ObservableCollection<Student> _allSubStudents;
        public ObservableCollection<Student> AllSubStudents
        {
            get { return _allSubStudents; }
            set
            {
                _allSubStudents = value;
                OnPropertyChanged(nameof(AllSubStudents));
            }
        }

        /// <summary>
        /// Выбранный предмет для отображения оценок всех учеников.
        /// </summary>
        private Subject _selectedAllSubject;
        public Subject SelectedAllSubject
        {
            get { return _selectedAllSubject; }
            set
            {
                _selectedAllSubject = value;
                OnPropertyChanged(nameof(SelectedAllSubject));
                UpdateAllStudentsGrades();
            }
        }

        /// <summary>
        /// Обновление оценок всех учеников по выбранному предмету.
        /// </summary>
        private void UpdateAllStudentsGrades()
        {
            if (SelectedAllSubject != null)
            {
                var studentsInClass = AllStudents.Select(student =>
                {
                    var gradesForSubject = student.Grades.Where(grade => grade.SubjectId == SelectedAllSubject.Id).ToList();
                    return new Student
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Grades = new ObservableCollection<Grade>(gradesForSubject),
                        Class = student.Class
                    };
                });

                AllSubStudents = new ObservableCollection<Student>(studentsInClass);
            }
            else
            {
                AllSubStudents = new ObservableCollection<Student>();
            }

            OnPropertyChanged(nameof(AllSubStudents));
        }


        /// <summary>
        /// Обновление оценок учеников на основе выбранного класса и предмета.
        /// </summary>
        private void UpdateStudentsGrades()
        {
            Students = new ObservableCollection<Student>();
            if (SelectedClass != null && SelectedSubject != null)
            {
                var studentsInClass = SelectedClass.Students.Select(student =>
                {
                    var gradesForSubject = student.Grades.Where(grade => grade.SubjectId == SelectedSubject.Id).ToList();
                    return new Student
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Grades = new ObservableCollection<Grade>(gradesForSubject)
                    };
                });

                Students = new ObservableCollection<Student>(studentsInClass);
            }
            OnPropertyChanged(nameof(Students));
        }

        /// <summary>
        /// Обновление предметов и оценок для выбранного ученика.
        /// </summary>
        private void UpdateSelectedStudentPerformance()
        {
            if (SelectedStudent != null && SelectedClass != null)
            {
                var performance = new ObservableCollection<Subject>();

                foreach (var grade in SelectedStudent.Grades)
                {
                    var subject = Subjects.FirstOrDefault(s => s.Id == grade.SubjectId);
                    if (subject != null)
                    {
                        var existingSubjectGrade = performance.FirstOrDefault(sg => sg.Name == subject.Name);
                        if (existingSubjectGrade == null)
                        {
                            performance.Add(new Subject
                            {
                                Name = subject.Name,
                                Grades = new ObservableCollection<Grade> { grade }
                            });
                        }
                        else
                        {
                            existingSubjectGrade.Grades.Add(grade);
                        }
                    }
                }

                PerfomanceSubjects = performance;
            }
            else
            {
                PerfomanceSubjects = new ObservableCollection<Subject>();
            }

            OnPropertyChanged(nameof(PerfomanceSubjects));
        }

        /// <summary>
        /// Генерация случайной даты.
        /// </summary>
        private string GetRandomDate()
        {
            Random random = new Random();
            int days = random.Next(0, DateTime.Today.DayOfYear);
            DateTime randomDate = new DateTime(DateTime.Today.Year, 1, 1).AddDays(days);
            return randomDate.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Обновление видимости вкладки "Директор".
        /// </summary>
        private Visibility _isDirectorTabVisible = Visibility.Collapsed;
        public Visibility IsDirectorTabVisible
        {
            get { return _isDirectorTabVisible; }
            set
            {
                _isDirectorTabVisible = value;
                OnPropertyChanged(nameof(IsDirectorTabVisible));
            }
        }

        /// <summary>
        /// Обновление видимости вкладки "Учитель".
        /// </summary>
        private Visibility _isTeacherTabVisible = Visibility.Collapsed;
        public Visibility IsTeacherTabVisible
        {
            get { return _isTeacherTabVisible; }
            set
            {
                _isTeacherTabVisible = value;
                OnPropertyChanged(nameof(IsTeacherTabVisible));
            }
        }

        /// <summary>
        /// Роль выбранного пользователя.
        /// </summary>
        private string _selectedUserRole;
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                if (_selectedUserRole != value)
                {
                    _selectedUserRole = value;
                    UpdateDirectorTabVisibility();
                    UpdateTeacherTabVisibility();
                }
            }
        }

        /// <summary>
        /// Обновление видимости вкладки "Директор" на основе выбранной роли пользователя.
        /// </summary>
        private void UpdateDirectorTabVisibility()
        {
            IsDirectorTabVisible = SelectedUserRole == "Директор" ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Обновление видимости вкладки "Учитель" на основе выбранной роли пользователя.
        /// </summary>
        private void UpdateTeacherTabVisibility()
        {
            IsTeacherTabVisible = SelectedUserRole == "Преподаватель" ? Visibility.Visible : Visibility.Collapsed;
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
