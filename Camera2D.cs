using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    [Export]
    Vector2 DesiredResolution = new Vector2(512, 300);

    Viewport view;

    float ScalingFactor = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        view = GetViewport();
        view.Connect("size_changed", this, nameof(_OnVPSizeChanged));
        _OnVPSizeChanged();
    }

    void _OnVPSizeChanged() {
        var scaleVector = view.Size / DesiredResolution;
        var newScalingFactor = Mathf.Max(Mathf.Floor(Mathf.Min(scaleVector[0], scaleVector[1])), 1);
        if (newScalingFactor != ScalingFactor) {
            ScalingFactor = newScalingFactor;
            Zoom = new Vector2(1 / ScalingFactor, 1 / ScalingFactor);
        }
    }
}
