using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Coursach_ver2.Model
{
    /// <summary>
    /// Представляет оценку студента в школьной системе.
    /// Содержит информацию об идентификаторе, дате, значении оценки, а также связанном студенте и предмете.
    /// </summary>
    public class Grade : INotifyPropertyChanged
    {
        private string? _id;
        /// <summary>
        /// Идентификатор оценки.
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

        private string _data;
        /// <summary>
        /// Дата оценки.
        /// </summary>
        public string Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        private int _value;
        /// <summary>
        /// Значение оценки.
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        /// <summary>
        /// Конструктор по умолчанию. Генерирует новый идентификатор для оценки.
        /// </summary>
        public Grade()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Конструктор для инициализации оценки с заданными параметрами.
        /// </summary>
        /// <param name="data">Дата оценки.</param>
        /// <param name="value">Значение оценки.</param>
        /// <param name="student">Связанный студент.</param>
        /// <param name="subject">Связанный предмет.</param>
        public Grade(string data, int value, Student student, Subject subject)
        {
            Id = Guid.NewGuid().ToString();
            Data = data;
            Value = value;
            Student = student;
            StudentId = student.Id;
            Subject = subject;
            SubjectId = subject.Id;
        }

        /// <summary>
        /// Связанный студент.
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        /// Идентификатор связанного студента.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Связанный предмет.
        /// </summary>
        public Subject Subject { get; set; }

        /// <summary>
        /// Идентификатор связанного предмета.
        /// </summary>
        public string SubjectId { get; set; }

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


