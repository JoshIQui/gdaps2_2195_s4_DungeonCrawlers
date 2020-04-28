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
    public partial class LevelCreator : Form
    {
        // --------------------
        // Fields
        // --------------------

        List<Button> buttons;
        Button saveButton;
        Button loadButton;
        Color clickColor;
        int rows;
        int columns;

        public LevelCreator()
        {
            InitializeComponent();

            //Sets the default click color to empty
            clickColor = Color.Transparent;

            //Initializes the list of buttons
            buttons = new List<Button>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Sets the amount of rows and columns
            rows = 15;
            columns = 57;

            //Loads a wall of buttons to interact with
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    //Creates a new button and gives it a location and size
                    Button b = new Button();
                    b.Location = new Point(25 * (column + 1), (25 * (row + 1)) + 25);
                    b.Width = 25;
                    b.Height = 25;

                    //Gives the button a name based on its coordinates
                    string buttonName = String.Format("{0},{1}", column, row);
                    b.Name = buttonName;

                    //Adds the button to the list
                    buttons.Add(b);

                    //Makes them appear
                    base.Controls.Add(b);

                    //Subscribes to the event
                    b.Click += ButtonClicked;

                    //Sets up the save and load buttons
                    if (row == (rows - 1))
                    {
                        //Creates a button for saving
                        saveButton = new Button();
                        saveButton.Location = new Point(this.Width - (this.Width / 4), b.Location.Y + 25);
                        saveButton.Width = 180;
                        saveButton.Height = 60;
                        saveButton.Text = "Save and Quit";

                        //Names the save button
                        saveButton.Name = "SaveButton";

                        //Adds the button to the controls
                        base.Controls.Add(saveButton);

                        //Subscribes to the event
                        saveButton.Click += SaveButtonClicked;

                        //Creates a button for loading
                        loadButton = new Button();
                        loadButton.Location = new Point(this.Width / 4, b.Location.Y + 25);
                        loadButton.Width = 180;
                        loadButton.Height = 60;
                        loadButton.Text = "Load a File";

                        //Names the load button
                        loadButton.Name = "LoadButton";

                        //Adds the button to the controls
                        base.Controls.Add(loadButton);

                        //Subscribes to the event
                        loadButton.Click += LoadButtonClicked;
                    }
                }
            }

            //Creates a toolbar of buttons that allow the user to change the color of the wall of buttons
            for (int column = 0; column < 15; column++)
            {
                //Creates a new button and gives it a location and size
                Button b = new Button();
                b.Location = new Point(70 * (column + 1), 20);
                b.Width = 60;
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
                label.Location = new Point(70 * (column + 1), 5);
                label.Width = 60;
                label.Height = 20;

                //Gives the label a name
                string labelName = String.Format("Button {0}", column + 1);
                label.Name = labelName;

                //Creates all the templates
                switch (column)
                {
                    case 0:
                        label.Text = "Floor";
                        b.BackColor = Color.Red;
                        break;
                    case 1:
                        label.Text = "Half Block";
                        b.BackColor = Color.Blue;
                        break;
                    case 2:
                        label.Text = "Divider";
                        b.BackColor = Color.Green;
                        break;
                    case 3:
                        label.Text = "Two Sided";
                        b.BackColor = Color.Yellow;
                        break;
                    case 4:
                        label.Text = "Corner";
                        b.BackColor = Color.Orange;
                        break;
                    case 5:
                        label.Text = "Empty Space";
                        b.BackColor = Color.Purple;
                        break;
                    case 6:
                        label.Text = "Stairs";
                        b.BackColor = Color.Teal;
                        break;
                    case 7:
                        label.Text = "Stair end";
                        b.BackColor = Color.Crimson;
                        break;
                    case 8:
                        label.Text = "Platform";
                        b.BackColor = Color.Brown;
                        break;
                    case 9:
                        label.Text = "PlatfrmEnd";
                        b.BackColor = Color.Salmon;
                        break;
                    case 10:
                        label.Text = "Spikes";
                        b.BackColor = Color.CadetBlue;
                        break;
                    case 11:
                        label.Text = "Enemy";
                        b.BackColor = Color.DarkViolet;
                        break;
                    case 12:
                        label.Text = "Transform";
                        b.BackColor = Color.White;
                        break;
                    case 13:
                        label.Text = "Eraser";
                        b.BackColor = Color.Transparent;
                        break;
                    case 14:
                        label.Text = "Flag";
                        b.BackColor = Color.HotPink;
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

                //Gets rid of the color if the button is clicked with the same color as the click color or if the color is empty
                if (tempButton.BackColor == clickColor || clickColor == Color.Transparent)
                {
                        tempButton.BackColor = Color.Transparent;
                        tempButton.Text = "";
                }
                else
                {
                    //IF the color is not white, do normal things
                    if (clickColor != Color.White && clickColor != Color.HotPink)
                    {
                        if (clickColor == Color.DarkViolet)
                        {
                            //Sets the color of the button to the color type
                            tempButton.BackColor = clickColor;
                            tempButton.Text = "G";
                        }
                        else
                        {
                            //Sets the color of the button to the color type
                            tempButton.BackColor = clickColor;
                            tempButton.Text = "1";
                        }
                    }
                    //Names the button as flagged if the flag template is used on it
                    else if (clickColor == Color.HotPink)
                    {
                        if (tempButton.BackColor == Color.Red || tempButton.BackColor == Color.Salmon)
                        {
                            string[] name = tempButton.Name.Split(',');

                            if (name.Length == 2)
                            {
                                tempButton.Name += ",Flagged";
                                infoLabel.Text = "Changed " + name[0] + "," + name[1] + "'s status to Flagged";
                            }
                            else
                            {
                                tempButton.Name = name[0] + "," + name[1];
                                infoLabel.Text = "Changed " + name[0] + "," + name[1] + "'s status to Unflagged";
                            }
                        }
                    }
                    //If the click color is white, rotate or flip the space based on the color
                    else
                    {
                        //Can rotate 360 degrees
                        if (tempButton.BackColor == Color.Red || tempButton.BackColor == Color.Blue || 
                            tempButton.BackColor == Color.Yellow || tempButton.BackColor == Color.Orange)
                        {
                            switch (tempButton.Text)
                            {
                                case "1":
                                    tempButton.Text = "2";
                                    break;
                                case "2":
                                    tempButton.Text = "3";
                                    break;
                                case "3":
                                    tempButton.Text = "4";
                                    break;
                                case "4":
                                    tempButton.Text = "1";
                                    break;
                            }
                        }
                        //Can rotate 90 degrees
                        else if (tempButton.BackColor == Color.Green)
                        {
                            switch (tempButton.Text)
                            {
                                case "1":
                                    tempButton.Text = "2";
                                    break;
                                case "2":
                                    tempButton.Text = "1";
                                    break;
                            }
                        }
                        //Can flip horizontally
                        else if (tempButton.BackColor == Color.Teal || tempButton.BackColor == Color.Crimson || tempButton.BackColor == Color.Brown ||
                                tempButton.BackColor == Color.Salmon)
                        {
                            switch (tempButton.Text)
                            {
                                case "1":
                                    tempButton.Text = "H";
                                    break;
                                case "H":
                                    tempButton.Text = "1";
                                    break;
                            }
                        }
                        //Can flip vertically
                        else if (tempButton.BackColor == Color.CadetBlue)
                        {
                            switch (tempButton.Text)
                            {
                                case "1":
                                    tempButton.Text = "V";
                                    break;
                                case "V":
                                    tempButton.Text = "1";
                                    break;
                            }
                        }
                        else if (tempButton.BackColor == Color.DarkViolet)
                        {
                            switch (tempButton.Text)
                            {
                                case "G":
                                    tempButton.Text = "S";
                                    break;
                                case "S":
                                    tempButton.Text = "W";
                                    break;
                                case "W":
                                    tempButton.Text = "G";
                                    break;
                            }
                        }
                    }
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

        //Saves the created level to a file
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
                    writer.WriteLine(columns + "," + rows);

                    //Determines the character from the nature of each button and writes them to the file in order
                    foreach (Button button in buttons)
                    {
                        string[] name = button.Name.Split(',');

                        //Determines the color and transformation of the box and writes to the file accordingly
                        switch (button.BackColor.Name)
                        {
                            case "Red":
                                switch (button.Text)
                                {
                                    case "1":
                                        if (name.Length == 2)
                                        {
                                            writer.Write("1");
                                        }
                                        else
                                        {
                                            writer.Write("!");
                                        }
                                        break;
                                    case "2":
                                        writer.Write("Q");
                                        break;
                                    case "3":
                                        writer.Write("A");
                                        break;
                                    case "4":
                                        writer.Write("Z");
                                        break;
                                }
                                break;
                            case "Blue":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("2");
                                        break;
                                    case "2":
                                        writer.Write("W");
                                        break;
                                    case "3":
                                        writer.Write("S");
                                        break;
                                    case "4":
                                        writer.Write("X");
                                        break;
                                }
                                break;
                            case "Green":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("3");
                                        break;
                                    case "2":
                                        writer.Write("E");
                                        break;
                                }
                                break;
                            case "Yellow":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("4");
                                        break;
                                    case "2":
                                        writer.Write("R");
                                        break;
                                    case "3":
                                        writer.Write("F");
                                        break;
                                    case "4":
                                        writer.Write("V");
                                        break;
                                }
                                break;
                            case "Orange":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("5");
                                        break;
                                    case "2":
                                        writer.Write("T");
                                        break;
                                    case "3":
                                        writer.Write("G");
                                        break;
                                    case "4":
                                        writer.Write("B");
                                        break;
                                }
                                break;
                            case "Purple":
                                writer.Write("6");
                                break;
                            case "Teal":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("7");
                                        break;
                                    case "H":
                                        writer.Write("U");
                                        break;
                                }
                                break;
                            case "Crimson":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("8");
                                        break;
                                    case "H":
                                        writer.Write("I");
                                        break;
                                }
                                break;
                            case "Brown":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("9");
                                        break;
                                    case "H":
                                        writer.Write("O");
                                        break;
                                }
                                break;
                            case "Salmon":
                                switch (button.Text)
                                {
                                    case "1":
                                        if (name.Length == 2)
                                        {
                                            writer.Write("0");
                                        }
                                        else
                                        {
                                            writer.Write(")");
                                        }
                                        break;
                                    case "H":
                                        if (name.Length == 2)
                                        {
                                            writer.Write("P");
                                        }
                                        else
                                        {
                                            writer.Write("p");
                                        }
                                        break;
                                }
                                break;
                            case "CadetBlue":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("-");
                                        break;
                                    case "V":
                                        writer.Write("[");
                                        break;
                                }
                                break;
                            case "DarkViolet":
                                switch (button.Text)
                                {
                                    case "1":
                                        writer.Write("=");
                                        break;
                                    case "2":
                                        writer.Write("+");
                                        break;
                                    case "3":
                                        writer.Write("]");
                                        break;
                                }
                                break;
                            case "Control":
                                writer.Write("~");
                                break;
                            case "Transparent":
                                writer.Write("~");
                                break;
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

            //Closes the form
            this.Close();
        }

        //Loads an existing level from a file
        private void LoadButtonClicked(object sender, EventArgs e)
        {
            //Sets the streamreader to null first
            StreamReader reader = null;

            try
            {
                infoLabel.Text = "Loading";

                //Has the user choose the file they want to load
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                string fileName = fileDialog.FileName;

                //Only works if the file exists
                if (fileName != null)
                {
                    //Opens the file in the reader
                    reader = new StreamReader(fileName);

                    //Sets up a variable to keep track of the current line
                    string line;

                    //Checks to see if the file is not empty and skips the first line
                    if ((line = reader.ReadLine()) != null)
                    {
                        //Loops through every line and fills out boxes in the wall accordingly
                        for (int row = 0; row < 15; row++)
                        {
                            //Updates the line
                            line = reader.ReadLine();

                            //Gets an array of all the characters in a line
                            char[] data = line.ToCharArray();

                            for (int column = 0; column < 57; column++)
                            {
                                int index = column + (row * 57);

                                //Resets the name of the button (in case it was flagged)
                                buttons[index].Name = String.Format("{0},{1}", column, row);

                                //Checks each character
                                switch (data[column])
                                {
                                    //Reds
                                    case '1':
                                        buttons[index].BackColor = Color.Red;
                                        buttons[index].Text = "1";
                                        break;
                                    case '!':
                                        buttons[index].BackColor = Color.Red;
                                        buttons[index].Text = "1";
                                        buttons[index].Name += ",Flagged";
                                        break;
                                    case 'Q':
                                        buttons[index].BackColor = Color.Red;
                                        buttons[index].Text = "2";
                                        break;
                                    case 'A':
                                        buttons[index].BackColor = Color.Red;
                                        buttons[index].Text = "3";
                                        break;
                                    case 'D':
                                        buttons[index].BackColor = Color.Red;
                                        buttons[index].Text = "4";
                                        break;
                                    //Blues
                                    case '2':
                                        buttons[index].BackColor = Color.Blue;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'W':
                                        buttons[index].BackColor = Color.Blue;
                                        buttons[index].Text = "2";
                                        break;
                                    case 'S':
                                        buttons[index].BackColor = Color.Blue;
                                        buttons[index].Text = "3";
                                        break;
                                    case 'X':
                                        buttons[index].BackColor = Color.Blue;
                                        buttons[index].Text = "4";
                                        break;
                                    //Greens
                                    case '3':
                                        buttons[index].BackColor = Color.Green;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'E':
                                        buttons[index].BackColor = Color.Green;
                                        buttons[index].Text = "2";
                                        break;
                                    //Yellows
                                    case '4':
                                        buttons[index].BackColor = Color.Yellow;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'R':
                                        buttons[index].BackColor = Color.Yellow;
                                        buttons[index].Text = "2";
                                        break;
                                    case 'F':
                                        buttons[index].BackColor = Color.Yellow;
                                        buttons[index].Text = "3";
                                        break;
                                    case 'V':
                                        buttons[index].BackColor = Color.Yellow;
                                        buttons[index].Text = "4";
                                        break;
                                    //Oranges
                                    case '5':
                                        buttons[index].BackColor = Color.Orange;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'T':
                                        buttons[index].BackColor = Color.Orange;
                                        buttons[index].Text = "2";
                                        break;
                                    case 'G':
                                        buttons[index].BackColor = Color.Orange;
                                        buttons[index].Text = "3";
                                        break;
                                    case 'B':
                                        buttons[index].BackColor = Color.Orange;
                                        buttons[index].Text = "4";
                                        break;
                                    //Purple
                                    case '6':
                                        buttons[index].BackColor = Color.Purple;
                                        buttons[index].Text = "1";
                                        break;
                                    //Teals
                                    case '7':
                                        buttons[index].BackColor = Color.Teal;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'U':
                                        buttons[index].BackColor = Color.Teal;
                                        buttons[index].Text = "H";
                                        break;
                                    //Crimsons
                                    case '8':
                                        buttons[index].BackColor = Color.Crimson;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'I':
                                        buttons[index].BackColor = Color.Crimson;
                                        buttons[index].Text = "H";
                                        break;
                                    //Browns
                                    case '9':
                                        buttons[index].BackColor = Color.Brown;
                                        buttons[index].Text = "1";
                                        break;
                                    case 'O':
                                        buttons[index].BackColor = Color.Brown;
                                        buttons[index].Text = "H";
                                        break;
                                    //Salmons
                                    case '0':
                                        buttons[index].BackColor = Color.Salmon;
                                        buttons[index].Text = "1";
                                        break;
                                    case ')':
                                        buttons[index].BackColor = Color.Salmon;
                                        buttons[index].Text = "1";
                                        buttons[index].Name += ",Flagged";
                                        break;
                                    case 'P':
                                        buttons[index].BackColor = Color.Salmon;
                                        buttons[index].Text = "H";
                                        break;
                                    case 'p':
                                        buttons[index].BackColor = Color.Salmon;
                                        buttons[index].Text = "H";
                                        buttons[index].Name += ",Flagged";
                                        break;
                                    //Cadet Blues
                                    case '-':
                                        buttons[index].BackColor = Color.CadetBlue;
                                        buttons[index].Text = "1";
                                        break;
                                    case '[':
                                        buttons[index].BackColor = Color.CadetBlue;
                                        buttons[index].Text = "V";
                                        break;
                                    //Dark Purples
                                    case '=':
                                        buttons[index].BackColor = Color.DarkViolet;
                                        buttons[index].Text = "G";
                                        break;
                                    case '+':
                                        buttons[index].BackColor = Color.DarkViolet;
                                        buttons[index].Text = "S";
                                        break;
                                    case ']':
                                        buttons[index].BackColor = Color.DarkViolet;
                                        buttons[index].Text = "W";
                                        break;
                                    //Empty Spaces
                                    case '~':
                                        buttons[index].BackColor = Color.Transparent;
                                        buttons[index].Text = "";
                                        break;
                                }
                            }
                        }
                    }
                }

                //Closes the reader and prints a success message
                reader.Close();
                infoLabel.Text = "Load Successful";
            }
            catch (Exception exception)
            {
                infoLabel.Text = "Load Failed";
                Console.WriteLine(exception.Message);
            }
        }
    }
}
