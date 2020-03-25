using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoonBurst.Helper
{

    class ConfirmationHelper
    {
        public static async Task<object> RequestConfirmationForDeletation()
        {
            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.TextAlignment = TextAlignment.Center;
            txt1.Text = $"You are about to delete this item, this action is irreversible.\n\nDo you confirm ?\n";

            Button btn1 = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatDarkBgButton") as Style;
            btn1.Style = style;
            btn1.Margin = new Thickness(5);
            btn1.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn1.CommandParameter = true;
            btn1.Content = "Yes, delete it.";

            Button btn2 = new Button();
            Style style2 = Application.Current.FindResource("MaterialDesignFlatDarkBgButton") as Style;
            btn2.Style = style2;
            btn2.Margin = new Thickness(5);
            btn2.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn2.CommandParameter = false;
            btn2.Content = "No, it was a mistake.";

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetColumn(btn2, 1);
            Grid.SetColumn(btn1, 0);

            grid.Children.Add(btn1);
            grid.Children.Add(btn2);

            StackPanel stk = new StackPanel();
            stk.Width = 500;
            stk.Children.Add(txt1);
            stk.Children.Add(grid);
            stk.Margin = new Thickness(50);

            return (await MaterialDesignThemes.Wpf.DialogHost.Show(stk));
        }
    }
}
