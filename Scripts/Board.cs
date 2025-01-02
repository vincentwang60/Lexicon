using Godot;
using System;
using System.Collections.Generic;

public partial class Board : Control {
	private Glyph[,] boardArr {get; set;}

	[Export]
	public PackedScene TileScene {get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		boardArr = new Glyph[Constants.gridDimension, Constants.gridDimension];
		GridContainer grid = GetNode<GridContainer>("Grid");
		for (int x = 0; x < Constants.gridDimension; x++) {
			for (int y = 0; y < Constants.gridDimension; y++) {
				Tile newTile = TileScene.Instantiate<Tile>();
				newTile.TileEntered += (GetParent() as Combat).UpdateHoveringTile;
				newTile.TileExited += (GetParent() as Combat).UpdateHoveringTile;
				newTile.tileCoords = new int[] {x,y};
				grid.AddChild(newTile);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetTile(int[] tileCoords, Glyph newTileContent) {
		boardArr[tileCoords[0],tileCoords[1]] = newTileContent;
	}

	public List<WordData> ProcessBoard(List<WordData> words) {
		List<WordData> wordsPlayed = new List<WordData>();
		// Check rows
		for (int y = 0; y < Constants.gridDimension; y++ ) {
			for (int x = 0; x < Constants.gridDimension; x++ ) {
				GD.Print(x, y, boardArr[y,x]);
			}
		}
		// Check cols
		for (int x = 0; x < Constants.gridDimension; x++ ) {
			for (int y = 0; y < Constants.gridDimension; y++ ) {
				GD.Print(x, y, boardArr[y,x]);
			}
		}
		return wordsPlayed;
	}
}
