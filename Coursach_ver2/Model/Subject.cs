using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Coursach_ver2.Model
{
    /// <summary>
    /// Представляет предмет в школьной системе.
    /// Содержит информацию об идентификаторе, имени и связанных оценках.
    /// </summary>
    public class Subject : INotifyPropertyChanged
    {
        private string? _id;
        /// <summary>
        /// Идентификатор предмета.
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

        private string _name;
        /// <summary>
        /// Название предмета.
        /// </summary>
        public string Name
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

        /// <summary>
        /// Коллекция оценок, связанных с предметом.
        /// </summary>
        public ObservableCollection<Grade> Grades { get; set; } = new ObservableCollection<Grade>();

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Subject()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Конструктор для инициализации предмета с заданным именем.
        /// </summary>
        /// <param name="name">Название предмета.</param>
        public Subject(string name)
        {
            Name = name;
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