using System;
using System.Windows.Input;

namespace ScreenSlicer.Commands
{
    public static class InjectableCommand
    {
        internal static TParam CastParam<TParam>(object parameter)
        {
            var typeofTParam = typeof(TParam);
            if (typeofTParam == typeof(bool))
                return (TParam)(object)(parameter == null ? false : (parameter is bool ? (bool)parameter : false));

            if (parameter == null)
                if (!typeofTParam.IsCanBeNull())
                    throw new ArgumentException($"Type '{nameof(TParam)}' can't be null");
                else
                    return (TParam)(object)null;
            else if (parameter is TParam param)
                return param;
            else
                throw new ArgumentException($"Cant cast type '{parameter.GetType().Name}' into '{nameof(TParam)}'");
        }
    }

    public abstract class InjectableCommand<T> : ICommand where T : InjectableCommand<T>
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(T).ToString());

        public void Execute(object parameter)
        {
            Logger.Info("Command begin execute");
            ExecuteInternal(parameter);
            Logger.Info("Command end execute");
        }
        protected abstract void ExecuteInternal(object parameter);

        public bool CanExecute(object parameter)
        {
            return CanExecuteInternal(parameter);
        }
        protected abstract bool CanExecuteInternal(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public abstract class InjectableCommand<T, TParam> : ICommand where T : InjectableCommand<T, TParam>
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(T).ToString());

        public void Execute(object parameter)
        {
            try
            {
                Execute(InjectableCommand.CastParam<TParam>(parameter));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Command cannot execute: {e.Message}");
            }
        }
        public void Execute(TParam parameter)
        {
            Logger.Info("Command begin execute");
            ExecuteInternal(parameter);
            Logger.Info("Command end execute");
        }
        protected abstract void ExecuteInternal(TParam parameter);

        public bool CanExecute(object parameter)
        {
            try
            {
                return CanExecute(InjectableCommand.CastParam<TParam>(parameter));
            }
            catch
            {
                return false;
            }
        }

        public bool CanExecute(TParam parameter)
        {
            return CanExecuteInternal(parameter);
        }
        protected abstract bool CanExecuteInternal(TParam parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public abstract class InjectableCommand<T, TParam1, TParam2> : ICommand where T : InjectableCommand<T, TParam1, TParam2>
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetLogger(typeof(T).ToString());

        public void Execute(object parameter)
        {
            try
            {
                var tuple = InjectableCommand.CastParam<Tuple<TParam1, TParam2>>(parameter);
                Execute(tuple.Item1, tuple.Item2);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Command cannot execute: {e.Message}");
            }
        }
        public void Execute(TParam1 parameter1, TParam2 parameter2)
        {
            Logger.Info("Command begin execute");
            ExecuteInternal(parameter1, parameter2);
            Logger.Info("Command end execute");
        }
        protected abstract void ExecuteInternal(TParam1 parameter1, TParam2 parameter2);

        public bool CanExecute(object parameter)
        {
            try
            {
                var tuple = InjectableCommand.CastParam<Tuple<TParam1, TParam2>>(parameter);
                return CanExecute(tuple.Item1, tuple.Item2);
            }
            catch
            {
                return false;
            }
        }

        public bool CanExecute(TParam1 parameter1, TParam2 parameter2)
        {
            return CanExecuteInternal(parameter1, parameter2);
        }
        protected abstract bool CanExecuteInternal(TParam1 parameter1, TParam2 parameter2);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
