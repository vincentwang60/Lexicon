using Godot;
using System;

public partial class Board : Control
{
	[Export]
	public PackedScene TileScene {get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GridContainer grid = GetNode<GridContainer>("Grid");
		for (int x = 0; x < Constants.gridDimension; x++) {
			for (int y = 0; y < Constants.gridDimension; y++) {
				Tile newTile = TileScene.Instantiate<Tile>();
				newTile.TileEntered += (GetParent() as Combat).UpdateHoveringTile;
				newTile.TileExited += (GetParent() as Combat).UpdateHoveringTile;
				newTile.tileCoords = new Vector2(x,y);
				grid.AddChild(newTile);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
