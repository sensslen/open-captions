namespace Pro.LyricsBot
{
    /// <summary>
    /// DataTemplateSelector used to identify data templates using interfaces
    /// this class is taken from https://stackoverflow.com/a/76451590/1199089 and adapted
    /// </summary>
    public class InterfaceDataTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary TemplateDictionary { get; set; } = new ResourceDictionary();

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is null)
            {
                return null;
            }
            return GetDataTemplate(item.GetType(), TemplateDictionary);
        }

        private static DataTemplate? GetDataTemplate(Type type, ResourceDictionary resourceDictionary)
        {
            return GetDataTemplateFromType(type, resourceDictionary)
                    ?? GetDataTemplateFromInterfaces(type, resourceDictionary);
        }

        private static DataTemplate? GetDataTemplateFromType(Type type, ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary.TryGetValue(type.Name, out var resource) &&
                resource is DataTemplate dataTemplate)
            {
                return dataTemplate;
            }
            return null;
        }

        private static DataTemplate? GetDataTemplateFromInterfaces(Type type, ResourceDictionary resourceDictionary)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                var dataTemplate = GetDataTemplateFromType(interfaceType, resourceDictionary);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }
            return null;
        }
    }
}
