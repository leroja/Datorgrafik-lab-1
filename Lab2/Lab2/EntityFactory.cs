using Engine.Source.Components;
using Engine.Source.Managers;
using Lab2.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatorGrafikLab1
{
    class EntityFactory
    {
        ContentManager Content;
        public EntityFactory(ContentManager Content)
        {
            this.Content = Content;
        }
        public void CreateSkyBox()
        {
            int skyboxEntity = ComponentManager.Instance.CreateID();
            List<IComponent> anotherList = new List<IComponent>
            {
                new ModelComponent(Content.Load<Model>("untitled")),
                new TransformComponent(new Vector3(250, 400, -500), new Vector3(5,5,5)),
            };
        }

        public void CreateChopper(GraphicsDevice Device)
        {
            int ChopperEnt = ComponentManager.Instance.CreateID();
            ModelComponent mcp = new ModelComponent(Content.Load<Model>("Chopper"));
            Matrix[] meshWorldMatrices = new Matrix[3];
            meshWorldMatrices[0] = Matrix.CreateRotationY(0);
            meshWorldMatrices[1] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[2] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            mcp.MeshWorldMatrices = meshWorldMatrices;

            List<IComponent> ChopperComponentList = new List<IComponent>
            {
                //Skapa och lägg till alla komponenter som vi behöver för modellen
                mcp,
                new TransformComponent(new Vector3(-300, -0, 0), new Vector3(1, 1, 1)),
                new CameraComponent(new Vector3(0, 100, 120), new Vector3(0, 500, 0), new Vector3(0, 1, 0), 10000.0f, 1.0f, Device.Viewport.AspectRatio),
                new ChaseCamComponent
                {
                    OffSet = new Vector3(0, 10, 20),
                    // sätt isDrunk till true om man vill ha en "drunk" kamera
                    IsDrunk = false
                },
                new ChopperComponent()
            };
            var keýComp = new KeyBoardComponent();
            keýComp.KeyBoardActions.Add("Forward", Keys.Up);
            keýComp.KeyBoardActions.Add("Backward", Keys.Down);
            keýComp.KeyBoardActions.Add("Right", Keys.Right);
            keýComp.KeyBoardActions.Add("Left", Keys.Left);
            keýComp.KeyBoardActions.Add("RotatenegativeX", Keys.W);
            keýComp.KeyBoardActions.Add("RotateX", Keys.S);
            keýComp.KeyBoardActions.Add("RotatenegativeY", Keys.D);
            keýComp.KeyBoardActions.Add("RotateY", Keys.A);
            keýComp.KeyBoardActions.Add("RotateZ", Keys.Q);
            keýComp.KeyBoardActions.Add("RotatenegativeZ", Keys.E);
            ComponentManager.Instance.AddAllComponents(ChopperEnt, ChopperComponentList);
            ComponentManager.Instance.AddComponentToEntity(ChopperEnt, keýComp);
        }
        
        public void CreateManyTrees(HeightmapComponentTexture hmp, int width, int height, VertexPositionNormalTexture[] terrainVertices)
        {
            int treeEntity;

            List<IComponent> components;
            Model tree = Content.Load<Model>("Leaf_Oak");

            int printedTrees = 0;
            bool printNextTree = true;
            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float terrainHeight = hmp.HeightMapData[x, y];
                    

                    if ((terrainHeight > 8) && (terrainHeight < 14))
                    {
                        
                        float flatness = Vector3.Dot(terrainVertices[x + y * width].Normal, new Vector3(0, 1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                            if (printNextTree == true && printedTrees <= 100)
                            {
                                float rand1 = (float)random.Next(1000) / 1000.0f;
                                float rand2 = (float)random.Next(1000) / 1000.0f;

                                float randomScale = (float)(random.Next(1, 5)/100.0f);

                                treeEntity = ComponentManager.Instance.CreateID();
                                components = new List<IComponent>(){
                                    new ModelComponent(tree),
                                    new TransformComponent(new Vector3((float)x - rand1, hmp.HeightMapData[x, y], -(float)y - rand2), new Vector3(randomScale,randomScale,randomScale))
                                };
                                ComponentManager.Instance.AddAllComponents(treeEntity, components);
                                printedTrees++;
                                x += 10;
                                if (x > width)
                                    x = 0;
                            }
                            

                            
                            

                        }

                        
                        
                    }
                }
            }

        }
    }
}
