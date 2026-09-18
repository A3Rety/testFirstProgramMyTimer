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

            Console.SetCursorPosition(8, 25);
            Console.Write("q - exit    e - stop");

            DrawBorder();

            while (true)
            {
                Console.ForegroundColor = GetRandomColor();
                Console.SetCursorPosition(0, 0);
                Console.Write($"Minutes:          ");

                Console.ForegroundColor = GetRandomColor();
                Console.SetCursorPosition(9, 0);
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

                Task taska = TimerStart(time * 60000, token);

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
                    }
                });

                await taska;
                _stopListener = true;
            }
        }


        // ----------   TIMER   ---------- //
        private static async Task TimerStart(int time, CancellationToken token)
        {
            Work();

            try
            {
                await Task.Delay(time, token);

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
            Console.SetCursorPosition(29, 2);
            Console.ForegroundColor = ConsoleColor.Gray; Console.Write("TIMES");

            Console.SetCursorPosition(10, 3);
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("Breaks (1-15): ");
            Console.ForegroundColor = GetRandomColor(); Console.Write(SHORT + breaksString);
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" / ");
            Console.Write("Works (15+): ");
            Console.ForegroundColor = GetRandomColor(); Console.Write(LONG + worksString);
        }

        // ----------   HISTORY   ---------- //
        private static void HistoryList(in System.Collections.Generic.List<byte> LONGlist, in System.Collections.Generic.List<byte> SHORTlist)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(52, 9);
            Console.Write("HISTORY");

            Console.ForegroundColor = ConsoleColor.Yellow; // yellow

            Console.SetCursorPosition(43, 10);
            Console.Write("Breaks: ");

            Console.SetCursorPosition(55, 10);
            Console.Write("|");

            Console.SetCursorPosition(58, 10);
            Console.Write("Works: ");

            Console.SetCursorPosition(51, 25);
            Console.Write("[]");
            Console.SetCursorPosition(65, 25);
            Console.Write("[]");

            for (int i = 0; i < SHORTlist.Count; i++)
            {
                Console.SetCursorPosition(51, 10 + i); Console.Write("   ");
                Console.SetCursorPosition(51, 10 + i);
                Console.ForegroundColor = GetRandomColor(); Console.Write(SHORTlist[i]);
            }

            for (int k = 0; k < LONGlist.Count; k++)
            {
                Console.SetCursorPosition(65, 10 + k); Console.Write("   ");
                Console.SetCursorPosition(65, 10 + k);
                Console.ForegroundColor = GetRandomColor(); Console.Write(LONGlist[k]);
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

            Console.SetCursorPosition(91, 14);
            Console.ForegroundColor = ConsoleColor.Gray; Console.Write("TIME SPENT");

            Console.ForegroundColor = ConsoleColor.Yellow; // Yellow
            Console.SetCursorPosition(85, 15); Console.Write("Breaks - ");
            Console.SetCursorPosition(91, 16); Console.Write("Works - ");
            Console.SetCursorPosition(96, 17); Console.Write("Total - ");

            Console.SetCursorPosition(94, 15);
            Console.ForegroundColor = GetRandomColor(); Console.Write(totalBreaksSpent);
            Console.SetCursorPosition(99, 16);
            Console.ForegroundColor = GetRandomColor(); Console.Write(totalWorksSpent);
            Console.SetCursorPosition(104, 17);
            Console.ForegroundColor = GetRandomColor(); Console.Write(totalTimeSpent);
        }

        // ----------   OPEN   ---------- //
        private static void OpenConsole()
        {
            ShowWindow(GetConsoleWindow(), 9);
        }

        // ----------   DONE   ---------- //
        private static void Done()
        {
            //Console.Clear();
            Console.SetCursorPosition(10, 5);
            Console.ForegroundColor = GetRandomColor(); Console.Write("D");
            Console.ForegroundColor = GetRandomColor(); Console.Write("O");
            Console.ForegroundColor = GetRandomColor(); Console.Write("N");
            Console.ForegroundColor = GetRandomColor(); Console.Write("E");
            Console.ForegroundColor = GetRandomColor(); Console.Write("!");
            Console.Write("        ");
        }

        // ----------   CANCELED   ---------- //
        private static void Canceled()
        {
            Console.SetCursorPosition(10, 5);
            Console.ForegroundColor = GetRandomColor(); Console.Write("C");
            Console.ForegroundColor = GetRandomColor(); Console.Write("A");
            Console.ForegroundColor = GetRandomColor(); Console.Write("N");
            Console.ForegroundColor = GetRandomColor(); Console.Write("C");
            Console.ForegroundColor = GetRandomColor(); Console.Write("E");
            Console.ForegroundColor = GetRandomColor(); Console.Write("L");
            Console.ForegroundColor = GetRandomColor(); Console.Write("E");
            Console.ForegroundColor = GetRandomColor(); Console.Write("D");
            Console.ForegroundColor = GetRandomColor(); Console.Write("!");
            Console.Write("        ");
        }

        // ----------   WORK   ---------- //
        private static void Work()
        {
            Console.SetCursorPosition(10, 5);
            Console.ForegroundColor = GetRandomColor(); Console.Write("P");
            Console.ForegroundColor = GetRandomColor(); Console.Write("R");
            Console.ForegroundColor = GetRandomColor(); Console.Write("O");
            Console.ForegroundColor = GetRandomColor(); Console.Write("C");
            Console.ForegroundColor = GetRandomColor(); Console.Write("E");
            Console.ForegroundColor = GetRandomColor(); Console.Write("S");
            Console.ForegroundColor = GetRandomColor(); Console.Write("S");
            Console.ForegroundColor = GetRandomColor(); Console.Write("I");
            Console.ForegroundColor = GetRandomColor(); Console.Write("N");
            Console.ForegroundColor = GetRandomColor(); Console.Write("G");
            Console.ForegroundColor = GetRandomColor(); Console.Write(".");
            Console.ForegroundColor = GetRandomColor(); Console.Write(".");
            Console.ForegroundColor = GetRandomColor(); Console.Write(".");
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
                Console.SetCursorPosition(i, 13);
                Console.Write("-");
                Console.SetCursorPosition(i, 18);
                Console.Write("-");
            }
            for (byte i = 14; i <= 17; i++) // -1
            {
                Console.SetCursorPosition(83, i);
                Console.Write("|");
                Console.SetCursorPosition(108, i);
                Console.Write("|");
            }

            Console.SetCursorPosition(83, 13);
            Console.Write("+");
            Console.SetCursorPosition(108, 13);
            Console.Write("+");
            Console.SetCursorPosition(83, 18);
            Console.Write("+");
            Console.SetCursorPosition(108, 18);
            Console.Write("+");
            //Console.WriteLine("═══════════════════");
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
