using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Holo
{
    public static class FFMpeg
    {
        const string PATH = "ffmpeg.exe";

        public static void Split(string filename1, string filename2, int length)
        {
            StartFFMPEG($"-i {filename1} -ss 0 -t {length} {filename2}");
        }

        public static void ExtractAudio(string filename1, string filename2)
        {
            StartFFMPEG($"-i {filename1} {filename2}");
        }

        private static void StartFFMPEG(string parameter)
        {
            bool ended = false;

            using (Process proc = new Process())
            {
                try
                {
                    proc.StartInfo.FileName = PATH;
                    proc.StartInfo.Arguments = parameter;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.EnableRaisingEvents = true;
                    proc.Exited += new EventHandler((object sender, EventArgs e) => { ended = true; });
                    proc.Start();
                }
                catch (Exception x)
                {
                    ended = true;
                }

                while (!ended) { }
            }
        }
    }
}
