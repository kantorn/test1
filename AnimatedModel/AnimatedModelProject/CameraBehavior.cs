using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;


namespace AnimatedModelProject
{
    //Class behavior
    public class CameraBehavior : Behavior
    {
        #region Variables
        [RequiredComponent]
        public Camera camera;

        private Entity followEntity;
        public static Vector3 positionOffset;
        private Vector3 lookatOffset;
        public static Vector3 followLocation;
        public static Vector3 cameraPosition;
        public static float angle = 0;

      
        #endregion

        #region Initialize
        public CameraBehavior(Entity followEntity)
            : base("CameraBehavior")
        {
            this.followEntity = followEntity;

        }
        #endregion

        #region Public Methods
        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            positionOffset = camera.Position - camera.LookAt;
            this.lookatOffset = camera.LookAt;
        }

        protected override void Update(TimeSpan gameTime)
        {
            Transform3D trans2D = followEntity.FindComponent<Transform3D>();
            int iRevert = 1;
            
            Vector3 buffer = positionOffset;
            float marginGap = positionOffset.Z;
            camera.LookAt = trans2D.Position + this.lookatOffset;
            double cos, sin;
            cos = Math.Cos(trans2D.Rotation.Y);
            sin = Math.Sin(trans2D.Rotation.Y);

            buffer.X = marginGap * iRevert * (float)sin;
            buffer.Z = marginGap * iRevert * (float)cos;
            
            camera.Position = camera.LookAt + buffer;

        }


        #endregion
    }
}
