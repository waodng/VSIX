using System;
namespace AspNetMvcScaffolder.Telemetry
{
    [Flags]
    internal enum WebApiControllerScaffolderOptions : uint
    {
        None = 0u,

        CreatedController = 1u,

        IsAsyncSelected = 2u,
    }
}
