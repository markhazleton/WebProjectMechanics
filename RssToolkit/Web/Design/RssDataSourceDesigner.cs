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
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using RssToolkit.Rss;
using RssToolkit.Web.WebControls;

namespace RssToolkit.Web.Design
{
    /// <summary>
    /// Provides design-time support in a design host for the System.Web.UI.DataSourceControl class.
    /// </summary>
    public class RssDataSourceDesigner : DataSourceDesigner
    {
        private RssDataSource _dataSource;
        private RssDesignerDataSourceView _view;

        /// <summary>
        /// Gets a value indicating whether the <see cref="M:System.Web.UI.Design.DataSourceDesigner.Configure"></see> method can be called.
        /// </summary>
        /// <value></value>
        /// <returns>true, if <see cref="M:System.Web.UI.Design.DataSourceDesigner.Configure"></see> can be called; otherwise, false. The default is false.</returns>
        public override bool CanConfigure
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes the control designer and loads the specified component.
        /// </summary>
        /// <param name="component">The control being designed.</param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            _dataSource = (RssDataSource)component;
        }
        
        /// <summary>
        /// Launches the data source configuration utility in the design host.
        /// </summary>
        public override void Configure()
        {
            InvokeTransactedChange(Component, new TransactedChangeCallback(ConfigureRssDataSource), null, "Configure Data Source");
        }

        /// <summary>
        /// Retrieves a <see cref="T:System.Web.UI.Design.DesignerDataSourceView"></see> object that is identified by the view name.
        /// </summary>
        /// <param name="viewName">The name of the view.</param>
        /// <returns>This implementation always returns null.</returns>
        public override DesignerDataSourceView GetView(string viewName)
        {
            if (_view == null)
            {
                _view = new RssDesignerDataSourceView(this, viewName);
            }

            return _view;
        }

        private bool ConfigureRssDataSource(object context)
        {
            try
            {
                SuppressDataSourceEvents();

                string oldUrl = _dataSource.Url;

                using (RssDataSourceConfigForm form = new RssDataSourceConfigForm(_dataSource))
                {
                    IUIService uiService = (IUIService)GetService(typeof(IUIService));
                    DialogResult result = uiService.ShowDialog(form);

                    if (result == DialogResult.OK && oldUrl != _dataSource.Url)
                    {
                        OnSchemaRefreshed(EventArgs.Empty);
                    }

                    return (result == DialogResult.OK);
                }
            }
            finally
            {
                ResumeDataSourceEvents();
            }
        }

        private class RssDesignerDataSourceView : DesignerDataSourceView
        {
            private RssDataSourceDesigner _owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="RssDesignerDataSourceView"/> class.
            /// </summary>
            /// <param name="owner">The owner.</param>
            /// <param name="viewName">Name of the view.</param>
            public RssDesignerDataSourceView(RssDataSourceDesigner owner, string viewName) : base(owner, viewName)
            {
                _owner = owner;
            }

            /// <summary>
            /// Gets a schema that describes the data source view that is represented by this view object.
            /// </summary>
            /// <value></value>
            /// <returns>An <see cref="T:System.Web.UI.Design.IDataSourceViewSchema"></see>.</returns>
            public override IDataSourceViewSchema Schema
            {
                get
                {
                    RssDocument rss = _owner._dataSource.Rss;

                    if (rss.Channel != null && rss.Channel.Items != null && rss.Channel.Items.Count == 0)
                    {
                        return base.Schema;
                    }

                    //// create a DataTable and infer schema from there
                    DataTable dt = rss.ToDataSet().Tables["item"];
                    return new DataSetViewSchema(dt);
                }
            }
        }
    }
}
