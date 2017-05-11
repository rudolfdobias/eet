using System.Xml;

namespace System.Security.Cryptography.Xml
{
    /// <summary>
    /// TODO: Remove this with .NET Core 2.0
    /// </summary>
    public abstract class KeyInfoClause
    {
        //
        // protected constructors
        //

        protected KeyInfoClause() { }

        //
        // public methods
        //

        public abstract XmlElement GetXml();
        internal virtual XmlElement GetXml(XmlDocument xmlDocument)
        {
            XmlElement keyInfo = GetXml();
            return (XmlElement)xmlDocument.ImportNode(keyInfo, true);
        }

        public abstract void LoadXml(XmlElement element);
    }
}