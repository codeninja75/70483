
namespace DotNet.E70483
{
    using DotNet.E70483.Helpers;
    using System;
    using System.Collections.Generic;

    public class MenuOptions
    {
        public int OptionId { get; set; }
        public MenuMethod Method { get; set; }
        public string Description { get; set; }
    }
    public delegate void MenuMethod();
    class Program
    {
        static Dictionary<int , MenuOptions> GetMenuOptions()
        {
            Dictionary<int, MenuOptions> list = new Dictionary<int, MenuOptions>();
            list.Add(1, new MenuOptions() { Description = "Non parallel invoke", OptionId = 1, Method = DotNet.E70483.ProgramFlow.MultiThreading.Non_Parallel_Invoke });
            list.Add(2, new MenuOptions() { Description = "Parallel invoke", OptionId = 2, Method = DotNet.E70483.ProgramFlow.MultiThreading.Parallel_Invoke });

            return list;
        }
        static void Main(string[] args)
        {
            var options = GetMenuOptions();
            PrintMenu(options);
            while (true)
            {
                var input = Console.ReadLine();
                int check;
                if (int.TryParse(input, out check))
                {
                    if (options.ContainsKey(check))
                    {
                        options[check].Method();
                        Console.WriteLine("Press anykey to refresh menu...");
                        Console.Read();
                        PrintMenu(options);
                    }
                }
               

            }

        }

        public static void PrintMenu(Dictionary<int, MenuOptions> list)
        {
            Console.Clear();
            foreach(var v in list.Values)
            {
                Console.WriteLine($"{v.OptionId}    : {v.Description}");
            }
        }
    }

}

