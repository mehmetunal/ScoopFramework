using System;
using ScoopFramework.Entity;
using System.ComponentModel;

namespace ScoopFramework.DataBussiens
{
	/// <summary>
	/// Represents a TBL_UserEmail.
	/// NOTE: This class is generated from a T4 template - you should not modify it manually.
	/// </summary>
	public class TBL_UserEmail :BaseEntity
	{
		public Guid? UserId { get; set; }
		
		public string Email { get; set; }
		}
}      
		