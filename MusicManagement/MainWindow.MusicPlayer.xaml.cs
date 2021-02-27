using System;
using System.Windows;
using System.Windows.Threading;
using MusicManagement.Model;
using NAudio.Wave;

namespace MusicManagement
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private readonly WaveOut _waveOut = new WaveOut();
        private Mp3FileReader _reader;

        private void PlaySelectedSong_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musicListView.Items.Count == 0 || musicListView.SelectedItem == null)
                {
                    InformationTextBox.Text = MusicManagement.Resources.Resources.NoSongsSelected;
                }
                else
                {
                    Song song = (Song)musicListView.SelectedItem;

                    _reader = new Mp3FileReader(GetFileWithPath(song.FileName));
                    _waveOut.Init(_reader);
                    TimeSpan songDuration = _reader.TotalTime;

                    TimerSongCurrentLabel.Content = 0;
                    SongProgressBar.Maximum = Convert.ToInt64(songDuration.TotalSeconds);
                    TimerSongLenghtLabel.Content = $"{SongProgressBar.Maximum}s";
                    DispatcherProgressBar();
                    _waveOut.Play();
                }
            }
            catch (Exception exc)
            {
                InformationTextBox.Text = exc.Message;
            }
        }

        private void DispatcherProgressBar()
        {
            _dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            SongProgressBar.Value += 1;
            TimerSongCurrentLabel.Content = SongProgressBar.Value;
            if (SongProgressBar.Value == SongProgressBar.Maximum)
            {
                StopSong();
            }
        }

        private void StopSelectedSong_ButtonClick(object sender, RoutedEventArgs e)
        {
            SongProgressBar.Value = 0;
            StopSong();
        }

        private void StopSong()
        {
            try
            {
                _dispatcherTimer.Stop();
                _waveOut.Stop();
                _waveOut.Dispose();
                if (_reader != null)
                    _reader.Dispose();
            }
            catch (Exception exc)
            {
                InformationTextBox.Text = exc.Message;
            }
        }
    }
}
