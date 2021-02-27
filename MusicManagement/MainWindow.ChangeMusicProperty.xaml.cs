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
                case "FileNameApplyButton":
                    ChangeFileNameForSong();
                    break;
                case "TitleApplyButton":
                    ChangeTitleForSelectedSongs();
                    break;
                case "AuthorApplyButton":
                    ChangeAuthorForSelectedSongs();
                    break;
                case "NumberApplyButton":
                    ChangeNumberForSelectedSongs();
                    break;
                case "AlbumApplyButton":
                    ChangeAlbumForSelectedSongs();
                    break;
                case "AlbumApplyToAllButton":
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
            string extension = Path.GetExtension(GetFileWithPath(selectedSong.FileName));
            string oldName = selectedSong.FileName.Replace(extension, "");

            string newName = FileNameTextBox.Text;
            if (newName == "")
            {
                InformationTextBox.Text = MusicManagement.Resources.Resources.EmptyName;
            }
            else if (newName == oldName)
            {
                InformationTextBox.Text = MusicManagement.Resources.Resources.ValueTheSame;
            }
            else
            {
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

        private void ChangeNumberForSelectedSongs()
        {
            try
            {
                var selectedSong = (Song)musicListView.SelectedItem;
                TagLib.File musicFileTags = TagLib.File.Create(GetFileWithPath(selectedSong.FileName));
                musicFileTags.Tag.Track = Convert.ToUInt32(NumberTextBox.Text);
                musicFileTags.Save();
                musicFileTags.Dispose();
                InformationTextBox.Text = MusicManagement.Resources.Resources.NumberChanged;
            }
            catch (Exception exc)
            {
                InformationTextBox.Text = exc.Message;
            }
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
