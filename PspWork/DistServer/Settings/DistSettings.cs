using Common.Settings;

namespace DistServer.Settings
{
    public class DistSettings
    {
        public SingleSettings ServerSettings { get; set; }

        public List<SingleSettings> SolversSettings { get; set; }
    }
}
