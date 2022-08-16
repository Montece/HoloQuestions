using System;
using System.Diagnostics;

namespace HoloQuestions
{
    public static class FFMpeg
    {
        public static string FFMpegPath = "ffmpeg.exe";

        public static void StartFFMPEG(string parameter)
        {
            bool ended = false;

            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo.FileName = FFMpegPath;
                    process.StartInfo.Arguments = parameter;
                    process.StartInfo.CreateNoWindow = true;
                    process.EnableRaisingEvents = true;
                    process.Exited += new EventHandler((object sender, EventArgs e) => { ended = true; });
                    process.Start();
                }
                catch
                {
                    ended = true;
                }

                while (!ended) { }
            }
        }
    }
}
