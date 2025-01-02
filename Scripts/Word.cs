using Godot;
using System;
using System.Collections.Generic;

public class WordData
{
    public string wordName { get; set; }
    public string wordDescription { get; set; }
    public GlyphType[] wordGlyphs { get; set; }

    public WordData(string name, string description, GlyphType[] glyphs)
    {
        wordName = name;
        wordDescription = description;
        wordGlyphs = glyphs;
    }
}

public partial class Word : PanelContainer
{

	public string wordName {get; set;}
	public string wordDescription {get; set;}
	public GlyphType[] wordGlyphs {get; set;}
	[Export]
	public PackedScene GlyphScene {get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label wordNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/WordTitle/WordName");
		wordNameLabel.Text = wordName;
		
		Label wordDescriptionLabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		wordDescriptionLabel.Text = wordDescription;
		
		GridContainer wordGlyphContainer = GetNode<GridContainer>("MarginContainer/VBoxContainer/WordTitle/Control/GlyphGrid");
		foreach (GlyphType glyphType in wordGlyphs) {
			Glyph glyph = GlyphScene.Instantiate<Glyph>();
			glyph.glyphType = glyphType;
			wordGlyphContainer.AddChild(glyph);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
