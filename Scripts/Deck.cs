using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{
	private List<Glyph> glyphs = new List<Glyph>();

	public Glyph DrawGlyph() {
        if (glyphs.Count == 0) {
            GD.Print("Deck is empty!");
            return null;
        }
        var glyph = glyphs[0];
        glyphs.RemoveAt(0);
        return glyph;

	}

	public void AddGlyph(Glyph glyph){
		glyphs.Add(glyph);
	}

	public List<Glyph> ShuffleDeck() {
		GD.Print("Shuffling deck");
		var random = new Random();
        for (int i = glyphs.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            ( glyphs[i], glyphs[j] ) = ( glyphs[j], glyphs[i] );
        }
		return glyphs;
	}

	public void PrintDeck() {
		string output = "Deck order: ";
		foreach (var glyph in glyphs) {
			output += glyph.glyphType + ", ";
		}
		GD.Print(output);
	}
}
