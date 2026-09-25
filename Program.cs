using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MyTimer.Logic;
using MyTimer.Misc;
using MyTimer.UI;

namespace MyTimer;

internal static class Program
{
    internal static int TotalBreaksSpent = 0;
    internal static int TotalWorksSpent = 0;
    internal static int TotalTimeSpent => TotalBreaksSpent + TotalWorksSpent;
    internal static volatile bool StopListener = true;

    private static readonly List<byte> LONGlist = new(16);
    private static readonly List<byte> SHORTlist = new(16);
    private static readonly Stopwatch stopWatch = new();


    // ----------   MAIN   ---------- //
    internal static async Task Main()
    {
        Console.Title = "❤️🌟⭐️💫💖";
        Console.SetBufferSize(120, 30);

        byte LONG = 0;
        byte SHORT = 0;

        WriteTextHere(text: "q - exit    e - time", left: 8, top: 25, color: 0);
        WriteTextHere(text: "r - stop", left: 20, top: 26, color: 0);
        Gui.DrawBorder();

        while (true)
        {
            WriteTextHere(text: "Minutes: " + new string (' ', 230), left: 0, top: 0);

            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }

            Console.ForegroundColor = Resources.GetRandomColor(); Console.SetCursorPosition(9, 0);
            string input = Console.ReadLine() ?? "";

            if (input == "q") return;
            if (!int.TryParse(input, out int time)) continue;

            string breaksString;
            string worksString;
            if (time > 15)
            {
                if (time >= 254) return;

                LONG++; worksString = "(+1)"; breaksString = "";

                LONGlist.Insert(0, (byte)time);

                if (LONGlist.Count > 15)
                {
                    LONGlist.RemoveAt(LONGlist.Count - 1);
                    LONGlist.Capacity = 15;
                }
            }
            else
            {
                if (time <= -1) return;

                SHORT++; worksString = ""; breaksString = "(+1)";

                SHORTlist.Insert(0, (byte)time);

                if (SHORTlist.Count > 15)
                {
                    SHORTlist.RemoveAt(SHORTlist.Count - 1);
                    SHORTlist.Capacity = 15;
                }
            }

            Architecture.TotalRepeats(in LONG, in SHORT, in worksString, in breaksString);
            Architecture.HistoryList(in LONGlist, in SHORTlist);


            StopListener = false;
            await StartTimerToken(time);
            StopListener = true;
        }
    }


    // ----------   TIMER   ---------- //
    internal static async Task TimerStart(int time, CancellationToken token)
    {
        stopWatch.Restart();
        Gui.Work();
        int oneMinuteInMS = 60000;

        try
        {
            await Task.Delay(time * oneMinuteInMS, token);

            stopWatch.Stop();
            StopListener = true;
            ConsoleCommands.PlayMusic();
            ConsoleCommands.OpenConsole();
            Architecture.TotalTimeSpent(in time);
            Gui.Done();
        }
        catch (OperationCanceledException)
        {
            stopWatch.Stop();
            int roundedDelta = (int)Math.Round(stopWatch.Elapsed.TotalMinutes, MidpointRounding.AwayFromZero);
            if (time > 15)
            {
                LONGlist[0] = (byte)roundedDelta;
            }
            else
            {
                SHORTlist[0] = (byte)roundedDelta;
            }

            Misc.ConsoleCommands.PlaySound();
            Architecture.TotalTimeSpent(in time, roundedDelta);
            Architecture.HistoryList(in LONGlist, in SHORTlist);
            Gui.Canceled();
        }

        stopWatch.Reset();
    }


    private static async Task StartTimerToken(int time)
    {
        using var cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;

        Task taska = TimerStart(time, token);

        _ = Task.Run(() =>
        {
            while (!StopListener)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.R)
                    {
                        cts.Cancel();
                        StopListener = true;
                        break;
                    }
                    else if (key.Key == ConsoleKey.E)
                    {
                        WriteTextHere(text: $"{stopWatch.Elapsed:hh\\:mm\\:ss}", left: 20, top: 23);
                        continue;
                    }
                }
                Thread.Sleep(150);
            }
            WriteTextHere(text: "        ", left: 20, top: 23);
        });

        await taska;
    }

}