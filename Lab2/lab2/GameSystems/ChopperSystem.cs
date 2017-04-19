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

                //if (keyBComp.State["RotateZ"] == ButtonStates.Hold)
                //{
                //    newRot.Z += 0.01f;
                //}
                //if (keyBComp.State["RotatenegativeZ"] == ButtonStates.Hold)
                //{
                //    newRot.Z += -0.01f;
                //}

                if (keyBComp.State["RotateY"] == ButtonStates.Hold)
                {
                    newRot.Y += 0.02f;
                }
                if (keyBComp.State["RotatenegativeY"] == ButtonStates.Hold)
                {
                    newRot.Y -= 0.02f;
                }
                //if (keyBComp.State[ActionsEnum.RotateX] == ButtonStates.Hold)
                //{
                //    newRot.X += 0.01f;
                //}
                //if (keyBComp.State[ActionsEnum.RotatenegativeX] == ButtonStates.Hold)
                //{
                //    newRot.X -= 0.01f;
                //}


                //var heightmaps = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();

                //foreach (var heightmap in heightmaps)
                //{
                //    var heghtmapComp = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightmap);
                //    foreach (var chunk in heghtmapComp.HeightMapChunks)
                //    {
                //        var lastVerticePos = chunk.Vertices[chunk.Vertices.Length - 1].Position;

                //        //Check which heightmap we are on
                //        if (transformComp.Position.X >= chunk.Vertices[0].Position.X &&
                //            transformComp.Position.X < lastVerticePos.X + 1
                //            && transformComp.Position.Z < chunk.Vertices[0].Position.Z + 1 &&
                //            transformComp.Position.Z >= lastVerticePos.Z)
                //        {
                //            var normalizedZ = (int)(-chunk.Height - transformComp.Position.Z);
                //            var normalizedX = (int)(transformComp.Position.X - chunk.Width);

                //            var index = (int)normalizedZ * chunk.Width + normalizedX;

                //            var y = chunk.Vertices[index];

                //            var hmPos = new Vector3(transformComp.Position.X, y.Position.Y, transformComp.Position.Z);
                //            transformComp.Position = Vector3.Lerp(transformComp.Position, hmPos, 0.3f);
                //        }
                //    }

                //var lastVerticePos = heightmap.Vertices[heightmap.Vertices.Length - 1].Position;

                ////Check which heightmap we are on
                //if (transform.Position.X >= heightmap.Vertices[0].Position.X &&
                //    transform.Position.X < lastVerticePos.X + 1
                //    && transform.Position.Z < heightmap.Vertices[0].Position.Z + 1 &&
                //    transform.Position.Z >= lastVerticePos.Z)
                //{
                //    var normalizedZ = (int)(-heightmap.ChunkHeight - transform.Position.Z);
                //    var normalizedX = (int)(transform.Position.X - heightmap.ChunkWidth);

                //    var index = (int)normalizedZ * heightmap.HeightMapChunkWidth + normalizedX;

                //    var y = heightmap.Vertices[index];

                //    var hmPos = new Vector3(transform.Position.X, y.Position.Y, transform.Position.Z);
                //    transform.Position = Vector3.Lerp(transform.Position, hmPos, 0.3f);
                //}

                //}

                var heightmapId = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>()[0];
                HeightmapComponentTexture heightmapComp = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightmapId);
                var heightMapTransformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightmapId);

                foreach (var chunk in heightmapComp.HeightMapChunks)
                {
                    
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
