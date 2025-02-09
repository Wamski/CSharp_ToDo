using Avalonia.Controls;
using Avalonia.Media;


namespace ToDoApp;

public partial class CreateProject : Window
{
    private MainWindow _mainWindow;
    public CreateProject(MainWindow mainWindow)
    {
        this._mainWindow = mainWindow;
        InitializeComponent();
    }

    private void OnCancelClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private void OnCreateClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
        // Append the name in the inputfield to the main window's sidebar
        string name = ProjectNameInput.Text;
        Button btn = new Button
        {
            Width = 160,
            Content = name,
            Background = Brushes.Transparent
        };
        _mainWindow.ProjectStack.Children.Add(btn);
        
        this.Close();
    }
}