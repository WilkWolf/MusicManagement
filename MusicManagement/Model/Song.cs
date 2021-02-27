using System;
using System.IO;
using System.Linq;
using NAudio.Wave;

namespace MusicManagement.Model
{
    class Song
    {
        public Song(string filePath)
        {
            var file = TagLib.File.Create(filePath);

            FileName = filePath.Replace(Path.GetDirectoryName(filePath) + "\\", "");
            Title = file.Tag.Title;
            Album = file.Tag.Album;
            Number = file.Tag.Track;
            if (file.Tag.AlbumArtists.ElementAtOrDefault(0) != null)
            {
                Artist = file.Tag.AlbumArtists[0];
            }

            Mp3FileReader reader = new Mp3FileReader(filePath);
            Duration = reader.TotalTime;
            reader.Close();
            file.Dispose();
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public uint Number { get; set; }
    }
}
