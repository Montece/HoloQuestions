namespace HoloQuestions.Managers
{
    public static class VideoManager
    {
        public static void Cut(string videopath_in, string videopath_out, int length)
        {
            FFMpeg.StartFFMPEG($"-i {videopath_in} -ss 0 -t {length} {videopath_out}");
        }
    }
}
