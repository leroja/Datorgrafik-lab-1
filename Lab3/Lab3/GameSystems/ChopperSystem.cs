using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using Engine.Source.Components;
using Engine.Source.Enums;
using Lab3.GameComponents;

namespace Lab3.GameSystems
{
    public class ChopperSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            var choppIds = ComponentManager.GetAllEntitiesWithComponentType<ChopperComponent>();
            if (choppIds == null)
                return;

            foreach (var chopperId in choppIds)
            {
                var chopperComp = ComponentManager.GetEntityComponent<ChopperComponent>(chopperId);
                chopperComp.MainRotorAngle -= 0.15f;
                chopperComp.TailRotorAngle -= 0.15f;
                


                var keyBComp = ComponentManager.GetEntityComponent<KeyBoardComponent>(chopperId);

                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(chopperId);
                var modelComp = ComponentManager.GetEntityComponent<ModelComponent>(chopperId);
                ReCalculateMatrices(chopperComp, modelComp);

                var newRot = transformComp.Rotation;

                // reset rotation, ta bort för typ konstant rotation
                newRot.X = 0;
                newRot.Z = 0;
                newRot.Y = 0;

                //if (newRot.X > MathHelper.TwoPi)
                //    newRot.X -= MathHelper.TwoPi;
                //if (newRot.Y > MathHelper.TwoPi)
                //    newRot.Y -= MathHelper.TwoPi;
                //if (newRot.Z > MathHelper.TwoPi)
                //    newRot.Z -= MathHelper.TwoPi;


                if (keyBComp.State["Forward"] == ButtonStates.Hold)
                {
                    transformComp.Position += transformComp.Forward;
                }
                if (keyBComp.State["Backward"] == ButtonStates.Hold)
                {
                    transformComp.Position -= transformComp.Forward;
                }
                if (keyBComp.State["Right"] == ButtonStates.Hold)
                {
                    transformComp.Position += transformComp.Right;
                }
                if (keyBComp.State["Left"] == ButtonStates.Hold)
                {
                    transformComp.Position -= transformComp.Right;
                }

                if (keyBComp.State["RotateZ"] == ButtonStates.Hold)
                {
                    newRot.Z += 0.01f;
                }
                if (keyBComp.State["RotatenegativeZ"] == ButtonStates.Hold)
                {
                    newRot.Z += -0.01f;
                }

                if (keyBComp.State["RotateY"] == ButtonStates.Hold)
                {
                    newRot.Y += 0.02f;
                }
                if (keyBComp.State["RotatenegativeY"] == ButtonStates.Hold)
                {
                    newRot.Y -= 0.02f;
                }
                if (keyBComp.State["RotateX"] == ButtonStates.Hold)
                {
                    newRot.X += 0.01f;
                }
                if (keyBComp.State["RotatenegativeX"] == ButtonStates.Hold)
                {
                    newRot.X -= 0.01f;
                }
                
                transformComp.Rotation = newRot;
            }
        }

        private void ReCalculateMatrices(ChopperComponent chopperComp, ModelComponent modelComp)
        {
            Matrix[] meshWorldMatrices = new Matrix[3];
            meshWorldMatrices[0] = Matrix.CreateRotationY(chopperComp.MainRotorAngle);
            meshWorldMatrices[1] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[2] = Matrix.Invert(Matrix.CreateTranslation(modelComp.Model.Bones["Back_Rotor"].Transform.Translation)) *
                                   Matrix.CreateRotationX(chopperComp.TailRotorAngle) *
                                   Matrix.CreateTranslation(modelComp.Model.Bones["Back_Rotor"].Transform.Translation);
            modelComp.MeshWorldMatrices = meshWorldMatrices;
            
        }
    }
}
