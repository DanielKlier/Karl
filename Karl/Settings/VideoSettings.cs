using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Karl.Settings
{
    public class VideoSettings : GameSettings
    {
        protected override string SettingsName
        {
            get { return "Video"; }
        }

        public VideoSettings()
        {
            Register("Window.Mode", "windowed", Validators.String);
            Register("Window.AspectRatio", "16:9", Validators.String);
            Register("Window.Width", 1024, Validators.Int);
            Register("Window.Height", 768, Validators.Int);
            Register("Device.VSync", true, Validators.Bool);
        }
    }
}
