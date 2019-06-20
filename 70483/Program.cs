
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
            int i=1;
            Dictionary<int, MenuOptions> list = new Dictionary<int, MenuOptions>();
            list.Add(i, new MenuOptions() { Description = "Non parallel invoke", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.Non_Parallel_Invoke });
            i++;
            list.Add(i, new MenuOptions() { Description = "Parallel invoke", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.Parallel_Invoke_Foreach });
            i++;
            list.Add(i, new MenuOptions() { Description = "Task use", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.TaskUse });
            i++;
            list.Add(i, new MenuOptions() { Description = "Task use return values", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.TaskReturnValues });
            i++;
            list.Add(i, new MenuOptions() { Description = "Raw Threads", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.RawThread });
            i++;
            list.Add(i, new MenuOptions() { Description = "Threads and Lambda", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.ThreadAndLambda });
            i++;
            list.Add(i, new MenuOptions() { Description = "Task use", OptionId = i, Method = DotNet.E70483.ProgramFlow.MultiThreading.TaskUse });
            i++;
            list.Add(i, new MenuOptions() { Description = "Slicing", OptionId = i, Method = DotNet.E70483.ProgramFlow.ManageMultiThreading.Slicing});
            i++;
            list.Add(i, new MenuOptions() { Description = "Locks", OptionId = i, Method = DotNet.E70483.ProgramFlow.ManageMultiThreading.GoodAndBadSums });
            i++;
            list.Add(i, new MenuOptions() { Description = "Program Flow", OptionId = i, Method = DotNet.E70483.ProgramFlow.ImplementProgramFlow.Seriously});
            i++;
            list.Add(i, new MenuOptions() { Description = "Events", OptionId = i, Method = DotNet.E70483.ProgramFlow.ImplementEventsAndCallbacks.CantStopTheSignal });
            i++;
            list.Add(i, new MenuOptions() { Description = "Exceptions", OptionId = i, Method = DotNet.E70483.ProgramFlow.ExceptionHandling.ThrowAllTheExceptions });
            i++;
            list.Add(i, new MenuOptions() { Description = "Type exercises", OptionId = i, Method = DotNet.E70483.CreateUseTypes.CreateTypes.ExerciseTypes });
            i++;

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

