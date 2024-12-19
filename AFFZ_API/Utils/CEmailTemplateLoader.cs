using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AFFZ_API.Utils
{

    public class EmailTemplateLoader
    {
        private readonly Dictionary<string, EmailTemplate> _templates;

        public EmailTemplateLoader(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Email templates file not found.", filePath);
            }

            var jsonContent = File.ReadAllText(filePath);
            _templates = JsonConvert.DeserializeObject<Dictionary<string, EmailTemplate>>(jsonContent)
                         ?? throw new InvalidOperationException("Failed to load email templates.");
        }

        public EmailTemplate GetEmailTemplate(string templateName)
        {
            if (_templates.ContainsKey(templateName))
            {
                return _templates[templateName];
            }
            throw new KeyNotFoundException($"Template '{templateName}' not found.");
        }
    }

    public class EmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

}

