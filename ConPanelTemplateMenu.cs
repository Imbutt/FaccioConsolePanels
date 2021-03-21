using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FaccioConsolePanelsLibrary
{
    public class ConPanelTemplateMenu : ConPanel
    {
        // Spaces to write before and after every option
        public int SpaceBefore { get; private set; } = 2;
        public int SpaceAfter { get; private set; } = 1;

        public int ChosenOptionNum { get; set; } = 0;
        public string[] Options { get; private set; }
        public char PointChar { get; set; } = '>';

        /// <summary>
        /// Construct the ConPanel with automatic width and height based on the options size
        /// </summary>
        /// <param name="panX"> Position of the Panel </param>
        /// <param name="panY"> Position of the Panel </param>
        /// <param name="box"> Draw the outside box </param>
        /// <param name="options"> Array of strings of every option written </param>
        public ConPanelTemplateMenu(int panX, int panY, bool box,
            string[] options) 
            : base(panX, panY, ConPanelUtils.GetAutoWidth(options) + 3,
                  options.Length, box, false,false)
        {
            StartPanel(options);
        }


        /// <summary>
        /// Construct the ConPanel with automatic width and height based on the options size
        /// with determined space before and after every option
        /// </summary>
        /// <param name="panX"> Position of the Panel </param>
        /// <param name="panY"> Position of the Panel </param>
        /// <param name="box"> Draw the outside box </param>
        /// <param name="options"> Array of strings of every option written </param>
        /// <param name="spaceBefore"> Spaces before every option written </param>
        /// <param name="spaceAfter"> Spaces before every option written </param>
        public ConPanelTemplateMenu(int panX, int panY, bool box,
    string[] options, int spaceBefore, int spaceAfter)
    : base(panX, panY, ConPanelUtils.GetAutoWidth(options) + spaceAfter + spaceAfter,
          options.Length, box, false, false)
        {
            this.SpaceBefore = spaceBefore;
            this.SpaceAfter = SpaceAfter;
            StartPanel(options);
        }

        /// <summary>
        /// Construct a ConPanel as the ConPanelMenu Template
        /// </summary>
        /// <param name="panX"> Position of the Panel </param>
        /// <param name="panY"> Position of the Panel </param>
        /// <param name="panWidth"> Width of the panel </param>
        /// <param name="panHeight"> Heigh of the panel </param>
        /// <param name="box"> Draw the outside box </param>
        /// <param name="options"> Array of strings of every option written </param>
        public ConPanelTemplateMenu(int panX, int panY,int panWidth,int panHeight, bool box,string[] options)
    : base(panX, panY, panWidth, panHeight, box, false, false)
        {
            StartPanel(options);
        }


        private void StartPanel(string[] options)
        {
            this.Options = options;
            this.PrintOptions();
            this.SetCursorPosition(0, this.ChosenOptionNum);
            this.WriteFast(this.PointChar);
        }

        private void PrintOptions()
        {
            for (int i = 0; i < this.Options.Length; i++)
            {
                this.WriteFast(new String(' ', this.SpaceBefore));  // Write Space Before
                this.WriteFast($"{this.Options[i]}");             // Write Option
                this.WriteFast(new String(' ', this.SpaceAfter));   // Write Space After

                this.WriteLine();
            }
        }

        public void MoveOption(int plusMove)
        {
            if(this.ChosenOptionNum + plusMove < Options.Length && 
                this.ChosenOptionNum + plusMove >= 0)
            {
                // Delete previous char
                this.SetCursorPosition(0, this.ChosenOptionNum);
                this.WriteFast(' ');
                
                this.ChosenOptionNum += plusMove;   // Set new position

                // Write next char
                this.SetCursorPosition(0, this.ChosenOptionNum);
                this.WriteFast(this.PointChar);
            }
        }

        public int ReadKeyChooseOnce()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    this.MoveOption(-1);
                    break;
                case ConsoleKey.DownArrow:
                    this.MoveOption(+1);
                    break;
                case ConsoleKey.Enter:
                    return this.ChosenOptionNum;
                    break;
            }

            return -1;
        }

        public int ReadKeyChooseForce()
        {
            int chosen = -1;
            do
            {
                chosen = this.ReadKeyChooseOnce();
            } while (chosen == -1);

            return chosen;
        }

    }
}