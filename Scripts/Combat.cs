using Godot;
using System;
using System.Collections.Generic;

public partial class Combat : Control
{
	private Deck deck {get; set;}
	private Hand hand {get; set;}
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

	public void PickUpGlyph(Glyph glyph) {
		Node2D cursor = GetNode<Node2D>("Cursor");
		var PickUp = new Action<Glyph>((glyph) => { 
			glyph.dragging = true;
			hand.grid.RemoveChild(glyph);
			cursor.AddChild(glyph);			
		 } );
		var Drop = new Action<Glyph>((glyph) => { 
			glyph.dragging = false;
			cursor.RemoveChild(glyph);
			hand.grid.AddChild(glyph);
		 } );
		 
		if (cursor.GetChildren().Count == 0) {
			// Pick up glyph with empty cursor
			GD.Print("Picking up new glyph ", glyph.glyphType);
			PickUp(glyph);
		}
		else if (glyph != cursor.GetChild(0)) {
			Glyph cursorChild = cursor.GetChild(0) as Glyph;
			// Drop currently dragged and swap for new glyph
			GD.Print("Dropping glyph ", cursorChild.glyphType, ", picking up ", glyph.glyphType);
			Drop(cursorChild);
			PickUp(glyph);
		}
	}

	private Deck InitDeck(Dictionary<GlyphType, int> dict) {
		GD.Print("Initializing deck");
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
