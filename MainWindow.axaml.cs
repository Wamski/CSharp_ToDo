using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media;


namespace ToDoApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        string todoDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/ToDoApp";
        GetProjects(todoDocs);
    }
    
    public void OnProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button btn = sender as Button;
        string projectName = btn.Content.ToString();

        ProjectNameOverview.Text = projectName + " Overview";

    }
    private void OnCreateProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var puCreateProject = new CreateProject(this);
        puCreateProject.Show();
    }

    private void GetProjects(string path)
    {
        try
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                Button btn = new Button
                {
                    Width = 160,
                    Content = name,
                    Background = Brushes.Transparent
                };
            
                btn.Click += OnProjectClicked;
                ProjectStack.Children.Add(btn);
            }
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Directory Not Found");
        }
        
    }
    
}