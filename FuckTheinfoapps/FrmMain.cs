﻿using System;
using System.Windows.Forms;

namespace FuckTheinfoapps
{
    public partial class FrmMain : Form
    {
        Theinfoapps tApps;
        ListViewItem itemx;
        int pNumber = 0;
        dynamic obj;

        public FrmMain()
        {
            InitializeComponent();
            listView1.Columns.RemoveAt(2);
            tApps = new Theinfoapps();
        }

        private void 再生PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count <= 0)
                return;

            MPlayer.URL = listView1.SelectedItems[0].SubItems[2].Text;
            MPlayer.Ctlcontrols.play();
        }

        private void ダウンロードDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count <= 0)
                return;

            SaveFileDialog sfd = SaveFileHelper.Create($"{listView1.SelectedItems[0].Text}.mp3");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FrmDownload frmDownload = new FrmDownload(sfd.FileName, listView1.SelectedItems[0].SubItems[2].Text);
                frmDownload.Show();
            }
        }

        private void リピート再生RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (リピート再生RToolStripMenuItem.CheckState == CheckState.Checked)
            {
                MPlayer.settings.setMode("loop", false);
                リピート再生RToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else if (リピート再生RToolStripMenuItem.CheckState == CheckState.Unchecked)
            {
                MPlayer.settings.setMode("loop", true);
                リピート再生RToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        private void アプリケーション終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.Items.Count <= 0)
                return;

            MPlayer.URL = listView1.SelectedItems[0].SubItems[2].Text;
            MPlayer.Ctlcontrols.play();
        }

        private void songName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Scan();
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void Scan()
        {
            if (scanButton.Text == "New Scan")
            {
                pNumber = 0;
                listView1.Items.Clear();
                nextButton.Enabled = false;
                scanButton.Text = "First Scan";
            }
            else if (scanButton.Text == "First Scan")
            {
                if (pNumber != 0)
                {
                    pNumber = 0;
                    MessageBox.Show("初期化に失敗しました。", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ScanMusicList(pNumber))
                {
                    MessageBox.Show("お探しの曲は存在しません。", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pNumber++;
                ScanMusicList(pNumber);

                scanButton.Text = "New Scan";
                nextButton.Enabled = true;
                pNumber++;
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Next()
        {
            if (!ScanMusicList(pNumber))
            {
                MessageBox.Show("これ以上は存在しません。", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            pNumber++;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private bool ScanMusicList(int _sCount)
        {
            obj = tApps.GetObject(songName.Text, _sCount);
            if (obj == null)
                return false;

            foreach (var data in obj.data.items)
            {
                itemx = listView1.Items.Add(data.title);
                itemx.SubItems.Add(data.artist);
                itemx.SubItems.Add(data.url);
            }
            return true;
        }
    }
}