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
    /// Stores property information for code generation
    /// </summary>
    internal class PropertyInfo
    {
        private readonly string _name;
        private readonly bool _isAttribute;
        private int _occurrences;

        /// <summary>
        /// Constructs a new instance of <see cref="PropertyInfo"/> class
        /// </summary>
        /// <param name="name">the name of the property</param>
        /// <param name="isAttribute">true if this is an attribute</param>
        public PropertyInfo(string name, bool isAttribute)
        {
            _name = name;
            _isAttribute = isAttribute;
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
        /// Gets a value indicating whether this instance is attribute.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is attribute; otherwise, <c>false</c>.
        /// </value>
        public bool IsAttribute
        {
            get
            {
                return _isAttribute;
            }
        }

        /// <summary>
        /// Gets or sets the occurrences.
        /// </summary>
        /// <value>The occurrences.</value>
        public int Occurrences
        {
            get 
            {
                return _occurrences; 
            }

            set 
            {
                _occurrences = value; 
            }
        }
    }
}
