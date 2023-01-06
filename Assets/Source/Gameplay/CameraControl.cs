using Events.Tick;
using GameEngine.GameSettings;
using Utils.Dots;
using Utils.Events;

namespace Gameplay
{
    public class CameraControl
    {
        private float _speed = 4;

        public CameraControl()
        {
            this.RegisterListener();
        }

        [EventHandler]
        private void OnFixedTick(AsyncFixedTickEvent e)
        {
            var dir = Dot2.Zero;
            if (Settings.Binds.CameraUp.IsPressed()) dir.Y = 1;
            else if (Settings.Binds.CameraDown.IsPressed()) dir.Y = -1;
            if (Settings.Binds.CameraLeft.IsPressed()) dir.X = 1;
            else if (Settings.Binds.CameraRight.IsPressed()) dir.X = -1;
            if (Settings.Binds.CameraBoost.IsPressed()) dir *= 2;
            //Undefined.Camera.Transform.Position += dir*e.DeltaTime*_speed;
        }
    }
}