using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaccioConsolePanelsLibrary
{
    public class ConPanel
    {
        // Size of the Panel starting from 1
        public int PanWidth { get; private set; }
        public int PanHeight { get; private set; }

        // Position of the Panel starting from 0
        public int PanX { get; private set; }
        public int PanY { get; private set; }

        // Position of the Panel Cursor starting from 0
        public int CursorX { get; private set; }
        public int CursorY { get; private set; }

        public bool WriteAutoNewLine { get; set; }  // Go to next line when run out of space in line
        public bool WriteAutoRebeginLines { get; set; } // Go to the top when run out of lines 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panWidth"> Width of the panel </param>
        /// <param name="panHeight"> Heigh of the panel </param>
        /// <param name="panX"> Position of the Panel </param>
        /// <param name="panY"> Position of the Panel </param>
        /// <param name="box"> Draw the outside box </param>
        /// <param name="writeAutoNewLine"> Go to next line when run out of space in line </param>
        /// <param name="writeAutoRebeginLines"> Go to the top when run out of lines </param>
        public ConPanel(int panWidth, int panHeight, int panX, int panY,bool box, bool writeAutoNewLine, 
            bool writeAutoRebeginLines)
        {
            // Asssing properties
            PanWidth = panWidth;
            PanHeight = panHeight;
            PanX = panX;
            PanY = panY;
            WriteAutoNewLine = writeAutoNewLine;
            WriteAutoRebeginLines = writeAutoRebeginLines;

            if (box)
                PrintBox();
        }

        public void PrintBox()
        {
            // TODO: Make it in a variable? Deletable? Customizable?
            int boxXMin = this.PanX - 1;
            int boxXMax = this.PanX + this.PanWidth;
            int boxYMin = this.PanY - 1;
            int boxYMax = this.PanY + this.PanHeight;

            Console.SetCursorPosition(boxXMin, boxYMin);
            Console.WriteLine("╔");
            Console.SetCursorPosition(boxXMax, boxYMin);
            Console.WriteLine("╗");
            Console.SetCursorPosition(boxXMin, boxYMax);
            Console.WriteLine("╚");
            Console.SetCursorPosition(boxXMax, boxYMax);
            Console.WriteLine("╝");

            // Stampo linee del boxo superiore e inferiore
            for (int i = 0; i < this.PanWidth; i++)
            {
                Console.SetCursorPosition(i + this.PanX, boxYMin);
                Console.Write("═");
                Console.SetCursorPosition(i + this.PanX, boxYMin + this.PanHeight + 1);
                Console.Write("═");
            }

            // Stampo linee del boxo destro e sinistro
            for (int i = 0; i < this.PanHeight; i++)
            {
                Console.SetCursorPosition(boxXMin, i + this.PanY);
                Console.Write("║");
                Console.SetCursorPosition(boxXMin + this.PanWidth + 1, i + this.PanY );
                Console.Write("║");
            }

        }



        /// <summary>
        /// Update the Cursor position properties to x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void UpdateCursorPosition(int x, int y)
        {
            CursorX = x;
            CursorY = y;
        }

        /// <summary>
        /// Update Cursor position properties using the current Console cursor position
        /// </summary>
        private void UpdateCursorPosition()
        {
            this.CursorX = Console.CursorLeft - this.PanX;
            this.CursorY = Console.CursorTop - this.PanY;
        }

        private void GoToCursorPosition()
        {
            Console.SetCursorPosition(this.PanX + this.CursorX, this.PanY + this.CursorY);
        }

        public void SetCursorPosition(int x,int y)
        {
            if ((x < this.PanWidth && x >= 0) && (y < this.PanHeight && y >= 0))
                Console.SetCursorPosition(this.PanX + x, this.PanY + y);
            else
                throw new ArgumentException("Pan cursor position of of bounds");

            UpdateCursorPosition();
        }

        /// <summary>
        /// Equivalent of Console.Write in the Panel
        /// </summary>
        /// <param name="_string"> String to write to console </param>
        public void Write(string _string)
        {
            int _stringPos = 0;

            bool loop = true;
            while(loop)
            {
                int HorSpaceLeft = Math.Min(this.PanWidth - CursorX - 1, _string.Length - _stringPos);

                // Write as much as space available
                this.GoToCursorPosition();
                Console.Write(_string.Substring(_stringPos, HorSpaceLeft));
                this.UpdateCursorPosition();

                _stringPos += HorSpaceLeft; // Advance in the _string

                if (_stringPos < _string.Length && HorSpaceLeft > 0) // If string to write is not over
                {
                    // Auto new line
                    if (this.CursorX + 1 >= this.PanWidth && this.WriteAutoNewLine)
                        this.WriteLine();
                }
                else
                    loop = false;       // String is over, end loop   
            }
        }

        /// <summary>
        /// Equivalent of Console.WriteLine but with outside console in mind
        /// </summary>
        /// <returns> Is next line is possible </returns>
        public bool WriteLine()
        {
            if (CursorY + 1 < PanHeight)
            {
                this.SetCursorPosition(0, CursorY + 1);
                return true;
            }
            else
            {
                if (WriteAutoRebeginLines == true)
                {
                    this.SetCursorPosition(0, 0);
                    return true;
                }
                else
                    return false;
            }

        }

        #region OTHERWRITES

        /// <summary>
        /// Equivalent of Console.Write in the Panel
        /// </summary>
        /// <param name="_string"> String to write to console </param>
        public void Write(char _char)
        {
            this.Write(_char.ToString());
        }

        public void Write(int _int)
        {
            this.Write(_int.ToString());
        }

       

        /// <summary>
        /// Equivalent of Console.WriteLine but with outside console in mind
        /// </summary>
        /// <param name="_string"> String to write to console with endline </param>
        public void WriteLine(string _string)
        {
            Write(_string);
            this.WriteLine();
        }

        /// <summary>
        /// Equivalent of Console.WriteLine but with outside console in mind
        /// </summary>
        /// <param name="_char"> Char to write to console with endline </param>
        public void WriteLine(char _char)
        {
            Write(_char.ToString());
            this.WriteLine();
        }

        public void WriteLine(int _int)
        {
            Write(_int.ToString());
            this.WriteLine();
        }


        /// <summary>
        /// Writes a char without any check, could crash the program if not used properly but much faster
        /// </summary>
        /// <param name="_char"> char to write </param>
        public void WriteFast(char _char)
        {
            this.GoToCursorPosition();
            Console.Write(_char);
            this.UpdateCursorPosition();
        }

        #endregion

        public void Clear()
        {
            for (int i = 0; i < this.PanHeight; i++)
            {
                this.WriteLine(new string(' ', this.PanWidth));
            }
        }

        /// <summary>
        ///  Equivalent of Console.ReadLine but with outside console in mind
        /// </summary>
        /// <returns> Returns the equivalent string of Console.ReadLine </returns>
        public string ReadLine()
        {
            // Don't accept the string if it's empty
            string _string = String.Empty;

            do
            {
                this.SetCursorPosition(CursorX, CursorY);
                _string = Console.ReadLine();

            } while (string.IsNullOrWhiteSpace(_string));

            return _string;
        }

        /// <summary>
        /// Equivalent of Console.ReadKey with the panel in mind
        /// </summary>
        /// <param name="hideKey"> Hide the </param>
        public ConsoleKeyInfo ReadKey(bool hideKey)
        {
            this.SetCursorPosition(CursorX, CursorY);
            ConsoleKeyInfo key = Console.ReadKey(hideKey);
            this.UpdateCursorPosition();

            return key;
        }

        /// <summary>
        /// Sposta il cursore dalla posizione corrente
        /// </summary>
        /// <param name="_x"> Cambio di posizione orizzontale </param>
        /// <param name="_y"> Cambio di posizione verticale </param>
        public void MoveCursorPosition(int _x, int _y)
        {
            this.SetCursorPosition(CursorX + _x, CursorY + _y);
        }

        public string DeleteChar(string _string, char deleteChar)
        {
            _string = _string.Remove(_string.Length - 1);
            this.MoveCursorPosition(-1, 0);
            Console.Write(deleteChar);
            this.MoveCursorPosition(-1, 0);

            return _string;
        }

        /// <summary>
        /// A more advanced Console.ReadLine with panels in mind
        /// </summary>
        /// <param name="max"> Maximum number of characters allowed </param>
        /// <param name="min"> Minimum number of characters allowed </param>
        /// <param name="pattern"> Characters accepted </param>
        /// <param name="deleteChar"> When pressing delete, replace with this character </param>
        /// <param name="toUpper"> Transfrom the characters to upper during the typing </param>
        /// <returns> Input of the user </returns>
        public string ConsoleLimitReadLine(int max, int min, string pattern, char deleteChar, bool toUpper)
        {
            string output = String.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = this.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (!String.IsNullOrEmpty(output))
                    {
                        output = this.DeleteChar(output, deleteChar);
                    }
                }
                else
                {
                    if (output.Length < max)
                    {
                        char _char = keyInfo.KeyChar;

                        if (pattern.Contains(_char.ToString()))
                        {

                            if (keyInfo.KeyChar != '\'')
                            {
                                if (toUpper)
                                    _char = Char.ToUpper(_char);

                                output += _char;
                                this.Write(_char);
                            }
                            else
                            {
                                // Controlla '
                                if (output.Length > 0)   //Deve esserci un carattere prima
                                {
                                    string accentiChars = "èòàìù";
                                    string normaliChars = "EOAIU";
                                    char lastChar = output[output.Length - 1];  // Carattere prima dell'apostrofo

                                    // Se il carattere prima non è già accentato
                                    if (!accentiChars.Contains(lastChar.ToString()))
                                    {
                                        var charIndex = normaliChars.IndexOf(lastChar);
                                        if (charIndex != -1)
                                        {
                                            output = DeleteChar(output, deleteChar);

                                            char accChar = Char.ToUpper(accentiChars[charIndex]);
                                            output += accChar;
                                            this.Write(accChar);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

            } while (keyInfo.Key != ConsoleKey.Enter || output.Length < min);

            return output;
        }






    }
}
