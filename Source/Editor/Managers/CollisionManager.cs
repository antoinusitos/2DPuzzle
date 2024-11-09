using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace _2DPuzzle
{
    public class CollisionManager
    {
        private static CollisionManager _instance;

        private static readonly object _lock = new object();

        public static CollisionManager GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new CollisionManager();
                    }
                }
            }
            return _instance;
        }

        private List<PhysicsComponent> registeredComponents = new List<PhysicsComponent>();

        public void RegisterPhysicsComponent(PhysicsComponent inPhysicsComponent)
        {
            registeredComponents.Add(inPhysicsComponent);
        }

        public void ProcessPhysics()
        {
            for(int physicsComponentIndex = 0; physicsComponentIndex < registeredComponents.Count; physicsComponentIndex++)
            {
                if (registeredComponents[physicsComponentIndex].collisionType == CollisionType.DYNAMIC)
                {
                    for (int againsPhysicsComponentIndex = 0; againsPhysicsComponentIndex < registeredComponents.Count; againsPhysicsComponentIndex++)
                    {
                        if (registeredComponents[physicsComponentIndex] == registeredComponents[againsPhysicsComponentIndex])
                        {
                            continue;
                        }

                        Vector2 contactPoint = Vector2.Zero;
                        Vector2 contactNormal = Vector2.Zero;
                        float contactTime = 0;
                        if(DynamicRectVsRect(registeredComponents[physicsComponentIndex], registeredComponents[againsPhysicsComponentIndex], ref contactPoint, ref contactNormal, ref contactTime, UpdateManager.GetInstance().deltaTime))
                        {
                            // Here we are multiplying by - 1 because up tend to 0
                            registeredComponents[physicsComponentIndex].velocity += contactNormal * -1 * new Vector2(System.Math.Abs(registeredComponents[physicsComponentIndex].velocity.X), System.Math.Abs(registeredComponents[physicsComponentIndex].velocity.Y)) * (1 - contactTime);
                        }
                    }

                    registeredComponents[physicsComponentIndex].owner.transformComponent.position += registeredComponents[physicsComponentIndex].velocity;
                }
            }
        }

        public bool RectVsRect(Rectangle inRect1, Rectangle inRect2)
        {
            return (inRect1.X < inRect2.X + inRect2.Width && inRect1.X + inRect1.Width > inRect2.X &&
                    inRect1.Y < inRect2.Y + inRect2.Height && inRect1.Y + inRect1.Height > inRect2.Y);
        }

        public bool RayVsRect(Vector2 inRayOrigin, Vector2 inRayDir, Rectangle inTarget, ref Vector2 contactPoint, ref Vector2 contactNormal, ref float hitNear)
        {
            Vector2 near = (new Vector2(inTarget.X, inTarget.Y) - inRayOrigin) / inRayDir;
            Vector2 far = (new Vector2(inTarget.X, inTarget.Y) + new Vector2(inTarget.Width, inTarget.Height) - inRayOrigin) / inRayDir;

            if(near.X > far.X)
            {
                float tempX = near.X;
                near.X = far.X;
                far.X = tempX;
            }
            if (near.Y > far.Y)
            {
                float tempY = near.Y;
                near.Y = far.Y;
                far.Y = tempY;
            }

            if (near.X > far.Y || near.Y > far.X) return false;

            hitNear = MathHelper.Max(near.X, near.Y);
            float hitFar = MathHelper.Min(far.X, far.Y);

            if (hitFar < 0) return false;

            contactPoint = inRayOrigin + hitNear * inRayDir;

            if(near.X > near.Y)
            {
                if(inRayDir.X < 0)
                {
                    contactNormal = Vector2.UnitX;
                }
                else
                {
                    contactNormal = -Vector2.UnitX;
                }
            }
            else if (near.X < near.Y)
            {
                if (inRayDir.Y < 0)
                {
                    contactNormal = Vector2.UnitY;
                }
                else
                {
                    contactNormal = -Vector2.UnitY;
                }
            }

            return true;
        }

        public bool DynamicRectVsRect(PhysicsComponent inRect1, PhysicsComponent inTarget, ref Vector2 contactPoint, ref Vector2 contactNormal, ref float contactTime, float elapsedTime)
        {
            if(inRect1.velocity.X == 0 && inRect1.velocity.Y == 0)
            {
                return false;
            }

            Vector2 pos = new Vector2(inTarget.rectangle.X, inTarget.rectangle.Y);
            pos -= new Vector2(inRect1.rectangle.Width, inRect1.rectangle.Height) / 2;
            Vector2 size = new Vector2(inTarget.rectangle.Width, inTarget.rectangle.Height);
            size.X += inRect1.rectangle.Width;
            size.Y += inRect1.rectangle.Height;
            Rectangle expandedTarget = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

            if(RayVsRect(new Vector2(inRect1.rectangle.X, inRect1.rectangle.Y) + new Vector2(inRect1.rectangle.Width, inRect1.rectangle.Height) / 2,
                inRect1.velocity * elapsedTime, expandedTarget, ref contactPoint, ref contactNormal, ref contactTime))
            {
                if(contactTime <= 1.0f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
