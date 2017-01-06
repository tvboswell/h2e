﻿namespace BlogSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Text;

    public class UrlGenerator : IUrlGenerator
    {
        public string GeneratePostUrl(int id, string title, DateTime createdOn)
        {
            return $"/Posts/{createdOn.Year:0000}/{createdOn.Month:00}/{this.GenerateUrl(title)}/{id}";
        }

        public string GenerateUrl(string uglyString)
        {
            StringBuilder resultString = new StringBuilder(uglyString.Length);
            bool isLastCharacterDash = false;

            uglyString = uglyString.Replace("C#", "CSharp");
            uglyString = uglyString.Replace("F#", "FSharp");
            uglyString = uglyString.Replace("C++", "CPlusPlus");
            uglyString = uglyString.Replace("ASP.NET", "AspNet");
            uglyString = uglyString.Replace(".NET", "DotNet");

            foreach (char character in uglyString)
            {
                if (char.IsLetterOrDigit(character))
                {
                    resultString.Append(character);

                    isLastCharacterDash = false;
                }
                else if (!isLastCharacterDash)
                {
                    resultString.Append('-');

                    isLastCharacterDash = true;
                }
            }

            return resultString.ToString().Trim('-');
        }
    }
}