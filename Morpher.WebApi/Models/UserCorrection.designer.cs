﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Morpher.WebService.V3.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="slepov_v5rpj")]
	public partial class UserCorrectionDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUserVote(UserVote instance);
    partial void UpdateUserVote(UserVote instance);
    partial void DeleteUserVote(UserVote instance);
    partial void InsertNameForm(NameForm instance);
    partial void UpdateNameForm(NameForm instance);
    partial void DeleteNameForm(NameForm instance);
    partial void InsertName(Name instance);
    partial void UpdateName(Name instance);
    partial void DeleteName(Name instance);
    #endregion
		
		public UserCorrectionDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["slepov_v5rpjConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public UserCorrectionDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UserCorrectionDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UserCorrectionDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UserCorrectionDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<UserVote> UserVotes
		{
			get
			{
				return this.GetTable<UserVote>();
			}
		}
		
		public System.Data.Linq.Table<NameForm> NameForms
		{
			get
			{
				return this.GetTable<NameForm>();
			}
		}
		
		public System.Data.Linq.Table<Name> Names
		{
			get
			{
				return this.GetTable<Name>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.sp_GetForms")]
		public ISingleResult<sp_GetFormsResult> sp_GetForms([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string normalizedLemma, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Char(2)")] string language, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="UniqueIdentifier")] System.Nullable<System.Guid> token)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), normalizedLemma, language, token);
			return ((ISingleResult<sp_GetFormsResult>)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.UserVotes")]
	public partial class UserVote : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _UserID;
		
		private System.Guid _NameID;
		
		private System.DateTime _SubmittedUTC;
		
		private string _Comment;
		
		private EntityRef<Name> _Name;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIDChanging(System.Guid value);
    partial void OnUserIDChanged();
    partial void OnNameIDChanging(System.Guid value);
    partial void OnNameIDChanged();
    partial void OnSubmittedUTCChanging(System.DateTime value);
    partial void OnSubmittedUTCChanged();
    partial void OnCommentChanging(string value);
    partial void OnCommentChanged();
    #endregion
		
		public UserVote()
		{
			this._Name = default(EntityRef<Name>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NameID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid NameID
		{
			get
			{
				return this._NameID;
			}
			set
			{
				if ((this._NameID != value))
				{
					if (this._Name.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnNameIDChanging(value);
					this.SendPropertyChanging();
					this._NameID = value;
					this.SendPropertyChanged("NameID");
					this.OnNameIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SubmittedUTC", DbType="Date NOT NULL")]
		public System.DateTime SubmittedUTC
		{
			get
			{
				return this._SubmittedUTC;
			}
			set
			{
				if ((this._SubmittedUTC != value))
				{
					this.OnSubmittedUTCChanging(value);
					this.SendPropertyChanging();
					this._SubmittedUTC = value;
					this.SendPropertyChanged("SubmittedUTC");
					this.OnSubmittedUTCChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Comment", DbType="NVarChar(MAX)")]
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if ((this._Comment != value))
				{
					this.OnCommentChanging(value);
					this.SendPropertyChanging();
					this._Comment = value;
					this.SendPropertyChanged("Comment");
					this.OnCommentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_UserVote", Storage="_Name", ThisKey="NameID", OtherKey="ID", IsForeignKey=true)]
		public Name Name
		{
			get
			{
				return this._Name.Entity;
			}
			set
			{
				Name previousValue = this._Name.Entity;
				if (((previousValue != value) 
							|| (this._Name.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Name.Entity = null;
						previousValue.UserVotes.Remove(this);
					}
					this._Name.Entity = value;
					if ((value != null))
					{
						value.UserVotes.Add(this);
						this._NameID = value.ID;
					}
					else
					{
						this._NameID = default(System.Guid);
					}
					this.SendPropertyChanged("Name");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.NameForms")]
	public partial class NameForm : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _NameID;
		
		private char _FormID;
		
		private bool _Plural;
		
		private string _LanguageID;
		
		private string _AccentedText;
		
		private EntityRef<Name> _Name;
		
		private EntityRef<Name> _Name1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnNameIDChanging(System.Guid value);
    partial void OnNameIDChanged();
    partial void OnFormIDChanging(char value);
    partial void OnFormIDChanged();
    partial void OnPluralChanging(bool value);
    partial void OnPluralChanged();
    partial void OnLanguageIDChanging(string value);
    partial void OnLanguageIDChanged();
    partial void OnAccentedTextChanging(string value);
    partial void OnAccentedTextChanged();
    #endregion
		
		public NameForm()
		{
			this._Name = default(EntityRef<Name>);
			this._Name1 = default(EntityRef<Name>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NameID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid NameID
		{
			get
			{
				return this._NameID;
			}
			set
			{
				if ((this._NameID != value))
				{
					if ((this._Name.HasLoadedOrAssignedValue || this._Name1.HasLoadedOrAssignedValue))
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnNameIDChanging(value);
					this.SendPropertyChanging();
					this._NameID = value;
					this.SendPropertyChanged("NameID");
					this.OnNameIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormID", DbType="NChar(1) NOT NULL", IsPrimaryKey=true)]
		public char FormID
		{
			get
			{
				return this._FormID;
			}
			set
			{
				if ((this._FormID != value))
				{
					this.OnFormIDChanging(value);
					this.SendPropertyChanging();
					this._FormID = value;
					this.SendPropertyChanged("FormID");
					this.OnFormIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Plural", DbType="Bit NOT NULL", IsPrimaryKey=true)]
		public bool Plural
		{
			get
			{
				return this._Plural;
			}
			set
			{
				if ((this._Plural != value))
				{
					this.OnPluralChanging(value);
					this.SendPropertyChanging();
					this._Plural = value;
					this.SendPropertyChanged("Plural");
					this.OnPluralChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LanguageID", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string LanguageID
		{
			get
			{
				return this._LanguageID;
			}
			set
			{
				if ((this._LanguageID != value))
				{
					if (this._Name1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnLanguageIDChanging(value);
					this.SendPropertyChanging();
					this._LanguageID = value;
					this.SendPropertyChanged("LanguageID");
					this.OnLanguageIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AccentedText", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string AccentedText
		{
			get
			{
				return this._AccentedText;
			}
			set
			{
				if ((this._AccentedText != value))
				{
					this.OnAccentedTextChanging(value);
					this.SendPropertyChanging();
					this._AccentedText = value;
					this.SendPropertyChanged("AccentedText");
					this.OnAccentedTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_NameForm", Storage="_Name", ThisKey="NameID", OtherKey="ID", IsForeignKey=true)]
		public Name Name
		{
			get
			{
				return this._Name.Entity;
			}
			set
			{
				Name previousValue = this._Name.Entity;
				if (((previousValue != value) 
							|| (this._Name.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Name.Entity = null;
						previousValue.NameForms.Remove(this);
					}
					this._Name.Entity = value;
					if ((value != null))
					{
						value.NameForms.Add(this);
						this._NameID = value.ID;
					}
					else
					{
						this._NameID = default(System.Guid);
					}
					this.SendPropertyChanged("Name");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_NameForm1", Storage="_Name1", ThisKey="NameID,LanguageID", OtherKey="ID,LanguageID", IsForeignKey=true)]
		public Name Name1
		{
			get
			{
				return this._Name1.Entity;
			}
			set
			{
				Name previousValue = this._Name1.Entity;
				if (((previousValue != value) 
							|| (this._Name1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Name1.Entity = null;
						previousValue.NameForms1.Remove(this);
					}
					this._Name1.Entity = value;
					if ((value != null))
					{
						value.NameForms1.Add(this);
						this._NameID = value.ID;
						this._LanguageID = value.LanguageID;
					}
					else
					{
						this._NameID = default(System.Guid);
						this._LanguageID = default(string);
					}
					this.SendPropertyChanged("Name1");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Names")]
	public partial class Name : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private string _Lemma;
		
		private bool _Verified;
		
		private string _LanguageID;
		
		private EntitySet<UserVote> _UserVotes;
		
		private EntitySet<NameForm> _NameForms;
		
		private EntitySet<NameForm> _NameForms1;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnLemmaChanging(string value);
    partial void OnLemmaChanged();
    partial void OnVerifiedChanging(bool value);
    partial void OnVerifiedChanged();
    partial void OnLanguageIDChanging(string value);
    partial void OnLanguageIDChanged();
    #endregion
		
		public Name()
		{
			this._UserVotes = new EntitySet<UserVote>(new Action<UserVote>(this.attach_UserVotes), new Action<UserVote>(this.detach_UserVotes));
			this._NameForms = new EntitySet<NameForm>(new Action<NameForm>(this.attach_NameForms), new Action<NameForm>(this.detach_NameForms));
			this._NameForms1 = new EntitySet<NameForm>(new Action<NameForm>(this.attach_NameForms1), new Action<NameForm>(this.detach_NameForms1));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Lemma", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string Lemma
		{
			get
			{
				return this._Lemma;
			}
			set
			{
				if ((this._Lemma != value))
				{
					this.OnLemmaChanging(value);
					this.SendPropertyChanging();
					this._Lemma = value;
					this.SendPropertyChanged("Lemma");
					this.OnLemmaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Verified", DbType="Bit NOT NULL")]
		public bool Verified
		{
			get
			{
				return this._Verified;
			}
			set
			{
				if ((this._Verified != value))
				{
					this.OnVerifiedChanging(value);
					this.SendPropertyChanging();
					this._Verified = value;
					this.SendPropertyChanged("Verified");
					this.OnVerifiedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LanguageID", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string LanguageID
		{
			get
			{
				return this._LanguageID;
			}
			set
			{
				if ((this._LanguageID != value))
				{
					this.OnLanguageIDChanging(value);
					this.SendPropertyChanging();
					this._LanguageID = value;
					this.SendPropertyChanged("LanguageID");
					this.OnLanguageIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_UserVote", Storage="_UserVotes", ThisKey="ID", OtherKey="NameID")]
		public EntitySet<UserVote> UserVotes
		{
			get
			{
				return this._UserVotes;
			}
			set
			{
				this._UserVotes.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_NameForm", Storage="_NameForms", ThisKey="ID", OtherKey="NameID")]
		public EntitySet<NameForm> NameForms
		{
			get
			{
				return this._NameForms;
			}
			set
			{
				this._NameForms.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Name_NameForm1", Storage="_NameForms1", ThisKey="ID,LanguageID", OtherKey="NameID,LanguageID")]
		public EntitySet<NameForm> NameForms1
		{
			get
			{
				return this._NameForms1;
			}
			set
			{
				this._NameForms1.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_UserVotes(UserVote entity)
		{
			this.SendPropertyChanging();
			entity.Name = this;
		}
		
		private void detach_UserVotes(UserVote entity)
		{
			this.SendPropertyChanging();
			entity.Name = null;
		}
		
		private void attach_NameForms(NameForm entity)
		{
			this.SendPropertyChanging();
			entity.Name = this;
		}
		
		private void detach_NameForms(NameForm entity)
		{
			this.SendPropertyChanging();
			entity.Name = null;
		}
		
		private void attach_NameForms1(NameForm entity)
		{
			this.SendPropertyChanging();
			entity.Name1 = this;
		}
		
		private void detach_NameForms1(NameForm entity)
		{
			this.SendPropertyChanging();
			entity.Name1 = null;
		}
	}
	
	public partial class sp_GetFormsResult
	{
		
		private System.Guid _NameID;
		
		private char _FormID;
		
		private bool _Plural;
		
		private string _LanguageID;
		
		private string _AccentedText;
		
		public sp_GetFormsResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NameID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid NameID
		{
			get
			{
				return this._NameID;
			}
			set
			{
				if ((this._NameID != value))
				{
					this._NameID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FormID", DbType="NChar(1) NOT NULL")]
		public char FormID
		{
			get
			{
				return this._FormID;
			}
			set
			{
				if ((this._FormID != value))
				{
					this._FormID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Plural", DbType="Bit NOT NULL")]
		public bool Plural
		{
			get
			{
				return this._Plural;
			}
			set
			{
				if ((this._Plural != value))
				{
					this._Plural = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LanguageID", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string LanguageID
		{
			get
			{
				return this._LanguageID;
			}
			set
			{
				if ((this._LanguageID != value))
				{
					this._LanguageID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AccentedText", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string AccentedText
		{
			get
			{
				return this._AccentedText;
			}
			set
			{
				if ((this._AccentedText != value))
				{
					this._AccentedText = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
