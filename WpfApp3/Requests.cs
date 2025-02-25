//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Requests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requests()
        {
            this.Feedback = new HashSet<Feedback>();
            this.Repairs = new HashSet<Repairs>();
        }
    
        public int ID { get; set; }
        public string RequestNumber { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int EquipmentID { get; set; }
        public string FaultType { get; set; }
        public string Description { get; set; }
        public int ClientID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedback { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repairs> Repairs { get; set; }
        public virtual Users Users { get; set; }
    }
}
