using Godot;
using System;
using System.Collections.Generic;

public partial class Combat : Control
{
	private Deck deck {get; set;}
	private Hand hand {get; set;}
	private Tile hoveringTile {get; set;}
	private Board board {get; set;}
	private Book book {get; set;}

	private List<WordData> wordList {get; set;}
	public override void _Ready()
	{
		hand = GetNode<Hand>("Hand");
		deck = InitDeck(new Dictionary<GlyphType, int>() {
			{GlyphType.Chaos, 8},
			{GlyphType.Order, 8},
			{GlyphType.Arcane, 3},
		});

		wordList = new List<WordData> {
			new WordData("Unravel", "Deal 5", new GlyphType[] {GlyphType.Chaos, GlyphType.Chaos}),
			new WordData("Unleash", "Deal 3, 4 times", new GlyphType[] {GlyphType.Chaos, GlyphType.Arcane, GlyphType.Chaos}),
			new WordData("Stabilize", "Shield 4", new GlyphType[] {GlyphType.Order, GlyphType.Order}),
			new WordData("Weave", "Repeat every third word", new GlyphType[] {GlyphType.Order, GlyphType.Arcane, GlyphType.Order,}) 
		};
		book = GetNode<Book>("Book");
		book.InitBook(wordList);

		board = GetNode<Board>("Board");
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
			// Handle removing glyph from board
			var parent = glyph.GetParent();
			if (parent.Name == "TileCenterContainer") {
				board.SetTile((parent.GetParent() as Tile).tileCoords, null);
				glyph.tileSource = parent.GetParent() as Tile;
			}
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
			// TODO currently not used, useful if click and move is implemented
			Drop(cursorChild);
			PickUp(glyph);
		}
	}

	public void HandleGlyphReleased(Glyph glyph) {
		Node2D cursor = GetNode<Node2D>("Cursor");
		if (glyph.tileSource != null) {
			glyph.tileSource.contents = null;
		}
		if (hoveringTile != null && glyph.dragging) {
			cursor.RemoveChild(glyph);
			board.SetTile(hoveringTile.tileCoords, glyph);
			if (hoveringTile.contents != null) {
				hoveringTile.contents.GetParent().RemoveChild(hoveringTile.contents);
				hand.grid.AddChild(hoveringTile.contents);
			}
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

	public void HandleButtonPress() {
		GD.Print("Playing board");
		board.ProcessBoard(wordList);
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
