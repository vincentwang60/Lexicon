using Godot;
using System;

public enum GlyphType
// Enum value used for Atlas texture splitting
{
    Chaos = 0,
    Order = 1,
    Arcane = 2,
    Creation = 3,
    Decay = 4,
    Growth = 5,
    Permanence = 6,
    Void = 7
}

public partial class Glyph : Control
{
    [Signal]
    public delegate void GlyphClickedEventHandler(Glyph glyph);
	[Signal]
    public delegate void GlyphReleasedEventHandler(Glyph glyph);
	[Export]
	public GlyphType glyphType {get; set;}
	// If picked up off Board, sets to Tile source. Removed when released
	public Tile tileSource {get; set;}
	public bool dragging {get; set;} = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var glyphSprite = GetNode<Sprite2D>("GlyphSprite");
		GD.Print("type:", glyphType);
		glyphSprite.Texture = GetTexture(glyphType);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (dragging) {
			GlobalPosition = GetViewport().GetMousePosition();
		}
	}
	
	private void OnGlyphClick(Node viewport, InputEvent @event, int shapeIdx) {
		if (@event.IsActionPressed("Click")) {
			dragging = true;
			EmitSignal("GlyphClicked", this);
		}
		if (@event.IsActionReleased("Click") && dragging) {
			EmitSignal("GlyphReleased", this);
			tileSource = null;
			dragging = false;
		}
	}

    private AtlasTexture GetTexture(GlyphType glyphType) {
		AtlasTexture glyphAtlas = new AtlasTexture {
			Atlas = GD.Load<Texture2D>("res://Assets/Sprites/Glyph.png"),
			Region = new Rect2((float)glyphType * Constants.glyphWidth, 0, Constants.glyphWidth, Constants.glyphWidth)
		};
		return glyphAtlas;
	}
}
