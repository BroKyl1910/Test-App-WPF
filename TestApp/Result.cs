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
    
    public partial class Result
    {
        public string ResultID { get; set; }
        public string TestID { get; set; }
        public string Username { get; set; }
        public int AttemptNumber { get; set; }
        public int UserResult { get; set; }
        public decimal ResultPercentage { get; set; }
    
        public virtual Test Test { get; set; }
        public virtual User User { get; set; }
    }
}
