/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.

  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RssToolkit.Rss.CodeGeneration
{
    /// <summary>
    /// To generate dictionary used in code generation
    /// </summary>
    internal class RssCodeTreeGenerator
    {
        private readonly Stack<ParentInfo> stack = new Stack<ParentInfo>();

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <returns>Dictionary</returns>
        public Dictionary<string, ClassInfo> ConvertToDictionary(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "xml"));
            }

            using (StringReader stringReader = new StringReader(xml))
            using (XmlTextReader reader = new XmlTextReader(stringReader))
            {
                return Parse(reader);
            }
        }

        private Dictionary<string, ClassInfo> Parse(XmlTextReader reader)
        {
            XmlNamespaceManager nps = new XmlNamespaceManager(reader.NameTable);
            Dictionary<string, ClassInfo> table = new Dictionary<string, ClassInfo>();
            string previousNode = null;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element ||
                    reader.NodeType == XmlNodeType.EndElement)
                {
                    ////Got an element. This will be the name of a class.
                    string className = reader.LocalName;
                    
                    if (className.Equals("rss", StringComparison.OrdinalIgnoreCase))
                    {
                        nps = new XmlNamespaceManager(reader.NameTable);

                        for (int index = 0; index < reader.AttributeCount; index++)
                        {
                            {
                                reader.MoveToAttribute(index);
                                if (reader.Name.StartsWith("xmlns"))
                                {
                                    nps.AddNamespace(reader.LocalName, reader.Value);
                                }
                            }
                        }
                    }

                    if (reader.IsStartElement() || reader.IsEmptyElement)
                    {
                        ParseStartElements(reader, table, null, nps, className);
                    }
                    else
                    {
                        ParseEndElements(table, className);
                    }

                    previousNode = className;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (!string.IsNullOrEmpty(previousNode))
                    {
                        ClassInfo classInfo = table[previousNode];
                        classInfo.IsText = true;
                    }
                }
            }

            return table;
        }

        private void ParseStartElements(XmlTextReader reader, Dictionary<string, ClassInfo> table, ParentInfo parent, XmlNamespaceManager nps, string className)
        {
            if (stack.Count != 0)
            {
                ////Get the parent
                parent = stack.Peek();

                ////Get the child element from inside the parent
                if (parent.ChildCount.ContainsKey(className))
                {
                    ////increment the Occurrence of the child element inside the parent
                    int childCount = parent.ChildCount[className];
                    parent.ChildCount[className] = ++childCount;
                }
                else
                {
                    ////if child does not exist in the parent add it with Occurrence = 1
                    parent.ChildCount.Add(className, 1);
                }
            }

            if (!reader.IsEmptyElement)
            {
                ////If its not an empty element push it in the stack which
                ////contains all parent elements
                stack.Push(new ParentInfo());
            }

            ClassInfo classInfo;

            if (!table.TryGetValue(className, out classInfo))
            {
                ////Create a ClassInfo every time we hit an element and it is not already
                ////contained in the global table.
                classInfo = new ClassInfo(className);
                table.Add(className, classInfo);
            }

            if (reader.LocalName != reader.Name)
            {
                string lookupName = reader.Prefix == "xmlns" ? reader.LocalName : reader.Prefix;
                classInfo.Namespace = nps.LookupNamespace(lookupName);
            }

            ////Add any new attributes that are not already defined in the classInfo....
            for (int index = 0; index < reader.AttributeCount; index++)
            {
                reader.MoveToAttribute(index);
                string localName = reader.LocalName;
                if (!classInfo.Properties.ContainsKey(localName) && (!reader.Name.StartsWith("xmlns")))
                {
                    PropertyInfo propertyInfo = new PropertyInfo(localName, true);
                    classInfo.Properties[propertyInfo.Name] = propertyInfo;
                }
            }
        }

        private void ParseEndElements(Dictionary<string, ClassInfo> table, string className)
        {
            ParentInfo parent = stack.Pop();
            ClassInfo classInfo = table[className];
            Dictionary<string, PropertyInfo> classMembers = classInfo.Properties;

            foreach (KeyValuePair<string, int> child in parent.ChildCount)
            {
                string childName = child.Key;
                int childCount = child.Value;
                int existingChildCount = 0;

                if (classMembers.ContainsKey(childName))
                {
                    existingChildCount = classMembers[childName].Occurrences;
                }
                else
                {
                    PropertyInfo property = new PropertyInfo(childName, false);
                    classMembers[childName] = property;
                }

                if (childCount > existingChildCount)
                {
                    classMembers[childName].Occurrences = childCount;
                }
            }
        }

        private class ParentInfo
        {
            private readonly Dictionary<string, int> _childCount = new Dictionary<string, int>();

            /// <summary>
            /// Gets or sets the child count.
            /// </summary>
            /// <value>The child count.</value>
            public Dictionary<string, int> ChildCount
            {
                get
                {
                    return _childCount;
                }
            }
        }
    }
}
