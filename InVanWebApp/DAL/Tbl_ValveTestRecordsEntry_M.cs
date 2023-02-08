//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InVanWebApp.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_ValveTestRecordsEntry_M
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_ValveTestRecordsEntry_M()
        {
            this.Tbl_PressureValues_T = new HashSet<Tbl_PressureValues_T>();
            this.Tbl_TemperatureValues_T = new HashSet<Tbl_TemperatureValues_T>();
        }
    
        public int TestRecordId { get; set; }
        public Nullable<int> ValveTypeId { get; set; }
        public Nullable<int> ValveModelId { get; set; }
        public Nullable<int> ValveSizeId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> TestCategoryId { get; set; }
        public Nullable<int> TestSubCategoryId { get; set; }
        public Nullable<int> SealingId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> PressureClassId { get; set; }
        public Nullable<int> PressureGaugeId { get; set; }
        public Nullable<int> PressureTransmitterId { get; set; }
        public string TestRecordNumber { get; set; }
        public Nullable<System.DateTime> TestingDate { get; set; }
        public Nullable<System.DateTime> NextDueDate { get; set; }
        public string TagNumber { get; set; }
        public string Make { get; set; }
        public string SerialNumber { get; set; }
        public string SetPressure { get; set; }
        public string FluidAndState { get; set; }
        public string CDTP { get; set; }
        public string BackPressure { get; set; }
        public string SpringNr { get; set; }
        public string OpTemperature { get; set; }
        public string Material { get; set; }
        public string TestResult { get; set; }
        public string Remarks { get; set; }
        public string PressureSystemId { get; set; }
        public string TestingOperator { get; set; }
        public string QualityEngineer { get; set; }
        public string WitnessBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> TestConfigurationId { get; set; }
        public string TestBenchSrNo { get; set; }
        public Nullable<int> NotesId { get; set; }
        public string PressureGaugeType { get; set; }
    
        public virtual Tbl_Customers_M Tbl_Customers_M { get; set; }
        public virtual Tbl_Notes_M Tbl_Notes_M { get; set; }
        public virtual Tbl_PressureClass_M Tbl_PressureClass_M { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_PressureValues_T> Tbl_PressureValues_T { get; set; }
        public virtual Tbl_SubTestCategory_M Tbl_SubTestCategory_M { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_TemperatureValues_T> Tbl_TemperatureValues_T { get; set; }
        public virtual Tbl_TestCategory_M Tbl_TestCategory_M { get; set; }
        public virtual Tbl_TestComponent_M Tbl_TestComponent_M { get; set; }
        public virtual Tbl_TestComponent_M Tbl_TestComponent_M1 { get; set; }
        public virtual Tbl_TestConfiguration_M Tbl_TestConfiguration_M { get; set; }
        public virtual Tbl_ValveModel_M Tbl_ValveModel_M { get; set; }
        public virtual Tbl_ValveSize_M Tbl_ValveSize_M { get; set; }
        public virtual Tbl_ValveTypes_M Tbl_ValveTypes_M { get; set; }
    }
}
