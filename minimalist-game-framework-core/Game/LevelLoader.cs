﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

/*
 * KEY
 * # = block
 * C = checkpoint
 * p = pit
 * l = ladder
 * S = levelSeperator
 * s = slide
 * * = flower
 */


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
                    blocks.Add(new Blocks(position, new Vector2(blockSize, blockSize), GameColor.Block1, new Vector2(0, 0)));
                }
                else if (lines[y][x] == 'p')
                {
                    Vector2 position = new Vector2(x * blockSize, y * blockSize);
                } else if (lines[y][x] == 'm')
                {
                    Vector2 position = new Vector2(x * blockSize, y * blockSize);
                    blocks.Add(new Blocks(position, new Vector2(blockSize, blockSize), GameColor.Block1, new Vector2(2f, 0)));
                } else if (lines[y][x] == 's')
                {
                    Vector2 position = new Vector2(x * blockSize, y * blockSize);
                    Blocks slideBlock = new Blocks(position, new Vector2(blockSize, blockSize), GameColor.White, new Vector2(0, 0));
                    slideBlock.slide = true;
                    blocks.Add(slideBlock);
                }
                else continue;
            }
        }

        return blocks;
    }

    public static Vector2 loadPlayerPosition(string filePath, Vector2 size)
    {
        List<Ladder> ladders = new List<Ladder>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'P')
                {
                    return new Vector2(x * size.X, y * size.Y);
                    
                }
            }
        }

        return new Vector2 (0,0);
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

    public static List<Pits> loadPits(string filePath, Vector2 pitsSize)
    {
        List<Pits> pits = new List<Pits>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'p')
                {
                    Vector2 position = new Vector2(x * pitsSize.X, y * pitsSize.Y);
                    pits.Add(new Pits(position, new Vector2(pitsSize.X, pitsSize.Y)));
                }
            }
        }

        return pits;
    }

    public static List<Ladder> loadLadder(string filePath, int ladderSize)
    {
        List<Ladder> ladders = new List<Ladder>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'l')
                {
                    Vector2 position = new Vector2(x * ladderSize, y * ladderSize);
                    ladders.Add(new Ladder(position, new Vector2(ladderSize, ladderSize)));
                }
            }
        }

        return ladders;
    }

    // In the LevelLoader class, add this new method
    public static List<Flower> LoadFlowers(string filePath, int blockSize)
    {
        List<Flower> flowers = new List<Flower>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '*')
                {
                    Vector2 position = new Vector2(x * blockSize, y * blockSize);
                    flowers.Add(new Flower(position));
                }
            }
        }

        return flowers;
    }

    public static List<LevelSeperator> loadLevelSeperator(string filePath, int size)
    {
        List<LevelSeperator> levelSep = new List<LevelSeperator>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 'S')
                {
                    Vector2 position = new Vector2(x * size, y * size);
                    levelSep.Add(new LevelSeperator(position, new Vector2(size, size)));
                }
            }
        }

        return levelSep;
    }

    public static List<Slides> LoadSlides(string filePath, Vector2 blockSize)
    {
        List<Slides> slide = new List<Slides>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == 's')
                {
                    Vector2 position = new Vector2(x * blockSize.X, y * blockSize.Y);
                    slide.Add(new Slides(position, blockSize, GameColor.White, new Vector2(0, 0)));
                }
            }
        }

        return slide;
    }
}

