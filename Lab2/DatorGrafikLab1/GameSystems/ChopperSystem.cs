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
using Lab2.GameComponents;

namespace Lab2.GameSystems
{
    public class ChopperSystem : IUpdate
    {
        public void Update(GameTime gameTime)
        {
            ComponentManager compMan = ComponentManager.Instance;

            var choppIds = compMan.GetAllEntitiesWithComponentType<ChopperComponent>();
            if (choppIds == null)
                return;

            foreach (var chopperId in choppIds)
            {
                var chopperComp = compMan.GetEntityComponent<ChopperComponent>(chopperId);
                chopperComp.MainRotorAngle -= 0.15f;
                chopperComp.TailRotorAngle -= 0.15f;
                


                var keyBComp = compMan.GetEntityComponent<KeyBoardComponent>(chopperId);

                var transformComp = compMan.GetEntityComponent<TransformComponent>(chopperId);
                var modelComp = compMan.GetEntityComponent<ModelComponent>(chopperId);
                ReCalculateMatrices(chopperComp, modelComp);

                var newRot = transformComp.Rotation;

                // reset rotation, ta bort för typ konstant rotation
                newRot.X = 0;
                newRot.Z = 0;
                newRot.Y = 0;


                if (keyBComp.State[ActionsEnum.Forward] == ButtonStates.Hold)
                {
                    transformComp.Position += transformComp.Forward;
                }
                if (keyBComp.State[ActionsEnum.RotateZ] == ButtonStates.Hold)
                {
                    newRot.Z += 0.01f;
                }
                if (keyBComp.State[ActionsEnum.RotatenegativeZ] == ButtonStates.Hold)
                {
                    newRot.Z += -0.01f;
                }
                if (keyBComp.State[ActionsEnum.RotateY] == ButtonStates.Hold)
                {
                    newRot.Y += 0.01f;
                }
                if (keyBComp.State[ActionsEnum.RotatenegativeY] == ButtonStates.Hold)
                {
                    newRot.Y -= 0.01f;
                }
                if (keyBComp.State[ActionsEnum.RotateX] == ButtonStates.Hold)
                {
                    newRot.X += 0.01f;
                }
                if (keyBComp.State[ActionsEnum.RotatenegativeX] == ButtonStates.Hold)
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
