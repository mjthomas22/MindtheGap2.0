//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MindTheGap.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLevel
    {
        public int levelId { get; set; }
        public string userId { get; set; }
        public int userLevel1 { get; set; }
        public int xp { get; set; }
        public int xpNeeded { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
