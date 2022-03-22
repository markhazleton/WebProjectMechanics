/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using RssToolkit.Web.WebControls;

namespace RssToolkit.Web.Design
{
    /// <summary>
    /// Rss Data Source View
    /// </summary>
    public class RssDataSourceView : DataSourceView
    {
        private RssDataSource _owner;

        internal RssDataSourceView(RssDataSource owner, string viewName)
            : base(owner, viewName)
        {
            _owner = owner;
        }

        /// <summary>
        /// Gets a list of data asynchronously from the underlying data storage.
        /// </summary>
        /// <param name="arguments">A <see cref="T:System.Web.UI.DataSourceSelectArguments"></see> that is used to request operations on the data beyond basic data retrieval.</param>
        /// <param name="callback">A <see cref="T:System.Web.UI.DataSourceViewSelectCallback"></see> delegate that is used to notify a data-bound control when the asynchronous operation is complete.</param>
        /// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Web.UI.DataSourceViewSelectCallback"></see> supplied is null.</exception>
        public override void Select(DataSourceSelectArguments arguments, DataSourceViewSelectCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            callback(ExecuteSelect(arguments));
        }

        /// <summary>
        /// Gets a list of data from the underlying data storage.
        /// </summary>
        /// <param name="arguments">A <see cref="T:System.Web.UI.DataSourceSelectArguments"></see> that is used to request operations on the data beyond basic data retrieval.</param>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerable"></see> list of data from the underlying data storage.
        /// </returns>
        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            return _owner.Rss.SelectItems(_owner.MaxItems);
        }
    }
}
