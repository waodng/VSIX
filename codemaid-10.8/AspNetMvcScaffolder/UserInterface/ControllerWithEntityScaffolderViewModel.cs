using AspNetMvcScaffolder.Scaffolders.CustomScaffolder;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using EnvDTE;

namespace AspNetMvcScaffolder.UserInterface
{
    public class ControllerWithEntityScaffolderViewModel : ControllerScaffolderViewModel
    {
        public bool IsViewModelSupported { get; set; }

        public bool IsServiceClassSupported { get; set; }

        private ModelType serviceType;

        private string viewModelTypeName;

        private string serviceTypeName;

        private ModelType viewModelType;

        private new ControllerWithEntityScaffolderModel Model;

        public ICollectionView ServiceTypes { get; private set; }

        public ICollectionView ViewModelTypes { get; private set; }

        internal ObservableCollection<ModelType> ViewModelTypesInternal { get; private set; }

        internal ObservableCollection<ModelType> ServiceTypesInternal { get; private set; }

        public ControllerWithEntityScaffolderViewModel(ControllerWithEntityScaffolderModel model) : base(model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            this.Model = model;

            if (IsViewGenerationSupported)
            {
                IsViewGenerationSelected = Model.IsViewGenerationSelected;
                IsLayoutPageSelected = Model.IsLayoutPageSelected;
                IsReferenceScriptLibrariesSelected = Model.IsReferenceScriptLibrariesSelected;
                LayoutPageFile = model.LayoutPageFile;
            }

            ServiceTypesInternal = new ObservableCollection<ModelType>();
            ServiceTypes = CollectionViewSource.GetDefaultView(ServiceTypesInternal);
            ServiceTypes.SortDescriptions.Add(new SortDescription("ShortTypeName", ListSortDirection.Ascending));

            ViewModelTypesInternal = new ObservableCollection<ModelType>();
            ViewModelTypes = CollectionViewSource.GetDefaultView(ViewModelTypesInternal);
            ViewModelTypes.SortDescriptions.Add(new SortDescription("ShortTypeName", ListSortDirection.Ascending));

            IsViewModelSupported = model.IsViewModelSupported;
            IsServiceClassSupported = model.IsServiceClassSupported;

            if (model.IsViewModelSupported)
            {
                foreach (ModelType modelType in model.ViewModelTypes)
                {
                    ViewModelTypesInternal.Add(modelType);
                }

                SetValidationMessage(model.ValidateViewModelType(null), "ViewModelType");
            }

            if (model.IsServiceClassSupported)
            {
                foreach (ModelType modelType in model.ServiceTypes)
                {
                    ServiceTypesInternal.Add(modelType);
                }

                SetValidationMessage(model.ValidateServiceType(null), "ServiceType");
            }

            AddNewViewModelCommand = new RelayCommand(AddNewViewModel);
            AddNewServiceCommand = new RelayCommand(AddNewService);
        }

        private void AddNewService(object param)
        {
            if (ModelType == null)
            {
                DisplayErrorMessage(DialogHost, "新增服务接口前需要指定模型。");
                return;
            }

            CreateServicetDialog dialog = new CreateServicetDialog();

            Model.ServiceTypeName = Model.GenerateDefaultServiceTypeName();

            ControllerWithEntityViewModelViewModel viewModel = new ControllerWithEntityViewModelViewModel(Model);

            dialog.DataContext = viewModel;

            if (dialog.ShowModal() == true)
            {
                ServiceType = AddNewService(viewModel.ServiceTypeName);
                ServiceTypeName = ServiceType.DisplayName;
            }
            else
            {
                ServiceType = null;
                ServiceTypeName = null;
            }
        }

        private void AddNewViewModel(object param)
        {
            if (ModelType == null)
            {
                DisplayErrorMessage(DialogHost, "新增视图模型前需要指定模型。");
                return;
            }

            CreateViewModelDialog dialog = new CreateViewModelDialog();

            Model.ViewModelTypeName = Model.GenerateDefaultViewModelTypeName();
            List<ViewModelProperty> viewModelPropertys = new List<ViewModelProperty>();
            foreach (CodeElement codeElement in Model.ModelType.CodeType.Children)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementProperty)
                {
                    CodeProperty property = codeElement as CodeProperty;
                    viewModelPropertys.Add(new ViewModelProperty
                    {
                        EntityPropertyName = property.Name,
                        ViewModelPropertyName = property.Name,
                        PropertyType = property.Type.AsString
                    });
                }
            }
            Model.ViewModelPropertys = viewModelPropertys;
            ControllerWithEntityViewModelViewModel viewModel = new ControllerWithEntityViewModelViewModel(Model);

            dialog.DataContext = viewModel;

            if (dialog.ShowModal() == true)
            {
                ViewModelType = AddNewViewModel(viewModel.ViewModelTypeName);
                ViewModelTypeName = ViewModelType.DisplayName;
            }
            else
            {
                ViewModelType = null;
                ViewModelTypeName = null;
                Model.ViewModelPropertys = null;
            }
        }

        public ModelType AddNewViewModel(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            if (AddedViewModelItem != null)
            {
                ViewModelType = null;
                ViewModelTypeName = null;
                ViewModelTypesInternal.Remove(AddedViewModelItem);
            }

            AddedViewModelItem = new ModelType(typeName);
            ViewModelTypesInternal.Add(AddedViewModelItem);
            return AddedViewModelItem;
        }

        public ModelType AddNewService(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            if (AddedServiceItem != null)
            {
                ServiceType = null;
                ServiceTypeName = null;
                ServiceTypesInternal.Remove(AddedServiceItem);
            }

            AddedServiceItem = new ModelType(typeName);
            ServiceTypesInternal.Add(AddedServiceItem);
            return AddedServiceItem;
        }


        private ModelType AddedServiceItem { get; set; }

        private ModelType AddedViewModelItem { get; set; }

        public ModelType ViewModelType
        {
            get { return viewModelType; }
            set
            {
                if (OnPropertyChanged(ref viewModelType, value))
                {
                    if (Model.IsViewModelSupported)
                    {
                        SetValidationMessage(Model.ValidateViewModelType(value));
                        Model.ViewModelType = value;
                    }
                }
            }
        }


        public ICommand AddNewViewModelCommand { get; private set; }

        public ICommand AddNewServiceCommand { get; private set; }

        public ModelType ServiceType
        {
            get { return serviceType; }
            set
            {
                if (OnPropertyChanged(ref serviceType, value))
                {
                    if (Model.IsServiceClassSupported)
                    {
                        SetValidationMessage(Model.ValidateServiceType(value));
                        Model.ServiceType = value;
                    }
                }
            }
        }
        public string ViewModelTypeName
        {
            get { return viewModelTypeName; }
            set
            {
                if (OnPropertyChanged(ref viewModelTypeName, value))
                {
                    if (Model.IsViewModelSupported && ViewModelType != null)
                    {
                        if (ViewModelType.DisplayName.StartsWith(value, StringComparison.Ordinal))
                        {
                            viewModelTypeName = viewModelType.DisplayName;
                        }
                    }
                }
            }
        }

        public string ServiceTypeName
        {
            get { return serviceTypeName; }
            set
            {
                if (OnPropertyChanged(ref serviceTypeName, value))
                {
                    if (Model.IsViewModelSupported&& ServiceType != null)
                    {
                        if (ServiceType.DisplayName.StartsWith(value, StringComparison.Ordinal))
                        {
                            serviceTypeName = serviceType.DisplayName;
                        }
                    }
                }
            }
        }
    }
}
