using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;



namespace ToDoApp;

public partial class MainWindow : Window
{
    private string currentFile = null;
    public MainWindow()
    {
        InitializeComponent();
        
        string todoDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/ToDoApp";
        GetProjects(todoDocs);
    }
    
    public void OnProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TodoStack.Children.Clear();
        
        Button btn = sender as Button;
        string projectName = btn.Content.ToString();

        currentFile = projectName;
        
        ProjectNameOverview.Text = projectName + " Overview";

    }

    private void OnTodoClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button btn = sender as Button;
        
        // Toggles the color of the button: Lime signifying completed and white meaning todo
        if (btn.Foreground == Brushes.White)
        {
            btn.Foreground = Brushes.Lime;
            
        }
        else
        {
            btn.Foreground = Brushes.White;
            
        }
    }

    private void OnAddTodoClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (currentFile == null || TodoItemInput.Text == "") return;
        
        // <Button HorizontalAlignment="Stretch" Content="Finish the todo app" Foreground="{DynamicResource FontColor}"></Button>
        Button btn = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Content = TodoItemInput.Text, 
            Foreground = Brushes.White,
        };

        btn.Click += OnTodoClicked;

        TodoStack.Children.Add(btn);

        TodoItemInput.Clear();
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