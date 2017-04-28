using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Components;
using Engine.Source.Enums;

namespace Engine.Source.Systems
{
    public class FreeRoamCamSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<FreeRoamCamComponent>();
            if (ids != null)
            {
                foreach (var id in ids)
                {

                    var FRCComp = ComponentManager.GetEntityComponent<FreeRoamCamComponent>(id);
                    var baseCamComp = ComponentManager.GetEntityComponent<CameraComponent>(id);
                    var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(id);
                    var KeyBoardComp = ComponentManager.GetEntityComponent<KeyBoardComponent>(id);

                    var newRot = transformComp.Rotation;
                    
                    newRot.X = 0;
                    newRot.Z = 0;
                    newRot.Y = 0;

                    if (KeyBoardComp.State["Forward"] == ButtonStates.Hold)
                    {
                        transformComp.Position += transformComp.Forward;
                    }
                    if (KeyBoardComp.State["Backward"] == ButtonStates.Hold)
                    {
                        transformComp.Position -= transformComp.Forward;
                    }
                    if (KeyBoardComp.State["Right"] == ButtonStates.Hold)
                    {
                        transformComp.Position += transformComp.Right;
                    }
                    if (KeyBoardComp.State["Left"] == ButtonStates.Hold)
                    {
                        transformComp.Position -= transformComp.Right;
                    }
                    if (KeyBoardComp.State["Up"] == ButtonStates.Hold)
                    {
                        transformComp.Position += transformComp.Up;
                    }
                    if (KeyBoardComp.State["Down"] == ButtonStates.Hold)
                    {
                        transformComp.Position -= transformComp.Up;
                    }

                    if (KeyBoardComp.State["RotateZ"] == ButtonStates.Hold)
                    {
                        newRot.Z += 0.01f;
                    }
                    if (KeyBoardComp.State["RotatenegativeZ"] == ButtonStates.Hold)
                    {
                        newRot.Z += -0.01f;
                    }
                    if (KeyBoardComp.State["RotateY"] == ButtonStates.Hold)
                    {
                        newRot.Y += 0.02f;
                    }
                    if (KeyBoardComp.State["RotatenegativeY"] == ButtonStates.Hold)
                    {
                        newRot.Y -= 0.02f;
                    }
                    if (KeyBoardComp.State["RotateX"] == ButtonStates.Hold)
                    {
                        newRot.X += 0.01f;
                    }
                    if (KeyBoardComp.State["RotatenegativeX"] == ButtonStates.Hold)
                    {
                        newRot.X -= 0.01f;
                    }

                    transformComp.Rotation = newRot;

                    var rotation = Matrix.CreateFromQuaternion(transformComp.QuaternionRotation);
                    
                    baseCamComp.Position = transformComp.Position;

                    var lookAt = Vector3.Transform(FRCComp.LookAtOffSet, rotation);

                    baseCamComp.UpVector = Vector3.Transform(Vector3.Up, rotation);
                    baseCamComp.LookAt = transformComp.Position + lookAt;
                }
            }
        }
    }
}
