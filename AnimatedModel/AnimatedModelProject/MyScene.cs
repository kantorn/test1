// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#region Using Statements
using System.Collections.Generic;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Materials;
#endregion

namespace AnimatedModelProject
{
    public class MyScene : Scene
    {
        Animation3D anim;
        public static AnimState currentState, lastState;
        public  enum AnimState { Idle, Up, Down, Left, Right };
        public static FreeCamera camera;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;


            //ViewCamera camera = new ViewCamera("MainCamera", new Vector3(0, 2f, -2.5f), new Vector3(0, 1, 0));
            camera = new FreeCamera("freeCamera", new Vector3(0, 2f, -3.5f), Vector3.UnitY * 0.9f)
            {
                Speed = 5
            };
            Entity animatedModel = new Entity("Isis")
                .AddComponent(new Transform3D())
                .AddComponent(new BoxCollider())
                .AddComponent(new SkinnedModel("Content/isis.wpk"))
                .AddComponent(new MaterialsMap(new BasicMaterial("Content/isis-difuse.wpk") { ReferenceAlpha = 0.5f }))
                .AddComponent(new Animation3D("Content/isis-animations.wpk"))
                .AddComponent(new SkinnedModelRenderer())
                .AddComponent(new TimBehavior());

            anim = animatedModel.FindComponent<Animation3D>();
            EntityManager.Add(animatedModel);

            
            Entity floor = new Entity("Floor")
                       .AddComponent(new Transform3D())
                       .AddComponent(new BoxCollider())
                       .AddComponent(new Model("Content/Model/floor.wpk"))
                       .AddComponent(new MaterialsMap(new BasicMaterial("Content/Texture/floorNight.wpk")))
                       .AddComponent(new ModelRenderer());

            //Suddenly columns! Thousand of them!  (4x2 columns)
          
            EntityManager.Add(floor);

            //Suddenly columns! Thousand of them!  (4x2 columns)
            /*int nColumns = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Entity column = new Entity("column_" + nColumns++)
                        .AddComponent(new Transform3D() { Position = new Vector3(4 - (8 * j), 0, 8 * i) })
                       .AddComponent(new Model("Content/Model/floor.wpk"))
                        .AddComponent(new ModelRenderer())
                       .AddComponent(new MaterialsMap(new BasicMaterial("Content/Texture/floorNight.wpk")));

                    EntityManager.Add(column);
                    Entity fern = new Entity("Fern" + nColumns++)
                             .AddComponent(new Transform3D() { Position = new Vector3(2f - (8 * j), 0f, 2.5f*i) })
                             .AddComponent(new BoxCollider())
                             .AddComponent(new Model("Content/Model/fern.wpk"))
                             .AddComponent(new MaterialsMap(new BasicMaterial("Content/Texture/FernTexture.wpk") { ReferenceAlpha = 0.5f }))
                             .AddComponent(new ModelRenderer());

                    EntityManager.Add(fern);
                }
            }*/


            camera.Entity.AddComponent(new CameraBehavior(EntityManager.Find("Isis")));

            //Add some light!
            PointLight light = new PointLight("light", Vector3.Zero)
            {
                Attenuation = 75,
                Color = new Color(1, 0.6f, 0.4f),
                IsVisible = true
            };
            light.Entity.AddComponent(new FollowCameraBehavior(camera.Entity));
            light.Entity.AddComponent(new TorchLightBehaviour());


            EntityManager.Add(light);
            EntityManager.Add(camera);
            RenderManager.SetActiveCamera(camera.Entity);
        }

        protected override void Start()
        {
            base.Start();

            anim.PlayAnimation("Idle", true);
            //Walk
            //Idle
        }
    }
}
