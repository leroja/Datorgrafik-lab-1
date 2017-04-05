using Engine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Engine.Source.Managers;
using DatorGrafikLab1.Content.GameComponents;
using Engine.Source.Components;
using Engine.Source.Enums;

namespace DatorGrafikLab1.Content.GameSystems
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
                chopperComp.mainRotorAngle -= 0.15f;
                chopperComp.tailRotorAngle -= 0.15f;
                


                var keyBComp = compMan.GetEntityComponent<KeyBoardComponent>(chopperId);

                var transformComp = compMan.GetEntityComponent<TransformComponent>(chopperId);
                var modelComp = compMan.GetEntityComponent<ModelComponent>(chopperId);
                RecalculateMetrices(chopperComp, modelComp);

                var newRot = transformComp.Rotation;
                

                //newRot.X = 0;
                //newRot.Z = 0;
                //newRot.Y = 0;

                //if (keyBComp.state[ActionsEnum.Forward] == ButtonStates.Hold)
                //{

                //}
                


                //if (keyBComp.state[ActionsEnum.RotateZ] == ButtonStates.Hold)
                //{
                //    newRot.Z = 0.03f;
                //}
                //if (keyBComp.state[ActionsEnum.RotatenegativeZ] == ButtonStates.Hold)
                //{
                //    newRot.Z = -0.03f;
                //}
                //if (keyBComp.state[ActionsEnum.RotateY] == ButtonStates.Hold)
                //{

                //}
                //if (keyBComp.state[ActionsEnum.RotatenegativeY] == ButtonStates.Hold)
                //{

                //}
                //if (keyBComp.state[ActionsEnum.RotateX] == ButtonStates.Hold)
                //{

                //}
                //if (keyBComp.state[ActionsEnum.RotatenegativeX] == ButtonStates.Hold)
                //{

                //}


            }
        }

        private void RecalculateMetrices(ChopperComponent chopperComp, ModelComponent modelComp)
        {
            Matrix[] meshWorldMatrices = new Matrix[3];
            meshWorldMatrices[0] = Matrix.CreateRotationY(chopperComp.mainRotorAngle);
            meshWorldMatrices[1] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[2] = Matrix.Invert(Matrix.CreateTranslation(modelComp.Model.Bones["Back_Rotor"].Transform.Translation)) *
                                   Matrix.CreateRotationX(chopperComp.tailRotorAngle) *
                                   Matrix.CreateTranslation(modelComp.Model.Bones["Back_Rotor"].Transform.Translation);
            modelComp.meshWorldMatrices = meshWorldMatrices;

        }


    }
}
