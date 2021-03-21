using System;
using System.Collections.Generic;
using System.Text;

namespace FaccioConsolePanelsLibrary
{
    public static class ConPanelEnum
    {
        public enum BORDERTYPE
        {
            EMPTY,
            LINE
        }

        public enum AUTOLINE
        {
            NEWLINE,
            NEWCOLUMN,
            BOTH,
            NONE
        }

        public static int[] FindTextSize(string[] options)
        {
            int[] maxSpace = new int[2];    // Max space of horizontal(0) and vertical (1)

            // Find width
            foreach (string option in options)
            {
                if (option.Length > maxSpace[0])
                    maxSpace[0] = option.Length + 2;
            }

            // Find height
            maxSpace[1] = options.Length;   

            return maxSpace;
        }
    }
}
