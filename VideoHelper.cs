using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//----------------------------------
using System.Web.Configuration;


namespace JimRadioMaint
{
    public class VideoHelper
    {
        // Take YouTube video durations, which are the count of seconds, and display them in
        // MM:SS format:
        public static string DurationMinutesSeconds(int duration_seconds)
        {
            return (duration_seconds / 60).ToString() +
                   ":" +
                   (duration_seconds % 60).ToString("00");
        }

        // Combine the constant prefix with the variable, resulting in a YouTube video URL:
        public static string VideoPlayUrl(string youtube_id)
        {
            return WebConfigurationManager.AppSettings["YOUTUBE_VIDEO_PLAY_URL_PREFIX"] + youtube_id;
        }
    }
}