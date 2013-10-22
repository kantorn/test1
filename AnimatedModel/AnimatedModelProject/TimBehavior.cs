using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace AnimatedModelProject
{
    class TimBehavior : Behavior
    {
        private const int SPEED = 1;
        private const double UP =  0.1;
        private const double DOWN = -0.1;
        private const double LEFT = 0.1;
        private const double RIGHT = -0.1;
        private const int NONE = 0;
        private const int BORDER_OFFSET = 0;
        private Vector3 CENTER = new Vector3(35, 100, 545);

        [RequiredComponent]
        public Animation3D anim2D;
        [RequiredComponent]
        public Transform3D trans2D;


        /// <summary>
        /// 1 or -1 indicating right or left respectively
        /// </summary>
        private double direction;
        string[] animations = { "Jog", "Walk", "Idle", };


        public TimBehavior()
            : base("TimBehavior")
        {
            this.direction = NONE;
            this.anim2D = null;
            this.trans2D = null;
            MyScene.currentState = MyScene.AnimState.Idle;
        }

        protected override void Update(TimeSpan gameTime)
        {
            MyScene.currentState = MyScene.AnimState.Idle;

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Up == ButtonState.Pressed)
            {
                MyScene.currentState = MyScene.AnimState.Up;
            }
            else if (keyboard.Down == ButtonState.Pressed)
            {
                MyScene.currentState = MyScene.AnimState.Down;
            }
            else if (keyboard.Left == ButtonState.Pressed)
            {
                MyScene.currentState = MyScene.AnimState.Left;
            }
            else if (keyboard.Right == ButtonState.Pressed)
            {
                MyScene.currentState = MyScene.AnimState.Right;
            }

            // Set current animation if that one is diferent
            if (MyScene.currentState != MyScene.lastState)
            {
                switch (MyScene.currentState)
                {
                    case MyScene.AnimState.Idle:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.PlayAnimation("Idle", true);
                        direction = NONE;
                        break;
                    case MyScene.AnimState.Up:
                        anim2D.CurrentAnimation = "Jog";
                        //trans2D.Effect = SpriteEffects.None;
                        anim2D.PlayAnimation("Jog", true);
                        direction = UP;
                        break;
                    case MyScene.AnimState.Down:
                        anim2D.CurrentAnimation = "Jog";
                        //trans2D.Effect = SpriteEffects.FlipHorizontally;
                        anim2D.PlayAnimation("Jog", true);
                        direction = DOWN;
                        break;
                    case MyScene.AnimState.Left:
                        anim2D.CurrentAnimation = "Idle";
                        //trans2D.Effect = SpriteEffects.FlipHorizontally;
                        anim2D.PlayAnimation("Idle", true);
                        direction = LEFT;
                        break;
                    case MyScene.AnimState.Right:
                        anim2D.CurrentAnimation = "Idle";
                        //trans2D.Effect = SpriteEffects.FlipHorizontally;
                        anim2D.PlayAnimation("Idle", true);
                        direction = RIGHT;
                        break;
                    default:
                        anim2D.PlayAnimation("Idle", true);
                        break;
                }
            }


            Vector3 rotation = trans2D.Rotation;
            if ((MyScene.currentState == MyScene.AnimState.Up) || (MyScene.currentState == MyScene.AnimState.Down))
            {
                // Move sprite
                float angle = 0f;
                angle = Math.Abs((360 / MathHelper.TwoPi) * (-trans2D.Rotation.Y));
                
                double cos, sin;
                cos = Math.Cos(trans2D.Rotation.Y);
                sin = Math.Sin(trans2D.Rotation.Y);

                trans2D.Position.X += (float)sin * (float)direction * (SPEED) * (gameTime.Milliseconds / 10)  ;
                trans2D.Position.Z += (float)cos * (float)direction * (SPEED) * (gameTime.Milliseconds / 10) ;
            }
            else if ((MyScene.currentState == MyScene.AnimState.Left) || (MyScene.currentState == MyScene.AnimState.Right))
            {
                float angle = 0f;
                switch (MyScene.currentState)
                {

                    case MyScene.AnimState.Left:
                        angle += (MathHelper.TwoPi / 360) * (gameTime.Milliseconds / 10);
                        break;
                    case MyScene.AnimState.Right:

                        angle -= (MathHelper.TwoPi / 360) * (gameTime.Milliseconds / 10);
                        break;
                }

                //roatation must be in 2Pi
                rotation.Y += angle;
                if (rotation.Y > MathHelper.TwoPi)
                {
                    rotation.Y -= MathHelper.TwoPi;
                }
                else if (rotation.Y < -MathHelper.TwoPi)
                {
                    rotation.Y += MathHelper.TwoPi;
                }
                trans2D.Rotation = rotation;
            }

            MyScene.lastState = MyScene.currentState;
        }
    }
}
