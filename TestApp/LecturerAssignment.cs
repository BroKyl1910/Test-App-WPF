//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class LecturerAssignment
    {
        public string LecturerAssignmentID { get; set; }
        public string Username { get; set; }
        public string ModuleID { get; set; }
    
        public virtual Module Module { get; set; }
        public virtual User User { get; set; }
    }
}
