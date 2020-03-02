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

        Color clickColor;

        public Form1()
        {
            InitializeComponent();

            //Sets the default click color to empty
            clickColor = Color.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Loads a wall of buttons to interact with
            for (int row = 0; row < 28; row++)
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

                    //Makes them appear
                    base.Controls.Add(b);

                    //Subscribes to the event
                    b.Click += ButtonClicked;
                }
            }

            //Creates a toolbar of buttons that allow the user to change the color of the wall of buttons
            for (int column = 0; column < 10; column++)
            {
                //Creates a new button and gives it a location and size
                Button b = new Button();
                b.Location = new Point(100 * (column + 1), 20);
                b.Width = 80;
                b.Height = 20;

                //Gives the button a name
                string buttonName = String.Format("Hotbar {0}", column + 1);
                b.Name = buttonName;

                //Makes the button appear
                base.Controls.Add(b);

                //Subscribes to the event
                b.Click += TemplateClicked;

                //Creates a label to go with the button
                Label label = new Label();

                //Location and size of the label
                label.Location = new Point(100 * (column + 1), 5);
                label.Width = 80;
                label.Height = 20;

                //Gives the label a name
                string labelName = String.Format("Button {0} label", column + 1);
                label.Name = labelName;

                switch (column)
                {
                    case 0:
                        label.Text = labelName;
                        b.BackgroundImage = Image.FromFile("Properties/exit_ovr");
                        break;
                    case 1:
                        label.Text = labelName;
                        b.BackColor = Color.Blue;
                        break;
                    case 2:
                        label.Text = labelName;
                        b.BackColor = Color.Green;
                        break;
                    case 3:
                        label.Text = labelName;
                        b.BackColor = Color.Yellow;
                        break;
                    case 4:
                        label.Text = labelName;
                        b.BackColor = Color.Orange;
                        break;
                    case 5:
                        label.Text = labelName;
                        b.BackColor = Color.Purple;
                        break;
                    case 6:
                        label.Text = labelName;
                        b.BackColor = Color.Teal;
                        break;
                    case 7:
                        label.Text = labelName;
                        b.BackColor = Color.Black;
                        break;
                    case 8:
                        label.Text = labelName;
                        b.BackColor = Color.Brown;
                        break;
                    case 9:
                        label.Text = labelName;
                        b.BackColor = Color.Empty;
                        break;
                }

                //Makes the label appear
                base.Controls.Add(label);
            }
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //Gets the button clicked (CODE USED FROM "Hand-coded & Multiple Forms" PE Writeup)
            if (sender is Button)
            {
                Button tempButton = (Button)sender;

                //Gets rid of the color if the button is clicked with the same color as the click color
                if (tempButton.BackColor == clickColor)
                {
                    tempButton.BackColor = Color.Empty;
                }
                else
                {
                    //Sets the color of the button to the color type
                    tempButton.BackColor = clickColor;
                }
            }
        }

        private void TemplateClicked(object sender, EventArgs e)
        {
            //Gets the button clicked (CODE USED FROM "Hand-coded & Multiple Forms" PE Writeup)
            if (sender is Button)
            {
                Button tempButton = (Button)sender;

                //Sets the color type to the type described in the button
                clickColor = tempButton.BackColor;
            }
        }
    }
}
