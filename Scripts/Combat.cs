using Godot;
using System;
using System.Collections.Generic;

public partial class Combat : Control
{
	private Deck deck {get; set;}
	private Hand hand {get; set;}
	private Tile hoveringTile {get; set;}
	public override void _Ready()
	{
		hand = GetNode<Hand>("Hand");
		deck = InitDeck(new Dictionary<GlyphType, int>() {
			{GlyphType.Chaos, 5},
			{GlyphType.Order, 5},
			{GlyphType.Arcane, 2},
		});
		deck.PrintDeck();
		deck.ShuffleDeck();
		deck.PrintDeck();
		DrawHand();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("RClick")) {
			DropGlyphInHand();
		}
    }

    public void HandleGlyphClicked(Glyph glyph) {
		Node2D cursor = GetNode<Node2D>("Cursor");
		var PickUp = new Action<Glyph>((glyph) => { 
			glyph.GetParent().RemoveChild(glyph);
			cursor.AddChild(glyph);			
		 } );
		var Drop = new Action<Glyph>((glyph) => { 
			cursor.RemoveChild(glyph);
			hand.grid.AddChild(glyph);
		 } );

		if (cursor.GetChildCount() == 0) {
			// Pick up glyph with empty cursor
			PickUp(glyph);
		}
		else if (glyph != cursor.GetChild(0)) {
			Glyph cursorChild = cursor.GetChild(0) as Glyph;
			// Drop currently dragged and swap for new glyph
			Drop(cursorChild);
			PickUp(glyph);
		}
	}

	public void HandleGlyphReleased(Glyph glyph) {
		Node2D cursor = GetNode<Node2D>("Cursor");
		if (hoveringTile != null && glyph.dragging) {
			cursor.RemoveChild(glyph);
			hoveringTile.AddGlyph(glyph);
		}
		else {
			cursor.RemoveChild(glyph);
			hand.grid.AddChild(glyph);
		}
	}

	public void UpdateHoveringTile(Tile tile, bool entered) {
		if (entered) {
			hoveringTile = tile;
		}
		else if (hoveringTile == tile) {
			hoveringTile = null;
		}
	}

	private void DropGlyphInHand() {
		Node2D cursor = GetNode<Node2D>("Cursor");
		if (cursor.GetChildCount() != 0) {
			Glyph cursorChild = cursor.GetChild(0) as Glyph;
			GD.Print("Dropping glyph ", cursorChild.glyphType);
			cursorChild.dragging = false;
			cursor.RemoveChild(cursorChild);
			hand.grid.AddChild(cursorChild);
		}
	}

	private Deck InitDeck(Dictionary<GlyphType, int> dict) {
		Deck deck = GetNode<Deck>("Deck");
		foreach(KeyValuePair<GlyphType, int> entry in dict) {
			for (int i = 0; i < entry.Value; i++) {
				deck.AddGlyph(new Glyph { glyphType = entry.Key });
			}
		}
		return deck;
	}

	private void DrawHand() {
		GD.Print("Drawing hand of ", Constants.handSize);
		int i = 0;
		while (i < Constants.handSize) {
			hand.AddGlyph(deck.DrawGlyph().glyphType);
			i++;
		}
	}
}
