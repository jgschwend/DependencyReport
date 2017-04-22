using System;
using System.Collections.Generic;
using System.Text;

namespace Zuehlke.DependencyReport.Console.PackageManagement
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRoot("packages")]
    public class NugetPackageConfig
    {
        private NugetPackage[] _nugetField;

        [System.Xml.Serialization.XmlElementAttribute("package")]
        public NugetPackage[] Nuget
        {
            get { return this._nugetField; }
            set { this._nugetField = value; }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class NugetPackage
    {
        private string _idField;
        private string _versionField;
        private string _targetFrameworkField;
        private bool _developmentDependencyField;
        private bool _developmentDependencyFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute("id")]
        public string id
        {
            get { return this._idField; }
            set { this._idField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("version")]
        public string version
        {
            get { return this._versionField; }
            set { this._versionField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("targetFramework")]
        public string TargetFramework
        {
            get { return this._targetFrameworkField; }
            set { this._targetFrameworkField = value; }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("developmentDependency")]
        public bool DevelopmentDependency
        {
            get { return this._developmentDependencyField; }
            set { this._developmentDependencyField = value; }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DevelopmentDependencySpecified
        {
            get { return this._developmentDependencyFieldSpecified; }
            set { this._developmentDependencyFieldSpecified = value; }
        }
    }
}
