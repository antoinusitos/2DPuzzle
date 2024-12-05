namespace _2DPuzzle
{
    public class Gizmo : Entity
    {
        public Gizmo(bool inInitializeNewEntity = true) : base(inInitializeNewEntity)
        {
            name = "Gizmo";

            components.Add(new GizmoComponent(this));
            SpriteRenderComponent spriteRenderComponent = new SpriteRenderComponent(this, "Gizmo");
            components.Add(spriteRenderComponent);
            spriteRenderComponent.SwitchLayer(3);
        }
    }
}
