using Godot;
using System;
using System.Collections.Generic;

public partial class Hand : Control
{

	[Export]
	public PackedScene GlyphScene {get; set;}
	public GridContainer grid {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		grid = GetNode<GridContainer>("HandGrid");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void AddGlyph(GlyphType glyphType) {
		Glyph glyph = GlyphScene.Instantiate<Glyph>();
		glyph.glyphType = glyphType;
		glyph.GlyphClicked += (GetParent() as Combat).HandleGlyphClicked;
		glyph.GlyphReleased += (GetParent() as Combat).HandleGlyphReleased;
		grid.AddChild(glyph);
	}
}
