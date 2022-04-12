namespace HoloQuestions.Managers
{
    public static class AudioManager
    {
        public static void ExtractFromVideo(string videopath_in, string audiopath_out)
        {
            FFMpeg.StartFFMPEG($"-i {videopath_in} {audiopath_out}");
        }
    }
}
