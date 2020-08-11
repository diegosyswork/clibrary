using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SysWork.Forms.Utilities;

namespace SysWork.Forms.Interfaces
{
    public interface ICRUDForm
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is final valition.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is final valition; otherwise, <c>false</c>.
        /// </value>
        bool _IsFinalValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loading data; otherwise, <c>false</c>.
        /// </value>
        bool _IsLoadingData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [no validate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no validate]; otherwise, <c>false</c>.
        /// </value>
        bool _DoNotValidateForm { get; set; }

        /// <summary>
        /// Gets or sets the unique key controls.
        /// </summary>
        /// <value>
        /// The unique key controls.
        /// </value>
        List<Control> _UniqueKeyControls { get; set; }

        /// <summary>
        /// Gets the data objects.
        /// </summary>
        void GetDataObjects();

        /// <summary>
        /// Initializes the controls.
        /// </summary>
        void InitControls();

        /// <summary>
        /// Cleans the controls.
        /// </summary>
        void InitializeCRUDForm();

        /// <summary>
        /// Sets the unique controls.
        /// </summary>
        void SetUniqueControls();

        /// <summary>
        /// Sets the state of the edit mode.
        /// </summary>
        /// <param name="editMode">The edit mode.</param>
        void SetEditModeState(EEditModeState editMode);

        void SetValidateControlEvent();
        /// <summary>
        /// Validates the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        void ValidateControl(object control, CancelEventArgs e);

        /// <summary>
        /// Entities the values to controls.
        /// </summary>
        void EntityValuesToControls();

        /// <summary>
        /// Controlses the values to entity.
        /// </summary>
        void ControlsValuesToEntity();

        /// <summary>
        /// Determines whether [is valid data for insert/update].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid data for insert update]; otherwise, <c>false</c>.
        /// </returns>
        bool IsValidDataForInsertUpdate();

        /// <summary>
        /// Determines whether [is valid data for delete].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid data for delete]; otherwise, <c>false</c>.
        /// </returns>
        bool IsValidDataForDelete();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool SaveData(out string errMessage);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        /// <returns></returns>
        bool DeleteData(out string errMessage);
        
        /// <summary>
        /// Calls the asociated report.
        /// </summary>
        void CallReport();

        /// <summary>
        /// Queries the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        bool QueryControl(Control control);
    }
}
