using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Mvc.Razor
{
    /// <summary>
    /// Web view page
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class NopRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        #region Fields

        private Localizer _localizer;

        #endregion

        #region Properties

        /// <summary>
        /// Localizer
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="args">Arguments for text</param>
        /// <returns>Localized string</returns>
        public delegate Task<HtmlString> Localizer(string text, params object[] args);

        [RazorInject]
        /// <summary>
        /// Injected localization service
        /// </summary>
        public ILocalizationService LocalizationService { get; init; }

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    _localizer = async (format, args) =>
                    {
                        var resFormat = await LocalizationService.GetResourceAsync(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new HtmlString(format);
                        }
                        return new HtmlString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }

        #endregion

    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class NopRazorPage : NopRazorPage<dynamic>
    {
    }
}