using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{
	private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	private static Tile previousSelected = null;

	private SpriteRenderer render;
	private bool isSelected = false;
	private bool isMatchFound = false;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	void Awake() 
	{
		render = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if(!render.sprite || BoardManager.Instance.IsShifting)
        {
			return;
        }

		if(isSelected)
        {
			Deselect();
        }
        else
        {
			if(!previousSelected)
            {
				Select();
            }
            else
            {
				if(GetAllAdjacentTiles().Contains(previousSelected.gameObject))
                {
					SwapTile(previousSelected.render);
					previousSelected.DeleteAllMatches();
					previousSelected.Deselect();
					DeleteAllMatches();
				}
                else
                {
					previousSelected.GetComponent<Tile>().Deselect();
					Select();
                }
            }
        }
    }

	public void SwapTile(SpriteRenderer inRender)
    {
		if(render.sprite == inRender.sprite)
        {
			return;
        }

		// Simply swapping the sprites using a temp sprite
		Sprite tempSprite = inRender.sprite;
		inRender.sprite = render.sprite;
		render.sprite = tempSprite;
		SFXManager.Instance.PlaySFX(Clip.Swap);
    }

	private List<GameObject> GetAllAdjacentTiles()
    {
		//This method will return a gameobject list of all adjacent tile

		List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
			adjacentTiles.Add(GetAdjacentTile(adjacentDirections[i]));
        }
		return adjacentTiles;
    }

	private GameObject GetAdjacentTile(Vector2 direction)
    {
		//This method will return the tile given the direction
		//using a raycast from the transform position to the direction

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
		if(hit.collider)
        {
			return hit.collider.gameObject;
        }
		return null;
    }
	private void DeleteMatch(Vector2[] directions)
    {
		//This method finds all the matching tiles given the directions, after that it resets the match

		List<GameObject> matchingTiles = new List<GameObject>();
        for (int i = 0; i < directions.Length; i++)
        {
			matchingTiles.AddRange(Match(directions[i]));
        }

		//We need to check only 2 or more, cause the third one is the initial tile
		if(matchingTiles.Count >= 2)
        {
            for (int i = 0; i < matchingTiles.Count; i++)
            {
				//We remove the sprites setting them to null, we are resetting
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            }
			isMatchFound = true;
        }
    }
	public void DeleteAllMatches()
    {
		//This method will delete all the tiles matched
		//If we find a match with horizonal directions or vertical directions it will set the current sprite to null

		if(!render.sprite)
        {
			return;
        }

		Vector2[] verticalDirections = new Vector2[2] { Vector2.up, Vector2.down };
		Vector2[] horizontalDirections = new Vector2[2] { Vector2.left, Vector2.right };
		DeleteMatch(verticalDirections);
		DeleteMatch(horizontalDirections);

		if(isMatchFound)
        {
			render.sprite = null;
			isMatchFound = false;
			StopCoroutine(BoardManager.Instance.LookForNullTiles());
			StartCoroutine(BoardManager.Instance.LookForNullTiles());
			SFXManager.Instance.PlaySFX(Clip.Clear);
			GUIManager.Instance.MovesCounter--;
        }
    }
	private List<GameObject> Match(Vector2 direction)
    {
		//This method will return a list of matching tiles
		//Using a direction, we throw a raycast from the transform position into the given direction
		//If the ray collides into a tile with the same sprite, it continues to cast a ray in the same direction
		//If it collides with nothing or with a tile with different sprites, it stops.

		List<GameObject> matchingTiles = new List<GameObject>();
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
		while(hit.collider && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
        {
			matchingTiles.Add(hit.collider.gameObject);
			hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }
		return matchingTiles;
    }

    private void Select() 
	{
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();
		SFXManager.Instance.PlaySFX(Clip.Select);
	}

	private void Deselect() 
	{
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}

}