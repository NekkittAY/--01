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
    
    public partial class Feedback
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public int Rating { get; set; }
        public string FeedbackText { get; set; }
    
        public virtual Requests Requests { get; set; }
    }
}
