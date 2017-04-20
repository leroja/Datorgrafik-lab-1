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


                var heightmaps = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();

                foreach (var heightmap in heightmaps)
                {
                    var heghtmapComp = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightmap);
                    var heightMapTransformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightmap);

                    //t.position = new Vector3(t.position.X, 1.7f + TerrainMapRenderSystem.GetTerrainHeight(tcomp, t.position.X, Math.Abs(t.position.Z)), t.position.Z);
                    //transformComp.Position = new Vector3(transformComp.Position.X, GetTerrainHeight(heghtmapComp, transformComp.Position.X + heightMapTransformComp.Position.X, Math.Abs(transformComp.Position.Z + heightMapTransformComp.Position.Z)) /*+ heightMapTransformComp.Position.Y*/, transformComp.Position.Z + heightMapTransformComp.Position.Z);

                    //System.Console.WriteLine(transformComp.Position.Y);
                    //transformComp.Position.Y += heightMapTransformComp.Position.Y;

                    //foreach (var chunk in heghtmapComp.HeightMapChunks)
                    //{
                    //    var lastVerticePos = chunk.Vertices[chunk.Vertices.Length - 1].Position + heightMapTransformComp.Position + new Vector3(chunk.Rectangle.X, 0, chunk.Rectangle.Y);

                    //    //Check which heightmap we are on
                    //    if (transformComp.Position.X >= chunk.Vertices[0].Position.X + heightMapTransformComp.Position.X &&
                    //        transformComp.Position.X < lastVerticePos.X + 1
                    //        && transformComp.Position.Z < chunk.Vertices[0].Position.Z + 1 + heightMapTransformComp.Position.Z &&
                    //        transformComp.Position.Z >= lastVerticePos.Z)
                    //    {
                    //        var normalizedZ = (int)(-chunk.Rectangle.Y - heightMapTransformComp.Position.Z - transformComp.Position.Z);
                    //        var normalizedX = (int)(transformComp.Position.X - chunk.Rectangle.X - heightMapTransformComp.Position.X);

                    //        var index = (int)normalizedZ * chunk.Width + normalizedX;

                    //        var y = chunk.Vertices[index];

                    //        var hmPos = new Vector3(transformComp.Position.X, y.Position.Y + heightMapTransformComp.Position.Y, transformComp.Position.Z);
                    //        //transformComp.Position = Vector3.Lerp(transformComp.Position, hmPos, 0.2f);
                    //        transformComp.Position = hmPos;
                    //        break;
                    //    }
                    //}
                }

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

                //var heightmapId = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>()[0];
                //HeightmapComponentTexture heightmapComp = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightmapId);
                //var heightMapTransformComp = ComponentManager.GetEntityComponent<TransformComponent>(heightmapId);

                //foreach (var chunk in heightmapComp.HeightMapChunks)
                //{

                //}




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

        ////t.position = new Vector3(t.position.X, 1.7f + TerrainMapRenderSystem.GetTerrainHeight(tcomp, t.position.X, Math.Abs(t.position.Z)), t.position.Z);
        //private float GetTerrainHeight(HeightmapComponentTexture terrain, float x, float z)
        //{
        //    if (x < 0
        //        || z < 0
        //        || x > terrain.HeightMapData.GetLength(0) - 1
        //        || z > terrain.HeightMapData.GetLength(1) - 1)
        //    {
        //        return 10f;
        //    }
        //    //find the two x vertices
        //    int xLow = (int)x;
        //    int xHigh = xLow + 1;
        //    //get the relative x value between the two points
        //    float xRel = (x - xLow) / ((float)xHigh - (float)xLow);

        //    //find the two z verticies
        //    int zLow = (int)z;
        //    int zHigh = zLow + 1;

        //    //get the relative z value between the two points
        //    float zRel = (z - zLow) / ((float)zHigh - (float)zLow);

        //    //get the minY and MaxY values from the four vertices
        //    float heightLowXLowZ = terrain.HeightMapData[xLow, zLow];
        //    float heightLowXHighZ = terrain.HeightMapData[xLow, zHigh];
        //    float heightHighXLowZ = terrain.HeightMapData[xHigh, zLow];
        //    float heightHighXHighZ = terrain.HeightMapData[xHigh, zHigh];

        //    //test if the position is above the low triangle
        //    bool posAboveLowTriangle = (xRel + zRel < 1);

        //    float resultHeight;

        //    if (posAboveLowTriangle)
        //    {
        //        resultHeight = heightLowXLowZ;
        //        resultHeight += zRel * (heightLowXHighZ - heightLowXLowZ);
        //        resultHeight += xRel * (heightHighXLowZ - heightLowXLowZ);
        //    }
        //    else
        //    {
        //        resultHeight = heightHighXHighZ;
        //        resultHeight += (1.0f - zRel) * (heightHighXLowZ - heightHighXHighZ);
        //        resultHeight += (1.0f - xRel) * (heightLowXHighZ - heightHighXHighZ);
        //    }
        //    return resultHeight;
        //}

    }
}
