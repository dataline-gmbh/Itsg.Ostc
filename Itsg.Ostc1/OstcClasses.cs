namespace Itsg.Ostc1 {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlType(AnonymousType=true, TypeName = "OSTCAntrag")]
    [XmlRootAttribute(Namespace="", IsNullable=false, ElementName = "OSTCAntrag")]
    public partial class OstcAntrag : object, System.ComponentModel.INotifyPropertyChanged {
        
        private OstcAntragTrustcenter _trustcenterField;
        
        private OstcAntragAntragsteller _antragstellerField;
        
        private OstcAntragAntragsinfo _antragsinfoField;
        
        private OstcAntragRechnungsadresse _rechnungsadresseField;
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public OstcAntragTrustcenter Trustcenter {
            get {
                return _trustcenterField;
            }
            set {
                _trustcenterField = value;
                RaisePropertyChanged("Trustcenter");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public OstcAntragAntragsteller Antragsteller {
            get {
                return _antragstellerField;
            }
            set {
                _antragstellerField = value;
                RaisePropertyChanged("Antragsteller");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public OstcAntragAntragsinfo Antragsinfo {
            get {
                return _antragsinfoField;
            }
            set {
                _antragsinfoField = value;
                RaisePropertyChanged("Antragsinfo");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public OstcAntragRechnungsadresse Rechnungsadresse {
            get {
                return _rechnungsadresseField;
            }
            set {
                _rechnungsadresseField = value;
                RaisePropertyChanged("Rechnungsadresse");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragTrustcenter")]
    public class OstcAntragTrustcenter : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string _returncodeField;
        
        private string _fehlercodeField;
        
        private string _eingangsnummerField;
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Returncode {
            get {
                return _returncodeField;
            }
            set {
                _returncodeField = value;
                RaisePropertyChanged("Returncode");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Fehlercode {
            get {
                return _fehlercodeField;
            }
            set {
                _fehlercodeField = value;
                RaisePropertyChanged("Fehlercode");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Eingangsnummer {
            get {
                return _eingangsnummerField;
            }
            set {
                _eingangsnummerField = value;
                RaisePropertyChanged("Eingangsnummer");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragAntragsteller")]
    public class OstcAntragAntragsteller : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string _ikbnField;
        
        private string _firmaField;
        
        private string _anredeField;
        
        private string _vornameField;
        
        private string _nachnameField;
        
        private string _strasseField;
        
        private string _hausnummerField;
        
        private string _pLzField;
        
        private string _ortField;
        
        private string _telefonField;
        
        private string _telefaxField;
        
        private string _emailField;
        
        private string _kennwortField;
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string IK_BN {
            get {
                return _ikbnField;
            }
            set {
                _ikbnField = value;
                RaisePropertyChanged("IK_BN");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Firma {
            get {
                return _firmaField;
            }
            set {
                _firmaField = value;
                RaisePropertyChanged("Firma");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Anrede {
            get {
                return _anredeField;
            }
            set {
                _anredeField = value;
                RaisePropertyChanged("Anrede");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Vorname {
            get {
                return _vornameField;
            }
            set {
                _vornameField = value;
                RaisePropertyChanged("Vorname");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Nachname {
            get {
                return _nachnameField;
            }
            set {
                _nachnameField = value;
                RaisePropertyChanged("Nachname");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Strasse {
            get {
                return _strasseField;
            }
            set {
                _strasseField = value;
                RaisePropertyChanged("Strasse");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Hausnummer {
            get {
                return _hausnummerField;
            }
            set {
                _hausnummerField = value;
                RaisePropertyChanged("Hausnummer");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string PLZ {
            get {
                return _pLzField;
            }
            set {
                _pLzField = value;
                RaisePropertyChanged("PLZ");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Ort {
            get {
                return _ortField;
            }
            set {
                _ortField = value;
                RaisePropertyChanged("Ort");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string Telefon {
            get {
                return _telefonField;
            }
            set {
                _telefonField = value;
                RaisePropertyChanged("Telefon");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string Telefax {
            get {
                return _telefaxField;
            }
            set {
                _telefaxField = value;
                RaisePropertyChanged("Telefax");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string Email {
            get {
                return _emailField;
            }
            set {
                _emailField = value;
                RaisePropertyChanged("Email");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string Kennwort {
            get {
                return _kennwortField;
            }
            set {
                _kennwortField = value;
                RaisePropertyChanged("Kennwort");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragAntragsinfo")]
    public class OstcAntragAntragsinfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string _ruecksendungField;
        
        private string _generierungField;
        
        private string _sperrungField;
        
        private string _softwarehausField;
        
        private string _fachanwendungField;
        
        private string _dakotaLizenzField;
        
        private string _datumField;
        
        private string _uhrzeitField;
        
        private string _bemerkungField;
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Ruecksendung {
            get {
                return _ruecksendungField;
            }
            set {
                _ruecksendungField = value;
                RaisePropertyChanged("Ruecksendung");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Generierung {
            get {
                return _generierungField;
            }
            set {
                _generierungField = value;
                RaisePropertyChanged("Generierung");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Sperrung {
            get {
                return _sperrungField;
            }
            set {
                _sperrungField = value;
                RaisePropertyChanged("Sperrung");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Softwarehaus {
            get {
                return _softwarehausField;
            }
            set {
                _softwarehausField = value;
                RaisePropertyChanged("Softwarehaus");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Fachanwendung {
            get {
                return _fachanwendungField;
            }
            set {
                _fachanwendungField = value;
                RaisePropertyChanged("Fachanwendung");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Dakota_Lizenz {
            get {
                return _dakotaLizenzField;
            }
            set {
                _dakotaLizenzField = value;
                RaisePropertyChanged("Dakota_Lizenz");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Datum {
            get {
                return _datumField;
            }
            set {
                _datumField = value;
                RaisePropertyChanged("Datum");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string Uhrzeit {
            get {
                return _uhrzeitField;
            }
            set {
                _uhrzeitField = value;
                RaisePropertyChanged("Uhrzeit");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Bemerkung {
            get {
                return _bemerkungField;
            }
            set {
                _bemerkungField = value;
                RaisePropertyChanged("Bemerkung");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType=true, TypeName = "OSTCAntragRechnungsadresse")]
    public class OstcAntragRechnungsadresse : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string _reFirmaField;
        
        private string _reAnredeField;
        
        private string _reVornameField;
        
        private string _reNachnameField;
        
        private string _reStrasseField;
        
        private string _reHausnummerField;
        
        private string _rePostfachField;
        
        private string _rePLZField;
        
        private string _reOrtField;
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Re_Firma {
            get {
                return _reFirmaField;
            }
            set {
                _reFirmaField = value;
                RaisePropertyChanged("Re_Firma");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Re_Anrede {
            get {
                return _reAnredeField;
            }
            set {
                _reAnredeField = value;
                RaisePropertyChanged("Re_Anrede");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Re_Vorname {
            get {
                return _reVornameField;
            }
            set {
                _reVornameField = value;
                RaisePropertyChanged("Re_Vorname");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Re_Nachname {
            get {
                return _reNachnameField;
            }
            set {
                _reNachnameField = value;
                RaisePropertyChanged("Re_Nachname");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Re_Strasse {
            get {
                return _reStrasseField;
            }
            set {
                _reStrasseField = value;
                RaisePropertyChanged("Re_Strasse");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Re_Hausnummer {
            get {
                return _reHausnummerField;
            }
            set {
                _reHausnummerField = value;
                RaisePropertyChanged("Re_Hausnummer");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string Re_Postfach {
            get {
                return _rePostfachField;
            }
            set {
                _rePostfachField = value;
                RaisePropertyChanged("Re_Postfach");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string Re_PLZ {
            get {
                return _rePLZField;
            }
            set {
                _rePLZField = value;
                RaisePropertyChanged("Re_PLZ");
            }
        }
        
        /// <remarks/>
        [XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Re_Ort {
            get {
                return _reOrtField;
            }
            set {
                _reOrtField = value;
                RaisePropertyChanged("Re_Ort");
            }
        }

        /// <remarks/>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <remarks/>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
