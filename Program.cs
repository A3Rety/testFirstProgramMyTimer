//using System;
//using System.Threading;
//using System.Runtime.InteropServices;
//using System.Collections.Generic;

namespace MyTimer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static Random rand = new Random();

        static int totalBreaksSpent = 0;
        static int totalWorksSpent = 0;
        static int totalTimeSpent => totalBreaksSpent + totalWorksSpent;

        public static void Main(string[] args)
        {
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "❤️🌟⭐️💫💖";
            Console.SetBufferSize(120, 30);


            byte LONG = 0;
            byte SHORT = 0;
            System.Collections.Generic.List<byte> LONGlist = new System.Collections.Generic.List<byte>(16);
            System.Collections.Generic.List<byte> SHORTlist = new System.Collections.Generic.List<byte>(16);

            DrawBorder();

            while (true)
            {
                //Count(in LONG, in SHORT, in worksString, in breaksString);
                //HistoryList(in LONGlist, in SHORTlist);

                Console.ForegroundColor = GetRandomColor();
                Console.SetCursorPosition(0, 0);
                Console.Write($"Minutes:          ");

                Console.ForegroundColor = GetRandomColor();
                Console.SetCursorPosition(9, 0);
                string input = Console.ReadLine() ?? "";

                if (input == "q") return;
                if (!int.TryParse(input, out int time)) break;

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
                    //if (LONGlist.Count >= 15) LONGlist.TrimExcess();
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
                    //if (SHORTlist.Count >= 15) SHORTlist.TrimExcess();
                }
                //GC.Collect();

                TotalRepeats(in LONG, in SHORT, in worksString, in breaksString);
                HistoryList(in LONGlist, in SHORTlist);
                Work();

                int convertedTimeValueInOneMinute = 60000;
                System.Threading.Thread.Sleep(time * convertedTimeValueInOneMinute);

                PlayMusic();
                OpenConsole();
                TotalTimeSpent(in time);
                Done();
            }
        }


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
            Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("TIME SPENT");

            Console.ForegroundColor = ConsoleColor.Yellow; // Yellow
            Console.SetCursorPosition(85, 15); Console.WriteLine("Breaks - ");
            Console.SetCursorPosition(91, 16); Console.WriteLine("Works - ");
            Console.SetCursorPosition(96, 17); Console.WriteLine("Total - ");

            Console.SetCursorPosition(94, 15);
            Console.ForegroundColor = GetRandomColor(); Console.WriteLine(totalBreaksSpent);
            Console.SetCursorPosition(99, 16);
            Console.ForegroundColor = GetRandomColor(); Console.WriteLine(totalWorksSpent);
            Console.SetCursorPosition(104, 17);
            Console.ForegroundColor = GetRandomColor(); Console.WriteLine(totalTimeSpent);
        }

        private static void OpenConsole()
        {
            ShowWindow(GetConsoleWindow(), 9);
        }

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

        private static ConsoleColor GetRandomColor()
        {
            var colors = new[] {
                ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Blue,
                ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan,
                ConsoleColor.DarkGray, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta,
                ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Gray
            };
            return colors[rand.Next(colors.Length)];
        }

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