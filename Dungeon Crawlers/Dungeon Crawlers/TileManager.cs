using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dungeon_Crawlers
{
    class TileManager
    {
        // Fields
        private const int numTilesWidth = 25;
        private const int numTilesHeight = 14;

        // Constructor
        public TileManager()
        {

        }

        // Methods
        private void LoadLevel()
        {
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("test.txt");

                for (int i = 0; i < numTilesHeight; i++)
                {
                    for (int j = 0; j < numTilesWidth; j++)
                    {
                        string row = reader.ReadLine();
                        string[] tiles = row.Split(',');

                    }
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
