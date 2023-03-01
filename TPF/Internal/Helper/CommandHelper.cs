using System.Windows;
using System.Windows.Input;

namespace TPF.Internal
{
    internal static class CommandHelper
    {
        internal static bool CanExecuteCommand(ICommandSource commandSource)
        {
            var command = commandSource.Command;

            if (command != null)
            {
                var parameter = commandSource.CommandParameter;

                var target = commandSource.CommandTarget;

                if (command is RoutedCommand routedCommand)
                {
                    if (target == null)
                    {
                        target = commandSource as IInputElement;
                    }

                    return routedCommand.CanExecute(parameter, target);
                }
                else return command.CanExecute(parameter);
            }

            return false;
        }
    }
}