using System;
using Adapters;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Infrastructure.Common;

public class LocalizedBinding : MarkupExtension
{
    private readonly string _path;

    public LocalizedBinding()
    {
        _path = "";
    }
    
    public LocalizedBinding(string path)
    {
        _path = path;
    }
    
    
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding(_path)
        {
            Converter = new LocalizedMessageConverter()
        };
    }
}