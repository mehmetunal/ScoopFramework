﻿using System.Web.Mvc.Razor;
using System.Web.Razor.Generator;
using ScoopFramework.MinifierLibrary.Minifiers;

namespace ScoopFramework.MinifierLibrary.Mvc
{
    public class MinifierRazorHost : MvcWebPageRazorHost
    {
        public MinifierRazorHost(string virtualPath, string physicalPath)
            : base(virtualPath, physicalPath)
        { }

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            var minifier = MinifierFactory.GetMinifier();
            var codeGenerator = CodeGeneratorFactory.GetCodeGenerator(this, incomingCodeGenerator, minifier);
            return codeGenerator;
        }
    }
}