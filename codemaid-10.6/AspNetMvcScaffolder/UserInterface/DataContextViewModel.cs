using AspNetMvcScaffolder.VisualStudio;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace AspNetMvcScaffolder.UserInterface
{
    public class MvcDataContextViewModel : ViewModel, IDialogSettings
    {
        private string dataContextName;

        public MvcDataContextViewModel(ControllerScaffolderModel model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            Model = model;
            DataContextName = model.DataContextName;
        }

        protected ControllerScaffolderModel Model { get; set; }

        public string DataContextName
        {
            get
            {
                return dataContextName;
            }
            set
            {
                if (OnPropertyChanged(ref dataContextName, value))
                {
                    Model.DataContextName = value;
                    SetValidationMessage(Model.ValidateDbContextName(value));
                }
            }
        }

        public virtual void LoadDialogSettings(IProjectSettings settings)
        {
            Contract.Assert(settings != null);

            double dialogWidth;
            if (settings.TryGetDouble(SavedSettingsKeys.DbContextDialogWidthKey, out dialogWidth))
            {
                DialogWidth = dialogWidth;
            }
        }

        public virtual void SaveDialogSettings(IProjectSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings[SavedSettingsKeys.DbContextDialogWidthKey] = DialogWidth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
