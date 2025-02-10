using System;
using Avalonia.Controls;


namespace ToDoApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void OnCreateProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var puCreateProject = new CreateProject(this);
        puCreateProject.Show();
    }

    public void OnProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button btn = sender as Button;
        string projectName = btn.Content.ToString();

        ProjectNameOverview.Text = projectName + " Overview";


    }
}