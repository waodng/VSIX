using Waodng.CodeMaid.Properties;
using Waodng.CodeMaid.UI.Enumerations;

namespace Waodng.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning general options.
    /// </summary>
    public class CleaningGeneralViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningGeneralViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningGeneralViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_AutoCleanupOnFileSave, x => AutoCleanupOnFileSave),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_AutoSaveAndCloseIfOpenedByCleanup, x => AutoSaveAndCloseIfOpenedByCleanup),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Formatting_AspxHtml, x => FormattingAspxHtmlTag),
                new SettingToOptionMapping<int, AskYesNo>(x => ActiveSettings.Cleaning_FormattingAspxCleanupAsk, x => FormattingAspxCleanupAsk),
                new SettingToOptionMapping<int, AskYesNo>(x => ActiveSettings.Cleaning_PerformPartialCleanupOnExternal, x => PerformPartialCleanupOnExternal)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.General;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if cleanup should run automatically on file save.
        /// </summary>
        public bool AutoCleanupOnFileSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if files should be automatically saved and closed if
        /// opened by cleanup.
        /// </summary>
        public bool AutoSaveAndCloseIfOpenedByCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }
        /// <summary>
        /// 新增属性用于设置是否修改aspx页面的样式
        /// </summary>
        public bool FormattingAspxHtmlTag
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the options for performing partial cleanup on external files.
        /// </summary>
        public AskYesNo PerformPartialCleanupOnExternal
        {
            get { return GetPropertyValue<AskYesNo>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// 清理aspx页面的代码 是全部还是部分
        /// </summary>
        public AskYesNo FormattingAspxCleanupAsk
        {
            get { return GetPropertyValue<AskYesNo>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}