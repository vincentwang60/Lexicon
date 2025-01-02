using Godot;
using System;
using System.Collections.Generic;

public partial class Book : Control
{
	List<WordData> bookData {get; set;}

	[Export]
	public PackedScene WordScene {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Book InitBook(List<WordData> bookData) {		
		VBoxContainer bookLeft = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/Control/BookLeft");
		VBoxContainer bookReft = GetNode<VBoxContainer>("PanelContainer/HBoxContainer/Control2/BookRight");
		
		var CreateWordFromData = new Func<WordData, Word>((wordData) => { 
			Word newWord = WordScene.Instantiate<Word>();
			newWord.wordName = wordData.wordName;
			newWord.wordDescription = wordData.wordDescription;
			newWord.wordGlyphs = wordData.wordGlyphs;
			return newWord;
		});
		
		bool leftPageFilled = false;
		foreach (WordData wordData in bookData) {
			if (!leftPageFilled) {
				// add to left page
				bookLeft.AddChild(CreateWordFromData(wordData));
			}
			else {
				// add to right page
			}
		}
		return this;
	}
}
