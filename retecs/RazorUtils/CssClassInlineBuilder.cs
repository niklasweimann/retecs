using System.Linq;

namespace retecs.RazorUtils
{
    public static class CssClassInlineBuilder
    {
        public static string CssClass(object obj) =>
            string.Join(' ', obj.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(bool) && (bool) p.GetMethod?.Invoke(obj, null))
                .Select(p => p.Name.ToLower()));
    }
}