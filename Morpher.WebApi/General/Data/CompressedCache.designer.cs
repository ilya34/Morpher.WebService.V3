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

namespace Morpher.WebService.V3.General
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
	
	
	[Database(Name="slepov_0546d")]
	public partial class CompressedCacheDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertCompressedCache(CompressedCache instance);
    partial void UpdateCompressedCache(CompressedCache instance);
    partial void DeleteCompressedCache(CompressedCache instance);
    #endregion
		
		public CompressedCacheDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["slepov_0546dConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public CompressedCacheDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CompressedCacheDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CompressedCacheDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public CompressedCacheDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<CompressedCache> CompressedCaches
		{
			get
			{
				return this.GetTable<CompressedCache>();
			}
		}
	}
	
	[Table(Name="dbo.CompressedCache")]
	public partial class CompressedCache : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.DateTime _Date;
		
		private string _GZipCache;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDateChanging(System.DateTime value);
    partial void OnDateChanged();
    partial void OnGZipCacheChanging(string value);
    partial void OnGZipCacheChanged();
    #endregion
		
		public CompressedCache()
		{
			OnCreated();
		}
		
		[Column(Storage="_Date", DbType="Date NOT NULL", IsPrimaryKey=true)]
		public System.DateTime Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this.OnDateChanging(value);
					this.SendPropertyChanging();
					this._Date = value;
					this.SendPropertyChanged("Date");
					this.OnDateChanged();
				}
			}
		}
		
		[Column(Storage="_GZipCache", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string GZipCache
		{
			get
			{
				return this._GZipCache;
			}
			set
			{
				if ((this._GZipCache != value))
				{
					this.OnGZipCacheChanging(value);
					this.SendPropertyChanging();
					this._GZipCache = value;
					this.SendPropertyChanged("GZipCache");
					this.OnGZipCacheChanged();
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
}
#pragma warning restore 1591
