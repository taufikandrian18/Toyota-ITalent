using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.WebAdmin.UserSide.Interfaces
{

    /// <summary>
    /// Templating service interface definition.
    /// Any service that implements this interface will have rendering / compiling functions that can be used to bind data into the template.
    /// </summary>
    public interface ITemplatingService
    {
        /// <summary>
        /// Render / compile the template data, which will automatically bind any C# object into the template.
        /// </summary>
        /// <typeparam name="T">Object that will be binded into the template. Compatible with any type.</typeparam>
        /// <param name="template">Template.</param>
        /// <param name="data">Object that will be binded into the template</param>
        /// <returns>Return the rendered / compiled template.</returns>
        string Render<T>(string template, T data);
    }
}
