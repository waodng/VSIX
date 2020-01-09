using AspNetMvcScaffolder.VisualStudio;
using Microsoft.AspNet.Scaffolding;
using System;
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
    public class ControllerScaffolderViewModel : ViewModel, IDialogSettings
    {
        private IDialogHost dialogHost;

        private string controllerName;

        private ModelType modelType;

        private string modelTypeName;

        private string dataContextTypeName;

        private ModelType dataContextType;

        private bool isViewGenerationSelected;

        private bool isLayoutPageSelected;

        private bool isReferenceScriptLibrariesSelected;

        private string layoutPageFile;

        private bool isAsyncSelected;

        public ControllerScaffolderViewModel(ControllerScaffolderModel model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            Model = model;

            ControllerName = model.ControllerName;
            IsAsyncSelected = model.IsAsyncSelected;
            SetValidationMessage(Model.ValidateControllerName(ControllerName), "ControllerName");

            IsViewGenerationSupported = Model.IsViewGenerationSupported;
            if (IsViewGenerationSupported)
            {
                IsViewGenerationSelected = Model.IsViewGenerationSelected;
                IsLayoutPageSelected = Model.IsLayoutPageSelected;
                IsReferenceScriptLibrariesSelected = Model.IsReferenceScriptLibrariesSelected;
                LayoutPageFile = model.LayoutPageFile;
            }

            DataContextTypesInternal = new ObservableCollection<ModelType>();
            ModelTypesInternal = new ObservableCollection<ModelType>();

            DataContextTypes = new ListCollectionView(DataContextTypesInternal);
            DataContextTypes.CustomSort = new DataContextModelTypeComparer();

            ModelTypes = CollectionViewSource.GetDefaultView(ModelTypesInternal);
            ModelTypes.SortDescriptions.Add(new SortDescription("ShortTypeName", ListSortDirection.Ascending));

            IsModelClassSupported = Model.IsModelClassSupported;
            IsDataContextSupported = Model.IsDataContextSupported;

            if (Model.IsModelClassSupported)
            {
                foreach (ModelType modelType in Model.ModelTypes)
                {
                    ModelTypesInternal.Add(modelType);
                }

                SetValidationMessage(Model.ValidateModelType(null), "ModelType");
            }

            if (Model.IsDataContextSupported)
            {
                foreach (ModelType modelType in Model.DataContextTypes)
                {
                    DataContextTypesInternal.Add(modelType);
                }

                if (model.DataContextType != null)
                {
                    DataContextType = Model.DataContextType;
                    DataContextTypeName = DataContextType.DisplayName;
                }

                SetValidationMessage(Model.ValidateDataContextType(DataContextType), "DataContextType");
            }

            AddNewDataContextCommand = new RelayCommand(AddNewDataContext);
            SelectLayoutCommand = new RelayCommand(SelectLayout);

            AsyncInformationIcon = GetInformationIcon();
        }

        public IDialogHost DialogHost
        {
            get { return dialogHost; }
            set
            {
                IDialogHost oldValue = dialogHost;
                if (OnPropertyChanged<IDialogHost>(ref dialogHost, value))
                {
                    if (oldValue != null)
                    {
                        oldValue.Closing -= DialogHost_Closing;
                    }

                    if (value != null)
                    {
                        value.Closing += DialogHost_Closing;
                    }
                }
            }
        }

        public string ControllerName
        {
            get { return controllerName; }
            set
            {
                if (OnPropertyChanged(ref controllerName, value))
                {
                    controllerName = value == null ? null : value.Trim();
                    SetValidationMessage(Model.ValidateControllerName(controllerName));
                    Model.ControllerName = controllerName;
                }
            }
        }

        public bool IsAsyncSelected
        {
            get { return isAsyncSelected; }
            set
            {
                if (OnPropertyChanged<bool>(ref isAsyncSelected, value))
                {
                    isAsyncSelected = value;
                    Model.IsAsyncSelected = isAsyncSelected;
                }
            }
        }

        public bool IsAsyncSupported
        {
            get { return Model.IsAsyncSupported; }
        }

        public ImageSource AsyncInformationIcon { get; private set; }

        public bool IsModelClassSupported { get; private set; }

        public bool IsDataContextSupported { get; private set; }

        public ICommand AddNewDataContextCommand { get; private set; }

        public ListCollectionView DataContextTypes { get; private set; }

        internal ObservableCollection<ModelType> DataContextTypesInternal { get; private set; }

        private ModelType AddedDataContextItem { get; set; }

        public ICollectionView ModelTypes { get; private set; }

        internal ObservableCollection<ModelType> ModelTypesInternal { get; private set; }

        public ModelType ModelType
        {
            get { return modelType; }
            set
            {
                if (OnPropertyChanged(ref modelType, value))
                {
                    if (Model.IsModelClassSupported)
                    {
                        SetValidationMessage(Model.ValidateModelType(value));

                        if (!IsControllerNameUserSet(Model.ModelType, ControllerName))
                        {
                            ControllerName = Model.GenerateControllerName(value == null ? null : value.ShortTypeName);
                        }
                        Model.ModelType = value;
                    }
                }
            }
        }

        public string ModelTypeName
        {
            get { return modelTypeName; }
            set
            {
                if (OnPropertyChanged(ref modelTypeName, value))
                {
                    if (Model.IsModelClassSupported && ModelType != null)
                    {
                        if (ModelType.DisplayName.StartsWith(value, StringComparison.Ordinal))
                        {
                            modelTypeName = ModelType.DisplayName;
                        }
                    }
                }
            }
        }

        public ModelType DataContextType
        {
            get { return dataContextType; }
            set
            {
                if (OnPropertyChanged(ref dataContextType, value))
                {
                    Model.DataContextType = value;
                    if (Model.IsModelClassSupported)
                    {
                        SetValidationMessage(Model.ValidateDataContextType(value));
                    }
                }
            }
        }

        public string DataContextTypeName
        {
            get { return dataContextTypeName; }
            set
            {
                if (OnPropertyChanged(ref dataContextTypeName, value))
                {
                    if (IsModelClassSupported && DataContextType != null)
                    {
                        if (DataContextType.DisplayName.StartsWith(value, StringComparison.Ordinal))
                        {
                            dataContextTypeName = DataContextType.DisplayName;
                        }
                    }
                }
            }
        }

        public bool IsViewGenerationSupported { get; private set; }

        public bool IsViewGenerationSelected
        {
            get { return isViewGenerationSelected; }
            set
            {
                if (OnPropertyChanged<bool>(ref isViewGenerationSelected, value))
                {
                    Model.IsViewGenerationSelected = value;
                }
            }
        }

        public bool IsLayoutPageSelected
        {
            get { return isLayoutPageSelected; }
            set
            {
                if (OnPropertyChanged<bool>(ref isLayoutPageSelected, value))
                {
                    Model.IsLayoutPageSelected = value;
                }
            }
        }

        public ICommand SelectLayoutCommand { get; private set; }

        public bool IsReferenceScriptLibrariesSelected
        {
            get { return isReferenceScriptLibrariesSelected; }
            set
            {
                if (OnPropertyChanged(ref isReferenceScriptLibrariesSelected, value))
                {
                    Model.IsReferenceScriptLibrariesSelected = value;
                }
            }
        }

        public string LayoutPageFile
        {
            get { return layoutPageFile; }
            set
            {
                if (OnPropertyChanged<string>(ref layoutPageFile, value))
                {
                    Model.LayoutPageFile = value;
                }
            }
        }

        protected virtual ControllerScaffolderModel Model { get; set; }

        public ModelType AddNewDataContext(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            if (AddedDataContextItem != null)
            {
                DataContextType = null;
                DataContextTypeName = null;
                DataContextTypesInternal.Remove(AddedDataContextItem);
            }

            AddedDataContextItem = new ModelType(typeName);
            DataContextTypesInternal.Add(AddedDataContextItem);
            return AddedDataContextItem;
        }

        public string GenerateDefaultDataContextTypeName()
        {
            return Model.GenerateDefaultDataContextTypeName();
        }

        private void DialogHost_Closing(object sender, CancelEventArgs e)
        {
            string errorMessage = Model.GetErrorIfInvalidIdentifier(ControllerName);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                DisplayErrorMessage(DialogHost, errorMessage);
                e.Cancel = true;
                return;
            }

            if (Model.ControllerExists(ControllerName))
            {
                MessageBoxResult result = DialogHost.RequestConfirmation(String.Format(CultureInfo.CurrentCulture, Resources.OverwriteMessage, ControllerName), Resources.AddControllerWindowTitle);

                if (result == MessageBoxResult.Yes)
                {
                    Model.IsOverwritingFiles = true;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void AddNewDataContext(object param)
        {
            CreateDataContextDialog dialog = new CreateDataContextDialog();

            Model.DataContextName = GenerateDefaultDataContextTypeName();
            MvcDataContextViewModel viewModel = new MvcDataContextViewModel(Model);
            dialog.DataContext = viewModel;

            if (dialog.ShowModal() == true)
            {
                DataContextType = AddNewDataContext(viewModel.DataContextName);
                DataContextTypeName = DataContextType.DisplayName;
            }
            else
            {
                DataContextType = null;
                DataContextTypeName = null;
            }
        }

        private void SelectLayout(object unused)
        {
            string filter;
            ProjectLanguage language = Model.ActiveProject.GetCodeLanguage();
            if (language == ProjectLanguage.CSharp)
            {
                filter = Resources.MasterPageCsHtmlFilter;
            }
            else if (language == ProjectLanguage.VisualBasic)
            {
                filter = Resources.MasterPageVbHtmlFilter;
            }
            else
            {
                Contract.Assert(false, "We shouldn't get here, this project's language is not supported.");
                return;
            }

            string file;
            if (DialogHost.TrySelectFile(
                Model.ActiveProject,
                Resources.LayoutPageSelectorHeading,
                filter,
                SavedSettingsKeys.LayoutPageFileKey,
                out file))
            {
                LayoutPageFile = file;
            }
        }

        private static BitmapSource GetInformationIcon()
        {
            return Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Information.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private bool IsControllerNameUserSet(ModelType previousModelSelected, string controllerName)
        {
            if (String.IsNullOrWhiteSpace(controllerName) || String.IsNullOrWhiteSpace(Model.ControllerRootName))
            {
                return false;
            }

            if (previousModelSelected != null)
            {
                string generatedControllerName = Model.GenerateControllerName(previousModelSelected.ShortTypeName);
                if (String.Equals(generatedControllerName, controllerName, StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void LoadDialogSettings(IProjectSettings settings)
        {
            Contract.Assert(settings != null);

            double dialogWidth;
            if (settings.TryGetDouble(SavedSettingsKeys.ControllerDialogWidthKey, out dialogWidth))
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

            settings[SavedSettingsKeys.ControllerDialogWidthKey] = DialogWidth.ToString(CultureInfo.InvariantCulture);
        }
    }
}
