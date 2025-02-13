using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Avalonia;
using Newtonsoft.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;

namespace ToDoApp;

public partial class MainWindow : Window
{
    private string currentFile = null;

    private List<ToDoItem> items;
    private string todoDocs;
    public MainWindow()
    {
        InitializeComponent();
        
        todoDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/ToDoApp";

        if (!Directory.Exists(todoDocs)) Directory.CreateDirectory(todoDocs);
        
        GetProjects(todoDocs);
        
        // Have it save the current list
        this.Closing += onClose;
    }
    
    public void OnProjectClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Write back data to json
        WriteJSON();
        
        TodoStack.Children.Clear();
        CompletedStack.Children.Clear();
        
        Button btn = sender as Button;
        string projectName = btn.Content.ToString();

        currentFile = projectName + ".json";
        
        ProjectNameOverview.Text = projectName + " Overview";

        // Read file and append to Visual list and itemsList
        
        ReadJSON(todoDocs + "/" + currentFile);
        
    }

    private void ReadJSON(string filePath)
    {
        items = new List<ToDoItem>();
        
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            List<ToDoItem> tempItems = JsonConvert.DeserializeObject<List<ToDoItem>>(jsonData);
            
            items = tempItems;
            
        }
        else
        {
            File.WriteAllText(filePath, "[\n\n]");
            TodoStack.Children.Clear();
            return;
        }
        
        TodoStack.Children.Clear();
        CompletedStack.Children.Clear();
        
        // Call visual add
        foreach (var item in items)
        {
            if (item is ToDoItem todoItem)
            {
                visualToDoAdd(todoItem.name, todoItem.completed);
            }
            
        }
    }

    private void visualToDoAdd(string Name, bool Completed = false)
    {
        TextBlock tb = new TextBlock
        {
            Text = Name,
            TextWrapping = TextWrapping.Wrap,
        };
        
        Button btn = new Button
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(1),
            Content = tb, 
            Foreground = Brushes.White,
        };

        btn.Click += OnTodoClicked;
        
        // Context Menu
        ContextMenu cm = new ContextMenu();
        MenuItem mi = new MenuItem
        {
            Header = "Remove",
            SelectedValue = Name,
        };

        mi.Click += removeItemClicked;
        
        cm.Items.Add(mi);
        
        btn.ContextMenu = cm;

        if (Completed)
        {
            btn.Foreground = Brushes.Lime;
            CompletedStack.Children.Add(btn);
        }
        else
        {
            TodoStack.Children.Add(btn);
        }
        
    }

    private void OnTodoClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Button btn = sender as Button;
        
        TextBlock tb = btn.Content as TextBlock;
        
        String name = tb.Text;
        
        // Toggles the color of the button: Lime signifying completed and white meaning todo
        if (btn.Foreground == Brushes.White)
        {
            btn.Foreground = Brushes.Lime;
            TodoStack.Children.Remove(btn);
            CompletedStack.Children.Add(btn);
            
            // Mark item completed in items
            toggleCompleted(name, true);
        }
        else
        {
            btn.Foreground = Brushes.White;
            CompletedStack.Children.Remove(btn);
            TodoStack.Children.Add(btn);
            
            // Mark item incomplete in items
            toggleCompleted(name, false);
        }
    }
    
    private void toggleCompleted(String name, bool completed = false)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == name)
            {
                items[i].completed = completed;
            }
        }
    }
    

    private void OnAddTodoClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (currentFile == null || TodoItemInput.Text == "") return;

        visualToDoAdd(TodoItemInput.Text, false);

        // Apend a new ToDoItem to the list
        ToDoItem temp = new ToDoItem {name = TodoItemInput.Text, completed = false};
        
        items.Add(temp);
        
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

                if (name != "")
                {
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
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Directory Not Found");
        }
    }

    private void removeItemClicked(object sender, RoutedEventArgs? e)
    {
        // First clear the fields
        // Removed the item from the object list
        // Re;rint fields
        
        MenuItem mi = sender as MenuItem;
        
        String sV = mi.SelectedValue.ToString();
        
        
        TodoStack.Children.Clear();
        CompletedStack.Children.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == sV)
            {
                items.RemoveAt(i);
            }
        }

        // Done this way because if it was but as an else statement it would skip over elements that weren't removed
        foreach (ToDoItem item in items)
        {
            visualToDoAdd(item.name, item.completed);
        }
        
    }
    

    private void onClose(object sender, EventArgs e)
    {
        WriteJSON();
    }
    private void WriteJSON()
    {
        if (currentFile == null) return;
        string jsonFile = todoDocs + "/" + currentFile;
        
        string json = JsonConvert.SerializeObject(items, Formatting.Indented);
        
        File.WriteAllText(jsonFile, json);
    }
    
}