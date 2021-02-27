using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using MusicManagement.Model;

namespace MusicManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string LangSwitch { get; private set; } = null;

        private string _folderName = "";

        public MainWindow() => InitializeComponent();

        private string GetFileWithPath(string songName) => _folderName + "\\" + songName;

        private void ChangeLanguage_ButtonClick(object sender, RoutedEventArgs e)
        {
            switch (LanguageButton.Content)
            {
                case "PL":
                    LangSwitch = "en-EN";
                    Close();
                    InformationTextBox.Text = MusicManagement.Resources.Resources.ChangedLanguage;
                    break;
                case "EN":
                    LangSwitch = "pl-PL";
                    Close();
                    InformationTextBox.Text = MusicManagement.Resources.Resources.ChangedLanguage;
                    break;
                default:
                    break;
            }
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
                catch (Exception exc)
                {
                    InformationTextBox.Text = MusicManagement.Resources.Resources.NoDictionarySelected;
                }
            }
            DisplaySongFromFolder();
            InformationTextBox.Text = MusicManagement.Resources.Resources.SongsLoaded;
        }

        private void DisplaySongFromFolder()
        {
            musicListView.Items.Clear();
            try
            {
                string[] music = Directory.GetFiles(_folderName, "*.mp3");

                foreach (var item in music)
                {
                    Song song = new Song(item);
                    musicListView.Items.Add(song);
                }

            }
            catch (Exception exc)
            {
                InformationTextBox.Text = MusicManagement.Resources.Resources.ProblemWithLoadedSongs;
            }
        }

        private void ClearListView_ButtonClick(object sender, RoutedEventArgs e)
        {
            musicListView.Items.Clear();
            InformationTextBox.Text = MusicManagement.Resources.Resources.ClearList;
        }

        private void ClearTextBoxFieldByClearButton_ButtonClick(object sender, RoutedEventArgs e)
        {
            TextBox textBox = SelectCorrectTextBoxByClearButton((Button)sender);
            textBox.Clear();
        }

        private TextBox SelectCorrectTextBoxByClearButton(Button clearButton)
        {
            switch (clearButton.Name)
            {
                case "FileNameClearButton":
                    InformationTextBox.Text = MusicManagement.Resources.Resources.AlbumTextBoxCleared;
                    return FileNameTextBox;
                case "TitleClearButton":
                    InformationTextBox.Text = MusicManagement.Resources.Resources.TitleTextBoxCleared;
                    return TitleTextBox;
                case "AuthorClearButton":
                    InformationTextBox.Text = MusicManagement.Resources.Resources.AuthorTextBoxCleared;
                    return AuthorTextBox;
                case "AlbumClearButton":
                    InformationTextBox.Text = MusicManagement.Resources.Resources.AlbumTextBoxCleared;
                    return AlbumTextBox;
                case "NumberClearButton":
                    return NumberTextBox;
                default:
                    return null;
            }
        }

        private void ClearFormBoxButtonClick(object sender, RoutedEventArgs e)
        {
            ClearForm();
            InformationTextBox.Text = MusicManagement.Resources.Resources.FormTextBoxCleared;
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            FileNameTextBox.Clear();
            AuthorTextBox.Clear();
            AlbumTextBox.Clear();
        }

        private void Unselect_ButtonClick(object sender, RoutedEventArgs e)
        {
            musicListView.UnselectAll();
            ClearForm();
            InformationTextBox.Text = MusicManagement.Resources.Resources.SongsUnselected;
        }

        private void DroppedFiles(object sender, DragEventArgs dragEvent)
        {
            try
            {
                musicListView.Items.Clear();

                var files = (string[])dragEvent.Data.GetData(DataFormats.FileDrop);

                foreach (string droppedFile in files)
                {
                    if (Directory.Exists(droppedFile))
                    {
                        foreach (var file in Directory.GetFiles(droppedFile, "*.*", SearchOption.TopDirectoryOnly))
                        {
                            Song song = new Song(file);
                            musicListView.Items.Add(song);
                        }
                        _folderName = droppedFile;
                    }
                    else if (File.Exists(droppedFile))
                    {
                        var file = Path.GetFileName(droppedFile);

                        Song song = new Song(droppedFile);
                        musicListView.Items.Add(song);

                        _folderName = Path.GetDirectoryName(droppedFile).Replace(file, "");
                    }
                }
            }
            catch (Exception exc)
            {
                InformationTextBox.Text = exc.Message;
            }
        }

        private void MusicListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (musicListView.SelectedItems.Count == 1)
            {
                SetFormFieldsForOneSong();
            }
            else if (musicListView.SelectedItems.Count > 1)
            {
                UpdateLayoutWhenMoreSongsSelected();
                SetFromFieldsFromSelectedSongs();
            }
        }

        private void SetFromFieldsFromSelectedSongs()
        {
            var selectedSongs = musicListView.SelectedItems;
            bool isAlbumTheSame = true;
            bool isAuthorTheSame = true;
            string author = "";
            string album = "";


            foreach (Song song in selectedSongs)
            {
                var fileTags = TagLib.File.Create(GetFileWithPath(song.FileName));

                if (selectedSongs.Count == 0)
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
            }

            AlbumTextBox.Text = isAlbumTheSame ? album : null;
            AuthorTextBox.Text = isAuthorTheSame ? author : null;
        }

        private void SetFormFieldsForOneSong()
        {
            Song selectedSong = (Song)musicListView.SelectedItem;

            TitleTextBox.Text = selectedSong.Title;
            AuthorTextBox.Text = selectedSong.Artist;
            AlbumTextBox.Text = selectedSong.Album;
            string extension = Path.GetExtension(GetFileWithPath(selectedSong.FileName));
            FileNameTextBox.Text = selectedSong.FileName.Replace(extension, "");
            ChangeButtonEnableProperty(true);
        }
        private void UpdateLayoutWhenMoreSongsSelected()
        {
            TimerSongCurrentLabel.Content = 0;
            TimerSongLenghtLabel.Content = 0;
            ChangeButtonEnableProperty(false);
        }

        private void ChangeButtonEnableProperty(Boolean isActive)
        {
            FileNameApplyButton.IsEnabled = isActive;
            TitleApplyButton.IsEnabled = isActive;
            PlayButton.IsEnabled = isActive;
            StopButton.IsEnabled = isActive;
        }

        private void ClickApplyButtonFromTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Button applyButton = SelectCorrectApplyButtonByTextBox((TextBox)sender);
            PressApplyButtonByEnter(applyButton, e);
        }

        private Button SelectCorrectApplyButtonByTextBox(TextBox textBox)
        {
            return textBox.Name switch
            {
                "FileNameTextBox" => FileNameApplyButton,
                "TitleTextBox" => TitleApplyButton,
                "AuthorTextBox" => AuthorApplyButton,
                "AlbumTextBox" => AlbumApplyButton,
                "NumberTextBox" => NumberApplyButton,
                _ => null,
            };
        }

        private void SetHighlightToApplyButtonFromTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Button applyButton = SelectCorrectApplyButtonByTextBox((TextBox)sender);
            SetApplyButtonHighlightWhenEnterHolded(applyButton, e);
        }

        private static void SetApplyButtonHighlightWhenEnterHolded(Button button, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { true });
            }
            else
            {
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { false });
            }
        }

        private static void PressApplyButtonByEnter(Button button, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && button.IsPressed)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { false });
            }
        }

        private void AuthorApplyToAllTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;
            double desiredHeight = 21;

            if (textBlock.ActualHeight > desiredHeight)
            {
                double fontSizeMultiplier = Math.Sqrt(desiredHeight / textBlock.ActualHeight);
                textBlock.FontSize *= fontSizeMultiplier;
            }
            textBlock.Height = desiredHeight;
        }
    }
}
