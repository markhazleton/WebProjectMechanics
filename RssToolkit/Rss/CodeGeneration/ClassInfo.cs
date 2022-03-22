/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.
 
  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/
using System;
using System.Collections.Generic;

namespace RssToolkit.Rss.CodeGeneration
{
    /// <summary>
    /// Is used inside the Code generation process
    /// </summary>
    internal class ClassInfo
    {
        private readonly string _name;
        private bool _isText;
        private string _namespace;
        private readonly Dictionary<string, PropertyInfo> _properties;

        public ClassInfo(string name)
        {
            _name = name;
            _properties = new Dictionary<string, PropertyInfo>();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get 
            { 
                return _name; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this Class is Text NodeType.
        /// </summary>
        /// <value><c>true</c> if this Class is Text NodeType; otherwise, <c>false</c>.</value>
        public bool IsText
        {
            get
            {
                return _isText;
            }

            set
            {
                _isText = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the namespace.
        /// </summary>
        /// <value>The name of the namespace.</value>
        public string Namespace
        {
            get
            {
                return _namespace;
            }

            set
            {
                _namespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        internal Dictionary<string, PropertyInfo> Properties
        {
            get 
            { 
                return _properties; 
            }
        }
    }
}
