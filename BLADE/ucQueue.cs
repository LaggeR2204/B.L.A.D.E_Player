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
        private bool _isShow;
        private Song _nowPlayingSong;
        public Song NowPlayingSong
        { 
            get => _nowPlayingSong; 
            set
            {
                if(_nowPlayingSong != value)
                {
                    _nowPlayingSong = value;
                    _nowPlayingSong.FavoriteChanged += _nowPlayingSong_FavoriteChanged;
                    if (NowPlayingSongChanged != null)
                        NowPlayingSongChanged(this, new EventArgs());
                }
            }
        }

        private void _nowPlayingSong_FavoriteChanged(object sender, EventArgs e)
        {
            SetFavoriteState(_nowPlayingSong);
        }

        public bool IsShow
        {
            get => _isShow;
            set
            {
                if (value != _isShow)
                {
                    _isShow = value;
                    if (ShowStateChanged != null)
                        ShowStateChanged(this, new EventArgs());
                }
            }
        }
        public event EventHandler NowPlayingSongChanged;
        public event EventHandler ShowStateChanged;
        public event EventHandler SongSelected;
        public event EventHandler SongRemoved;
        public ucQueue()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            
        }
        public void SetFavoriteState(Song src)
        {
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
            picbArtCover.Image = _nowPlayingSong.SongImage;
            lbSongName.Text = _nowPlayingSong.SongName;
            lbSongSinger.Text = _nowPlayingSong.Singer;
            SetFavoriteState(_nowPlayingSong);
        }

        private void SongSelected_Handler(object sender, EventArgs e)
        {
            if (SongSelected != null)
                SongSelected(sender, e);
        }
        private void SongRemoved_Handler(object sender, EventArgs e)
        {
            SongItemInQueue src = sender as SongItemInQueue;
            fpnlQueue.Controls.Remove(src);
            if (SongRemoved != null)
                SongRemoved(src.Song, e);
        }
        public void UpdateQueue(List<Song> src)
        {
            fpnlQueue.Controls.Clear();
            for(int i = 0; i<src.Count; i++)
            {
                ShowSongQueue(src[i]);
            }
        }
        public void ShowSongQueue(Song song)
        {
            SongItemInQueue songItem = new SongItemInQueue(song);
            songItem.SelectedSong += SongSelected_Handler;
            songItem.SongRemoved += SongRemoved_Handler;
            this.fpnlQueue.Controls.Add(songItem);
        }

        private void btnSongLove_Click(object sender, EventArgs e)
        {
            _nowPlayingSong.IsFavorite = true;
        }

        private void btnSongLoveOff_Click(object sender, EventArgs e)
        {
            _nowPlayingSong.IsFavorite = false;
        }
    }
}
