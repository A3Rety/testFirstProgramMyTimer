namespace MyTimer.Misc;

internal static class ConsoleCommands
{
    [System.Runtime.InteropServices.DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // ----------   MUSIC   ---------- //
    internal static void PlayMusic()
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

    internal static void PlaySound()
    {
        Console.Beep(330, 150);
    }

    // ----------   OPEN   ---------- //
    internal static void OpenConsole()
    {
        ShowWindow(GetConsoleWindow(), 9);
    }

}
