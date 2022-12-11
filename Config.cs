using System.Collections.Generic;

namespace OmegaWarhead
{
    public class Config
    {
        public float MinutesTillOmegaWarhead { get; set; } = 25;

        public string OmegaWarheadAnnouncement { get; set; } = "Omega Warhead is initiated";

        public ushort OmegaWarheadAnnouncementDuration { get; set; } = 5;

        public string OmegaWarheadDeathReason { get; set; } = "Killed by Omaga Warhead";
    }
}
