using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MusicManagement.Model;

namespace MusicManagement
{
    public partial class MainWindow : Window
    {
        private void ChangeSongProperty_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                if (musicListView.SelectedItem == null && button.Name.ToString().Contains("ToAllSongs") == false)
                {
                    InformationTextBox.Text = MusicManagement.Resources.Resources.NoSongsSelected;
                }
                else
                {
                    ChangeCorrectProperty((Button)sender);
                    DisplaySongFromFolder();
                }
            }
            catch (Exception exc)
            {
                InformationTextBox.Text = $"Error: {exc.Message}";
            }
        }

        private void ChangeCorrectProperty(Button applyButton)
        {
            switch (applyButton.Name)
            {
                case "SetFileNameButton":
                    ChangeFileNameForSong();
                    break;
                case "SetTitleButton":
                    ChangeTitleForSelectedSongs();
                    break;
                case "AuthorSelectedSongButton":
                    ChangeAuthorForSelectedSongs();
                    break;
                case "NumberSelectedSongButton":
                    break;
                case "AlbumSelectedSongButton":
                    ChangeAlbumForSelectedSongs();
                    break;
                case "ApplyAlbumToAllSongs":
                    ChangeAlbumForAllSongs();
                    break;
                case "ApplyAuthorToAllSongs":
                    ChangeAuthorForAllSongs();
                    break;
                default:
                    break;
            }
        }

        private void ChangeFileNameForSong()
        {
            var selectedSong = (Song)musicListView.SelectedItem;

            string newName = FileNameTextBox.Text;
            if (newName == "")
            {
                InformationTextBox.Text = MusicManagement.Resources.Resources.EmptyName;
            }
            else
            {
                string extension = Path.GetExtension(GetFileWithPath(selectedSong.FileName));
                newName += extension;
                File.Move(GetFileWithPath(selectedSong.FileName), GetFileWithPath(newName));
                InformationTextBox.Text = $"{MusicManagement.Resources.Resources.ChangedName} {selectedSong} => {newName}";
            }
        }

        private void ChangeTitleForSelectedSongs()
        {
            foreach (Song item in musicListView.SelectedItems)
            {
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(item.FileName));
                musicFileTags.Tag.Title = TitleTextBox.Text;
                musicFileTags.Save();
                musicFileTags.Dispose();
            }
            InformationTextBox.Text = MusicManagement.Resources.Resources.TitleUpdated;
        }

        private void ChangeAuthorForSelectedSongs()
        {
            foreach (Song item in musicListView.SelectedItems)
            {
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(item.FileName));
                musicFileTags.Tag.AlbumArtists = new string[] { AuthorTextBox.Text };
                musicFileTags.Save();
                musicFileTags.Dispose();
            }
            InformationTextBox.Text = MusicManagement.Resources.Resources.AuthorUpdated;
        }

        private void ChangeAlbumForSelectedSongs()
        {
            foreach (Song item in musicListView.SelectedItems)
            {
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(item.FileName));
                musicFileTags.Tag.Album = AlbumTextBox.Text;
                musicFileTags.Save();
                musicFileTags.Dispose();
            }
            InformationTextBox.Text = MusicManagement.Resources.Resources.AlbumUpdated;
        }

        private void ChangeAuthorForAllSongs()
        {
            foreach (Song item in musicListView.Items)
            {
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(item.FileName));
                musicFileTags.Tag.AlbumArtists = new string[] { AuthorTextBox.Text };
                musicFileTags.Save();
                musicFileTags.Dispose();
            }
            InformationTextBox.Text = MusicManagement.Resources.Resources.AllSongsAuthorUpdated;
        }
        private void ChangeAlbumForAllSongs()
        {
            foreach (Song item in musicListView.Items)
            {
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(item.FileName));
                musicFileTags.Tag.Album = AlbumTextBox.Text;
                musicFileTags.Save();
                musicFileTags.Dispose();
            }
            InformationTextBox.Text = MusicManagement.Resources.Resources.AllSongsAlbumUpdated;
        }
    }
}
