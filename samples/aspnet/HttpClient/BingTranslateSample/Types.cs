namespace BingTranslate
{
    // These types were copied from the BING translate documentation

    /// <summary>
    /// Classes for GetTranslations method deserilization
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", IsNullable = true)]
    public partial class GetTranslationsResponse
    {
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string From { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string State { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
        public TranslationMatch[] Translations { get; set; }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", IsNullable = true)]
    public partial class TranslationMatch
    {
        public int Count { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string Error { get; set; }

        public int MatchDegree { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string MatchedOriginalText { get; set; }

        public int Rating { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TranslatedText { get; set; }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", IsNullable = true)]
    public partial class ArrayOfTranslationMatch
    {

        [System.Xml.Serialization.XmlElementAttribute("TranslationMatch", IsNullable = true)]
        public TranslationMatch[] TranslationMatch { get; set; }
    }

    /// <summary>
    /// Class for GetTranslationsArray
    /// </summary>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", IsNullable = true)]
    public partial class ArrayOfGetTranslationsResponse
    {

        [System.Xml.Serialization.XmlElementAttribute("GetTranslationsResponse", IsNullable = true)]
        public GetTranslationsResponse[] GetTranslationsResponse { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2", IsNullable = false)]
    public partial class TranslateOptions
    {
        private string categoryField;
        private string contentTypeField;
        private object reservedFlagsField;
        private string stateField;
        private string uriField;
        private string userField;

        public TranslateOptions()
        { 
        }

        public TranslateOptions(string uri, string user)
        {
            this.Uri = uri;
            this.User = user;
        }

        /// <remarks/>
        public string Category
        {
            get { return this.categoryField; }
            set { this.categoryField = value; }
        }

        /// <remarks/>
        public string ContentType
        {
            get { return this.contentTypeField; }
            set { this.contentTypeField = value; }
        }

        /// <remarks/>
        public object ReservedFlags
        {
            get { return this.reservedFlagsField; }
            set { this.reservedFlagsField = value; }
        }

        /// <remarks/>
        public string State
        {
            get { return this.stateField; }
            set { this.stateField = value; }
        }

        /// <remarks/>
        public string Uri
        {
            get { return this.uriField; }
            set { this.uriField = value; }
        }

        /// <remarks/>
        public string User
        {
            get { return this.userField; }
            set { this.userField = value; }
        }
    }
}
