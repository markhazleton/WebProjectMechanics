/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.

  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RssToolkit.Rss;
using RssToolkit.Web.WebControls;

namespace RssToolkit.Web.Design
{
    /// <summary>
    /// WinForm dialog to configure RSS data source 
    /// </summary>
    internal partial class RssDataSourceConfigForm : Form
    {
        private static List<string> _history = new List<string>();
        private RssDataSource _dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssDataSourceConfigForm"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        public RssDataSourceConfigForm(RssDataSource dataSource)
        {
            _dataSource = dataSource;
            InitializeComponent();

            lock (_history)
            {
                AddToHistory(dataSource.Url);

                foreach (string url in _history)
                {
                    urlComboBox.Items.Add(url);
                }
            }

            urlComboBox.Text = dataSource.Url;
        }

        private static void AddToHistory(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            foreach (string s in _history)
            {
                if (url == s)
                {
                    return;
                }
            }

            _history.Insert(0, url);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "rss"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Windows.Forms.MessageBox.Show(System.Windows.Forms.IWin32Window,System.String,System.String,System.Windows.Forms.MessageBoxButtons,System.Windows.Forms.MessageBoxIcon)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, EventArgs e)
        {
            string url = urlComboBox.Text;

            if (url != _dataSource.Url)
            {
                try
                {
                    RssDocument rss = RssDocument.Load(new System.Uri(url));
                    lock (_history)
                    {
                        AddToHistory(url);
                    }
                }
                catch
                {
                    MessageBox.Show(this, "Failed to load RSS feed for the specified URL", "RssDataSource Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _dataSource.Url = url;
            DialogResult = DialogResult.OK;
        }
    }
}