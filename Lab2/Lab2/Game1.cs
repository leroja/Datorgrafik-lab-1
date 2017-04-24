using Lab2.GameComponents;
using Lab2.GameSystems;
using Engine.Source.Components;
using Engine.Source.Enums;
using Engine.Source.Managers;
using Engine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Engine.Source.Factories;
using Lab2.Humanoid;

namespace Lab2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Engine.Engine
    {
        public Game1()
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Device = Graphics.GraphicsDevice;
            //RasterizerState r = new RasterizerState();
            //r.CullMode = CullMode.None;
            //r.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = r;
            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;


            HeightMapFactory heightmapFactory = new HeightMapFactory(Device);
            EntityFactory factory = new EntityFactory(Content);
            //factory.CreateSkyBox();
            //factory.CreateChopper(Device);
            factory.CreateHumanoidEntity(Content.Load<Texture2D>("canyon_rgb"),Device);

            int HeightmapEnt = ComponentManager.Instance.CreateID();

            HeightmapComponentTexture hmp = heightmapFactory.CreateTexturedHeightMap(Content.Load<Texture2D>("Canyon_elev_1024"), Content.Load<Texture2D>("grass"), 10);

            List<IComponent> HeightmapCompList = new List<IComponent>
            {
                hmp,
                new TransformComponent(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            };
            ComponentManager.Instance.AddAllComponents(HeightmapEnt, HeightmapCompList);

            factory.CreateManyTrees(hmp, heightmapFactory.Width, heightmapFactory.Height, heightmapFactory.VerticesTexture);
            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new HeightmapSystemColour(Device));
            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(Device));
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new ChopperSystem());
            
            SystemManager.Instance.AddSystem(new HumanoidSystemUpdate());
            SystemManager.Instance.AddSystem(new HumanoidSystemRender());

            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new ChaseCamSystem());
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
        }
        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }
    }
}
