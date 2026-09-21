using System;
//using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyTimer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static int totalBreaksSpent = 0;
        private static int totalWorksSpent = 0;
        private static int totalTimeSpent => totalBreaksSpent + totalWorksSpent;
        private static volatile bool _stopListener = true;


        // ----------   MAIN   ---------- //
        public static async Task Main(string[] args)
        {
            Console.Title = "❤️🌟⭐️💫💖";
            Console.SetBufferSize(120, 30);


            byte LONG = 0;
            byte SHORT = 0;
            var LONGlist = new List<byte>(16);
            var SHORTlist = new List<byte>(16);

            WriteTextHere(text: "q - exit    e - stop", left: 8, top: 25, color: 0);

            DrawBorder();

            while (true)
            {
                WriteTextHere(text: "Minutes:          ", left: 0, top: 0);

                Console.ForegroundColor = GetRandomColor(); Console.SetCursorPosition(9, 0);
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

                TotalRepeats(in LONG, in SHORT, in worksString, in breaksString);
                HistoryList(in LONGlist, in SHORTlist);


                using var cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task taska = TimerStart(time, token);

                _stopListener = false;
                _ = Task.Run(() =>
                {
                    while (!_stopListener)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(intercept: true);
                            if (key.Key == ConsoleKey.E)
                            {
                                cts.Cancel();
                                _stopListener = true;
                                break;
                            }
                        }
                        Thread.Sleep(150);
                    }});

                await taska;
                _stopListener = true;
            }
        }


        // ----------   TIMER   ---------- //
        private static async Task TimerStart(int time, CancellationToken token)
        {
            Work();
            int oneMinuteInMS = 60000;

            try
            {
                await Task.Delay(time * oneMinuteInMS, token);

                _stopListener = true;
                PlayMusic();
                OpenConsole();
                TotalTimeSpent(in time);
                Done();
            }
            catch (OperationCanceledException)
            {
                Canceled();
            }
        }

        // ----------   TOTAL   ---------- //
        private static void TotalRepeats(in byte LONG, in byte SHORT, in string worksString, in string breaksString)
        {
            WriteTextHere(text: "TIMES", left: 29, top: 2, color: ConsoleColor.Gray);

            WriteTextHere(text: "Breaks (1-15): ", left: 10, top: 3, color: ConsoleColor.Yellow);
            WriteTextHere(text: $"{SHORT + breaksString}", randomColorWord: false);
            WriteTextHere(text: " / Works (15+): ", color: ConsoleColor.Yellow);
            WriteTextHere(text: $"{LONG + worksString}", randomColorWord: false);
        }

        // ----------   HISTORY   ---------- //
        private static void HistoryList(in System.Collections.Generic.List<byte> LONGlist, in System.Collections.Generic.List<byte> SHORTlist)
        {
            WriteTextHere(text: "HISTORY", left: 52, top: 9, color: ConsoleColor.Gray);

            WriteTextHere(text: "Breaks: ", left: 43, top: 10, color: ConsoleColor.Yellow);
            WriteTextHere(text: "|", left: 55, top: 10, color: 0);
            WriteTextHere(text: "Works: ", left: 58, top: 10, color: 0);

            WriteTextHere(text: "[]", left: 51, top: 25, color: 0);
            WriteTextHere(text: "[]", left: 65, top: 25, color: 0);

            for (int i = 0; i < SHORTlist.Count; i++)
            {
                WriteTextHere(text: "   ", left: 51, top: 10 + i, color: 0);
                WriteTextHere(text: $"{SHORTlist[i]}", left: 51, top: 10 + i);
            }

            for (int k = 0; k < LONGlist.Count; k++)
            {
                WriteTextHere(text: "   ", left: 65, top: 10 + k, color: 0);
                WriteTextHere(text: $"{LONGlist[k]}", left: 65, top: 10 + k);
            }
        }

        // ----------   TIMESPENT   ---------- //
        private static void TotalTimeSpent(in int time)
        {
            if (time > 15) // long | work
            {
                totalWorksSpent += time;
            }
            else // short | break
            {
                totalBreaksSpent += time;
            }

            WriteTextHere(text: "TIME SPENT", left: 91, top: 14, color: ConsoleColor.Gray);
            WriteTextHere(text: "Breaks - ", left: 85, top: 15, color: ConsoleColor.Yellow);
            WriteTextHere(text: "Works - ", left: 91, top: 16, color: 0);
            WriteTextHere(text: "Total - ", left: 96, top: 17, color: 0);

            WriteTextHere(text: $"{totalBreaksSpent}", left: 94, top: 15);
            WriteTextHere(text: $"{totalWorksSpent}", left: 99, top: 16);
            WriteTextHere(text: $"{totalTimeSpent}", left: 104, top: 17);

        }

        // ----------   OPEN   ---------- //
        private static void OpenConsole()
        {
            ShowWindow(GetConsoleWindow(), 9);
        }

        // ----------   DONE   ---------- //
        private static void Done()
        {
            WriteTextHere(text: "DONE!        ", left: 10, top: 5, randomColorWord: true);
        }

        // ----------   CANCELED   ---------- //
        private static void Canceled()
        {
            WriteTextHere(text: "CANCELED!        ", left: 10, top: 5, randomColorWord: true);
        }

        // ----------   WORK   ---------- //
        private static void Work()
        {
            WriteTextHere(text: "PROCESSING...", left: 10, top: 5, randomColorWord: true);
        }

        // ----------   COLOR   ---------- //
        private static ConsoleColor GetRandomColor()
        {
            return Colors[Random.Shared.Next(Colors.Length)];
        }

        private static readonly ConsoleColor[] Colors =
        [
                ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Blue,
                ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan,
                ConsoleColor.DarkGray, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta,
                ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Gray
        ];

        // ----------   DRAW   ---------- //
        private static void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // yellow

            for (byte i = 83; i <= 108; i++)
            {
                WriteTextHere(text: "-", left: i, top: 13, color: 0);
                WriteTextHere(text: "-", left: i, top: 18, color: 0);
            }
            for (byte i = 14; i <= 17; i++) // -1
            {
                WriteTextHere(text: "|", left: 83, top: i, color: 0);
                WriteTextHere(text: "|", left: 108, top: i, color: 0);
            }

            WriteTextHere(text: "+", left: 83, top: 13, color: 0);
            WriteTextHere(text: "+", left: 108, top: 13, color: 0);
            WriteTextHere(text: "+", left: 83, top: 18, color: 0);
            WriteTextHere(text: "+", left: 108, top: 18, color: 0);
            //Console.WriteLine("═══════════════════");
        }

        // ---------- DRAW TEXT HERE!!! ----------//
        private static void WriteTextHere(string text, int left = -1, int top = -1)
        {
            Console.ForegroundColor = GetRandomColor();

            if (left >= 0 && top >= 0)
                Console.SetCursorPosition(left, top);
            Console.Write(text);
        }

        private static void WriteTextHere(string text, int left = -1, int top = -1, bool randomColorWord = false)
        {
            if (randomColorWord == false)
            {
                WriteTextHere(text, left, top);
                return;
            }

            if (left >= 0 && top >= 0)
                Console.SetCursorPosition(left, top);
            for (int i = 0; i < text.Length; i++)
            {
                Console.ForegroundColor = GetRandomColor(); Console.Write(text[i]);
            }
        }

        private static void WriteTextHere(string text, int left = -1, int top = -1, ConsoleColor color = 0)
        {
            if (color != 0)
            {
                Console.ForegroundColor = color;
            }

            if (left >= 0 && top >= 0)
                Console.SetCursorPosition(left, top);
            Console.Write(text);
        }

        // ----------   MUSIC   ---------- //
        private static void PlayMusic()
        {
            // ♩ ♪ ♪ ♪ | ♩ ♪ ♪ ♪ | финал (медленный диско)
            Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
            Console.Beep(294, 250); System.Threading.Thread.Sleep(40);  // ту  
            Console.Beep(330, 250); System.Threading.Thread.Sleep(80);  // ту

            Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
            Console.Beep(294, 250); System.Threading.Thread.Sleep(40);  // ту
            Console.Beep(330, 250); System.Threading.Thread.Sleep(80);  // ту

            Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
            Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
            Console.Beep(220, 400); System.Threading.Thread.Sleep(100); // тин (завершение)
        }
    }
}
