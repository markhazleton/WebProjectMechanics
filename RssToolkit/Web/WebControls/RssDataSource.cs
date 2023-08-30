/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using RssToolkit.Rss;
using RssToolkit.Web.Design;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace RssToolkit.Web.WebControls
{
    /// <summary>
    /// RSS data source control implementation, including the designer
    /// </summary>
    [Designer(typeof(RssDataSourceDesigner))]
    [DefaultProperty("Url")]
    public class RssDataSource : DataSourceControl
    {
        private int _maxItems;
        private string _url;
        private RssDataSourceView _itemsView;
        private RssDocument _rss;

        /// <summary>
        /// Gets or sets the max items.
        /// </summary>
        /// <value>The max items.</value>
        public int MaxItems
        {
            get
            {
                return _maxItems;
            }

            set
            {
                _maxItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _rss = null;
                _url = value;
            }
        }

        /// <summary>
        /// Gets the RSS.
        /// </summary>
        /// <value>The RSS.</value>
        public RssDocument Rss
        {
            get
            {
                if (_rss == null)
                {
                    _rss = RssDocument.Load(new Uri(_url));
                }

                return _rss;
            }
        }

        /// <summary>
        /// Gets the named data source view associated with the data source control.
        /// </summary>
        /// <param name="viewName">The name of the <see cref="T:System.Web.UI.DataSourceView"></see> to retrieve. In data source controls that support only one view, such as <see cref="T:System.Web.UI.WebControls.SqlDataSource"></see>, this parameter is ignored.</param>
        /// <returns>
        /// Returns the named <see cref="T:System.Web.UI.DataSourceView"></see> associated with the <see cref="T:System.Web.UI.DataSourceControl"></see>.
        /// </returns>
        protected override DataSourceView GetView(string viewName)
        {
            if (_itemsView == null)
            {
                _itemsView = new RssDataSourceView(this, viewName);
            }

            return _itemsView;
        }
    }
}
