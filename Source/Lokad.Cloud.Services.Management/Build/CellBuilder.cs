﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ionic.Zip;

namespace Lokad.Cloud.Services.Management.Build
{
    public class CellBuilder
    {
        private readonly DeploymentBuilder _deploymentBuilder;
        private readonly DeploymentWriter _writer;
        private readonly DeploymentReader _reader;

        private readonly XElement _xml;

        internal CellBuilder(DeploymentBuilder deploymentBuilder, DeploymentWriter writer, DeploymentReader reader, XElement cellXml)
        {
            _deploymentBuilder = deploymentBuilder;
            _writer = writer;
            _reader = reader;
            _xml = cellXml;
        }

        public void FromExistingCell(string cellName)
        {
            var cell = _deploymentBuilder.GetCellXmlIfExists(cellName);
            if (cell == null)
            {
                throw new ArgumentException(string.Format("Cell with the name {0} not found in the current deployment", cellName), "cellName");
            }

            _xml.RemoveAll();
            _xml.Add(cell.Attributes());
            _xml.Add(cell.Elements());
        }

        public void FromExistingCell(string cellName, string deploymentName)
        {
            var cell = _reader.ReadDeploymentCellXml(deploymentName, cellName);

            _xml.RemoveAll();
            _xml.Add(cell.Attributes());
            _xml.Add(cell.Elements());
        }

        public void ExistingAssemblies(string assembliesName)
        {
            _xml.GetCreateElement("Assemblies").SetAttributeValue("name", assembliesName);
        }

        /// <summary>
        /// Include all the provided assemblies (dll) and symbols (pdb).
        /// </summary>
        /// <returns>The new assembliesName for the provided assemblies</returns>
        public string Assemblies(IEnumerable<FileInfo> assemblyFiles)
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    zip.AddFiles(assemblyFiles.OrderBy(f => f.Name).ThenBy(f => f.FullName).Select(f => f.FullName).Distinct());
                    zip.Save(stream);
                }

                return _writer.WriteAssembliesZip(stream.ToArray());
            }
        }

        /// <summary>
        /// Include all the assemblies (dll) and symbols (pdb) in the provided directory.
        /// </summary>
        /// <returns>The new assembliesName for the provided assemblies</returns>
        public string Assemblies(DirectoryInfo assembliesDirectory)
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    zip.AddSelectedFiles("(name = *.dll) OR (name = *.pdb)", assembliesDirectory.FullName, true);
                    zip.Save(stream);
                }

                return _writer.WriteAssembliesZip(stream.ToArray());
            }
        }

        public void EntryPoint(string typeName)
        {
            _xml.GetCreateElement("EntryPoint").SetAttributeValue("typeName", typeName);
        }

        public void EntryPoint(Type type)
        {
            EntryPoint(string.Concat(type.FullName, ", ", type.Assembly.GetName().Name));
        }

        public void CloudServicesEntryPoint()
        {
            EntryPoint("Lokad.Cloud.Services.AppEntryPoint.EntryPoint, Lokad.Cloud.Services.AppEntryPoint");
        }

        public void Settings(Action<XElement> configure)
        {
            configure(_xml.GetCreateElement("Settings"));
        }

        public void CloudServicesAutofacConfig(Action<AutofacConfigBuilder> configure)
        {
            Settings(settings => configure(new AutofacConfigBuilder(this, _writer, settings.GetCreateElement("Config"))));
        }

        public void CloudServicesSettings(Action<CloudServicesBuilder> configure)
        {
            Settings(settings => configure(new CloudServicesBuilder(this, _writer, settings.GetCreateElement("Services"))));
        }
    }
}