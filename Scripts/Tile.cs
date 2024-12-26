using Godot;
using System;

public partial class Tile : Control
{
    [Signal]
    public delegate void TileEnteredEventHandler(Tile tile, bool entered);

    [Signal]
    public delegate void TileExitedEventHandler(Tile tile, bool entered);
	[Export]
	public Vector2 tileCoords {get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnTileEntered() {
		EmitSignal("TileEntered", this, true);
	}

	private void OnTileExited() {
		EmitSignal("TileExited", this, false);
	}

	public void AddGlyph(Glyph glyph) {
		CenterContainer center = GetNode<CenterContainer>("CenterContainer");
		center.AddChild(glyph);
	}
}
