namespace MyTimer.UI;

internal static class Gui
{
    // ----------   DRAW   ---------- //
    internal static void DrawBorder()
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

    // ----------   DONE   ---------- //
    internal static void Done()
    {
        WriteTextHere(text: "DONE!        ", left: 10, top: 5, randomColorPerChar: true);
    }

    // ----------   CANCELED   ---------- //
    internal static void Canceled()
    {
        WriteTextHere(text: "CANCELED!        ", left: 10, top: 5, randomColorPerChar: true);
    }

    // ----------   WORK   ---------- //
    internal static void Work()
    {
        WriteTextHere(text: "PROCESSING...", left: 10, top: 5, randomColorPerChar: true);
    }

}
