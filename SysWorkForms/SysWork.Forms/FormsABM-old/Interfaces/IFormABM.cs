namespace SysWork.Forms.FormsABM.Interfaces
{
    public enum EEditMode
    {

    }
       
    public interface IFormABM
    {
        bool _noThrowComboEvents { get; set; }
        bool _noValidateForm { get; set; }
        bool _isFinalValidation { get; set; }
        void GetRepositories();
        void InitFormControls();
        void SetEditMode(bool permiteEdicion);
        void AssignData();
        bool IsValidData();
        bool AddEditRecord();
        bool IsValidDataForDelete();
        bool DeleteRecord();
        void GetNewCode();
    }
}
