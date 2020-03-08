using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LevelCreator
{
    public partial class Form1 : Form
    {
        // --------------------
        // Fields
        // --------------------

        List<Button> buttons;
        Button saveButton;
        Color clickColor;
        int rows;
        int columns;

        public Form1()
        {
            InitializeComponent();

            //Sets the default click color to empty
            clickColor = Color.Empty;

            //Initializes the list of buttons
            buttons = new List<Button>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Sets the amount of rows and columns
            rows = 27;
            columns = 57;

            //Loads a wall of buttons to interact with
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    //Creates a new button and gives it a location and size
                    Button b = new Button();
                    b.Location = new Point(20 * (column + 1), (20 * (row + 1)) + 30);
                    b.Width = 20;
                    b.Height = 20;

                    //Gives the button a name based on its coordinates
                    string buttonName = String.Format("{0},{1}", column, row);
                    b.Name = buttonName;

                    //Adds the button to the list
                    buttons.Add(b);

                    //Makes them appear
                    base.Controls.Add(b);

                    //Subscribes to the event
                    b.Click += ButtonClicked;

                    if (row == 26)
                    {
                        //Creates a button for saving
                        saveButton = new Button();
                        saveButton.Location = new Point(525, b.Location.Y + 20);
                        saveButton.Width = 120;
                        saveButton.Height = 30;
                        saveButton.Text = "Save and Quit";

                        //Names the save button
                        saveButton.Name = "SaveButton";

                        //Adds the button to the controls
                        base.Controls.Add(saveButton);

                        //Subscribes to the event
                        saveButton.Click += SaveButtonClicked;
                    }
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
                        b.BackColor = Color.Red;
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

        private void SaveButtonClicked(object sender, EventArgs e)
        {
            //Sets the streamwriter to null first
            StreamWriter writer = null;

            try
            {
                //Has the user enter the filename for the file to save
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.ShowDialog();
                string fileName = fileDialog.FileName;

                //Only works if there is a filename
                if (fileName != null)
                {
                    //Opens the writer
                    writer = new StreamWriter(fileName);

                    //Writes the dimensions in the file
                    writer.WriteLine(rows + "," + columns);

                    foreach (Button button in buttons)
                    {
                        if (button.BackColor == Color.Red)
                        {
                            writer.Write("R");
                        }
                        else if (button.BackColor == Color.Blue)
                        {
                            writer.Write("B");
                        }
                        else if (button.BackColor == Color.Green)
                        {
                            writer.Write("G");
                        }
                        else if (button.BackColor == Color.Yellow)
                        {
                            writer.Write("Y");
                        }
                        else if (button.BackColor == Color.Orange)
                        {
                            writer.Write("Q");
                        }
                        else if (button.BackColor == Color.Purple)
                        {
                            writer.Write("P");
                        }
                        else if (button.BackColor == Color.Teal)
                        {
                            writer.Write("T");
                        }
                        else if (button.BackColor == Color.Black)
                        {
                            writer.Write("L");
                        }
                        else if (button.BackColor == Color.Brown)
                        {
                            writer.Write("K");
                        }
                        else
                        {
                            writer.Write("O");
                        }

                        //Splits the name into the two coordinates
                        string[] coordinates = button.Name.Split(',');

                        //Skips a line for the last column in the row
                        if (int.Parse(coordinates[0]) == columns - 1)
                        {
                            writer.WriteLine();
                        }
                    }
                }
            }
            //Prints an error message in console if an error is reached
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            //Closes the writer when done
            writer.Close();
        }
    }
}
