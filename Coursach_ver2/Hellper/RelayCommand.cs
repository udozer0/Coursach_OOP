using System.Windows.Input;

namespace Coursach_ver2.Hellper
{
    /// <summary>
    /// Реализация интерфейса ICommand для создания команд в WPF.
    /// Позволяет связывать действия и условия их выполнения с элементами управления.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Событие, вызываемое при изменении условий выполнения команды.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор команды.
        /// </summary>
        /// <param name="execute">Действие, выполняемое командой.</param>
        /// <param name="canExecute">Функция, определяющая, может ли команда выполняться.</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда выполняться.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True, если команда может выполняться; иначе False.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
