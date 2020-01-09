using AspNetMvcScaffolder.Scaffolders.CustomScaffolder;
using AspNetMvcScaffolder.VisualStudio;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AspNetMvcScaffolder.UserInterface
{
    public class ControllerWithEntityViewModelViewModel : ViewModel, IDialogSettings
    {
        private string viewModelTypeName;

        private string serviceTypeName;

        public IEnumerable<ViewModelProperty> ViewModelPropertys { get; set; }

        public  ControllerWithEntityScaffolderModel Model { get; set; }

        public ControllerWithEntityViewModelViewModel(ControllerWithEntityScaffolderModel model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }


            Model = model;
            ServiceTypeName = model.ServiceTypeName;
            ViewModelTypeName = model.ViewModelTypeName;
            ViewModelPropertys = model.ViewModelPropertys;
        }

        public string ServiceTypeName
        {
            get
            {
                return serviceTypeName;
            }
            set
            {
                if (OnPropertyChanged(ref serviceTypeName, value))
                {
                    Model.ServiceTypeName = value;
                    SetValidationMessage(Model.ValidateServiceTypeName(value));
                }
            }
        }

        public string ViewModelTypeName
        {
            get
            {
                return viewModelTypeName;
            }
            set
            {
                if (OnPropertyChanged(ref viewModelTypeName, value))
                {
                    Model.ViewModelTypeName = value;
                    SetValidationMessage(Model.ValidateViewModelTypeName(value));
                }
            }
        }

        public virtual void LoadDialogSettings(IProjectSettings settings)
        {
            Contract.Assert(settings != null);

            double dialogWidth;
            if (settings.TryGetDouble(SavedSettingsKeys.CreateViewModelDialogWidthKey, out dialogWidth))
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

            settings[SavedSettingsKeys.CreateViewModelDialogWidthKey] = DialogWidth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
