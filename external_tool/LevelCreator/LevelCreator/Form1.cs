using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelCreator
{
    public partial class Form1 : Form
    {
        // --------------------
        // Fields
        // --------------------

        List<Button> buttons;

        public Form1()
        {
            InitializeComponent();

            //Creates a list of buttons
            buttons = new List<Button>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Loads a wall of buttons to interact with
            for (int row = 0; row < 30; row++)
            {
                for (int column = 0; column < 57; column++)
                {
                    //Creates a new button and gives it a location and size
                    Button b = new Button();
                    b.Location = new Point(20 * (column + 1), (20 * (row + 1)) + 30);
                    b.Width = 20;
                    b.Height = 20;

                    //Gives the button a name based on its coordinates
                    string buttonName = String.Format("{0}, {1}", column, row);
                    b.Name = buttonName;

                    //Adds the button to the list of buttons
                    buttons.Add(b);

                    //Makes them appear
                    base.Controls.Add(b);

                    foreach (Button button in buttons)
                    {
                        b.Click += ButtonClicked;
                    }
                }
            }
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //Gets the button clicked (CODE USED FROM "Hand-coded & Multiple Forms" PE Writeup)
            if (sender is Button)
            {
                Button tempButton = (Button)sender;

                tempButton.BackColor = Color.Red;
            }
        }
    }
}
