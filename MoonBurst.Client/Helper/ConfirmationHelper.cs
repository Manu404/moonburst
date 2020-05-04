using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoonBurst.Helper
{

    class ConfirmationHelper
    {
        public static async Task<object> RequestConfirmationBeforeDeletation()
        {
            TextBlock txt1 = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4),
                TextWrapping = TextWrapping.WrapWithOverflow,
                FontSize = 18,
                TextAlignment = TextAlignment.Center,
                Text = "You are about to delete this item, this action is irreversible.\n\nDo you confirm ?\n"
            };

            Style style = Application.Current.FindResource("MaterialDesignFlatDarkBgButton") as Style;
            Button btn1 = new Button
            {
                Style = style,
                Margin = new Thickness(5),
                Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand,
                CommandParameter = true,
                Content = "Yes, delete it."
            };

            Style style2 = Application.Current.FindResource("MaterialDesignFlatDarkBgButton") as Style;
            Button btn2 = new Button
            {
                Style = style2,
                Margin = new Thickness(5),
                Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand,
                CommandParameter = false,
                Content = "No, it was a mistake."
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetColumn(btn2, 1);
            Grid.SetColumn(btn1, 0);

            grid.Children.Add(btn1);
            grid.Children.Add(btn2);

            StackPanel stk = new StackPanel {Width = 500};
            stk.Children.Add(txt1);
            stk.Children.Add(grid);
            stk.Margin = new Thickness(50);

            return (await MaterialDesignThemes.Wpf.DialogHost.Show(stk));
        }
    }
}
