using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoonBurst.Helper
{
    class ConfirmationHelper
    {
        public static async Task<object> RequestConfirmationBeforeCreateNew()
        {
            return await ShowDialogControl("You are about to create a new layout.\nUnsaved changed will be lost.\n\nDo you confirm ?\n", "Yes, loose changes");
        }

        public static async Task<object> RequestConfirmationBeforeDeletation()
        {
            return await ShowDialogControl("You are about to delete this item.\nThis action is irreversible.\n\nDo you confirm ?\n", "Yes, delete this item");
        }

        public static async Task<object> ShowDialogControl(string message, string confirmContent = "Yes", string cancelContent = "No")
        {
            TextBlock txt1 = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4),
                TextWrapping = TextWrapping.WrapWithOverflow,
                FontSize = 18,
                TextAlignment = TextAlignment.Center,
                Text = message
            };

            Style style = Application.Current.FindResource("MaterialDesignRaisedLightButton") as Style;
            Button btn1 = new Button
            {
                Style = style,
                Margin = new Thickness(5),
                Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand,
                CommandParameter = true,
                Content = confirmContent
            };

            Style style2 = Application.Current.FindResource("MaterialDesignRaisedLightButton") as Style;
            Button btn2 = new Button
            {
                Style = style2,
                Margin = new Thickness(5),
                Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand,
                CommandParameter = false,
                Content = cancelContent
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetColumn(btn2, 1);
            Grid.SetColumn(btn1, 0);

            grid.Children.Add(btn1);
            grid.Children.Add(btn2);

            StackPanel stk = new StackPanel { Width = 500 };
            stk.Children.Add(txt1);
            stk.Children.Add(grid);
            stk.Margin = new Thickness(50);

            return (await MaterialDesignThemes.Wpf.DialogHost.Show(stk));
        }
    }
}
