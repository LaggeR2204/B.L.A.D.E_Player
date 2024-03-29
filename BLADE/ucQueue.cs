﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLADE
{
    public partial class ucQueue : UserControl
    {
        private Song _nowPlayingSong;
        public Song NowPlayingSong
        {
            get => _nowPlayingSong;
            set
            {
                if (_nowPlayingSong != value)
                {
                    _nowPlayingSong = value;
                    if (_nowPlayingSong != null)
                        _nowPlayingSong.FavoriteChanged += _nowPlayingSong_FavoriteChanged;
                    SetSongInfo();
                    //if (NowPlayingSongChanged != null)
                    //    NowPlayingSongChanged(this, new EventArgs());
                }
            }
        }
        //public event EventHandler NowPlayingSongChanged;
        public event EventHandler SongSelected;
        public event EventHandler SongRemoved;
        public event EventHandler SongControled;
        public ucQueue()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _nowPlayingSong = null;
            SetFavoriteState(_nowPlayingSong);
        }
        public void SetFavoriteState(Song src)
        {
            if (src == null)
            {
                btnSongLoveOff.Hide();
                btnSongLove.Hide();
            }
            else
            if (src.IsFavorite)
            {
                btnSongLoveOff.Show();
                btnSongLove.Hide();
            }
            else
            {
                btnSongLove.Show();
                btnSongLoveOff.Hide();
            }
        }
        public void SetSongInfo()
        {
            if(_nowPlayingSong==null)
            {
                picbArtCover.Image = null;
                labelRunSongName.lbText.Text = "";
                lbSongSinger.Text ="";
                SetFavoriteState(_nowPlayingSong);
                return;
            }
            picbArtCover.Image = _nowPlayingSong.SongImage;
            labelRunSongName.lbText.Text = _nowPlayingSong.SongName;
            lbSongSinger.Text = _nowPlayingSong.Singer;
            SetFavoriteState(_nowPlayingSong);
            labelRunSongName.ResetLabelLocation();
        }
        public void UpdateQueue(List<Song> src)
        {
            foreach (SongItemInQueue ctrl in fpnlPlayback.Controls)
            {
                ctrl.DisposeSongEvent();
            }
            fpnlPlayback.Controls.Clear();
            for (int i = 0; i < src.Count; i++)
            {
                ShowSongQueue(src[i]);
            }
        }
        public void UpdateQueue(Song src)
        {
            ShowSongQueue(src);
        }
        public void ShowSongQueue(Song song)
        {
            SongItemInQueue songItem = new SongItemInQueue(song);
            songItem.SelectedSong += SongSelected_Handler;
            songItem.SongRemoved += SongRemoved_Handler;
            songItem.SongControled += SongItem_SongControled;
            this.fpnlPlayback.Controls.Add(songItem);
            songItem.Width = panelContainFPNL.Width;
        }
        public void RemoveSongItem(Song src)
        {
            foreach (SongItemInQueue item in fpnlPlayback.Controls)
            {
                if (item.Song == src)
                {
                    fpnlPlayback.Controls.Remove(item);
                    return;
                }
            }
        }
        #region Event
        private void SongSelected_Handler(object sender, EventArgs e)
        {
            if (SongSelected != null)
                SongSelected(sender, e);
        }
        private void SongRemoved_Handler(object sender, EventArgs e)
        {
            SongItemInQueue src = sender as SongItemInQueue;
            if (_nowPlayingSong == src.Song)
            {
                _nowPlayingSong = null;
                SetSongInfo();
            }
            fpnlPlayback.Controls.Remove(src);
            if (SongRemoved != null)
                SongRemoved(src.Song, e);
        }
        private void _nowPlayingSong_FavoriteChanged(object sender, EventArgs e)
        {
            SetFavoriteState(_nowPlayingSong);
        }
        private void SongItem_SongControled(object sender, EventArgs e)
        {
            if (SongControled != null)
                SongControled(sender, e);
        }

        private void btnSongLove_Click(object sender, EventArgs e)
        {
            _nowPlayingSong.IsFavorite = true;
        }

        private void btnSongLoveOff_Click(object sender, EventArgs e)
        {
            _nowPlayingSong.IsFavorite = false;
        }

        private void ucQueue_SizeChanged(object sender, EventArgs e)
        {
            panelContainFPNL.Width = this.Width - 294;
            foreach (Control item in fpnlPlayback.Controls)
            {
                item.Width = panelContainFPNL.Width;
            }
        }
        #endregion
       
    }
}
