using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Mews.Eet.Communication
{
    public enum SignAlgorithm
    {
        Sha1 = 0,
        Sha256 = 1
    }

    public class SoapSigner
    {
        public SoapSigner(X509Certificate2 certificate, SignAlgorithm signAlgorithm)
        {
            SecurityToken = Convert.ToBase64String(certificate.RawData);
            SignatureMethod = GetSignatureMethodUri(signAlgorithm);
            DigestMethod = GetDigestMethod(signAlgorithm);
            RsaKey = GetRsaKey(signAlgorithm, certificate);
        }


        private string SecurityToken { get; }

        private string SignatureMethod { get; }

        private string DigestMethod { get; }

        private RSACryptoServiceProvider RsaKey { get; }

        public XmlDocument SignMessage(XmlDocument xmlDoc)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

            var soapHeaderNode = xmlDoc.DocumentElement.SelectSingleNode("//s:Header", namespaceManager) as XmlElement;
            var bodyNode = xmlDoc.DocumentElement.SelectSingleNode("//s:Body", namespaceManager) as XmlElement;

            if (bodyNode == null)
            {
                throw new Exception("No body tag found.");
            }

            var securityNode = xmlDoc.CreateElement(
                prefix: "wsse",
                localName: "Security",
                namespaceURI: "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"
            );

            var binarySecurityTokenElement = xmlDoc.CreateElement("wse", "BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            binarySecurityTokenElement.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            binarySecurityTokenElement.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            binarySecurityTokenElement.SetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", "BinaryToken1");
            binarySecurityTokenElement.InnerText = SecurityToken;

            securityNode.AppendChild(binarySecurityTokenElement);
            soapHeaderNode.AppendChild(securityNode);

            string digest;
            string signedInfo;
            string signature;
            using (var sha = SHA256.Create())
            {
                digest = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(bodyNode.InnerXml)));
                signedInfo = FillSignedInfo(SignatureMethod, DigestMethod, digest);
                signature = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(signedInfo)));
            }

            var signatureElementString = FillSignatureTemplate(signedInfo, signature);
            var signedElement = new XmlDocument();

            signedElement.LoadXml(signatureElementString);

           /* var signedXml = new SignedXmlWithId(xmlDoc);
            signedXml.SignedInfo.SignatureMethod = SignatureMethod;
            signedXml.SigningKey = RsaKey;

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new SecurityTokenReference("BinaryToken1"));
            signedXml.KeyInfo = keyInfo;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            var reference = new Reference();
            reference.Uri = "#_1";
            reference.DigestMethod = DigestMethod;

            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            var signedElement = signedXml.GetXml();*/
            securityNode.AppendChild(signedElement);

            return xmlDoc;
        }

        private RSACryptoServiceProvider GetRsaKey(SignAlgorithm signAlgorithm, X509Certificate2 certificate)
        {
           /* if (signAlgorithm == SignAlgorithm.Sha1)
            {
                return certificate.GetRSAPrivateKey() ;
            }

            if (signAlgorithm == SignAlgorithm.Sha256)
            {*/
                var key = certificate.GetRSAPrivateKey();// as RSACryptoServiceProvider;
                var enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
                var cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, enhCsp.KeyContainerName);
                return new RSACryptoServiceProvider(cspparams);
            /*}

            throw new ArgumentException($"Unsupported signing algorithm {signAlgorithm}.");*/
        }

        private string GetSignatureMethodUri(SignAlgorithm signAlgorithm)
        {
            if (signAlgorithm == SignAlgorithm.Sha1)
            {
                return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            }
            if (signAlgorithm == SignAlgorithm.Sha256)
            {
                return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            }

            throw new ArgumentException($"Unsupported signing algorithm {signAlgorithm}.");
        }

        private string GetDigestMethod(SignAlgorithm signAlgorithm)
        {
            if (signAlgorithm == SignAlgorithm.Sha1)
            {
                return "http://www.w3.org/2000/09/xmldsig#sha1";
            }
            if (signAlgorithm == SignAlgorithm.Sha256)
            {
                return "http://www.w3.org/2001/04/xmlenc#sha256";
            }

            throw new ArgumentException($"Unsupported signing algorithm {signAlgorithm}.");
        }

        private string FillSignedInfo(
            string signatureMethodAlgo,
            string digestMethod,
            string digest)
        {
            return string.Format(SignatureTemplate, 
                signatureMethodAlgo,
                digestMethod,
                digest);
        }

        private string FillSignatureTemplate(string signedInfo, string signature)
        {
            return string.Format(SignatureTemplate, signedInfo, signature);
        }

        private const string SignedInfo =
            "<ds:SignedInfo> " +
                "<ds:CanonicalizationMethod Algorithm =\"http://www.w3.org/2001/10/xml-exc-c14n#\">" +
                  "<ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"soap\"/>" +
                "</ds:CanonicalizationMethod>" +
                "<ds:SignatureMethod Algorithm =\"{0}\"/>" +
                "<ds:Reference URI =\"#_1\">" +
                  "<ds:Transforms>" +
                    "<ds:Transform Algorithm =\"http://www.w3.org/2001/10/xml-exc-c14n#\">" +
                      "<ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"\"/>" +
                    "</ds:Transform>" +
                  "</ds:Transforms>" +
                  "<ds:DigestMethod Algorithm =\"{1}\"/>" +
                  "<ds:DigestValue>{2}</ds:DigestValue>" +
                "</ds:Reference>" +
              "</ds:SignedInfo>";

        private const string SignatureTemplate =
            "<ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"SIG-AB79979F3364F5119A14761286404065\">" +
              "{0}" +
              "<ds:SignatureValue>{1}</ds:SignatureValue>" +
              "<ds:KeyInfo Id =\"KI-AB79979F3364F5119A14761286403862\">" +
                "<wsse:SecurityTokenReference wsu:Id=\"STR-AB79979F3364F5119A14761286403893\">" +
                  "<wsse:Reference URI =\"#BinaryToken1\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\"/>" +
                "</wsse:SecurityTokenReference>" +
              "</ds:KeyInfo>" +
            "</ds:Signature>";
    }
}