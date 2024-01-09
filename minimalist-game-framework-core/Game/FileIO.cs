using System;
using System.Collections.Generic;
using System.IO;

class FileIO {

    private string filePath = @"..\..\..\Assets\stats.txt";
    public void writeToFile()
    {
        string[] lines = File.ReadAllLines(filePath);
        int i = 1;
        foreach (string line in lines)
        {
            i++;
        }
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            // Append the text to the file
            
            writer.WriteLine(i + ")           "+ Game.enemiesKilled + " Enemies Killed      |     ");
        }
    }

    public string[] readFromFile()
    {
        return File.ReadAllLines(filePath);
    }
}
