﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34003
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Dieser Quellcode wurde automatisch generiert von xsd, Version=4.0.30319.33440.
// 
namespace Itsg.Ostc2 {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntrag")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false, ElementName = "OSTCAntrag")]
    public partial class OstcAntrag : object, System.ComponentModel.INotifyPropertyChanged {
        
        private OstcAntragAntragsteller antragstellerField;
        
        private OstcAntragAntragsinfo antragsinfoField;
        
        private OstcAntragRechnungsadresse rechnungsadresseField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OstcAntragAntragsteller Antragsteller {
            get {
                return this.antragstellerField;
            }
            set {
                this.antragstellerField = value;
                this.RaisePropertyChanged("Antragsteller");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OstcAntragAntragsinfo Antragsinfo {
            get {
                return this.antragsinfoField;
            }
            set {
                this.antragsinfoField = value;
                this.RaisePropertyChanged("Antragsinfo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OstcAntragRechnungsadresse Rechnungsadresse {
            get {
                return this.rechnungsadresseField;
            }
            set {
                this.rechnungsadresseField = value;
                this.RaisePropertyChanged("Rechnungsadresse");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragAntragsteller")]
    public partial class OstcAntragAntragsteller : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string iK_BNField;
        
        private string firmaField;
        
        private string anredeField;
        
        private string vornameField;
        
        private string nachnameField;
        
        private string strasseField;
        
        private string hausnummerField;
        
        private string länderkennzeichenField;
        
        private string pLZField;
        
        private string ortField;
        
        private string telefonField;
        
        private string telefaxField;
        
        private string emailField;
        
        private string kennwortField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IK_BN {
            get {
                return this.iK_BNField;
            }
            set {
                this.iK_BNField = value;
                this.RaisePropertyChanged("IK_BN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Firma {
            get {
                return this.firmaField;
            }
            set {
                this.firmaField = value;
                this.RaisePropertyChanged("Firma");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Anrede {
            get {
                return this.anredeField;
            }
            set {
                this.anredeField = value;
                this.RaisePropertyChanged("Anrede");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vorname {
            get {
                return this.vornameField;
            }
            set {
                this.vornameField = value;
                this.RaisePropertyChanged("Vorname");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Nachname {
            get {
                return this.nachnameField;
            }
            set {
                this.nachnameField = value;
                this.RaisePropertyChanged("Nachname");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Strasse {
            get {
                return this.strasseField;
            }
            set {
                this.strasseField = value;
                this.RaisePropertyChanged("Strasse");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Hausnummer {
            get {
                return this.hausnummerField;
            }
            set {
                this.hausnummerField = value;
                this.RaisePropertyChanged("Hausnummer");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Länderkennzeichen {
            get {
                return this.länderkennzeichenField;
            }
            set {
                this.länderkennzeichenField = value;
                this.RaisePropertyChanged("Länderkennzeichen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PLZ {
            get {
                return this.pLZField;
            }
            set {
                this.pLZField = value;
                this.RaisePropertyChanged("PLZ");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Ort {
            get {
                return this.ortField;
            }
            set {
                this.ortField = value;
                this.RaisePropertyChanged("Ort");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Telefon {
            get {
                return this.telefonField;
            }
            set {
                this.telefonField = value;
                this.RaisePropertyChanged("Telefon");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Telefax {
            get {
                return this.telefaxField;
            }
            set {
                this.telefaxField = value;
                this.RaisePropertyChanged("Telefax");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
                this.RaisePropertyChanged("Email");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Kennwort {
            get {
                return this.kennwortField;
            }
            set {
                this.kennwortField = value;
                this.RaisePropertyChanged("Kennwort");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragAntragsinfo")]
    public partial class OstcAntragAntragsinfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string softwarehausField;
        
        private string fachanwendungField;
        
        private string dakota_LizenzField;
        
        private string datumField;
        
        private string uhrzeitField;
        
        private byte[] requestschlüsselField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Softwarehaus {
            get {
                return this.softwarehausField;
            }
            set {
                this.softwarehausField = value;
                this.RaisePropertyChanged("Softwarehaus");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Fachanwendung {
            get {
                return this.fachanwendungField;
            }
            set {
                this.fachanwendungField = value;
                this.RaisePropertyChanged("Fachanwendung");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Dakota_Lizenz {
            get {
                return this.dakota_LizenzField;
            }
            set {
                this.dakota_LizenzField = value;
                this.RaisePropertyChanged("Dakota_Lizenz");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Datum {
            get {
                return this.datumField;
            }
            set {
                this.datumField = value;
                this.RaisePropertyChanged("Datum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Uhrzeit {
            get {
                return this.uhrzeitField;
            }
            set {
                this.uhrzeitField = value;
                this.RaisePropertyChanged("Uhrzeit");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")]
        public byte[] Requestschlüssel {
            get {
                return this.requestschlüsselField;
            }
            set {
                this.requestschlüsselField = value;
                this.RaisePropertyChanged("Requestschlüssel");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragRechnungsadresse")]
    public partial class OstcAntragRechnungsadresse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string re_FirmaField;
        
        private string re_AnredeField;
        
        private string re_VornameField;
        
        private string re_NachnameField;
        
        private string re_StrasseField;
        
        private string re_HausnummerField;
        
        private string re_PostfachField;
        
        private string re_LänderkennzeichenField;
        
        private string re_PLZField;
        
        private string re_OrtField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Firma {
            get {
                return this.re_FirmaField;
            }
            set {
                this.re_FirmaField = value;
                this.RaisePropertyChanged("Re_Firma");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Anrede {
            get {
                return this.re_AnredeField;
            }
            set {
                this.re_AnredeField = value;
                this.RaisePropertyChanged("Re_Anrede");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Vorname {
            get {
                return this.re_VornameField;
            }
            set {
                this.re_VornameField = value;
                this.RaisePropertyChanged("Re_Vorname");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Nachname {
            get {
                return this.re_NachnameField;
            }
            set {
                this.re_NachnameField = value;
                this.RaisePropertyChanged("Re_Nachname");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Strasse {
            get {
                return this.re_StrasseField;
            }
            set {
                this.re_StrasseField = value;
                this.RaisePropertyChanged("Re_Strasse");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Hausnummer {
            get {
                return this.re_HausnummerField;
            }
            set {
                this.re_HausnummerField = value;
                this.RaisePropertyChanged("Re_Hausnummer");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Postfach {
            get {
                return this.re_PostfachField;
            }
            set {
                this.re_PostfachField = value;
                this.RaisePropertyChanged("Re_Postfach");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Länderkennzeichen {
            get {
                return this.re_LänderkennzeichenField;
            }
            set {
                this.re_LänderkennzeichenField = value;
                this.RaisePropertyChanged("Re_Länderkennzeichen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_PLZ {
            get {
                return this.re_PLZField;
            }
            set {
                this.re_PLZField = value;
                this.RaisePropertyChanged("Re_PLZ");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Re_Ort {
            get {
                return this.re_OrtField;
            }
            set {
                this.re_OrtField = value;
                this.RaisePropertyChanged("Re_Ort");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
