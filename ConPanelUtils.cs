using System;
using System.Collections.Generic;
using System.Text;

namespace FaccioConsolePanelsLibrary
{
    public static class ConPanelUtils
    {

        public static int GetAutoWidth(string[] options)
        {
            int widthMax = 0;
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].Length > widthMax)
                    widthMax = options[i].Length;
            }

            return widthMax;
        }
    }
}
