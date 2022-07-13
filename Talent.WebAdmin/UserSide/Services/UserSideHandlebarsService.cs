using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Talent.WebAdmin.UserSide.Interfaces;

namespace Talent.WebAdmin.UserSide.Services
{
    /// <summary>
    /// Templating service that use Handlebars.NET for template binding.
    /// </summary>
    public class UserSideHandlebarsService : ITemplatingService
    {
        /// <summary>
        /// Render / compile template using Handlebars.NET.
        /// </summary>
        /// <typeparam name="T">The binded data's data type.</typeparam>
        /// <param name="template">Template.</param>
        /// <param name="data">The binded data.</param>
        /// <returns>Return the rendered / compiled template.</returns>
        public string Render<T>(string template, T data)
        {
            if (string.IsNullOrEmpty(template) == true)
            {
                throw new NullReferenceException("You must specify a template name");
            }

            var hasTemplate = template;
            
            if (hasTemplate == "")
            {
                throw new FileNotFoundException("The template was not found");
            }

            var dataTemplate = Handlebars.Compile(template);

            var result = dataTemplate(data);

            return result;
        }
    }
}
