using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Collections;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAppDemo;

class Program
{
    static void Main(string[] args)
    {
        List<TodoEntry> todoList = new List<TodoEntry>()
        {
            new TodoEntry("Sample Todo"),
            new TodoEntry("Due Todo", dueDate: DateTime.Now.AddDays(3))
        };

        while (true)
        {
            Console.WriteLine("Enter command (type \"exit\" to quit): ");
            Console.Write(">> ");
            var command = Console.ReadLine();

            if (command == "exit")
            {
                break;
            }
            if (string.IsNullOrEmpty(command))
            {
                continue;
            }

            Console.WriteLine("\n------------------------------------------------------------------------------------");

            if (command.StartsWith("create"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: create <todo-name> [<todo-description>] [<todo-due-date>]");
                    continue;
                }

                DateTime dueDate = default;
                bool hasDueDate = todoParams.Length == 4 && DateTime.TryParse(todoParams[3], out dueDate);
                DateTime? dueDateParam = hasDueDate ? dueDate : null;

                var newEntry = new TodoEntry(todoParams![1], (todoParams.Length >= 3 ? todoParams[2] : null), dueDateParam);
                todoList.Add(newEntry);

                string dueDateMessage = hasDueDate ? $"(Due date: {dueDateParam})" : "";
                Console.WriteLine($"Added '{newEntry.Title}' to Todo List {dueDateMessage}");
            }

            // command "list"
            else if (command.StartsWith("list"))
            {
                //Console.WriteLine("Id\tTitle\t\tDescription\t\tDueDate");
                Console.WriteLine("{0,-40} {1,-20} {2,-10} {3,-10}", "Id", "Title", "Description", "DueDate");

                Console.WriteLine("------------------------------------------------------------------------------------");

                todoList.ForEach(i => Console.WriteLine($"{i.Id,-40} {i.Title,-15} {i.Description,-20} {i.DueDate}"));
            }


            // command "remove" by Guid
            else if (command.StartsWith("remove"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: remove <todo-id>");
                    continue;
                }
                Guid todoId;
                if (!Guid.TryParse(todoParams[1], out todoId))
                {
                    Console.WriteLine("Invalid todo Id format.");
                    continue;
                }

                TodoEntry removeEntry = todoList.Find(i => i.Id.Equals(todoId));

                if (removeEntry != null)
                {
                    todoList.Remove(removeEntry);
                    Console.WriteLine($"Removed '{removeEntry.Title}' '{removeEntry.Id}'");
                }
                else Console.WriteLine($"Not Found : TodoId '{todoParams[1]}' Cannot Remove");
            }

            // command "filter" by Title
            else if (command.StartsWith("filter"))
            {
                string[] todoParams = command.Split(" ");
                if (todoParams.Length <= 1)
                {
                    Console.WriteLine($"USAGE: filter <todo->title");
                    continue;
                }
                Console.Write("Id\tTitle\t\tDescription\t\tDueDate\n");
                Console.WriteLine("------------------------------------------------------------------------------------\n");
                foreach (var todo in todoList)
                {
                    if (todo.Title.ToLower().Contains(todoParams[1].ToLower()))
                    {
                        Console.WriteLine($"{todo.Id}\t{todo.Title}\t{todo.Description}\t{todo.DueDate}");
                    }
                }
            }

            Console.WriteLine("Your command: {0}", command);
            Console.WriteLine("------------------------------------------------------------------------------------\n");
        }
    }
}
