using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

/*
 * KEY
 * # = block
 * m = moving block
 * C = checkpoint
 * p = pit
 * l = ladder
 * S = levelSeperator
 * s = slide
 * * = flower
 */

internal class LevelLoader
{

    public static void LoadLevel(string filePath)
    {
        List<Blocks> blocks = new List<Blocks>();
        Vector2 playerPosition;
        List<Checkpoint> checkpoints = new List<Checkpoint>();
        List<Pits> pits = new List<Pits>();
        List<Slides> slide = new List<Slides>();
        List<Ladder> ladders = new List<Ladder>();
        List<Flower> flowers = new List<Flower>();
        List<LevelSeperator> levelSep = new List<LevelSeperator>();
        string[] lines = File.ReadAllLines(filePath);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {


                if (lines[y][x] == '#') // block
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, y * Blocks.size.Y);
                    blocks.Add(new Blocks(position, GameColor.Block1, new Vector2(0, 0)));
                    if (y == lines.Length - 1) // floor
                    {
                        Game.player.floorY = y * Blocks.size.Y;
                    }
                }
                else if (lines[y][x] == 'p') //pits
                {
                    Vector2 position = new Vector2(x * Pits.size.X, y * Pits.size.Y);
                    pits.Add(new Pits(position));
                }
                else if (lines[y][x] == 'm') // moving block
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, y * Blocks.size.Y);
                    blocks.Add(new Blocks(position, GameColor.Block1, new Vector2(2f, 0)));
                }
                else if (lines[y][x] == 's') //slide
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, y * Blocks.size.Y);
                    Blocks slideBlock = new Blocks(position, GameColor.White, new Vector2(0, 0));
                    slideBlock.slide = true;
                    blocks.Add(slideBlock);
                    slide.Add(new Slides(position, GameColor.White, new Vector2(0, 0)));
                }
                else if (lines[y][x] == 'Q') //player
                {
                    //LevelLoader.playerPosition = new Vector2(x * Game.player.size.X, Math.Abs(640 - y * Game.player.size.Y));

                }
                else if (lines[y][x] == 'C') //checkpoint
                {
                    Vector2 position = new Vector2(x * 50, y * 50);
                    checkpoints.Add(new Checkpoint(position, new Vector2(50, 50)));
                }
                else if (lines[y][x] == 'l') // ladder
                {
                    Vector2 position = new Vector2(x * Ladder.size.X, y * Ladder.size.Y);
                    ladders.Add(new Ladder(position));
                }
                else if (lines[y][x] == '*') // flower
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, y * Blocks.size.Y);
                    flowers.Add(new Flower(position));
                }
                else if (lines[y][x] == 'S') //slide
                {
                    Vector2 position = new Vector2(x * Blocks.size.X, y * Blocks.size.Y);
                    levelSep.Add(new LevelSeperator(position, Blocks.size));
                }
                else continue;
            }
        }
        //loadEntities();
        //DIFF SIXES OF SLIDE LADDER??




    }
}

