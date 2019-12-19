﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using WMPLib;

namespace BLADE
{
    public partial class ucSearch : UserControl
    {
        public event EventHandler SelectSong;

        public ucSearch()
        {
            InitializeComponent();
            pnlSearchTitle.Hide();
            fpnlSongSearchView.Hide();
        }
        public void ShowListSearch(List<ucSongSearchDetail> src)
        {
            foreach (var item in src)
            {
                fpnlSongSearchView.Controls.Add(item);
                item.Width = panelContainFpnl.Width;
            }
        }

        private void panelContainFpnl_SizeChanged(object sender, EventArgs e)
        {
            foreach (Control item in fpnlSongSearchView.Controls)
            {
                item.Width = panelContainFpnl.Width;
            }
        }
    }
}
