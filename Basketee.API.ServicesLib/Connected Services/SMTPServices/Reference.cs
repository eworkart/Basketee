﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Basketee.API.SMTPServices {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Common.ResponseMessage", Namespace="http://schemas.datacontract.org/2004/07/WCF_MessagingDirect")]
    [System.SerializableAttribute()]
    public partial class CommonResponseMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescMsgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Code {
            get {
                return this.CodeField;
            }
            set {
                if ((object.ReferenceEquals(this.CodeField, value) != true)) {
                    this.CodeField = value;
                    this.RaisePropertyChanged("Code");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DescMsg {
            get {
                return this.DescMsgField;
            }
            set {
                if ((object.ReferenceEquals(this.DescMsgField, value) != true)) {
                    this.DescMsgField = value;
                    this.RaisePropertyChanged("DescMsg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Common.AttachmentDoc", Namespace="http://schemas.datacontract.org/2004/07/WCF_MessagingDirect")]
    [System.SerializableAttribute()]
    public partial class CommonAttachmentDoc : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] filebyteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string filenameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] filebyte {
            get {
                return this.filebyteField;
            }
            set {
                if ((object.ReferenceEquals(this.filebyteField, value) != true)) {
                    this.filebyteField = value;
                    this.RaisePropertyChanged("filebyte");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string filename {
            get {
                return this.filenameField;
            }
            set {
                if ((object.ReferenceEquals(this.filenameField, value) != true)) {
                    this.filenameField = value;
                    this.RaisePropertyChanged("filename");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SMTPServices.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendMessage", ReplyAction="http://tempuri.org/IService1/SendMessageResponse")]
        Basketee.API.SMTPServices.CommonResponseMessage SendMessage(string userName, string password, string To, string CC, string BCC, string Subject, string Content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendMessage", ReplyAction="http://tempuri.org/IService1/SendMessageResponse")]
        System.Threading.Tasks.Task<Basketee.API.SMTPServices.CommonResponseMessage> SendMessageAsync(string userName, string password, string To, string CC, string BCC, string Subject, string Content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendMessageAtt", ReplyAction="http://tempuri.org/IService1/SendMessageAttResponse")]
        Basketee.API.SMTPServices.CommonResponseMessage SendMessageAtt(string userName, string password, string To, string CC, string BCC, string Subject, string Content, Basketee.API.SMTPServices.CommonAttachmentDoc[] Attachment);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SendMessageAtt", ReplyAction="http://tempuri.org/IService1/SendMessageAttResponse")]
        System.Threading.Tasks.Task<Basketee.API.SMTPServices.CommonResponseMessage> SendMessageAttAsync(string userName, string password, string To, string CC, string BCC, string Subject, string Content, Basketee.API.SMTPServices.CommonAttachmentDoc[] Attachment);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : Basketee.API.SMTPServices.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<Basketee.API.SMTPServices.IService1>, Basketee.API.SMTPServices.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Basketee.API.SMTPServices.CommonResponseMessage SendMessage(string userName, string password, string To, string CC, string BCC, string Subject, string Content) {
            return base.Channel.SendMessage(userName, password, To, CC, BCC, Subject, Content);
        }
        
        public System.Threading.Tasks.Task<Basketee.API.SMTPServices.CommonResponseMessage> SendMessageAsync(string userName, string password, string To, string CC, string BCC, string Subject, string Content) {
            return base.Channel.SendMessageAsync(userName, password, To, CC, BCC, Subject, Content);
        }
        
        public Basketee.API.SMTPServices.CommonResponseMessage SendMessageAtt(string userName, string password, string To, string CC, string BCC, string Subject, string Content, Basketee.API.SMTPServices.CommonAttachmentDoc[] Attachment) {
            return base.Channel.SendMessageAtt(userName, password, To, CC, BCC, Subject, Content, Attachment);
        }
        
        public System.Threading.Tasks.Task<Basketee.API.SMTPServices.CommonResponseMessage> SendMessageAttAsync(string userName, string password, string To, string CC, string BCC, string Subject, string Content, Basketee.API.SMTPServices.CommonAttachmentDoc[] Attachment) {
            return base.Channel.SendMessageAttAsync(userName, password, To, CC, BCC, Subject, Content, Attachment);
        }
    }
}
