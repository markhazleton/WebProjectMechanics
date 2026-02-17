/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using RssToolkit.Rss;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RssToolkit.Web.WebControls
{
    /// <summary>
    /// RssHyperLink control - works with RssHttpHandler 
    /// </summary>
    public class RssHyperLink : HyperLink
    {
        private string _channelName;
        private bool _includeUserName;
        private bool _renderStandardImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="RssHyperLink"/> class.
        /// </summary>
        public RssHyperLink()
        {
            Text = Resources.RssToolkit.RssText;
        }

        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        /// <value>The name of the channel.</value>
        public string ChannelName
        {
            get { return _channelName; }
            set { _channelName = value; }
        }

        /// <summary>
        /// when flag is set, the current user'd name is passed to RssHttpHandler
        /// </summary>
        /// <value><c>true</c> if [include user name]; otherwise, <c>false</c>.</value>
        public bool IncludeUserName
        {
            get { return _includeUserName; }
            set { _includeUserName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to render standard image].
        /// </summary>
        /// <value><c>true</c> if [render standard image]; otherwise, <c>false</c>.</value>
        public bool RenderStandardImage
        {
            get { return _renderStandardImage; }
            set { _renderStandardImage = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // modify the NavigateUrl to include optional user name and channel name
            string channel = _channelName ?? string.Empty;
            string user = _includeUserName ? Context.User.Identity.Name : string.Empty;
            NavigateUrl = RssHttpHandlerHelper.GenerateChannelLink(NavigateUrl, channel, user);

            // add <link> to <head> tag (if <head runat=server> is present)
            if (Page.Header != null)
            {
                string title = string.IsNullOrEmpty(channel) ? Text : channel;

                Page.Header.Controls.Add(
                    new LiteralControl(
                    string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "\r\n<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0}\" href=\"{1}\" />",
                    title,
                    NavigateUrl)));
            }

            if (_renderStandardImage)
            {
                ////String imgPath = "WindowsApplication1.Durer_Melancolia.jpg";
                ////Assembly asm = Assembly.GetExecutingAssembly();
                ////System.IO.Stream s = asm.GetManifestResourceStream(imgPath);
                ////this.pictureBox1.Image = Image.FromStream(s);
            }

            base.OnPreRender(e);
        }
    }
}
