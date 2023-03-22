using System;
using System.IO;
using System.IO.Compression;
using System.Reflection.PortableExecutable;

namespace Scrapper;

public static class Program
{
    private static string[] args;
    private static uint argc;


    private static string Shift()
    {
        try
        {
            return args[argc++];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    private static void Usage()
    {
        Console.WriteLine("scrapper help");
        Console.WriteLine("scrapper build <name>");
        Console.WriteLine("scrapper load <name>");
        Console.WriteLine("scrapper list");
    }

    public static int Main(string[] args)
    {
        argc = 0;
        Program.args = args;

        if (args.Length == 0)
        {
            Usage();
            return 1;
        }

        string command = Shift();
        string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string data = Path.Combine(roaming, "scrapper");
        string cwd = Environment.CurrentDirectory;


        if (command == "build")
        {
            string name = Shift();
            if (name == null)
            {
                Usage();
                return 1;
            }

            CheckFolder(roaming);
            ZipFile.CreateFromDirectory(cwd, Path.Combine(data, name));
        }
        else if (command == "load")
        {
            string name = Shift();
            if (name == null)
            {
                Usage();
                return 1;
            }

            CheckFolder(roaming);
            if (File.Exists(Path.Combine(data, name)))
            {
                ZipFile.ExtractToDirectory(Path.Combine(data, name), cwd, true);
            }
            else
            {
                Console.WriteLine($"ERROR: No build found with the name '{name}'");
                return 1;
            }
        }
        else if (command == "list")
        {
            CheckFolder(roaming);
            foreach (string file in Directory.GetFiles(data))
            {
                Console.WriteLine("> " + Path.GetFileName(file));
            }
        }
        else
        {
            Usage();
        }

        return 0;
    }

    private static void CheckFolder(string folder)
    {
        if (!Directory.Exists(Path.Combine(folder, "scrapper")))
        {
            Directory.CreateDirectory(Path.Combine(folder, "scrapper"));
        }
    }
}