/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.

  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RssToolkit.Rss.CodeGeneration
{
    /// <summary>
    /// Used for generating Code using CodeDom
    /// </summary>
    public sealed class RssCodeGenerator
    {
        #region Constants
        /// <summary>
        /// Rss Constant
        /// </summary>
        public const string RssConstant = "Rss";

        /// <summary>
        /// Channel Constant
        /// </summary>
        public const string ChannelConstant = "channel";

        /// <summary>
        /// Namespace Constant
        /// </summary>
        public const string NamespaceConstant = "RssToolkit.Rss";

        /// <summary>
        /// Rss Namespace Constant
        /// </summary>
        public const string RssNamespaceConstant = NamespaceConstant + ".RssDocumentBase";

        /// <summary>
        /// Http Handler Namespace Constant
        /// </summary>
        public const string HttpHandlerNamespaceConstant = NamespaceConstant + ".RssHttpHandlerBase";
        private const string varName = "doc";
        #endregion

        #region Constructors
        private RssCodeGenerator()
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="rssDefinition">The RSS definition.</param>
        /// <param name="outputLanguage">The output language.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="classNamePrefix">The class name prefix.</param>
        /// <param name="outputCode">The output code.</param>
        /// <param name="useBaseClass">if set to <c>true</c> [use base class].</param>
        public static void GenerateCode(
            RssDocumentBase rssDefinition,
            string outputLanguage,
            string namespaceName,
            string classNamePrefix,
            TextWriter outputCode,
            bool useBaseClass)
        {
            if (rssDefinition == null)
            {
                throw new ArgumentNullException("rssDefinition");
            }

            if (string.IsNullOrEmpty(outputLanguage))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "outputLanguage"));
            }

            if (namespaceName == null)
            {
                throw new ArgumentNullException("namespaceName");
            }

            if (string.IsNullOrEmpty(classNamePrefix))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "classNamePrefix"));
            }

            if (outputCode == null)
            {
                throw new ArgumentNullException("outputCode");
            }

            GenerateCode(rssDefinition.ToXml(DocumentType.Rss), rssDefinition.Url, outputLanguage, namespaceName, classNamePrefix, outputCode, useBaseClass);
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="codeXml">The code XML.</param>
        /// <param name="url">The URL.</param>
        /// <param name="outputLanguage">The output language.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="classNamePrefix">The class name prefix.</param>
        /// <param name="outputCode">The output code.</param>
        /// <param name="useBaseClass">if set to <c>true</c> [use base class].</param>
        public static void GenerateCode(
            string codeXml,
            string url,
            string outputLanguage,
            string namespaceName,
            string classNamePrefix,
            TextWriter outputCode,
            bool useBaseClass)
        {
            if (string.IsNullOrEmpty(codeXml))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "codeXml"));
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrEmpty(outputLanguage))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "outputLanguage"));
            }

            if (namespaceName == null)
            {
                throw new ArgumentNullException("namespaceName");
            }

            if (string.IsNullOrEmpty(classNamePrefix))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "classNamePrefix"));
            }

            if (outputCode == null)
            {
                throw new ArgumentNullException("outputCode");
            }

            // get the CodeDom provider for the language
            CodeDomProvider provider = CodeDomProvider.CreateProvider(outputLanguage);

            // generate the CodeDom tree
            CodeCompileUnit unit = new CodeCompileUnit();
            GenerateCodeDomTree(codeXml, url, namespaceName, classNamePrefix, unit, useBaseClass);

            // generate source
            CodeGeneratorOptions options = new CodeGeneratorOptions() { BlankLinesBetweenMembers = true, BracingStyle = "Block", ElseOnClosing = false, IndentString = "    " };

            provider.GenerateCodeFromCompileUnit(unit, outputCode, options);
        }

        /// <summary>
        /// Generates the code DOM tree.
        /// </summary>
        /// <param name="rssDefinition">The RSS definition.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="classNamePrefix">The class name prefix.</param>
        /// <param name="outputCodeCompileUnit">The output code compile unit.</param>
        /// <param name="useBaseClass">if set to <c>true</c> [use base class].</param>
        public static void GenerateCodeDomTree(
            RssDocumentBase rssDefinition,
            string namespaceName,
            string classNamePrefix,
            CodeCompileUnit outputCodeCompileUnit,
            bool useBaseClass)
        {
            if (rssDefinition == null)
            {
                throw new ArgumentNullException("rssDefinition");
            }

            if (namespaceName == null)
            {
                throw new ArgumentNullException("namespaceName");
            }

            if (string.IsNullOrEmpty(classNamePrefix))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "classNamePrefix"));
            }

            if (outputCodeCompileUnit == null)
            {
                throw new ArgumentNullException("outputCodeCompileUnit");
            }

            GenerateCodeDomTree(rssDefinition.ToXml(DocumentType.Rss), rssDefinition.Url, namespaceName, classNamePrefix, outputCodeCompileUnit, useBaseClass);
        }

        /// <summary>
        /// Generates the code DOM tree.
        /// </summary>
        /// <param name="codeXml">The code XML.</param>
        /// <param name="url">The URL.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="classNamePrefix">The class name prefix.</param>
        /// <param name="outputCodeCompileUnit">The output code compile unit.</param>
        /// <param name="useBaseClass">if set to <c>true</c> [use base class].</param>
        public static void GenerateCodeDomTree(
            string codeXml,
            string url,
            string namespaceName,
            string classNamePrefix,
            CodeCompileUnit outputCodeCompileUnit,
            bool useBaseClass)
        {
            if (string.IsNullOrEmpty(codeXml))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "codeXml"));
            }

            if (namespaceName == null)
            {
                throw new ArgumentNullException("namespaceName");
            }

            if (string.IsNullOrEmpty(classNamePrefix))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "classNamePrefix"));
            }

            if (outputCodeCompileUnit == null)
            {
                throw new ArgumentNullException("outputCodeCompileUnit");
            }

            if (outputCodeCompileUnit.Namespaces == null)
            {
                throw new ArgumentNullException("outputCodeCompileUnit.Namespaces");
            }

            RssCodeTreeGenerator codeTree = new RssCodeTreeGenerator();
            Dictionary<string, ClassInfo> classDictionary = codeTree.ConvertToDictionary(codeXml);

            // generate namespace
            CodeNamespace generatedNamespace = new CodeNamespace(namespaceName);
            generatedNamespace.Imports.Add(new CodeNamespaceImport("System"));
            generatedNamespace.Imports.Add(new CodeNamespaceImport("System.Xml"));
            generatedNamespace.Imports.Add(new CodeNamespaceImport("System.Xml.Serialization"));
            generatedNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            generatedNamespace.Imports.Add(new CodeNamespaceImport(NamespaceConstant));

            foreach (string classKey in classDictionary.Keys)
            {
                ClassInfo classInfo = classDictionary[classKey];
                if (classInfo.Properties.Count > 0)
                {
                    string className = classNamePrefix + CodeNameFromRssName(classInfo.Name);
                    CodeTypeDeclaration itemType = new CodeTypeDeclaration(className);

                    if (classInfo.Name.Equals(RssConstant, StringComparison.OrdinalIgnoreCase))
                    {
                        itemType.CustomAttributes.Add(new CodeAttributeDeclaration("System.Serializable"));
                        itemType.CustomAttributes.Add(new CodeAttributeDeclaration("System.Xml.Serialization.XmlRoot", new CodeAttributeArgument(new CodePrimitiveExpression("rss"))));

                        if (useBaseClass)
                        {
                            itemType.BaseTypes.Add(new CodeTypeReference(RssNamespaceConstant));
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            string itemTypeName = classNamePrefix + "Item";

                            //// LoadRss (and LoadRssItems) method
                            GenerateLoad(itemType, url);
                            GenerateLoadRssByUrl(itemType);
                            GenerateLoadRssByXmlTextReader(itemType);
                            GenerateLoadRssByXml(itemType);
                            GenerateLoadRssItems(itemType, itemTypeName);
                        }

                        GenerateToXml(itemType);
                        GenerateToDataSet(itemType);
                    }

                    if (!NamespaceContainsType(generatedNamespace, itemType.Name))
                    {
                        generatedNamespace.Types.Add(itemType);
                    }

                    foreach (string propertyKey in classInfo.Properties.Keys)
                    {
                        PropertyInfo property = classInfo.Properties[propertyKey];

                        string propertyName = classNamePrefix +
                            CodeNameFromRssName(property.Name);
                        AddCodeProperty(
                            property.Name,
                            property.IsAttribute ? XmlNodeType.Attribute : XmlNodeType.Element,
                            (classDictionary.ContainsKey(property.Name) && classDictionary[property.Name].Properties.Count > 0) ? new CodeTypeReference(propertyName) : new CodeTypeReference("System.String"),
                            itemType,
                            property.Occurrences > 1 ? true : false,
                            (classDictionary.ContainsKey(property.Name) ? classDictionary[property.Name].Namespace : string.Empty));
                    }

                    if (classInfo.IsText)
                    {
                        AddCodeProperty("Text", XmlNodeType.Text, new CodeTypeReference("System.String"), itemType, false, string.Empty);
                    }
                }
            }

            if (useBaseClass)
            {
                // generate http handler base class
                string handlerTypeName = classNamePrefix + "HttpHandlerBase";
                CodeTypeDeclaration handlerType = new CodeTypeDeclaration(handlerTypeName);

                handlerType.BaseTypes.Add(new CodeTypeReference(HttpHandlerNamespaceConstant, new CodeTypeReference[1] { new CodeTypeReference(classNamePrefix + RssConstant) }));

                generatedNamespace.Types.Add(handlerType);
            }

            // add the generated namespace to the code compile unit
            outputCodeCompileUnit.Namespaces.Add(generatedNamespace);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Codes the name of the name from RSS.
        /// </summary>
        /// <param name="rssName">Name of the RSS.</param>
        /// <returns>string</returns>
        private static string CodeNameFromRssName(string rssName)
        {
            if (string.IsNullOrEmpty(rssName))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "rssName"));
            }

            StringBuilder sb = new StringBuilder(rssName.Length);

            if (rssName.IndexOf(':') >= 0)
            {
                foreach (string s in rssName.Split(':'))
                {
                    if (s.Length > 0)
                    {
                        sb.Append(s.Substring(0, 1).ToUpperInvariant());
                    }

                    if (s.Length > 1)
                    {
                        sb.Append(s.Substring(1));
                    }
                }
            }
            else
            {
                if (rssName.Length > 0)
                {
                    sb.Append(rssName.Substring(0, 1).ToUpperInvariant());
                }

                if (rssName.Length > 1)
                {
                    sb.Append(rssName.Substring(1));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Codes the name of the param name from RSS.
        /// </summary>
        /// <param name="rssName">Name of the RSS.</param>
        /// <returns>string</returns>
        private static string CodeParamNameFromRssName(string rssName)
        {
            if (String.IsNullOrEmpty(rssName))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "rssName"));
            }

            string name = CodeNameFromRssName(rssName);

            if (name.Length > 1)
            {
                name = name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
            }
            else
            {
                name = name.ToLowerInvariant();
            }

            return name;
        }

        /// <summary>
        /// Codes the name of the private name from RSS.
        /// </summary>
        /// <param name="rssName">Name of the RSS.</param>
        /// <returns>string</returns>
        private static string CodePrivateNameFromRssName(string rssName)
        {
            return "_" + CodeParamNameFromRssName(rssName);
        }

        /// <summary>
        /// Namespaces the type of the contains.
        /// </summary>
        /// <param name="nameSpace">The nameSpace.</param>
        /// <param name="typeName">The type name.</param>
        /// <returns>bool</returns>
        private static bool NamespaceContainsType(CodeNamespace nameSpace, string typeName)
        {
            if (nameSpace == null)
            {
                throw new ArgumentNullException("nameSpace");
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "typeName"));
            }

            foreach (CodeTypeDeclaration type in nameSpace.Types)
            {
                if (string.Compare(type.Name, typeName, false, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Types the contains property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyToFind">The property to find.</param>
        /// <returns>bool</returns>
        private static bool TypeContainsProperty(CodeTypeDeclaration type, string propertyToFind)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrEmpty(propertyToFind))
            {
                throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "propertyToFind"));
            }

            foreach (CodeTypeMember property in type.Members)
            {
                if (string.Compare(property.Name, propertyToFind, false, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    return true;
            }

            return false;
        }

        private static void AddCodeProperty(
            string nodeName,
            XmlNodeType nodeType,
            CodeTypeReference propertyType,
            CodeTypeDeclaration classType,
            bool collection,
            string nameSpaceName)
        {
            string propertyVariableName = CodePrivateNameFromRssName(nodeName);
            string propertyName = CodeNameFromRssName(nodeName);

            if (collection)
            {
                propertyVariableName = Pluralizer.ToPlural(propertyVariableName);
                propertyName = Pluralizer.ToPlural(propertyName);
                propertyType = new CodeTypeReference(
                    "List",
                    new CodeTypeReference[1] { propertyType });
            }

            bool alreadyThere = TypeContainsProperty(classType, propertyVariableName);
            if (alreadyThere)
                return;

            classType.Members.Add(new CodeMemberField(propertyType, propertyVariableName));

            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes &= ~MemberAttributes.AccessMask;
            property.Attributes |= MemberAttributes.Public;
            property.Name = propertyName;
            property.Type = propertyType;

            // prevent warnings for RssDocumentBase properties
            if (propertyName.Equals("Version", StringComparison.Ordinal)
                || propertyName.Equals("Channel", StringComparison.Ordinal))
            {
                property.Attributes |= MemberAttributes.New;
            }

            CodeAttributeDeclaration cad;
            if (nodeType != XmlNodeType.Text)
            {
                cad = new CodeAttributeDeclaration(
                    "Xml" + nodeType,
                    new CodeAttributeArgument(new CodePrimitiveExpression(nodeName)));
            }
            else
            {
                cad = new CodeAttributeDeclaration("Xml" + nodeType);
            }

            if (!String.IsNullOrEmpty(nameSpaceName))
            {
                cad.Arguments.Add(new CodeAttributeArgument("Namespace", new CodePrimitiveExpression(nameSpaceName)));
            }

            property.CustomAttributes.Add(cad);

            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(propertyVariableName)));
            property.SetStatements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression(propertyVariableName), new CodeArgumentReferenceExpression("value")));

            classType.Members.Add(property);
        }

        private static void GenerateLoad(CodeTypeDeclaration channelType, string url)
        {
            string typeName = channelType.Name;

            CodeMemberMethod m = new CodeMemberMethod() { Name = "Load" };
            m.Attributes &= ~MemberAttributes.AccessMask;
            m.Attributes |= MemberAttributes.Public | MemberAttributes.Static;
            m.ReturnType = new CodeTypeReference(typeName);

            m.Statements.Add(new CodeVariableDeclarationStatement(typeName, varName));

            m.Statements.Add(
                new CodeAssignStatement(
                new CodeVariableReferenceExpression(varName),
                new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression(typeName),
                    "Load",
                    new CodeExpression[1] { new CodeObjectCreateExpression("System.Uri", new CodeExpression[1] { new CodePrimitiveExpression(url) }) })));

            m.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression(varName)));

            channelType.Members.Add(m);
        }

        private static void GenerateToXml(CodeTypeDeclaration channelType)
        {
            string typeName = channelType.Name;

            CodeMemberMethod m = new CodeMemberMethod() { Name = "ToXml", Attributes = MemberAttributes.Override | MemberAttributes.Public };
            m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(DocumentType), "outputType"));
            m.ReturnType = new CodeTypeReference("System.String");

            m.Statements.Add(
                new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(
                            null,
                            "RssDocumentBase.ToXml",
                            new CodeTypeReference[1] { new CodeTypeReference(typeName) }),
                            new CodeExpression[2] { new CodeArgumentReferenceExpression("outputType"), new CodeThisReferenceExpression() })));

            channelType.Members.Add(m);
        }

        private static void GenerateToDataSet(CodeTypeDeclaration channelType)
        {
            CodeMemberMethod m = new CodeMemberMethod() { Name = "ToDataSet", Attributes = MemberAttributes.Public, ReturnType = new CodeTypeReference("System.Data.DataSet") };

            m.Statements.Add(new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(null, "RssXmlHelper.ToDataSet"),
                new CodeExpression[1] { new CodeArgumentReferenceExpression("ToXml(DocumentType.Rss)") })));

            channelType.Members.Add(m);
        }

        private static void GenerateLoadRssByUrl(CodeTypeDeclaration channelType)
        {
            string typeName = channelType.Name;

            CodeMemberMethod m = new CodeMemberMethod() { Name = "Load" };
            m.Attributes &= ~MemberAttributes.AccessMask;
            m.Attributes |= MemberAttributes.Public | MemberAttributes.Static;
            m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Uri), "url"));
            m.ReturnType = new CodeTypeReference(typeName);

            ////Create if Statement
            CodeThrowExceptionStatement throwException = new CodeThrowExceptionStatement(
                new CodeObjectCreateExpression(
                new CodeTypeReference(typeof(ArgumentNullException)),
                new CodeExpression[] { }));

            CodeConditionStatement instanceCondition = new CodeConditionStatement();

            CodeBinaryOperatorExpression ifCondition = new CodeBinaryOperatorExpression() { Left = new CodeVariableReferenceExpression("url"), Right = new CodePrimitiveExpression(null), Operator = CodeBinaryOperatorType.ValueEquality };
            instanceCondition.Condition = ifCondition;
            instanceCondition.TrueStatements.Add(throwException);
            m.Statements.Add(instanceCondition);

            m.Statements.Add(new CodeVariableDeclarationStatement(typeName, varName));
            m.Statements.Add(new CodeAssignStatement(
                            new CodeVariableReferenceExpression(varName),
                            new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(null, "RssDocumentBase.Load",
                            new CodeTypeReference[1] {
                                new CodeTypeReference(typeName) }),
                                new CodeExpression[1] { new CodeArgumentReferenceExpression("url") })));

            m.Statements.Add(new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression(varName)));

            channelType.Members.Add(m);
        }

        private static void GenerateLoadRssByXml(CodeTypeDeclaration channelType)
        {
            string typeName = channelType.Name;

            CodeMemberMethod m = new CodeMemberMethod() { Name = "Load" };
            m.Attributes &= ~MemberAttributes.AccessMask;
            m.Attributes |= MemberAttributes.Public | MemberAttributes.Static;
            m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "xml"));
            m.ReturnType = new CodeTypeReference(typeName);

            ////Create if Statement
            CodeThrowExceptionStatement throwException = new CodeThrowExceptionStatement(
                new CodeObjectCreateExpression(
                new CodeTypeReference(typeof(ArgumentNullException)),
                new CodeExpression[] { }));

            CodeConditionStatement codecondition = new CodeConditionStatement(new CodeSnippetExpression("String.IsNullOrEmpty(xml)"), throwException);
            m.Statements.Add(codecondition);

            m.Statements.Add(new CodeVariableDeclarationStatement(typeName, varName));
            m.Statements.Add(new CodeAssignStatement(
                            new CodeVariableReferenceExpression(varName),
                            new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(null, "RssDocumentBase.Load",
                            new CodeTypeReference[1] {
                                new CodeTypeReference(typeName) }),
                                new CodeExpression[1] { new CodeArgumentReferenceExpression("xml") })));

            m.Statements.Add(new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression(varName)));

            channelType.Members.Add(m);
        }

        private static void GenerateLoadRssByXmlTextReader(CodeTypeDeclaration channelType)
        {
            string typeName = channelType.Name;

            CodeMemberMethod m = new CodeMemberMethod() { Name = "Load" };
            m.Attributes &= ~MemberAttributes.AccessMask;
            m.Attributes |= MemberAttributes.Public | MemberAttributes.Static;
            m.Parameters.Add(new CodeParameterDeclarationExpression(typeof(XmlTextReader), "reader"));
            m.ReturnType = new CodeTypeReference(typeName);

            ////Create if Statement
            CodeThrowExceptionStatement throwException = new CodeThrowExceptionStatement(
                new CodeObjectCreateExpression(
                new CodeTypeReference(typeof(ArgumentNullException)),
                new CodeExpression[] { }));

            CodeConditionStatement instanceCondition = new CodeConditionStatement();

            CodeBinaryOperatorExpression ifCondition = new CodeBinaryOperatorExpression() { Left = new CodeVariableReferenceExpression("reader"), Right = new CodePrimitiveExpression(null), Operator = CodeBinaryOperatorType.ValueEquality };
            instanceCondition.Condition = ifCondition;
            instanceCondition.TrueStatements.Add(throwException);
            m.Statements.Add(instanceCondition);
            m.Statements.Add(new CodeVariableDeclarationStatement(typeName, varName));
            m.Statements.Add(
                new CodeAssignStatement(
                            new CodeVariableReferenceExpression(varName),
                            new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(null, "RssDocumentBase.Load",
                            new CodeTypeReference[1] { new CodeTypeReference(typeName) }),
                            new CodeExpression[1] { new CodeArgumentReferenceExpression("reader") })));

            m.Statements.Add(new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression(varName)));

            channelType.Members.Add(m);
        }

        private static void GenerateLoadRssItems(CodeTypeDeclaration channelType, string itemTypeName)
        {
            CodeMemberMethod m = new CodeMemberMethod() { Name = "LoadRssItems" };
            m.Attributes &= ~MemberAttributes.AccessMask;
            m.Attributes |= MemberAttributes.Public | MemberAttributes.Static;
            m.ReturnType = new CodeTypeReference(
                "System.Collections.Generic.List",
                new CodeTypeReference[1] { new CodeTypeReference(itemTypeName) });

            m.Statements.Add(
                new CodeMethodReturnStatement(
                new CodePropertyReferenceExpression(
                    new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(null, "Load", null)),
                    "Channel.Items")));

            channelType.Members.Add(m);
        }
        #endregion
    }
}
