using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCacher
{
    internal class Timer
    {

        static void TimerCallback(object state)
        {
            // Clear the current line by moving the cursor to the beginning and writing spaces
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));

            // Move the cursor back to the beginning of the line
            Console.SetCursorPosition(0, Console.CursorTop);

            // Write the current time
            Console.Write($"Timer fired at: {DateTime.Now}");
        }
    }
}
