using System;
using System.IO;

class FileIO {

    private string filePath = "Game\\stats.txt";
    public void writeToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            // Append the text to the file
            writer.WriteLine(Game.enemiesKilled);
        }
    }
}
