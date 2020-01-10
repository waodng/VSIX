using System;
using Waodng.CodeMaid.Helpers;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace Waodng.CodeMaid.Integration.Package
{
    /// <summary>
    /// A package that provides for hide vs menu .  shortcut key ALT Show 
    /// </summary>
    internal sealed class HideVsMenuPack:BasePack
    {
        #region Singleton

        public static HideVsMenuPack Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new HideVsMenuPack(package);
            package.SettingsMonitor.Watch(s => s.Feature_HideVsMenu, Instance.Switch);
            Instance.OnExecute();
        }

        #endregion Singleton

        #region Fields

		private FrameworkElement _menuContainer;

		private bool _isMenuVisible;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HideMenuCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal HideVsMenuPack(CodeMaidPackage package)
            : base(package)
        {

        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the Hide menu.
        /// </summary>
        public override void OnExecute()
        {
            base.OnExecute();

            EventManager.RegisterClassHandler(typeof(UIElement), UIElement.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this.PopupLostKeyboardFocus));
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                EventHandler layoutUpdated = null;
                layoutUpdated = delegate(object sender, EventArgs e)
                {
                    bool flag = false;
                    foreach (Menu menu in mainWindow.FindDescendants<Menu>())
                    {
                        if (AutomationProperties.GetAutomationId(menu) == "MenuBar")
                        {
                            FrameworkElement frameworkElement = menu;
                            DependencyObject visualOrLogicalParent = menu.GetVisualOrLogicalParent();
                            if (visualOrLogicalParent != null)
                            {
                                frameworkElement = ((visualOrLogicalParent.GetVisualOrLogicalParent() as DockPanel) ?? frameworkElement);
                            }
                            flag = true;
                            this.MenuContainer = frameworkElement;
                        }
                    }
                    if (flag)
                    {
                        mainWindow.LayoutUpdated -= layoutUpdated;
                    }
                };
                mainWindow.LayoutUpdated += layoutUpdated;
            }
        }

        #endregion BaseCommand Methods

        #region Properties
        /// <summary>
        /// Menu is not show
        /// </summary>
        private bool IsMenuVisible
		{
			get
			{
				return this._isMenuVisible;
			}
			set
			{
				if (this._isMenuVisible != value)
				{
					this._isMenuVisible = value;
					if (this._menuContainer != null)
					{
						if (this._isMenuVisible)
						{
							this._menuContainer.ClearValue(FrameworkElement.HeightProperty);
							return;
						}
						this._menuContainer.Height = 0.0;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private FrameworkElement MenuContainer
		{
			get
			{
				return this._menuContainer;
			}
			set
			{
				if (this._menuContainer != null)
				{
					this._menuContainer.IsKeyboardFocusWithinChanged -= this.OnMenuContainerFocusChanged;
				}
				this._menuContainer = value;
				if (this._menuContainer != null)
				{
					if (this._isMenuVisible)
					{
						this._menuContainer.ClearValue(FrameworkElement.HeightProperty);
					}
					else
					{
						this._menuContainer.Height = 0.0;
					}
					this._menuContainer.IsKeyboardFocusWithinChanged += this.OnMenuContainerFocusChanged;
				}
			}
		}

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuContainer"></param>
        /// <returns></returns>
        private bool IsAggregateFocusInMenuContainer(FrameworkElement menuContainer)
        {
	        if (menuContainer.IsKeyboardFocusWithin)
	        {
		        return true;
	        }
	        for (DependencyObject dependencyObject = (DependencyObject)Keyboard.FocusedElement; dependencyObject != null; dependencyObject = dependencyObject.GetVisualOrLogicalParent())
	        {
		        if (dependencyObject == menuContainer)
		        {
			        return true;
		        }
	        }
	        return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMenuContainerFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
	        this.IsMenuVisible = this.IsAggregateFocusInMenuContainer(this.MenuContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
	        if (this.IsMenuVisible && this.MenuContainer != null && !this.IsAggregateFocusInMenuContainer(this.MenuContainer))
	        {
		        this.IsMenuVisible = false;
	        }
        }
        #endregion Methods
    }
}