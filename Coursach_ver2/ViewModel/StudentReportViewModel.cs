using Coursach_ver2.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Coursach_ver2.ViewModel
{
    /// <summary>
    /// ViewModel для отчета о успеваемости студента.
    /// </summary>
    public class StudentReportViewModel : INotifyPropertyChanged
    {
        private Student _selectedStudent;

        /// <summary>
        /// Выбранный студент.
        /// </summary>
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private ObservableCollection<Subject> _subjects;

        /// <summary>
        /// Коллекция предметов.
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
        /// Инициализирует новый экземпляр ViewModel для отчета о успеваемости студента.
        /// </summary>
        public StudentReportViewModel(Student student, ObservableCollection<Subject> subjects)
        {
            SelectedStudent = student;
            Subjects = subjects;
            UpdateSelectedStudentPerformance();
        }

        private ObservableCollection<Subject> _perfomanceSubjects;

        /// <summary>
        /// Коллекция предметов с оценками.
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
        /// Обновляет информацию об успеваемости выбранного студента.
        /// </summary>
        private void UpdateSelectedStudentPerformance()
        {
            if (SelectedStudent != null)
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
