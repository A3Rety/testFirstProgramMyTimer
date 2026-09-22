using MyTimer.Misc;
using System.Collections.Generic;

namespace MyTimer.Logic;

internal static class Architecture
{
    // ----------   TOTAL   ---------- //
    internal static void TotalRepeats(in byte LONG, in byte SHORT, in string worksString, in string breaksString)
    {
        WriteTextHere(text: "TIMES", left: 29, top: 2, color: ConsoleColor.Gray);

        WriteTextHere(text: "Breaks (1-15): ", left: 10, top: 3, color: ConsoleColor.Yellow);
        WriteTextHere(text: $"{SHORT + breaksString}", randomColorPerChar: false);
        WriteTextHere(text: " / Works (15+): ", color: ConsoleColor.Yellow);
        WriteTextHere(text: $"{LONG + worksString}", randomColorPerChar: false);
    }

    // ----------   HISTORY   ---------- //
    internal static void HistoryList(in List<byte> LONGlist, in List<byte> SHORTlist)
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
    internal static void TotalTimeSpent(in int time, in int diffTime = -1)
    {
        int addValue;
        if (diffTime > -1)
        {
            addValue = diffTime;
        }
        else
        {
            addValue = time;
        }


        if (time > 15) // long | work
        {
            Program.TotalWorksSpent += addValue;
        }
        else // short | break
        {
            Program.TotalBreaksSpent += addValue;
        }

        WriteTextHere(text: "TIME SPENT", left: 91, top: 14, color: ConsoleColor.Gray);
        WriteTextHere(text: "Breaks - ", left: 85, top: 15, color: ConsoleColor.Yellow);
        WriteTextHere(text: "Works - ", left: 91, top: 16, color: 0);
        WriteTextHere(text: "Total - ", left: 96, top: 17, color: 0);

        WriteTextHere(text: $"{Program.TotalBreaksSpent}", left: 94, top: 15);
        WriteTextHere(text: $"{Program.TotalWorksSpent}", left: 99, top: 16);
        WriteTextHere(text: $"{Program.TotalTimeSpent}", left: 104, top: 17);
    }

}
