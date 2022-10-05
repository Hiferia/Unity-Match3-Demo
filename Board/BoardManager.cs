

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; //Here I create a singleton, cause I need other scripts to have access to it
    public List<Sprite> Blocks = new List<Sprite>();
    public GameObject Tile;
    public int SizeX, SizeY;
    public int EarningPoints = 20;

    public float ShiftingDelay = 0.3f;

    private GameObject[,] tiles;

    public bool IsShifting { get; set; }

    void Start()
    {
        Instance = GetComponent<BoardManager>();

        Vector2 offset = Tile.GetComponent<SpriteRenderer>().bounds.size;
        GenerateBoard(offset.x, offset.y);
    }

    private void GenerateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[SizeX, SizeY];

        float startingX = transform.position.x;
        float startingY = transform.position.y;

        Sprite[] previousOnLeft = new Sprite[SizeY];
        Sprite previousBelow = null;

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                //Instantiate a new tile
                GameObject newTile = Instantiate(Tile, new Vector3(startingX + (xOffset * x), startingY + (yOffset * y), 0), Tile.transform.rotation);

                newTile.transform.parent = transform;

                //Assign a random sprite to the tile
                //Checking if the sprite we are using is the same with the one on the left and the one below
                //Doing this prevents to have rows with the same sprite at the beginning of the game
                List<Sprite> availableBlocks = new List<Sprite>();
                availableBlocks.AddRange(Blocks);
                availableBlocks.Remove(previousOnLeft[y]);
                availableBlocks.Remove(previousBelow);
                Sprite randomSprite = availableBlocks[Random.Range(0, availableBlocks.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = randomSprite;

                //Setting the previous below and onLeft as the current sprite
                previousBelow = randomSprite;
                previousOnLeft[y] = randomSprite;

                //Position of the tile
                tiles[x, y] = newTile;
            }
        }
    }

    public IEnumerator LookForNullTiles()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                tiles[x, y].GetComponent<Tile>().DeleteAllMatches();
            }
        }
    }
    private IEnumerator ShiftTilesDown(int inX, int inY)
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullTilesCount = 0;

        for (int y = inY; y < SizeY; y++)
        {
            SpriteRenderer render = tiles[inX, y].GetComponent<SpriteRenderer>();
            if (!render.sprite)
            {
                nullTilesCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullTilesCount; i++)
        {
            yield return new WaitForSeconds(ShiftingDelay);
            GUIManager.Instance.Score += EarningPoints;
            for (int j = 0; j < renders.Count - 1; j++)
            {
                renders[j].sprite = renders[j + 1].sprite;
                renders[j + 1].sprite = GetNewSprite(inX, SizeY - 1);
            }
        }
        IsShifting = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        // This method return a random sprite from a list of available sprites
        // We remove from the list all possible duplicates that could cause an accidental match

        List<Sprite> availableSprites = new List<Sprite>();
        availableSprites.AddRange(Blocks);

        if (x > 0)
        {
            availableSprites.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if(x < SizeX - 1)
        {
            availableSprites.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            availableSprites.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return availableSprites[Random.Range(0, availableSprites.Count)];
    }
}
