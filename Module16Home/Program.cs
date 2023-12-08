using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program
{
    static string logFilePath = "file.txt";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Просмотр содержимого директории");
            Console.WriteLine("2. Создание файла/директории");
            Console.WriteLine("3. Удаление файла/директории");
            Console.WriteLine("4. Копирование файла/директории");
            Console.WriteLine("5. Перемещение файла/директории");
            Console.WriteLine("6. Чтение из файла");
            Console.WriteLine("7. Запись в файл");
            Console.WriteLine("8. Вывести лог действий");
            Console.WriteLine("0. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewDirectoryContents();
                    break;

                case "2":
                    CreateFileOrDirectory();
                    break;

                case "3":
                    DeleteFileOrDirectory();
                    break;

                case "4":
                    CopyFileOrDirectory();
                    break;

                case "5":
                    MoveFileOrDirectory();
                    break;

                case "6":
                    ReadFromFile();
                    break;

                case "7":
                    WriteToFile();
                    break;

                case "8":
                    DisplayLog();
                    break;

                case "0":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Неверный ввод. Повторите попытку.");
                    break;
            }
        }
    }

    static void ViewDirectoryContents()
    {
        Console.Write("Введите путь к директории: ");
        string path = Console.ReadLine();

        try
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            Console.WriteLine("\nФайлы:");
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }

            Console.WriteLine("\nДиректории:");
            foreach (var directory in directories)
            {
                Console.WriteLine(Path.GetFileName(directory));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CreateFileOrDirectory()
    {
        Console.Write("Введите путь: ");
        string path = Console.ReadLine();

        Console.Write("Введите имя файла/директории: ");
        string name = Console.ReadLine();

        Console.Write("Выберите тип (1 - файл, 2 - директория): ");
        string typeChoice = Console.ReadLine();

        try
        {
            if (typeChoice == "1")
            {
                File.Create(Path.Combine(path, name)).Close();
                LogAction($"Создан файл: {Path.Combine(path, name)}");
            }
            else if (typeChoice == "2")
            {
                Directory.CreateDirectory(Path.Combine(path, name));
                LogAction($"Создана директория: {Path.Combine(path, name)}");
            }
            else
            {
                Console.WriteLine("Неверный выбор типа. Повторите попытку.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void DeleteFileOrDirectory()
    {
        Console.Write("Введите путь к файлу/директории: ");
        string path = Console.ReadLine();

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                LogAction($"Удален файл: {path}");
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                LogAction($"Удалена директория: {path}");
            }
            else
            {
                Console.WriteLine("Файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CopyFileOrDirectory()
    {
        Console.Write("Введите путь к файлу/директории для копирования: ");
        string sourcePath = Console.ReadLine();

        Console.Write("Введите путь, куда скопировать: ");
        string destinationPath = Console.ReadLine();

        try
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)), true);
                LogAction($"Скопирован файл: {sourcePath} -> {Path.Combine(destinationPath, Path.GetFileName(sourcePath))}");
            }
            else if (Directory.Exists(sourcePath))
            {
                CopyDirectory(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                LogAction($"Скопирована директория: {sourcePath} -> {Path.Combine(destinationPath, Path.GetFileName(sourcePath))}");
            }
            else
            {
                Console.WriteLine("Файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void MoveFileOrDirectory()
    {
        Console.Write("Введите путь к файлу/директории для перемещения: ");
        string sourcePath = Console.ReadLine();

        Console.Write("Введите путь, куда переместить: ");
        string destinationPath = Console.ReadLine();

        try
        {
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                LogAction($"Перемещен файл: {sourcePath} -> {Path.Combine(destinationPath, Path.GetFileName(sourcePath))}");
            }
            else if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                LogAction($"Перемещена директория: {sourcePath} -> {Path.Combine(destinationPath, Path.GetFileName(sourcePath))}");
            }
            else
            {
                Console.WriteLine("Файл/директория не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void ReadFromFile()
    {
        Console.Write("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        try
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine($"\nСодержимое файла {filePath}:\n{content}");
            }
            else
            {
                Console.WriteLine("Файл не существует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void WriteToFile()
    {
        Console.Write("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        Console.WriteLine("Введите текст для записи в файл (для завершения ввода введите пустую строку):");
        string content = "";
        string line;
        do
        {
            line = Console.ReadLine();
            content += line + Environment.NewLine;
        } while (!string.IsNullOrWhiteSpace(line));

        try
        {
            File.WriteAllText(filePath, content);
            LogAction($"Записан текст в файл: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void LogAction(string action)
    {
        try
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {action}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
        }
    }

    static void DisplayLog()
    {
        try
        {
            if (File.Exists(logFilePath))
            {
                string logContent = File.ReadAllText(logFilePath);
                Console.WriteLine($"\nЛог действий:\n{logContent}");
            }
            else
            {
                Console.WriteLine("Лог действий отсутствует.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CopyDirectory(string sourcePath, string destinationPath)
    {
        Directory.CreateDirectory(destinationPath);

        foreach (string file in Directory.GetFiles(sourcePath))
        {
            string dest = Path.Combine(destinationPath, Path.GetFileName(file));
            File.Copy(file, dest, true);
        }

        foreach (string dir in Directory.GetDirectories(sourcePath))
        {
            string dest = Path.Combine(destinationPath, Path.GetFileName(dir));
            CopyDirectory(dir, dest);
        }
    }
}

