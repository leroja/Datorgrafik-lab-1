﻿using DatorGrafikLab1.GameComponents;
using DatorGrafikLab1GameSystems;
using Engine.Source.Components;
using Engine.Source.Enums;
using Engine.Source.Managers;
using Engine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DatorGrafikLab1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Engine.Engine
    {
        SpriteBatch spriteBatch;

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

            var t = Device;

            
            int skyboxEntity = ComponentManager.Instance.CreateID();
            List<IComponent> anotherList = new List<IComponent>
            {
                new ModelComponent(Content.Load<Model>("untitled")),
                new TransformComponent(new Vector3(250, 400, -500), new Vector3(5,5,5)),
            };

            ComponentManager.Instance.AddAllComponents(skyboxEntity, anotherList);

            //Entitet för planet
            int entityID = ComponentManager.Instance.CreateID();
            ModelComponent mcp = new ModelComponent(Content.Load<Model>("Chopper"));
            Matrix []meshWorldMatrices = new Matrix[3];
            meshWorldMatrices[0] = Matrix.CreateRotationY(0); 
            meshWorldMatrices[1] = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            meshWorldMatrices[2] = Matrix.CreateTranslation(new Vector3(0, 0, 0)); 


            mcp.MeshWorldMatrices = meshWorldMatrices;

            List<IComponent> componentList = new List<IComponent>
            {
                //Skapa och lägg till alla komponenter som vi behöver för modellen
                mcp,
                new TransformComponent(new Vector3(502, 51, -598), new Vector3(1, 1, 1)),
                new CameraComponent(new Vector3(0, 100, 120), new Vector3(0, 500, 0), new Vector3(0, 1, 0), 10000.0f, 1.0f, Device.Viewport.AspectRatio),
                new ChaseCamComponent
                {
                    OffSet = new Vector3(0, 10, 20)
                },
                new ChopperComponent()
                
            };

            var keýComp = new KeyBoardComponent();
            keýComp.KeyBoardActions.Add(ActionsEnum.Forward, Keys.Up);

            keýComp.KeyBoardActions.Add(ActionsEnum.RotatenegativeX, Keys.W);
            keýComp.KeyBoardActions.Add(ActionsEnum.RotateX, Keys.S);
            keýComp.KeyBoardActions.Add(ActionsEnum.RotatenegativeY, Keys.D);
            keýComp.KeyBoardActions.Add(ActionsEnum.RotateY, Keys.A);

            keýComp.KeyBoardActions.Add(ActionsEnum.RotateZ, Keys.Left);
            keýComp.KeyBoardActions.Add(ActionsEnum.RotatenegativeZ, Keys.Right);


            ComponentManager.Instance.AddAllComponents(entityID, componentList);
            ComponentManager.Instance.AddComponentToEntity(entityID, keýComp);

            int entityID1 = ComponentManager.Instance.CreateID();
            List<IComponent> componentList1 = new List<IComponent>
            {
                new HeightmapComponent(Content.Load<Texture2D>("US_Canyon"), Device),
                new TransformComponent(new Vector3(-300, -100, 0), new Vector3(1, 1, 1))
            };
            ComponentManager.Instance.AddAllComponents(entityID1, componentList1);

            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new HeightmapSystem(Device));
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new ChopperSystem());

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            
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
