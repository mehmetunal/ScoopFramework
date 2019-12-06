using System;
using ScoopFramework.Entity;
using System.ComponentModel;

namespace ScoopFramework.DataBussiens
{
	/// <summary>
	/// Represents a Users.
	/// NOTE: This class is generated from a T4 template - you should not modify it manually.
	/// </summary>
	public class Users :BaseEntity
	{
		public string UserName { get; set; }
		
		public string UserEmail { get; set; }
		
		public string Password { get; set; }
		
		public bool? IsActive { get; set; }
		
		public bool? IsDelete { get; set; }
		
		public int Order { get; set; }
		}
}      
		