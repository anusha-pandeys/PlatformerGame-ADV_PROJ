using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


internal class LevelLoader
{
    public static List<Blocks> LoadLevel(string filePath, int blockSize)
    {
        List<Blocks> blocks = new List<Blocks>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                {
                    Vector2 position = new Vector2(x * blockSize, y * blockSize);
                    blocks.Add(new Blocks(position, new Vector2(blockSize, blockSize), GameColor.Block1));
                }           
                else continue;
            }
        }

        return blocks;
    }

    public static List<Checkpoint> LoadCheckpoints(string filePath, int checkpointSize)
    {
        List<Checkpoint> checkpoints = new List<Checkpoint>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'C')
                {
                    Vector2 position = new Vector2(x * checkpointSize, y * checkpointSize);
                    checkpoints.Add(new Checkpoint(position, new Vector2(checkpointSize, checkpointSize)));
                }
            }
        }

        return checkpoints;
    }

}

