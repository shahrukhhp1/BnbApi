//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BnbApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class Feature
    {
        public int UId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual User User { get; set; }
    }
}
