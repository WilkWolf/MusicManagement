using System;
using System.IO;
using System.Threading;
using System.Windows;



namespace MusicManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private string _folderName = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetFoldersContent_ButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();

            var result = openFileDlg.ShowDialog();

            if (result.ToString() != string.Empty)
            {
                try
                {
                    _folderName = openFileDlg.SelectedPath;
                }
                catch
                {
                    Console.WriteLine("No directory selected");
                }
            }
            DisplaySongFromFolder();
        }

        private void DisplaySongFromFolder()
        {
            musicListView.Items.Clear();
            try
            {
                string[] music = Directory.GetFiles(_folderName, "*.mp3");

                foreach (var item in music)
                {
                    musicListView.Items.Add(item.Replace($"{_folderName}\\", ""));
                }
                InformationLabel.Content = $"Songs loaded";

            }
            catch
            {
                InformationLabel.Content = "Problem with loaded songs. Please be sure that you selected folder";
            }
        }

        private void ClearListView_ButtonClick(object sender, RoutedEventArgs e)
        {
            musicListView.Items.Clear();
            InformationLabel.Content = $"Songs list cleared";
        }

        private void ClearTitleTextBox_ButtonClick(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Clear();
            InformationLabel.Content = $"Song title textbox cleared";
        }

        private void ClearAuthorTextBoxButtonClick(object sender, RoutedEventArgs e)
        {
            AuthorTextBox.Clear();
            InformationLabel.Content = $"Author textbox cleared";
        }

        private void ClearAlbumTextBoxButtonClick(object sender, RoutedEventArgs e)
        {
            AlbumTextBox.Clear();
            InformationLabel.Content = $"Album textbox cleared";
        }

        private void ClearFormBoxButtonClick(object sender, RoutedEventArgs e)
        {
            ClearForm();
            InformationLabel.Content = $"Form cleared";
        }

        private void ClearFileNameTextBox_ButtonClick(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.Clear();
            InformationLabel.Content = $"Album textbox cleared";
        }
        private void ClearForm()
        {
            TitleTextBox.Clear();
            FileNameTextBox.Clear();
            AuthorTextBox.Clear();
            AlbumTextBox.Clear();
        }

        private void ChangeSongFileName_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.SelectedItem == null)
                {
                    InformationLabel.Content = "Non song was selected.";
                }
                else
                {
                    string selectedSong = musicListView.SelectedItem.ToString();

                    string newName = FileNameTextBox.Text;
                    if (newName == "")
                    {
                        InformationLabel.Content = "New name cannot be empty";
                    }
                    else
                    {
                        string extension = Path.GetExtension(GetFileWithPath(selectedSong));
                        newName += extension;
                        File.Move(GetFileWithPath(selectedSong), GetFileWithPath(newName));
                        DisplaySongFromFolder();
                        InformationLabel.Content = $"Changed name: {selectedSong} => {newName}";
                    }
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }

        private void ChangeSongTitle_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.SelectedItem == null)
                {
                    InformationLabel.Content = "Non song was selected.";
                }
                else
                {
                    foreach (var item in musicListView.SelectedItems)
                    {
                        var musicFileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));
                        musicFileTags.Tag.Title = TitleTextBox.Text;
                        musicFileTags.Save();
                    }

                    DisplaySongFromFolder();
                    InformationLabel.Content = $"Selected songs title updated";
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }

        private void ChangeAuthorForSelectedSongs_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.SelectedItem == null)
                {
                    InformationLabel.Content = "Non song was selected.";
                }
                else
                {
                    foreach (var item in musicListView.SelectedItems)
                    {
                        var musicFileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));
                        musicFileTags.Tag.AlbumArtists = new string[] { AuthorTextBox.Text };
                        musicFileTags.Save();
                    }

                    DisplaySongFromFolder();
                    InformationLabel.Content = $"Selected songs author updated";
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }

        private void ChangeAlbumForSelectedSongs_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.SelectedItem == null)
                {
                    InformationLabel.Content = "Non song was selected.";
                }
                else
                {
                    foreach (var item in musicListView.SelectedItems)
                    {
                        var musicFileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));
                        musicFileTags.Tag.Album = AlbumTextBox.Text;
                        musicFileTags.Save();
                    }

                    DisplaySongFromFolder();
                    InformationLabel.Content = $"Selected songs album updated";
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }

        private void ChangeAuthorForAllSongs_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.Items.Count == 0)
                {
                    InformationLabel.Content = "Non song on list.";
                }
                else
                {
                    foreach (var item in musicListView.Items)
                    {
                        var musicFileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));
                        musicFileTags.Tag.AlbumArtists = new string[] { AuthorTextBox.Text };
                        musicFileTags.Save();
                    }

                    DisplaySongFromFolder();
                    InformationLabel.Content = $"All songs author updated";
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }

        private void Unselect_ButtonClick(object sender, RoutedEventArgs e)
        {
            musicListView.UnselectAll();
            ClearForm();
            InformationLabel.Content = "Songs unselected";
        }

        private string GetFileWithPath(string songName)
        {
            return _folderName + "\\" + songName;
        }

        private void MusicListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (musicListView.SelectedItems.Count == 1)
            {
                string selectedItem = musicListView.SelectedItem.ToString();
                var selectedFile = TagLib.File.Create(GetFileWithPath(selectedItem));
                TitleTextBox.Text = selectedFile.Tag.Title;
                AuthorTextBox.Text = selectedFile.Tag.FirstAlbumArtist;
                AlbumTextBox.Text = selectedFile.Tag.Album;
                SetFileNameButton.IsEnabled = true;
                SetTitleButton.IsEnabled = true;
                string extension = Path.GetExtension(GetFileWithPath(selectedItem));
                FileNameTextBox.Text = selectedItem.Replace(extension, "");
            }
            else if (musicListView.SelectedItems.Count > 1)
            {
                var selectedItem = musicListView.SelectedItems;
                int i = 0;
                bool isAlbumTheSame = true;
                bool isAuthorTheSame = true;
                string author = "";
                string album = "";
                SetFileNameButton.IsEnabled = false;
                SetTitleButton.IsEnabled = false;

                foreach (var item in selectedItem)
                {
                    var fileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));

                    if (i == 0)
                    {
                        album = fileTags.Tag.Album;
                        author = fileTags.Tag.FirstAlbumArtist;

                    }
                    else
                    {
                        if (album != fileTags.Tag.Album)
                        {
                            isAlbumTheSame = false;
                        }
                        if (author != fileTags.Tag.FirstAlbumArtist)
                        {
                            isAuthorTheSame = false;
                        }
                    }
                    i++;
                }

                AlbumTextBox.Text = isAlbumTheSame ? album : null;
                AuthorTextBox.Text = isAuthorTheSame ? author : null;
            }
        }

        private void ChangeAlbumForAllSongs_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.Items.Count == 0)
                {
                    InformationLabel.Content = "Non song on list.";
                }
                else
                {
                    foreach (var item in musicListView.Items)
                    {
                        var musicFileTags = TagLib.File.Create(GetFileWithPath(item.ToString()));
                        musicFileTags.Tag.Album = AlbumTextBox.Text;
                        musicFileTags.Save();
                    }

                    DisplaySongFromFolder();
                    InformationLabel.Content = $"All songs albums updated";
                }
            }
            catch (Exception exc)
            {
                InformationLabel.Content = $"Error: {exc.Message}";
            }
        }
    }
}
