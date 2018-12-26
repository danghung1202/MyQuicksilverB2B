using EPiServer.Cms.TinyMce.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace EPiServer.Reference.Commerce.Site.Infrastructure.InitializationModules
{
    /// <summary>
    /// https://www.epinova.no/en/blog/configuring-default-toolbar-buttons-in-tinymce-4-for-episerver/
    /// https://c2experience.com/blog/2018/07/big-changes-for-tinymce-editor-configuration-for-update-206-with-code-examples/
    /// </summary>
    [ModuleDependency(typeof(TinyMceInitialization))]
    public class CustomizedTinyMceInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Configure<TinyMceConfiguration>(config =>
            {
                var row1 = DefaultValues.Toolbar;
                var row2 = "code paste anchor table media";
                config.Default().AddPlugin("code", "paste", "anchor", "table", "media").Toolbar(row1, row2);
            });
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}